using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Common.Services.Interface
{
    public interface IUserSkillRelationService
    {
        Task<List<TJoin>> CreateUserSkillRelationsAsync<TJoin>(
            List<Guid> skillIds,
            Guid userId,
            Guid parentEntityId,
            Expression<Func<UserSkill, ICollection<TJoin>>> joinCollectionSelector,
            Expression<Func<TJoin, Guid>> getParentIdExpr,
            Action<TJoin, Guid> setParentIdAction,
            CancellationToken cancellationToken
        ) where TJoin : class, new();

        Task UpdateUserSkillRelationsAsync<TEntity, TJoin>(
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
            where TJoin : class;
    }
}
