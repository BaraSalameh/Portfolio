using Application.Common.Services.Interface;
using DataAccess.Interfaces;
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
            var userSkills = new List<TJoin>();

            foreach (var skillId in skillIds)
            {
                var existingSkill = await _context.UserSkill
                    .Include(joinCollectionSelector)
                    .FirstOrDefaultAsync(us => us.UserID == userId && us.LKP_SkillID == skillId, cancellationToken);

                if (existingSkill != null)
                {
                    var compiledParentIdGetter = getParentIdExpr.Compile();

                    var joinCollection = joinCollectionSelector.Compile().Invoke(existingSkill);
                    var existingJoin = joinCollection.FirstOrDefault(j => compiledParentIdGetter(j) == parentEntityId);

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

                    var joinCollection = joinCollectionSelector.Compile().Invoke(newSkill);
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
            var joinCollection = joinCollectionSelector.Compile().Invoke(parentEntity);

            // Get existing skill IDs from join entities
            var existingSkillIds = joinCollection
                .Select(joinSkillIdSelector.Compile())
                .ToHashSet();

            var newSkillIdSet = newSkillIds.ToHashSet();

            // Remove join entities no longer linked
            var toRemove = joinCollection
                .Where(j => !newSkillIdSet.Contains(joinSkillIdSelector.Compile()(j)))
                .ToList();

            foreach (var item in toRemove)
            {
                joinCollection.Remove(item);
                _context.Set<TJoin>().Remove(item);
            }

            // Add new links for new skills
            var toAdd = newSkillIds.Except(existingSkillIds);

            foreach (var skillId in toAdd)
            {
                // Try find existing UserSkill for user + skill
                var existingUserSkill = await _context.UserSkill
                    .FirstOrDefaultAsync(us => us.UserID == userId && us.LKP_SkillID == skillId, cancellationToken);

                if (existingUserSkill == null)
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
