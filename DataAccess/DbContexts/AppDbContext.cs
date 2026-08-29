using DataAccess.Configurations;
using Application.Common.Persistence;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Application.Common.Identity;
using Application.Common.Constants;

namespace DataAccess.DbContexts
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        private readonly Application.Common.Services.Interface.IDateTimeProvider? _dateTimeProvider;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            Application.Common.Services.Interface.IDateTimeProvider? dateTimeProvider = null) : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<PendingEmailConfirmation> PendingEmailConfirmation { get; set; }
        public DbSet<EmailOutboxMessage> EmailOutboxMessage { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserSkill> UserSkill { get; set; }
        public DbSet<UserSkillEducation> UserSkillEducation { get; set; }
        public DbSet<UserSkillExperience> UserSkillExperience { get; set; }
        public DbSet<UserSkillProject> UserSkillProject { get; set; }
        public DbSet<UserSkillCertificate> UserSkillCertificate { get; set; }
        public DbSet<LKP_Skill> LKP_Skill { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<LKP_Institution> LKP_Institution { get; set; }
        public DbSet<LKP_Degree> LKP_Degree { get; set; }
        public DbSet<LKP_FieldOfStudy> LKP_FieldOfStudy { get; set; }
        public DbSet<Experience> Experience { get; set; }
        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<BlogPostTag> BlogPostTag { get; set; }
        public DbSet<LKP_BlogPostStatus> LKP_BlogPostStatus { get; set; }
        public DbSet<SocialLink> SocialLink { get; set; }
        public DbSet<ContactMessage> ContactMessage { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<UserLanguage> UserLanguage { get; set; }
        public DbSet<LKP_Language> LKP_Language { get; set; }
        public DbSet<LKP_LanguageProficiency> LKP_LanguageProficiency { get; set; }
        public DbSet<LKP_Preference> LKP_Preference { get; set; }
        public DbSet<UserPreference> UserPreference { get; set; }
        public DbSet<LKP_Widget> LKP_Widget { get; set; }
        public DbSet<LKP_ChartType> LKP_ChartType { get; set; }
        public DbSet<UserChartPreference> UserChartPreference { get; set; }
        public DbSet<Certificate> Certificate { get; set; }
        public DbSet<CertificateMedia> CertificateMedia { get; set; }
        public DbSet<LKP_Certificate> LKP_Certificate { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = OnModelCreateKeys(modelBuilder);
            modelBuilder = OnModelCreateRelations(modelBuilder);

            // Physical cascade deletes are incompatible with the application's
            // soft-delete model. Required dependents must be removed or archived
            // intentionally by the owning use case.
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyPersistenceConventions();
            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task<TResult> ExecuteInTransactionAsync<TResult>(
            Func<CancellationToken, Task<TResult>> operation,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(operation);

            // A composed application service may already own the unit of work.
            // In that case participate in it; only the creator may commit or roll
            // back the transaction.
            if (Database.CurrentTransaction is not null)
            {
                return await operation(cancellationToken);
            }

            await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
            var result = await operation(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }

        internal void ApplyPersistenceConventions()
        {
            var now = _dateTimeProvider?.UtcNow ?? DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AbstractEntity>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.IsDeleted = false;
                    entry.Entity.DeletedAt = null;
                    continue;
                }

                if (entry.State != EntityState.Modified)
                {
                    continue;
                }

                entry.Entity.UpdatedAt = now;
                if (entry.Entity.IsDeleted)
                {
                    entry.Entity.DeletedAt = now;
                }
            }
        }

        private ModelBuilder OnModelCreateKeys(ModelBuilder modelBuilder)
        {

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var idProperty = entityType.FindProperty("ID");

                if (idProperty != null && idProperty.ClrType == typeof(Guid))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("ID")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("gen_random_uuid()");
                }
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (typeof(AbstractEntity).IsAssignableFrom(clrType))
                {
                    modelBuilder.Entity(clrType).Property(nameof(AbstractEntity.CreatedAt))
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    modelBuilder.Entity(clrType).Property(nameof(AbstractEntity.IsDeleted))
                        .HasDefaultValue(false);

                    // PostgreSQL's system xmin column provides migration-free
                    // optimistic concurrency for every mutable audited entity.
                    modelBuilder.Entity(clrType).Property<uint>("xmin").IsRowVersion();

                    var entityParameter = System.Linq.Expressions.Expression.Parameter(clrType, "entity");
                    var isDeleted = System.Linq.Expressions.Expression.Property(
                        entityParameter,
                        nameof(AbstractEntity.IsDeleted));
                    var activeEntityFilter = System.Linq.Expressions.Expression.Lambda(
                        System.Linq.Expressions.Expression.Equal(
                            isDeleted,
                            System.Linq.Expressions.Expression.Constant(false)),
                        entityParameter);
                    modelBuilder.Entity(clrType).HasQueryFilter(activeEntityFilter);
                }
            }

            // Required dependents mirror the active-record filter of their
            // principal. This makes soft-delete semantics explicit and avoids
            // EF producing different results depending on whether a required
            // navigation is included in the query.
            modelBuilder.Entity<User>()
                .HasQueryFilter(user => !user.Role.IsDeleted);
            modelBuilder.Entity<PendingEmailConfirmation>()
                .HasQueryFilter(confirmation => !confirmation.User.Role.IsDeleted);
            modelBuilder.Entity<RefreshToken>()
                .HasQueryFilter(token => !token.User.Role.IsDeleted);
            modelBuilder.Entity<BlogPostTag>()
                .HasQueryFilter(relation => !relation.BlogPost.IsDeleted);
            modelBuilder.Entity<UserLanguage>()
                .HasQueryFilter(relation =>
                    !relation.LKP_Language.IsDeleted &&
                    !relation.LKP_LanguageProficiency!.IsDeleted);
            modelBuilder.Entity<UserSkillCertificate>()
                .HasQueryFilter(relation =>
                    !relation.UserSkill.IsDeleted &&
                    !relation.Certificate.IsDeleted);
            modelBuilder.Entity<UserSkillEducation>()
                .HasQueryFilter(relation =>
                    !relation.UserSkill.IsDeleted &&
                    !relation.Education.IsDeleted);
            modelBuilder.Entity<UserSkillExperience>()
                .HasQueryFilter(relation =>
                    !relation.UserSkill.IsDeleted &&
                    !relation.Experience.IsDeleted);
            modelBuilder.Entity<UserSkillProject>()
                .HasQueryFilter(relation =>
                    !relation.UserSkill.IsDeleted &&
                    !relation.Project.IsDeleted);

            // Converter for DateOnly ↔ DateTime
            var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
                d => d.ToDateTime(TimeOnly.MinValue),      // to provider
                d => DateOnly.FromDateTime(d)              // from provider
            );

            // Apply to all DateOnly properties and set column type to 'date'
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var entityBuilder = modelBuilder.Entity(entity.ClrType);

                foreach (var property in entity.GetProperties())
                {
                    if (property.ClrType == typeof(DateOnly))
                    {
                        entityBuilder
                            .Property(property.Name)
                            .HasConversion(dateOnlyConverter)
                            .HasColumnType("date");
                    }
                }
            }

            modelBuilder.Entity<RefreshToken>().HasIndex(token => token.Token).IsUnique();
            modelBuilder.Entity<RefreshToken>().HasIndex(token => new { token.UserID, token.IsRevoked });
            modelBuilder.Entity<RefreshToken>().HasIndex(token => token.ExpiresAt);
            modelBuilder.Entity<PendingEmailConfirmation>().HasIndex(p => p.TokenHash).IsUnique();
            modelBuilder.Entity<PendingEmailConfirmation>().HasIndex(p => p.ExpiresAt);
            modelBuilder.Entity<PendingEmailConfirmation>()
                .HasIndex(p => p.UserID)
                .IsUnique()
                .HasFilter("\"RevokedAt\" IS NULL");
            modelBuilder.Entity<EmailOutboxMessage>()
                .HasIndex(message => new { message.ProcessedAt, message.NextAttemptAt, message.LockedUntil });
            modelBuilder.Entity<EmailOutboxMessage>()
                .HasIndex(message => new { message.Kind, message.CreatedAt });
            modelBuilder.Entity<EmailOutboxMessage>()
                .HasIndex(message => new { message.Kind, message.AggregateID })
                .IsUnique()
                .HasFilter("\"ProcessedAt\" IS NULL");
            modelBuilder.Entity<BlogPostTag>().HasKey(pt => new { pt.BlogPostID, pt.TagId });
            modelBuilder.Entity<UserLanguage>().HasKey(pt => new { pt.UserID, pt.LKP_LanguageID });
            modelBuilder.Entity<UserSkillEducation>().HasKey(use => new { use.UserSkillID, use.EducationID });
            modelBuilder.Entity<UserSkillExperience>().HasKey(use => new { use.UserSkillID, use.ExperienceID });
            modelBuilder.Entity<UserSkillProject>().HasKey(use => new { use.UserSkillID, use.ProjectID });
            modelBuilder.Entity<UserSkillCertificate>().HasKey(use => new { use.UserSkillID, use.CertificateID });
            modelBuilder.Entity<UserPreference>().HasKey(pt => new { pt.UserID, pt.LKP_PreferenceID });
            modelBuilder.Entity<UserChartPreference>().HasKey(ucp => new { ucp.UserID, ucp.LKP_WidgetID, ucp.LKP_ChartTypeID });
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.Firstname).HasMaxLength(100);
            modelBuilder.Entity<User>().Property(x => x.Lastname).HasMaxLength(100);
            modelBuilder.Entity<User>().Property(x => x.Email).HasMaxLength(320);
            modelBuilder.Entity<User>().Property(x => x.Username).HasMaxLength(UsernameGenerator.MaxLength);
            modelBuilder.Entity<User>().Property(x => x.Password).HasMaxLength(1024);
            modelBuilder.Entity<User>().Property(x => x.Title).HasMaxLength(200);
            modelBuilder.Entity<User>().Property(x => x.Bio).HasMaxLength(5000);
            modelBuilder.Entity<User>().Property(x => x.Phone).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.ProfilePicture).HasMaxLength(2048);
            modelBuilder.Entity<User>().Property(x => x.CoverPhoto).HasMaxLength(2048);

            modelBuilder.Entity<Project>().Property(x => x.Title).HasMaxLength(200);
            modelBuilder.Entity<Project>().Property(x => x.Description).HasMaxLength(5000);
            modelBuilder.Entity<Project>().Property(x => x.LiveLink).HasMaxLength(2048);
            modelBuilder.Entity<Project>().Property(x => x.SourceCode).HasMaxLength(2048);
            modelBuilder.Entity<Project>().Property(x => x.ImageUrl).HasMaxLength(2048);
            modelBuilder.Entity<Experience>().Property(x => x.JobTitle).HasMaxLength(200);
            modelBuilder.Entity<Experience>().Property(x => x.CompanyName).HasMaxLength(200);
            modelBuilder.Entity<Experience>().Property(x => x.Location).HasMaxLength(300);
            modelBuilder.Entity<Experience>().Property(x => x.Description).HasMaxLength(5000);
            modelBuilder.Entity<Education>().Property(x => x.Description).HasMaxLength(5000);
            modelBuilder.Entity<Certificate>().Property(x => x.CredintialID).HasMaxLength(300);
            modelBuilder.Entity<Certificate>().Property(x => x.CredintialUrl).HasMaxLength(2048);
            modelBuilder.Entity<CertificateMedia>().Property(x => x.Url).HasMaxLength(2048);
            modelBuilder.Entity<SocialLink>().Property(x => x.Platform).HasMaxLength(100);
            modelBuilder.Entity<SocialLink>().Property(x => x.Url).HasMaxLength(2048);
            modelBuilder.Entity<SocialLink>().Property(x => x.Icon).HasMaxLength(2048);
            modelBuilder.Entity<BlogPost>().Property(x => x.Title).HasMaxLength(200);
            modelBuilder.Entity<BlogPost>().Property(x => x.Slug).HasMaxLength(200);
            modelBuilder.Entity<BlogPost>().Property(x => x.Content).HasMaxLength(100000);
            modelBuilder.Entity<BlogPost>().Property(x => x.Thumbnail).HasMaxLength(2048);
            modelBuilder.Entity<BlogPost>().Property(x => x.Excerpt).HasMaxLength(5000);
            modelBuilder.Entity<ContactMessage>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<ContactMessage>().Property(x => x.Email).HasMaxLength(320);
            modelBuilder.Entity<ContactMessage>().Property(x => x.Subject).HasMaxLength(200);
            modelBuilder.Entity<ContactMessage>().Property(x => x.Message).HasMaxLength(5000);
            modelBuilder.Entity<UserPreference>().Property(x => x.Value).HasMaxLength(1000);
            modelBuilder.Entity<UserChartPreference>().Property(x => x.GroupBy).HasMaxLength(100);
            modelBuilder.Entity<UserChartPreference>().Property(x => x.ValueSource).HasMaxLength(200);

            modelBuilder.Entity<Role>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<Tag>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<LKP_Preference>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<LKP_Widget>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<LKP_ChartType>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<LKP_Certificate>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<LKP_Language>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<LKP_LanguageProficiency>().Property(x => x.Level).HasMaxLength(100);
            modelBuilder.Entity<LKP_Degree>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<LKP_Degree>().Property(x => x.Abbreviation).HasMaxLength(100);
            modelBuilder.Entity<LKP_FieldOfStudy>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<LKP_Institution>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<LKP_Institution>().Property(x => x.Logo).HasMaxLength(2048);
            modelBuilder.Entity<LKP_Skill>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<LKP_Skill>().Property(x => x.IconUrl).HasMaxLength(2048);
            modelBuilder.Entity<LKP_BlogPostStatus>().Property(x => x.Name).HasMaxLength(100);

            modelBuilder.Entity<RefreshToken>().Property(x => x.Token).HasMaxLength(64);
            modelBuilder.Entity<RefreshToken>().Property(x => x.CreatedByIp).HasMaxLength(45);
            modelBuilder.Entity<PendingEmailConfirmation>().Property(x => x.TokenHash).HasMaxLength(64);
            modelBuilder.Entity<EmailOutboxMessage>().Property(x => x.LastError).HasMaxLength(2000);

            modelBuilder.Entity<User>().ToTable(table => table.HasCheckConstraint(
                "CK_User_Gender",
                "\"Gender\" IS NULL OR \"Gender\" BETWEEN 0 AND 2"));
            modelBuilder.Entity<Project>().ToTable(table => table.HasCheckConstraint(
                "CK_Project_Order",
                "\"Order\" >= 0"));
            modelBuilder.Entity<Education>().ToTable(table =>
            {
                table.HasCheckConstraint("CK_Education_Order", "\"Order\" >= 0");
                table.HasCheckConstraint(
                    "CK_Education_DateRange",
                    "\"EndDate\" IS NULL OR \"EndDate\" >= \"StartDate\"");
            });
            modelBuilder.Entity<Experience>().ToTable(table =>
            {
                table.HasCheckConstraint("CK_Experience_Order", "\"Order\" >= 0");
                table.HasCheckConstraint(
                    "CK_Experience_DateRange",
                    "\"EndDate\" IS NULL OR \"EndDate\" >= \"StartDate\"");
            });
            modelBuilder.Entity<Certificate>().ToTable(table =>
            {
                table.HasCheckConstraint("CK_Certificate_Order", "\"Order\" >= 0");
                table.HasCheckConstraint(
                    "CK_Certificate_DateRange",
                    "\"IssueDate\" IS NULL OR \"ExpirationDate\" IS NULL OR \"ExpirationDate\" >= \"IssueDate\"");
            });
            modelBuilder.Entity<EmailOutboxMessage>().ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_EmailOutboxMessage_Kind",
                    "\"Kind\" IN (1, 2)");
                table.HasCheckConstraint(
                    "CK_EmailOutboxMessage_AttemptCount",
                    $"\"AttemptCount\" BETWEEN 0 AND {EmailOutboxPolicy.MaximumAttempts}");
                table.HasCheckConstraint(
                    "CK_EmailOutboxMessage_LeasePair",
                    "(\"LockID\" IS NULL) = (\"LockedUntil\" IS NULL)");
                table.HasCheckConstraint(
                    "CK_EmailOutboxMessage_ProcessedLease",
                    "\"ProcessedAt\" IS NULL OR \"LockID\" IS NULL");
            });
            modelBuilder.Entity<CertificateMedia>().HasIndex(x => x.CertificateID);
            modelBuilder.Entity<CertificateMedia>()
                .HasIndex(x => new { x.CertificateID, x.Url })
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");
            modelBuilder.Entity<User>().HasIndex(x => new { x.IsConfirmed, x.CreatedAt, x.ID });
            modelBuilder.Entity<Project>().HasIndex(x => new { x.UserID, x.IsDeleted, x.Order, x.ID });
            modelBuilder.Entity<Education>().HasIndex(x => new { x.UserID, x.IsDeleted, x.Order, x.ID });
            modelBuilder.Entity<Experience>().HasIndex(x => new { x.UserID, x.IsDeleted, x.Order, x.ID });
            modelBuilder.Entity<Certificate>().HasIndex(x => new { x.UserID, x.IsDeleted, x.Order, x.ID });
            modelBuilder.Entity<ContactMessage>().HasIndex(x => new { x.UserID, x.IsDeleted, x.CreatedAt });
            modelBuilder.Entity<ContactMessage>()
                .HasIndex(x => new { x.UserID, x.Email, x.CreatedAt })
                .HasDatabaseName("IX_ContactMessage_SubmissionCooldown")
                .HasFilter("\"IsDeleted\" = false");
            modelBuilder.Entity<BlogPost>().HasIndex(x => new { x.UserID, x.IsDeleted, x.CreatedAt });
            modelBuilder.Entity<BlogPost>().HasIndex(x => new
            {
                x.UserID,
                x.LKP_BlogPostStatusID,
                x.IsDeleted,
                x.PublishedAt,
                x.ID
            }).HasDatabaseName("IX_BlogPost_PublicVisibility");
            modelBuilder.Entity<BlogPost>()
                .HasIndex(x => new { x.UserID, x.Slug })
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");
            modelBuilder.Entity<SocialLink>().HasIndex(x => new { x.UserID, x.IsDeleted });
            modelBuilder.Entity<UserSkill>()
                .HasIndex(x => new { x.UserID, x.LKP_SkillID })
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");
            modelBuilder.Entity<User>().Property(x => x.IsConfirmed).HasDefaultValue(false);
            modelBuilder.Entity<LKP_Language>()
                .HasIndex(x => x.Name)
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");
            modelBuilder.Entity<LKP_LanguageProficiency>()
                .HasIndex(x => x.Level)
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");

            modelBuilder.ApplyConfiguration(new RoleSeedConfiguration());
            modelBuilder.ApplyConfiguration(new InstitutionSeedConfiguration());
            modelBuilder.ApplyConfiguration(new DegreeSeedConfiguration());
            modelBuilder.ApplyConfiguration(new FieldOfStudySeedConfiguration());
            modelBuilder.ApplyConfiguration(new BlogPostStatusSeedConfiguration());
            modelBuilder.ApplyConfiguration(new PreferencesSeedConfiguration());
            modelBuilder.ApplyConfiguration(new WidgetSeedConfiguration());
            modelBuilder.ApplyConfiguration(new ChartTypeSeedConfiguration());
            modelBuilder.ApplyConfiguration(new SkillSeedConfiguration());
            modelBuilder.ApplyConfiguration(new CertificateSeedConfiguration());

            return modelBuilder;
        }

        private ModelBuilder OnModelCreateRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .HasOne(p => p.Role)
               .WithMany(u => u.LstUsers)
               .HasForeignKey(p => p.RoleID);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(x => x.User)
                .WithMany(x => x.LstRefreshTokens)
                .HasForeignKey(x => x.UserID);

            modelBuilder.Entity<PendingEmailConfirmation>()
                .HasOne(x => x.User)
                .WithMany(x => x.LstPendingEmailConfirmations)
                .HasForeignKey(x => x.UserID);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.LstProjects)
                .HasForeignKey(p => p.UserID);
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Education)
                .WithMany(u => u.LstProjects)
                .HasForeignKey(p => p.EducationID);
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Experience)
                .WithMany(u => u.LstProjects)
                .HasForeignKey(p => p.ExperienceID);

            modelBuilder.Entity<UserSkill>()
                .HasOne(s => s.User)
                .WithMany(u => u.LstUserSkills)
                .HasForeignKey(s => s.UserID);
            modelBuilder.Entity<UserSkill>()
                .HasOne(p => p.LKP_Skill)
                .WithMany(u => u.LstSkillUsers)
                .HasForeignKey(p => p.LKP_SkillID);

            modelBuilder.Entity<UserSkillEducation>()
                .HasOne(usp => usp.UserSkill)
                .WithMany(us => us.LstEducations)
                .HasForeignKey(usp => usp.UserSkillID)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<UserSkillEducation>()
                .HasOne(usp => usp.Education)
                .WithMany(p => p.LstUserSkillEducations)
                .HasForeignKey(usp => usp.EducationID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSkillExperience>()
                .HasOne(usp => usp.UserSkill)
                .WithMany(us => us.LstExperiences)
                .HasForeignKey(usp => usp.UserSkillID)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<UserSkillExperience>()
                .HasOne(usp => usp.Experience)
                .WithMany(p => p.LstUserSkillExperiences)
                .HasForeignKey(usp => usp.ExperienceID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSkillProject>()
                .HasOne(usp => usp.UserSkill)
                .WithMany(us => us.LstProjects)
                .HasForeignKey(usp => usp.UserSkillID)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<UserSkillProject>()
                .HasOne(usp => usp.Project)
                .WithMany(p => p.LstUserSkillProjects)
                .HasForeignKey(usp => usp.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSkillCertificate>()
                .HasOne(usc => usc.UserSkill)
                .WithMany(us => us.LstCertificates)
                .HasForeignKey(usc => usc.UserSkillID)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<UserSkillCertificate>()
                .HasOne(usc => usc.Certificate)
                .WithMany(us => us.LstUserSkillCertificates)
                .HasForeignKey(usc => usc.CertificateID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Education>()
                .HasOne(e => e.User)
                .WithMany(u => u.LstEducations)
                .HasForeignKey(e => e.UserID);
            modelBuilder.Entity<Education>()
                .HasOne(e => e.LKP_Institution)
                .WithMany(u => u.LstEducations)
                .HasForeignKey(e => e.LKP_InstitutionID);
            modelBuilder.Entity<Education>()
                .HasOne(e => e.LKP_Degree)
                .WithMany(u => u.LstEducations)
                .HasForeignKey(e => e.LKP_DegreeID);
            modelBuilder.Entity<Education>()
                .HasOne(e => e.LKP_FieldOfStudy)
                .WithMany(u => u.LstEducations)
                .HasForeignKey(e => e.LKP_FieldOfStudyID);

            modelBuilder.Entity<Experience>()
                .HasOne(e => e.User)
                .WithMany(u => u.LstExperiences)
                .HasForeignKey(e => e.UserID);

            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.User)
                .WithMany(u => u.LstBlogPosts)
                .HasForeignKey(b => b.UserID);
            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.LKP_BlogPostStatus)
                .WithMany(u => u.LstBlogPosts)
                .HasForeignKey(b => b.LKP_BlogPostStatusID);

            modelBuilder.Entity<BlogPostTag>()
                .HasOne(b => b.BlogPost)
                .WithMany(u => u.LstBlogPostTags)
                .HasForeignKey(b => b.BlogPostID);
            modelBuilder.Entity<BlogPostTag>()
                .HasOne(b => b.Tag)
                .WithMany(u => u.LstBlogPostTags)
                .HasForeignKey(b => b.TagId);

            modelBuilder.Entity<SocialLink>()
                .HasOne(s => s.User)
                .WithMany(u => u.LstSocialLinks)
                .HasForeignKey(s => s.UserID);

            modelBuilder.Entity<ContactMessage>()
                .HasOne(c => c.User)
                .WithMany(u => u.LstContactMessages)
                .HasForeignKey(c => c.UserID);

            modelBuilder.Entity<UserLanguage>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.LstUserLanguages)
                .HasForeignKey(pt => pt.UserID);
            modelBuilder.Entity<UserLanguage>()
                .HasOne(pt => pt.LKP_Language)
                .WithMany(t => t.LstLanguageUsers)
                .HasForeignKey(pt => pt.LKP_LanguageID);
            modelBuilder.Entity<UserLanguage>()
                .HasOne(pt => pt.LKP_LanguageProficiency)
                .WithMany(t => t.LstUsersAndLanguages)
                .HasForeignKey(pt => pt.LKP_LanguageProficiencyID);

            modelBuilder.Entity<UserPreference>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.LstUserPreferences)
                .HasForeignKey(pt => pt.UserID);
            modelBuilder.Entity<UserPreference>()
                .HasOne(pt => pt.LKP_Preference)
                .WithMany(t => t.LstPreferenceUsers)
                .HasForeignKey(pt => pt.LKP_PreferenceID);

            modelBuilder.Entity<UserChartPreference>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.LstUserChartPreferences)
                .HasForeignKey(pt => pt.UserID);
            modelBuilder.Entity<UserChartPreference>()
                .HasOne(pt => pt.LKP_Widget)
                .WithMany(t => t.LstWidgetPreferences)
                .HasForeignKey(pt => pt.LKP_WidgetID);
            modelBuilder.Entity<UserChartPreference>()
                .HasOne(pt => pt.LKP_ChartType)
                .WithMany(t => t.LstChartPreferences)
                .HasForeignKey(pt => pt.LKP_ChartTypeID);

            modelBuilder.Entity<Certificate>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.LstCertificates)
                .HasForeignKey(pt => pt.UserID);
            modelBuilder.Entity<Certificate>()
                .HasOne(pt => pt.LKP_Certificate)
                .WithMany(p => p.LstCertificates)
                .HasForeignKey(pt => pt.LKP_CertificateID);

            modelBuilder.Entity<CertificateMedia>()
                .HasOne(pt => pt.Certificate)
                .WithMany(p => p.LstCertificateMedias)
                .HasForeignKey(pt => pt.CertificateID);

            return modelBuilder;
        }
    }
}
