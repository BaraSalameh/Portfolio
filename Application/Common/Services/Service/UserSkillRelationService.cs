using Application.Common.Services.Interface;
using Application.Common.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Common.Services.Service
{
    public class UserSkillRelationService : IUserSkillRelationService
    {
        private readonly IAppDbContext _context;

        public UserSkillRelationService(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AreValidSkillIdsAsync(
            IReadOnlyCollection<Guid> skillIds,
            CancellationToken cancellationToken)
        {
            var distinctIds = skillIds.Distinct().ToList();
            if (distinctIds.Count == 0)
            {
                return true;
            }

            var existingCount = await _context.LKP_Skill.CountAsync(
                skill => distinctIds.Contains(skill.ID),
                cancellationToken);
            return existingCount == distinctIds.Count;
        }

        public async Task<List<TJoin>> CreateUserSkillRelationsAsync<TJoin>(
            List<Guid> skillIds,
            Guid userId,
            Guid parentEntityId,
            Expression<Func<UserSkill, ICollection<TJoin>>> joinCollectionSelector,
            Expression<Func<TJoin, Guid>> getParentIdExpr,
            Action<TJoin, Guid> setParentIdAction,
            CancellationToken cancellationToken
        )
            where TJoin : class, new()
        {
            var distinctSkillIds = skillIds.Distinct().ToList();
            if (distinctSkillIds.Count == 0)
            {
                return [];
            }

            var existingSkills = await _context.UserSkill
                .Where(skill => skill.UserID == userId && distinctSkillIds.Contains(skill.LKP_SkillID))
                .Include(joinCollectionSelector)
                .ToDictionaryAsync(skill => skill.LKP_SkillID, cancellationToken);

            var userSkills = new List<TJoin>(distinctSkillIds.Count);
            var joinCollectionAccessor = joinCollectionSelector.Compile();
            var parentIdAccessor = getParentIdExpr.Compile();

            foreach (var skillId in distinctSkillIds)
            {
                if (existingSkills.TryGetValue(skillId, out var existingSkill))
                {
                    var joinCollection = joinCollectionAccessor(existingSkill);
                    var existingJoin = joinCollection.FirstOrDefault(join => parentIdAccessor(join) == parentEntityId);

                    if (existingJoin != null)
                    {
                        userSkills.Add(existingJoin);
                        continue;
                    }

                    var link = new TJoin();
                    setParentIdAction(link, parentEntityId);
                    joinCollection.Add(link);
                    userSkills.Add(link);
                }
                else
                {
                    var newSkill = new UserSkill
                    {
                        UserID = userId,
                        LKP_SkillID = skillId,
                    };

                    var joinCollection = joinCollectionAccessor(newSkill);
                    var newJoin = new TJoin();
                    setParentIdAction(newJoin, parentEntityId);
                    joinCollection.Add(newJoin);

                    _context.UserSkill.Add(newSkill);
                    userSkills.Add(newJoin);
                }
            }

            return userSkills;
        }

        public async Task UpdateUserSkillRelationsAsync<TEntity, TJoin>(
            TEntity parentEntity,
            List<Guid> newSkillIds,
            Guid userId,
            Expression<Func<TEntity, ICollection<TJoin>>> joinCollectionSelector,
            Expression<Func<TJoin, UserSkill>> userSkillSelector,
            Action<TJoin, UserSkill> setUserSkill,
            Expression<Func<TJoin, Guid>> joinSkillIdSelector,
            Func<Guid, Guid, TJoin> createJoinEntity,
            CancellationToken cancellationToken
        )
            where TEntity : class
            where TJoin : class
        {
            var joinCollection = joinCollectionSelector.Compile()(parentEntity);
            var skillIdAccessor = joinSkillIdSelector.Compile();
            var distinctSkillIds = newSkillIds.Distinct().ToList();

            var existingSkillIds = joinCollection
                .Select(skillIdAccessor)
                .ToHashSet();

            var newSkillIdSet = distinctSkillIds.ToHashSet();

            // Remove join entities no longer linked
            var toRemove = joinCollection
                .Where(join => !newSkillIdSet.Contains(skillIdAccessor(join)))
                .ToList();

            foreach (var item in toRemove)
            {
                joinCollection.Remove(item);
                _context.Set<TJoin>().Remove(item);
            }

            // Add new links for new skills
            var toAdd = distinctSkillIds.Except(existingSkillIds).ToList();
            if (toAdd.Count == 0)
            {
                return;
            }

            var existingUserSkills = await _context.UserSkill
                .Where(skill => skill.UserID == userId && toAdd.Contains(skill.LKP_SkillID))
                .ToDictionaryAsync(skill => skill.LKP_SkillID, cancellationToken);

            foreach (var skillId in toAdd)
            {
                if (!existingUserSkills.TryGetValue(skillId, out var existingUserSkill))
                {
                    existingUserSkill = new UserSkill
                    {
                        UserID = userId,
                        LKP_SkillID = skillId
                    };
                    _context.UserSkill.Add(existingUserSkill);
                }

                var joinEntity = createJoinEntity(skillId, userId);
                setUserSkill(joinEntity, existingUserSkill);
                joinCollection.Add(joinEntity);
            }
        }
    }
}
