using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.UserSkillCommands;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Owner.Handlers.UserSkillHandlers
{
    public class EditDeleteUserSkillCommandHandler : IRequestHandler<EditDeleteUserSkillCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;

        public EditDeleteUserSkillCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<CommandResponse> Handle(EditDeleteUserSkillCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            if (request.LstUserSkills == null)
            {
                response.lstError.Add("Skill list can't be null.");
                return response;
            }


            var existingEntity = await _context.User
                .Include(y => y.LstUserSkills).ThenInclude(us => us.LstEducations)
                .Include(y => y.LstUserSkills).ThenInclude(us => us.LstExperiences)
                .Include(y => y.LstUserSkills).ThenInclude(us => us.LstProjects)
                .Include(y => y.LstUserSkills).ThenInclude(us => us.LstCertificates)
                .AsSplitQuery()
                .FirstOrDefaultAsync(u => u.ID == _currentUser.UserID!.Value, cancellationToken);

            if (existingEntity == null)
            {
                response.lstError.Add("User not found.");
                return response;
            }

            var RequestedSkills = request.LstUserSkills.Select(x => x.LKP_SkillID).ToList();

            var LKP_SkillIDs = await _context.LKP_Skill
                .AsNoTracking()
                .Where(l => RequestedSkills.Contains(l.ID))
                .Select(l => l.ID)
                .ToListAsync(cancellationToken);

            if (RequestedSkills.Count != LKP_SkillIDs.Count)
            {
                response.lstError.Add("Wrong Skills Entry.");
                return response;
            }

            var userId = _currentUser.UserID!.Value;
            var educationIds = request.LstUserSkills
                .SelectMany(skill => skill.EducationIDs ?? [])
                .Distinct()
                .ToList();
            var experienceIds = request.LstUserSkills
                .SelectMany(skill => skill.ExperienceIDs ?? [])
                .Distinct()
                .ToList();
            var projectIds = request.LstUserSkills
                .SelectMany(skill => skill.ProjectIDs ?? [])
                .Distinct()
                .ToList();
            var certificateIds = request.LstUserSkills
                .SelectMany(skill => skill.CertificateIDs ?? [])
                .Distinct()
                .ToList();

            var ownedEducationCount = educationIds.Count == 0 ? 0 : await _context.Education.CountAsync(
                entity => entity.UserID == userId && educationIds.Contains(entity.ID),
                cancellationToken);
            var ownedExperienceCount = experienceIds.Count == 0 ? 0 : await _context.Experience.CountAsync(
                entity => entity.UserID == userId && experienceIds.Contains(entity.ID),
                cancellationToken);
            var ownedProjectCount = projectIds.Count == 0 ? 0 : await _context.Project.CountAsync(
                entity => entity.UserID == userId && projectIds.Contains(entity.ID),
                cancellationToken);
            var ownedCertificateCount = certificateIds.Count == 0 ? 0 : await _context.Certificate.CountAsync(
                entity => entity.UserID == userId && certificateIds.Contains(entity.ID),
                cancellationToken);

            if (ownedEducationCount != educationIds.Count ||
                ownedExperienceCount != experienceIds.Count ||
                ownedProjectCount != projectIds.Count ||
                ownedCertificateCount != certificateIds.Count)
            {
                response.lstError.Add("Skill relations must reference resources owned by the current user.");
                return response;
            }

            var requestedBySkill = request.LstUserSkills.ToDictionary(skill => skill.LKP_SkillID);
            var retainedSkillIds = requestedBySkill.Keys.ToHashSet();
            foreach (var removedSkill in existingEntity.LstUserSkills
                .Where(skill => !retainedSkillIds.Contains(skill.LKP_SkillID)))
            {
                removedSkill.IsDeleted = true;
            }

            foreach (var existingSkill in existingEntity.LstUserSkills
                .Where(skill => retainedSkillIds.Contains(skill.LKP_SkillID)))
            {
                var requested = requestedBySkill[existingSkill.LKP_SkillID];
                Reconcile(
                    existingSkill.LstEducations,
                    requested.EducationIDs ?? [],
                    relation => relation.EducationID,
                    id => new UserSkillEducation { UserSkillID = existingSkill.ID, EducationID = id });
                Reconcile(
                    existingSkill.LstExperiences,
                    requested.ExperienceIDs ?? [],
                    relation => relation.ExperienceID,
                    id => new UserSkillExperience { UserSkillID = existingSkill.ID, ExperienceID = id });
                Reconcile(
                    existingSkill.LstProjects,
                    requested.ProjectIDs ?? [],
                    relation => relation.ProjectID,
                    id => new UserSkillProject { UserSkillID = existingSkill.ID, ProjectID = id });
                Reconcile(
                    existingSkill.LstCertificates,
                    requested.CertificateIDs ?? [],
                    relation => relation.CertificateID,
                    id => new UserSkillCertificate { UserSkillID = existingSkill.ID, CertificateID = id });
                requestedBySkill.Remove(existingSkill.LKP_SkillID);
            }

            existingEntity.LstUserSkills.AddRange(requestedBySkill.Values.Select(requested =>
                new UserSkill
                {
                    UserID = userId,
                    LKP_SkillID = requested.LKP_SkillID,
                    LstEducations = (requested.EducationIDs ?? []).Distinct()
                        .Select(id => new UserSkillEducation { EducationID = id }).ToList(),
                    LstExperiences = (requested.ExperienceIDs ?? []).Distinct()
                        .Select(id => new UserSkillExperience { ExperienceID = id }).ToList(),
                    LstProjects = (requested.ProjectIDs ?? []).Distinct()
                        .Select(id => new UserSkillProject { ProjectID = id }).ToList(),
                    LstCertificates = (requested.CertificateIDs ?? []).Distinct()
                        .Select(id => new UserSkillCertificate { CertificateID = id }).ToList()
                }));

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }

        private void Reconcile<TRelation>(
            ICollection<TRelation> existing,
            IEnumerable<Guid> requestedIds,
            Func<TRelation, Guid> relationId,
            Func<Guid, TRelation> create)
            where TRelation : class
        {
            var requested = requestedIds.ToHashSet();
            var removed = existing.Where(relation => !requested.Contains(relationId(relation))).ToArray();
            _context.Set<TRelation>().RemoveRange(removed);

            var existingIds = existing.Select(relationId).ToHashSet();
            foreach (var id in requested.Except(existingIds))
            {
                existing.Add(create(id));
            }
        }
    }
}
