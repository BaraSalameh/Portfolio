using Npgsql;
using DataAccess;
using Application.Client.MappingProfiles;
using Application.Client;
using Application.Client.Commands;
using Application.Client.Handlers;
using Application.Admin.Commands.LKP_PreferenceCommands;
using Application.Admin.Handlers.LKP_PreferenceHandlers;
using Application.Owner.Commands.UserPreferenceCommands;
using Application.Owner.Commands.UserChartPreferenceCommands;
using Application.Owner.Handlers.UserPreferenceHandlers;
using Application.Owner.Handlers.UserChartPreferenceHandlers;
using Application.Client.Queries;
using Application.Common.Services.Interface;
using Application.Common.Services.Service;
using Application.Owner.Commands.UserSkillCommands;
using Application.Owner.Commands.ProjectCommands;
using Application.Owner.Commands.BlogPostCommands;
using Application.Owner.Handlers.BlogPostHandlers;
using Application.Owner.Handlers.ProjectHandlers;
using Application.Owner.Handlers.UserSkillHandlers;
using Application.Owner.Handlers.UserHandlers;
using Application.Owner.Queries.UserQueries;
using Application.Owner.MappingProfiles;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataAccess.DbContexts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Domain.Enums;
using System.Data.Common;
using Application.Common.Constants;
using DataAccess.Services;
using Application.Account.Commands;
using Application.Account.Handlers;
using Application.Admin.Commands.RoleCommands;
using Application.Admin.Handlers.RoleHandlers;
using Application.Common.Identity;
using Application.Owner.Commands.UserLanguageCommands;
using Application.Owner.Handlers.UserLanguageHandlers;
using Application.Owner.Commands.CertificaeCommands;
using Application.Owner.Handlers.CertificateHandlers;
using Application.Account.Queries;

namespace Portfolio.IntegrationTests;

public sealed class PostgreSqlConnectivityTests
{
    [Fact]
    public void MultipleSiblingCollectionIncludes_RequireAnExplicitSplitQuery()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var unsafeQuery = context.Certificate
            .Include(certificate => certificate.LstCertificateMedias)
            .Include(certificate => certificate.LstUserSkillCertificates);

        Assert.Throws<InvalidOperationException>(() => unsafeQuery.ToQueryString());

        var splitSql = unsafeQuery.AsSplitQuery().ToQueryString();
        Assert.Contains("SELECT", splitSql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void UserIdentityFields_AreBoundedToPublicContractLimits()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var user = context.Model.FindEntityType(typeof(User));

        Assert.NotNull(user);
        Assert.Equal(100, user.FindProperty(nameof(User.Firstname))?.GetMaxLength());
        Assert.Equal(100, user.FindProperty(nameof(User.Lastname))?.GetMaxLength());
        Assert.Equal(UsernameGenerator.MaxLength, user.FindProperty(nameof(User.Username))?.GetMaxLength());
        Assert.Equal(320, user.FindProperty(nameof(User.Email))?.GetMaxLength());
        Assert.Equal(typeof(Guid), typeof(User).GetProperty(nameof(User.ID))?.PropertyType);
    }

    [Fact]
    public void MutableTextColumns_MirrorApiAndInternalLengthBoundaries()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var boundaries = new (Type Entity, string Property, int Maximum)[]
        {
            (typeof(User), nameof(User.Password), 1024),
            (typeof(User), nameof(User.Bio), 5000),
            (typeof(User), nameof(User.ProfilePicture), 2048),
            (typeof(Project), nameof(Project.Description), 5000),
            (typeof(Project), nameof(Project.LiveLink), 2048),
            (typeof(Experience), nameof(Experience.Location), 300),
            (typeof(ContactMessage), nameof(ContactMessage.Email), 320),
            (typeof(ContactMessage), nameof(ContactMessage.Message), 5000),
            (typeof(BlogPost), nameof(BlogPost.Content), 100000),
            (typeof(UserPreference), nameof(UserPreference.Value), 1000),
            (typeof(RefreshToken), nameof(RefreshToken.Token), 64),
            (typeof(PendingEmailConfirmation), nameof(PendingEmailConfirmation.TokenHash), 64),
            (typeof(EmailOutboxMessage), nameof(EmailOutboxMessage.LastError), 2000)
        };

        Assert.All(boundaries, boundary => Assert.Equal(
            boundary.Maximum,
            context.Model.FindEntityType(boundary.Entity)?
                .FindProperty(boundary.Property)?
                .GetMaxLength()));
    }

    [Fact]
    public void MutableTextBoundaryMigration_FailsBeforeImplicitTruncation()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var script = context.GetService<IMigrator>().GenerateScript(
            "20260825154611_EnforceActiveCertificateMediaUniqueness",
            "20260826185703_BoundMutableTextColumns");

        Assert.Contains("correct existing rows before migration", script, StringComparison.Ordinal);
        Assert.Contains("length(%I) > $1", script, StringComparison.Ordinal);
        Assert.Contains("('User', 'Password', 1024)", script, StringComparison.Ordinal);
        Assert.Contains("('BlogPost', 'Content', 100000)", script, StringComparison.Ordinal);
        Assert.Contains("('RefreshToken', 'Token', 64)", script, StringComparison.Ordinal);
        Assert.Contains("character varying(100000)", script, StringComparison.Ordinal);
        Assert.Contains("SET LOCAL lock_timeout = '5s'", script, StringComparison.Ordinal);
        Assert.Contains("SET LOCAL statement_timeout = '5min'", script, StringComparison.Ordinal);
        Assert.True(
            script.IndexOf("SET LOCAL lock_timeout", StringComparison.Ordinal) <
            script.IndexOf("correct existing rows before migration", StringComparison.Ordinal));
        Assert.True(
            script.IndexOf("correct existing rows before migration", StringComparison.Ordinal) <
            script.IndexOf("ALTER TABLE \"UserPreference\"", StringComparison.Ordinal));
    }

    [Fact]
    public void MutableTextBoundaryRollback_OnlyWidensColumnsBackToText()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var script = context.GetService<IMigrator>().GenerateScript(
            "20260826185703_BoundMutableTextColumns",
            "20260825154611_EnforceActiveCertificateMediaUniqueness");

        Assert.Contains("ALTER TABLE \"User\" ALTER COLUMN \"Password\" TYPE text", script, StringComparison.Ordinal);
        Assert.Contains("ALTER TABLE \"BlogPost\" ALTER COLUMN \"Content\" TYPE text", script, StringComparison.Ordinal);
        Assert.All(
            script.Split('\n').Where(line => line.Contains("DELETE FROM", StringComparison.OrdinalIgnoreCase)),
            line => Assert.Contains("__EFMigrationsHistory", line, StringComparison.Ordinal));
        Assert.DoesNotContain("UPDATE ", script, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("TRUNCATE ", script, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("DROP TABLE", script, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ImpossibleStateCheckConstraints_AreRepresentedInTheEfModel()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var model = context.GetService<IDesignTimeModel>().Model;
        var expected = new Dictionary<Type, string[]>()
        {
            [typeof(User)] = ["CK_User_Gender"],
            [typeof(Project)] = ["CK_Project_Order"],
            [typeof(Education)] = ["CK_Education_Order", "CK_Education_DateRange"],
            [typeof(Experience)] = ["CK_Experience_Order", "CK_Experience_DateRange"],
            [typeof(Certificate)] = ["CK_Certificate_Order", "CK_Certificate_DateRange"],
            [typeof(EmailOutboxMessage)] =
            [
                "CK_EmailOutboxMessage_AttemptCount",
                "CK_EmailOutboxMessage_Kind",
                "CK_EmailOutboxMessage_LeasePair",
                "CK_EmailOutboxMessage_ProcessedLease"
            ]
        };

        Assert.All(expected, pair =>
        {
            var names = model.FindEntityType(pair.Key)!
                .GetCheckConstraints()
                .Select(constraint => constraint.Name)
                .ToHashSet(StringComparer.Ordinal);
            Assert.All(pair.Value, name => Assert.Contains(name, names));
        });
    }

    [Fact]
    public void ImpossibleStateMigration_UsesOnlineValidationAndBoundedLocks()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var script = context.GetService<IMigrator>().GenerateScript(
            "20260826185703_BoundMutableTextColumns",
            "20260826190513_EnforceStateInvariants");

        Assert.Contains("SET LOCAL lock_timeout = '5s'", script, StringComparison.Ordinal);
        Assert.Contains("NOT VALID", script, StringComparison.Ordinal);
        Assert.Contains("VALIDATE CONSTRAINT \"CK_User_Gender\"", script, StringComparison.Ordinal);
        Assert.Contains("VALIDATE CONSTRAINT \"CK_EmailOutboxMessage_LeasePair\"", script, StringComparison.Ordinal);
        Assert.True(
            script.IndexOf("NOT VALID", StringComparison.Ordinal) <
            script.IndexOf("VALIDATE CONSTRAINT", StringComparison.Ordinal));
    }

    [Fact]
    public async Task PostgreSql_RejectsImpossibleGenderState()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "invalid-gender");
        user.Gender = 99;
        context.AddRange(role, user);

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());

        Assert.Equal(
            PostgresErrorCodes.CheckViolation,
            Assert.IsType<PostgresException>(exception.InnerException).SqlState);
        await transaction.RollbackAsync();
    }

    [Fact]
    public void OwnerCollectionOrderingMigration_IsInitializedBeforeAtomicInsertTriggers()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var script = context.GetService<IMigrator>().GenerateScript(
            "20260826190513_EnforceStateInvariants",
            "20260826191128_AppendOwnerCollectionOrdering");

        Assert.Contains("SET LOCAL lock_timeout = '5s'", script, StringComparison.Ordinal);
        Assert.Contains("CREATE TABLE \"OwnerCollectionOrder\"", script, StringComparison.Ordinal);
        Assert.Contains("ON CONFLICT (\"UserID\", \"Collection\") DO UPDATE", script, StringComparison.Ordinal);
        Assert.Contains("GREATEST(", script, StringComparison.Ordinal);
        Assert.Contains("CREATE FUNCTION \"AssignOwnerCollectionOrder\"", script, StringComparison.Ordinal);
        Assert.Contains("TR_Project_AssignOwnerCollectionOrder", script, StringComparison.Ordinal);
        Assert.Contains("TR_Education_AssignOwnerCollectionOrder", script, StringComparison.Ordinal);
        Assert.Contains("TR_Experience_AssignOwnerCollectionOrder", script, StringComparison.Ordinal);
        Assert.Contains("TR_Certificate_AssignOwnerCollectionOrder", script, StringComparison.Ordinal);
        Assert.True(
            script.IndexOf("SELECT \"UserID\", 'Project'", StringComparison.Ordinal) <
            script.IndexOf("TR_Project_AssignOwnerCollectionOrder", StringComparison.Ordinal));
    }

    [Fact]
    public async Task OwnerCollectionInsert_AppendsSequentialProjectsInPostgreSql()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "append-sequential");
        context.AddRange(role, user);
        await context.SaveChangesAsync();

        context.Project.Add(new Project { Title = "Imported", UserID = user.ID, Order = 7 });
        await context.SaveChangesAsync();
        context.Project.Add(new Project { Title = "First append", UserID = user.ID });
        await context.SaveChangesAsync();
        context.Project.Add(new Project { Title = "Second append", UserID = user.ID });
        await context.SaveChangesAsync();

        var orders = await context.Project.AsNoTracking()
            .Where(project => project.UserID == user.ID)
            .OrderBy(project => project.Order)
            .Select(project => project.Order)
            .ToListAsync();
        Assert.Equal([7, 8, 9], orders);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task OwnerCollectionInsert_SerializesConcurrentProjectAppendsInPostgreSql()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        Guid roleId;
        Guid userId;
        await using (var seedContext = CreateContext(connectionString))
        {
            await seedContext.Database.MigrateAsync();
            var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
            var user = CreateUser(role, "append-concurrent");
            seedContext.AddRange(role, user);
            await seedContext.SaveChangesAsync();
            roleId = role.ID;
            userId = user.ID;
        }

        try
        {
            await using var firstContext = CreateContext(connectionString);
            await using var secondContext = CreateContext(connectionString);
            firstContext.Project.Add(new Project { Title = "Concurrent A", UserID = userId });
            secondContext.Project.Add(new Project { Title = "Concurrent B", UserID = userId });

            await Task.WhenAll(
                firstContext.SaveChangesAsync(),
                secondContext.SaveChangesAsync());

            await using var assertionContext = CreateContext(connectionString);
            var orders = await assertionContext.Project.AsNoTracking()
                .Where(project => project.UserID == userId)
                .OrderBy(project => project.Order)
                .Select(project => project.Order)
                .ToListAsync();
            Assert.Equal([1, 2], orders);
        }
        finally
        {
            await using var cleanupContext = CreateContext(connectionString);
            await cleanupContext.Database.ExecuteSqlInterpolatedAsync(
                $"DELETE FROM \"Project\" WHERE \"UserID\" = {userId}");
            await cleanupContext.Database.ExecuteSqlInterpolatedAsync(
                $"DELETE FROM \"OwnerCollectionOrder\" WHERE \"UserID\" = {userId}");
            await cleanupContext.Database.ExecuteSqlInterpolatedAsync(
                $"DELETE FROM \"User\" WHERE \"ID\" = {userId}");
            await cleanupContext.Database.ExecuteSqlInterpolatedAsync(
                $"DELETE FROM \"Role\" WHERE \"ID\" = {roleId}");
        }
    }

    [Fact]
    public void UserPrimaryKeyNullabilityMigration_IsMetadataOnly()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var script = context.GetService<IMigrator>().GenerateScript(
            "20260825000426_BoundUserIdentityFields",
            "20260825005629_MakeUserPrimaryKeyNonNullable");

        Assert.Contains("MakeUserPrimaryKeyNonNullable", script, StringComparison.Ordinal);
        Assert.DoesNotContain("ALTER TABLE", script, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("DROP DEFAULT", script, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CertificateMediaUrlMigration_RejectsOversizedLegacyValues()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var script = context.GetService<IMigrator>().GenerateScript(
            "20260825005629_MakeUserPrimaryKeyNonNullable",
            "20260825010013_BoundCertificateMediaUrls");

        Assert.Contains("CertificateMedia.Url values exceed", script, StringComparison.Ordinal);
        Assert.Contains("character varying(2048)", script, StringComparison.Ordinal);
    }

    [Fact]
    public void CertificateMediaModel_EnforcesUniqueActiveUrlsPerCertificate()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var media = context.Model.FindEntityType(typeof(CertificateMedia));

        Assert.NotNull(media);
        Assert.Contains(media.GetIndexes(), index =>
            index.IsUnique &&
            index.GetFilter() == "\"IsDeleted\" = false" &&
            index.Properties.Select(property => property.Name)
                .SequenceEqual([nameof(CertificateMedia.CertificateID), nameof(CertificateMedia.Url)]));
        Assert.Contains(media.GetIndexes(), index =>
            !index.IsUnique &&
            index.Properties.Select(property => property.Name)
                .SequenceEqual([nameof(CertificateMedia.CertificateID)]));
    }

    [Fact]
    public void CertificateMediaUniquenessMigration_RejectsActiveLegacyDuplicates()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var script = context.GetService<IMigrator>().GenerateScript(
            "20260825152433_IndexConfirmationResendCooldown",
            "20260825154611_EnforceActiveCertificateMediaUniqueness");

        Assert.Contains("Duplicate active certificate media URLs", script, StringComparison.Ordinal);
        Assert.Contains("CREATE UNIQUE INDEX", script, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("\"CertificateID\", \"Url\"", script, StringComparison.Ordinal);
        Assert.Contains("WHERE \"IsDeleted\" = false", script, StringComparison.Ordinal);
    }

    [Fact]
    public void ContactSubmissionCooldown_HasPredicateAlignedPartialIndex()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var entity = context.Model.FindEntityType(typeof(ContactMessage));

        Assert.NotNull(entity);
        Assert.Contains(entity.GetIndexes(), index =>
            index.GetDatabaseName() == "IX_ContactMessage_SubmissionCooldown" &&
            index.GetFilter() == "\"IsDeleted\" = false" &&
            index.Properties.Select(property => property.Name).SequenceEqual(
                [nameof(ContactMessage.UserID), nameof(ContactMessage.Email), nameof(ContactMessage.CreatedAt)]));
    }

    [Fact]
    public void ContactSubmissionCooldownMigration_HasBoundedLockAndPartialIndex()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var script = context.GetService<IMigrator>().GenerateScript(
            "20260826191128_AppendOwnerCollectionOrdering",
            "20260828184004_IndexContactSubmissionCooldown",
            MigrationsSqlGenerationOptions.Idempotent);

        Assert.Contains("SET LOCAL lock_timeout = '5s'", script, StringComparison.Ordinal);
        Assert.Contains("SET LOCAL statement_timeout = '5min'", script, StringComparison.Ordinal);
        Assert.Contains("CREATE INDEX \"IX_ContactMessage_SubmissionCooldown\"", script, StringComparison.Ordinal);
        Assert.Contains("(\"UserID\", \"Email\", \"CreatedAt\")", script, StringComparison.Ordinal);
        Assert.Contains("WHERE \"IsDeleted\" = false", script, StringComparison.Ordinal);
        var guardedOperation = script[script.IndexOf("SET LOCAL lock_timeout", StringComparison.Ordinal)..];
        Assert.True(
            guardedOperation.IndexOf("CREATE INDEX", StringComparison.Ordinal) <
            guardedOperation.IndexOf("END IF", StringComparison.Ordinal));
    }

    [Fact]
    public void BoundUserIdentityFieldsMigration_RejectsOversizedLegacyValues()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var script = context.GetService<IMigrator>().GenerateScript(
            "20260824235906_IndexPublicBlogVisibility",
            "20260825000426_BoundUserIdentityFields");

        Assert.Contains("values exceed the new production limits", script, StringComparison.Ordinal);
        Assert.Contains("character varying(100)", script, StringComparison.Ordinal);
    }

    [Fact]
    public void OptimisticConcurrencyMigrations_DoNotAttemptToCreatePostgreSqlSystemColumn()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var script = context.GetService<IMigrator>().GenerateScript(
            "20260824220420_RestrictPhysicalCascadeDeletes",
            "20260824222936_AddGlobalOptimisticConcurrency");

        Assert.Contains("AddOwnerOrderingConcurrency", script, StringComparison.Ordinal);
        Assert.Contains("AddGlobalOptimisticConcurrency", script, StringComparison.Ordinal);
        Assert.DoesNotContain("ADD \"xmin\"", script, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("DROP COLUMN \"xmin\"", script, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task SystemRoles_CannotBeModifiedOrDeleted()
    {
        await using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var editHandler = new AddEditRoleCommandHandler(context, null!);
        var deleteHandler = new DeleteRoleCommandHandler(context);

        var edit = await editHandler.Handle(
            new AddEditRoleCommand { ID = RoleIdentifiers.Owner, Name = "Changed" },
            CancellationToken.None);
        var delete = await deleteHandler.Handle(
            new DeleteRoleCommand { ID = RoleIdentifiers.Admin },
            CancellationToken.None);

        Assert.Contains("System roles cannot be modified.", edit.lstError);
        Assert.Contains("System roles cannot be deleted.", delete.lstError);
    }

    [Fact]
    public void LegacyRefreshTokenMigration_HashesRawValuesBeforePlaintextFallbackRemoval()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var migrator = context.GetService<IMigrator>();
        var script = migrator.GenerateScript(
            "20260824224935_AddDurableEmailOutbox",
            "20260824230446_HashLegacyRefreshTokens");

        Assert.Contains("SHA256(CONVERT_TO", script, StringComparison.Ordinal);
        Assert.Contains("transformed duplicates exist", script, StringComparison.Ordinal);
        Assert.Contains("WHERE \"Token\" !~ '^[0-9A-F]{64}$'", script, StringComparison.Ordinal);
    }

    [Fact]
    public void EmailOutbox_HasIdempotencyAndDispatchIndexes()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var entity = context.Model.FindEntityType(typeof(EmailOutboxMessage));

        Assert.NotNull(entity);
        var indexes = entity.GetIndexes().ToList();
        Assert.Contains(indexes, index => index.IsUnique &&
            index.Properties.Select(property => property.Name).SequenceEqual(new[] { "Kind", "AggregateID" }) &&
            index.GetFilter() == "\"ProcessedAt\" IS NULL");
        Assert.Contains(indexes, index =>
            index.Properties.Select(property => property.Name).SequenceEqual(
                new[] { "ProcessedAt", "NextAttemptAt", "LockedUntil" }));
        Assert.Contains(indexes, index =>
            index.Properties.Select(property => property.Name).SequenceEqual(
                new[] { "Kind", "CreatedAt" }));
    }

    [Fact]
    public async Task ConfirmationResend_UsesDurablePerAccountCooldown()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        var now = DateTime.UtcNow;
        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "resend-cooldown");
        user.IsConfirmed = false;
        var confirmation = new PendingEmailConfirmation
        {
            ID = Guid.NewGuid(),
            User = user,
            TokenHash = Convert.ToHexString(Guid.NewGuid().ToByteArray()),
            ExpiresAt = now.AddMinutes(15)
        };
        var outbox = new EmailOutboxMessage
        {
            Kind = EmailOutboxKind.EmailConfirmation,
            AggregateID = confirmation.ID,
            CreatedAt = now,
            NextAttemptAt = now
        };
        context.AddRange(role, user, confirmation, outbox);
        await context.SaveChangesAsync();
        var handler = new ResendConfirmEmailQueryHandler(
            emailOutboxService: null!,
            pendingEmailConfirmationService: null!,
            context,
            new FixedDateTimeProvider(now),
            new EmailConfirmationLock(context));

        var response = await handler.Handle(
            new ResendConfirmEmailQuery { Username = user.Username },
            CancellationToken.None);

        Assert.Empty(response.lstError);
        Assert.Equal(1, await context.EmailOutboxMessage.CountAsync(message =>
            message.Kind == EmailOutboxKind.EmailConfirmation));
        Assert.Equal(1, await context.PendingEmailConfirmation.CountAsync(candidate =>
            candidate.UserID == user.ID));
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task ConcurrentConfirmationResend_SerializesCooldownAndReturnsGenericSuccess()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        var now = DateTime.UtcNow.AddSeconds(1);
        Guid roleId;
        Guid userId;
        string username;
        await using (var setup = CreateContext(connectionString))
        {
            await setup.Database.MigrateAsync();
            var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
            var user = CreateUser(role, "confirmation-resend-race");
            user.IsConfirmed = false;
            setup.AddRange(role, user);
            await setup.SaveChangesAsync();
            roleId = role.ID;
            userId = user.ID;
            username = user.Username;
        }

        try
        {
            await using var firstContext = CreateContext(connectionString);
            await using var secondContext = CreateContext(connectionString);
            var firstClock = new FixedDateTimeProvider(now);
            var secondClock = new FixedDateTimeProvider(now);
            var firstHandler = new ResendConfirmEmailQueryHandler(
                CreateOutboxService(firstContext, new TestNotificationService(), firstClock),
                new PendingEmailConfirmationService(firstClock, new TestTokenService()),
                firstContext,
                firstClock,
                new EmailConfirmationLock(firstContext));
            var secondHandler = new ResendConfirmEmailQueryHandler(
                CreateOutboxService(secondContext, new TestNotificationService(), secondClock),
                new PendingEmailConfirmationService(secondClock, new TestTokenService()),
                secondContext,
                secondClock,
                new EmailConfirmationLock(secondContext));

            var responses = await Task.WhenAll(
                firstHandler.Handle(
                    new ResendConfirmEmailQuery { Username = username },
                    CancellationToken.None),
                secondHandler.Handle(
                    new ResendConfirmEmailQuery { Username = username },
                    CancellationToken.None));

            Assert.All(responses, response => Assert.Empty(response.lstError));
            await using var verification = CreateContext(connectionString);
            var confirmations = await verification.PendingEmailConfirmation
                .Where(candidate => candidate.UserID == userId)
                .ToListAsync();
            Assert.Single(confirmations);
            Assert.Equal(1, await verification.EmailOutboxMessage.CountAsync(message =>
                message.Kind == EmailOutboxKind.EmailConfirmation &&
                message.AggregateID == confirmations[0].ID));
        }
        finally
        {
            await using var cleanup = CreateContext(connectionString);
            var confirmationIds = await cleanup.PendingEmailConfirmation.IgnoreQueryFilters()
                .Where(candidate => candidate.UserID == userId)
                .Select(candidate => candidate.ID)
                .ToListAsync();
            await cleanup.EmailOutboxMessage
                .Where(message => message.Kind == EmailOutboxKind.EmailConfirmation &&
                    confirmationIds.Contains(message.AggregateID))
                .ExecuteDeleteAsync();
            await cleanup.PendingEmailConfirmation.IgnoreQueryFilters()
                .Where(candidate => candidate.UserID == userId)
                .ExecuteDeleteAsync();
            await cleanup.User.IgnoreQueryFilters().Where(user => user.ID == userId).ExecuteDeleteAsync();
            await cleanup.Role.IgnoreQueryFilters().Where(role => role.ID == roleId).ExecuteDeleteAsync();
        }
    }

    [Fact]
    public async Task ConcurrentContactSubmission_UsesRaceFreeNormalizedSenderCooldown()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        var now = DateTime.UtcNow.AddSeconds(1);
        Guid roleId;
        Guid userId;
        string targetEmail;
        await using (var setup = CreateContext(connectionString))
        {
            await setup.Database.MigrateAsync();
            var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
            var user = CreateUser(role, "contact-cooldown");
            user.IsConfirmed = true;
            setup.AddRange(role, user);
            await setup.SaveChangesAsync();
            roleId = role.ID;
            userId = user.ID;
            targetEmail = user.Email;
        }

        var mapping = new MapperConfiguration(
            expression => expression.AddProfile<Application.Client.MappingProfiles.ContactMessageMappingProfiles>(),
            NullLoggerFactory.Instance).CreateMapper();
        var firstRequest = new SendEmailCommand
        {
            EmailTo = targetEmail,
            Name = "Visitor",
            Email = "  VISITOR@EXAMPLE.TEST ",
            Subject = "First",
            Message = "First message"
        };
        var secondRequest = new SendEmailCommand
        {
            EmailTo = targetEmail,
            Name = "Visitor",
            Email = "visitor@example.test",
            Subject = "Second",
            Message = "Second message"
        };

        try
        {
            await using var firstContext = CreateContext(connectionString);
            await using var secondContext = CreateContext(connectionString);
            var firstClock = new FixedDateTimeProvider(now);
            var secondClock = new FixedDateTimeProvider(now);
            var firstHandler = new SendEmailCommandHandler(
                firstContext,
                mapping,
                new UserResolverService(firstContext),
                CreateOutboxService(firstContext, new TestNotificationService(), firstClock),
                new ContactSubmissionGuard(firstContext, firstClock));
            var secondHandler = new SendEmailCommandHandler(
                secondContext,
                mapping,
                new UserResolverService(secondContext),
                CreateOutboxService(secondContext, new TestNotificationService(), secondClock),
                new ContactSubmissionGuard(secondContext, secondClock));

            var responses = await Task.WhenAll(
                firstHandler.Handle(firstRequest, CancellationToken.None),
                secondHandler.Handle(secondRequest, CancellationToken.None));

            Assert.All(responses, response => Assert.Empty(response.lstError));
            await using var verification = CreateContext(connectionString);
            var persistedContact = await verification.ContactMessage
                .SingleAsync(message => message.UserID == userId);
            Assert.Equal("visitor@example.test", persistedContact.Email);
            Assert.Equal(1, await verification.EmailOutboxMessage.CountAsync(message =>
                message.Kind == EmailOutboxKind.ContactNotification &&
                message.AggregateID == persistedContact.ID));
        }
        finally
        {
            await using var cleanup = CreateContext(connectionString);
            var contactIds = await cleanup.ContactMessage.IgnoreQueryFilters()
                .Where(message => message.UserID == userId)
                .Select(message => message.ID)
                .ToListAsync();
            await cleanup.EmailOutboxMessage
                .Where(message => message.Kind == EmailOutboxKind.ContactNotification &&
                    contactIds.Contains(message.AggregateID))
                .ExecuteDeleteAsync();
            await cleanup.ContactMessage.IgnoreQueryFilters()
                .Where(message => message.UserID == userId)
                .ExecuteDeleteAsync();
            await cleanup.User.IgnoreQueryFilters().Where(user => user.ID == userId).ExecuteDeleteAsync();
            await cleanup.Role.IgnoreQueryFilters().Where(role => role.ID == roleId).ExecuteDeleteAsync();
        }
    }

    [Fact]
    public void PublicPortfolioSensitiveFieldGates_TranslateToPostgreSqlProjection()
    {
        using var context = CreateContext(
            "Host=127.0.0.1;Port=1;Database=model_only;Username=test;Password=test");
        var configuration = new MapperConfiguration(
            expression => expression.AddProfile<Application.Client.MappingProfiles.UserMappingProfiles>(),
            NullLoggerFactory.Instance);

        var sql = context.User.AsNoTracking()
            .Where(user => user.IsConfirmed)
            .AsSplitQuery()
            .ProjectTo<UBUQ_Response>(configuration, new
            {
                currentPublicDate = new DateOnly(2026, 8, 29)
            })
            .ToQueryString();

        Assert.Contains("UserPreference", sql, StringComparison.Ordinal);
        Assert.Contains(PublicProfilePrivacy.ShowEmailPreference, sql, StringComparison.Ordinal);
        Assert.Contains(PublicProfilePrivacy.ShowPhonePreference, sql, StringComparison.Ordinal);
        Assert.Contains(PublicProfilePrivacy.ShowBirthDatePreference, sql, StringComparison.Ordinal);
        Assert.Contains(PublicProfilePrivacy.ShowGenderPreference, sql, StringComparison.Ordinal);

        var blogPost = context.Model.FindEntityType(typeof(BlogPost));
        Assert.NotNull(blogPost);
        Assert.Contains(blogPost.GetIndexes(), index =>
            index.GetDatabaseName() == "IX_BlogPost_PublicVisibility" &&
            index.Properties.Select(property => property.Name).SequenceEqual(new[]
            {
                "UserID",
                "LKP_BlogPostStatusID",
                "IsDeleted",
                "PublishedAt",
                "ID"
            }));
    }

    [Fact]
    public void PublicUserListing_HasFilterAndStableOrderingIndex()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var entity = context.Model.FindEntityType(typeof(User));

        Assert.NotNull(entity);
        Assert.Contains(entity.GetIndexes(), index =>
            index.Properties.Select(property => property.Name).SequenceEqual(
                new[] { "IsConfirmed", "CreatedAt", "ID" }));
    }

    [Fact]
    public void PublicUserSearch_GatesEmailMatchingBehindExplicitVisibilityPreference()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");

        var sql = PublicUserSearch.Apply(
                context.User.AsNoTracking().Where(user => user.IsConfirmed),
                "private@example.test")
            .ToQueryString();

        Assert.Contains("UserPreference", sql, StringComparison.Ordinal);
        Assert.Contains(PublicProfilePrivacy.ShowEmailPreference, sql, StringComparison.Ordinal);
        Assert.Contains("Email", sql, StringComparison.Ordinal);
        Assert.Contains("IsDeleted", sql, StringComparison.Ordinal);
    }

    public static TheoryData<Type> RequiredSoftDeleteDependentTypes => new()
    {
        typeof(User),
        typeof(PendingEmailConfirmation),
        typeof(RefreshToken),
        typeof(BlogPostTag),
        typeof(UserLanguage),
        typeof(UserSkillCertificate),
        typeof(UserSkillEducation),
        typeof(UserSkillExperience),
        typeof(UserSkillProject)
    };

    [Theory]
    [MemberData(nameof(RequiredSoftDeleteDependentTypes))]
    public void RequiredSoftDeleteDependents_HaveMatchingQueryFilters(Type entityType)
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        Assert.NotNull(context.Model.FindEntityType(entityType)?.GetQueryFilter());
    }

    [Fact]
    public void Relationships_DoNotPermitPhysicalCascadeDeletes()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var foreignKeys = context.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()).ToList();

        Assert.NotEmpty(foreignKeys);
        Assert.All(foreignKeys, foreignKey => Assert.Equal(DeleteBehavior.Restrict, foreignKey.DeleteBehavior));
    }

    [Fact]
    public void AuditedEntities_UsePostgreSqlOptimisticConcurrency()
    {
        using var context = CreateContext("Host=localhost;Database=model;Username=model;Password=model");
        var auditedEntityTypes = context.Model.GetEntityTypes()
            .Where(entity => typeof(Domain.AbstractEntity).IsAssignableFrom(entity.ClrType))
            .ToList();

        Assert.NotEmpty(auditedEntityTypes);
        Assert.All(auditedEntityTypes, entityType =>
        {
            var xmin = entityType.FindProperty("xmin");
            Assert.NotNull(xmin);
            Assert.True(xmin.IsConcurrencyToken);
            Assert.Equal(ValueGenerated.OnAddOrUpdate, xmin.ValueGenerated);
        });
    }

    [Fact]
    public async Task TestDatabase_IsPostgreSqlAndReachable()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return; // CI always supplies PostgreSQL; local runs require TEST_DATABASE_URL to execute this test.
        }

        await using var connection = new NpgsqlConnection(PostgreSqlConnectionString.Normalize(connectionString));
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("select current_database()", connection);
        Assert.False(string.IsNullOrWhiteSpace((string?)await command.ExecuteScalarAsync()));
    }

    [Fact]
    public async Task EmailOutbox_DispatchesContactNotificationAndMarksItProcessed()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "outbox-success");
        var contact = new ContactMessage
        {
            ID = Guid.NewGuid(),
            User = user,
            Name = "Contact",
            Email = "contact@example.test",
            Subject = "Subject",
            Message = "Message"
        };
        context.AddRange(role, user, contact);
        var notifications = new TestNotificationService();
        var clock = new FixedDateTimeProvider(DateTime.UtcNow);
        var service = CreateOutboxService(context, notifications, clock);
        service.EnqueueContactNotification(contact);
        await context.SaveChangesAsync();

        var result = await service.DispatchPendingAsync(CancellationToken.None);
        context.ChangeTracker.Clear();
        var persisted = await context.EmailOutboxMessage.SingleAsync();

        Assert.Equal(new EmailOutboxDispatchResult(1, 1, 0, 0), result);
        Assert.Equal(1, notifications.ContactNotifications);
        Assert.NotNull(persisted.ProcessedAt);
        Assert.Null(persisted.LastError);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task EmailOutbox_ReclaimsLeaseAtExactExpirationBoundary()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var now = DateTime.UtcNow;
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "outbox-boundary");
        var contact = CreateContact(user, "Expired lease");
        context.AddRange(role, user, contact);
        var notifications = new TestNotificationService();
        var service = CreateOutboxService(
            context,
            notifications,
            new FixedDateTimeProvider(now));
        service.EnqueueContactNotification(contact);
        await context.SaveChangesAsync();
        var message = await context.EmailOutboxMessage.SingleAsync();
        message.LockID = Guid.NewGuid();
        message.LockedUntil = now;
        await context.SaveChangesAsync();

        var result = await service.DispatchPendingAsync(CancellationToken.None);
        context.ChangeTracker.Clear();
        var persisted = await context.EmailOutboxMessage.SingleAsync();

        Assert.Equal(new EmailOutboxDispatchResult(1, 1, 0, 0), result);
        Assert.Equal(1, notifications.ContactNotifications);
        Assert.NotNull(persisted.ProcessedAt);
        Assert.Null(persisted.LockID);
        Assert.Null(persisted.LockedUntil);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task EmailOutbox_ImmediateDispatchProcessesOnlyRequestedMessage()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "outbox-targeted");
        var firstContact = CreateContact(user, "Immediate");
        var secondContact = CreateContact(user, "Cron fallback");
        context.AddRange(role, user, firstContact, secondContact);
        var notifications = new TestNotificationService();
        var service = CreateOutboxService(
            context,
            notifications,
            new FixedDateTimeProvider(DateTime.UtcNow));
        service.EnqueueContactNotification(firstContact);
        service.EnqueueContactNotification(secondContact);
        await context.SaveChangesAsync();
        var requestedMessageId = await context.EmailOutboxMessage
            .Where(message => message.AggregateID == firstContact.ID)
            .Select(message => message.ID)
            .SingleAsync();

        var result = await service.DispatchAsync(requestedMessageId, CancellationToken.None);
        context.ChangeTracker.Clear();
        var messages = await context.EmailOutboxMessage
            .OrderBy(message => message.AggregateID)
            .ToListAsync();

        Assert.Equal(new EmailOutboxDispatchResult(1, 1, 0, 0), result);
        Assert.Equal(1, notifications.ContactNotifications);
        Assert.NotNull(Assert.Single(messages, message => message.ID == requestedMessageId).ProcessedAt);
        Assert.Null(Assert.Single(messages, message => message.ID != requestedMessageId).ProcessedAt);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task EmailOutbox_DailyRecoveryDrainsMoreThanOneBatch()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "outbox-multi-batch");
        var contacts = Enumerable.Range(0, EmailOutboxPolicy.BatchSize + 5)
            .Select(index => CreateContact(user, $"Message {index}"))
            .ToArray();
        context.Add(role);
        context.Add(user);
        context.AddRange(contacts);
        var notifications = new TestNotificationService();
        var service = CreateOutboxService(
            context,
            notifications,
            new FixedDateTimeProvider(DateTime.UtcNow));
        foreach (var contact in contacts)
        {
            service.EnqueueContactNotification(contact);
        }
        await context.SaveChangesAsync();

        var result = await service.DrainPendingAsync(CancellationToken.None);
        context.ChangeTracker.Clear();

        Assert.Equal(new EmailOutboxDispatchResult(contacts.Length, contacts.Length, 0, 0), result);
        Assert.Equal(contacts.Length, notifications.ContactNotifications);
        Assert.Equal(
            contacts.Length,
            await context.EmailOutboxMessage.CountAsync(message => message.ProcessedAt != null));
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task EmailOutbox_OverlappingWorkersClaimMessageOnlyOnce()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        Guid roleId;
        Guid userId;
        var contactId = Guid.NewGuid();
        await using (var setup = CreateContext(connectionString))
        {
            await setup.Database.MigrateAsync();
            var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
            var user = CreateUser(role, "outbox-overlap");
            var contact = new ContactMessage
            {
                ID = contactId,
                User = user,
                Name = "Contact",
                Email = "contact@example.test",
                Subject = "Subject",
                Message = "Message"
            };
            setup.AddRange(role, user, contact);
            var enqueue = CreateOutboxService(
                setup,
                new TestNotificationService(),
                new FixedDateTimeProvider(DateTime.UtcNow));
            enqueue.EnqueueContactNotification(contact);
            await setup.SaveChangesAsync();
            roleId = role.ID;
            userId = user.ID;
        }

        var notifications = new TestNotificationService();
        var clock = new FixedDateTimeProvider(DateTime.UtcNow);
        await using var firstContext = CreateContext(connectionString);
        await using var secondContext = CreateContext(connectionString);
        var results = await Task.WhenAll(
            CreateOutboxService(firstContext, notifications, clock)
                .DispatchPendingAsync(CancellationToken.None),
            CreateOutboxService(secondContext, notifications, clock)
                .DispatchPendingAsync(CancellationToken.None));

        await using var verification = CreateContext(connectionString);
        var message = await verification.EmailOutboxMessage
            .SingleAsync(item => item.AggregateID == contactId);
        Assert.Equal(1, results.Sum(result => result.Claimed));
        Assert.Equal(1, results.Sum(result => result.Processed));
        Assert.Equal(1, notifications.ContactNotifications);
        Assert.NotNull(message.ProcessedAt);

        await verification.EmailOutboxMessage
            .Where(item => item.AggregateID == contactId)
            .ExecuteDeleteAsync();
        await verification.ContactMessage.IgnoreQueryFilters()
            .Where(item => item.ID == contactId)
            .ExecuteDeleteAsync();
        await verification.User.IgnoreQueryFilters()
            .Where(item => item.ID == userId)
            .ExecuteDeleteAsync();
        await verification.Role.IgnoreQueryFilters()
            .Where(item => item.ID == roleId)
            .ExecuteDeleteAsync();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task EmailOutbox_LostLeaseDoesNotReportSuccessOrAbortTheBatch(bool deliveryFails)
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "outbox-lost-lease");
        var contact = CreateContact(user, "Lost lease");
        context.AddRange(role, user, contact);
        var notifications = new TestNotificationService
        {
            Fail = deliveryFails,
            OnContactDelivery = async _ =>
            {
                await context.EmailOutboxMessage.ExecuteUpdateAsync(updates => updates
                    .SetProperty(message => message.LockID, Guid.NewGuid()));
            }
        };
        var logger = new RecordingLogger<EmailOutboxService>();
        var service = CreateOutboxService(
            context,
            notifications,
            new FixedDateTimeProvider(DateTime.UtcNow),
            logger);
        service.EnqueueContactNotification(contact);
        await context.SaveChangesAsync();

        var result = await service.DispatchPendingAsync(CancellationToken.None);
        context.ChangeTracker.Clear();
        var pending = await context.EmailOutboxMessage.SingleAsync();

        Assert.Equal(new EmailOutboxDispatchResult(1, 0, 1, 0), result);
        Assert.Null(pending.ProcessedAt);
        Assert.Equal(0, pending.AttemptCount);
        Assert.Contains(logger.Messages, message =>
            message.Contains("lost its claim", StringComparison.OrdinalIgnoreCase));
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task EmailOutbox_EachSequentialClaimReceivesAFreshFullLease()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var startedAt = DateTime.UtcNow;
        var clock = new AdvancingDateTimeProvider(startedAt);
        using var cancellation = new CancellationTokenSource();
        var notifications = new TestNotificationService
        {
            OnContactDelivery = delivery =>
            {
                if (delivery == 1)
                {
                    clock.Advance(EmailOutboxPolicy.ClaimDuration.Add(TimeSpan.FromMinutes(1)));
                    return Task.CompletedTask;
                }

                cancellation.Cancel();
                throw new OperationCanceledException(cancellation.Token);
            }
        };
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "outbox-fresh-lease");
        var firstContact = CreateContact(user, "First");
        var secondContact = CreateContact(user, "Second");
        context.AddRange(role, user, firstContact, secondContact);
        var service = CreateOutboxService(context, notifications, clock);
        service.EnqueueContactNotification(firstContact);
        service.EnqueueContactNotification(secondContact);
        await context.SaveChangesAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            service.DispatchPendingAsync(cancellation.Token));
        context.ChangeTracker.Clear();
        var messages = await context.EmailOutboxMessage.OrderBy(message => message.ID).ToListAsync();

        Assert.Single(messages, message => message.ProcessedAt != null);
        var stillClaimed = Assert.Single(messages, message => message.ProcessedAt == null);
        Assert.NotNull(stillClaimed.LockID);
        Assert.Equal(clock.UtcNow.Add(EmailOutboxPolicy.ClaimDuration), stillClaimed.LockedUntil);
        Assert.True(stillClaimed.LockedUntil > startedAt.Add(EmailOutboxPolicy.ClaimDuration));
        await transaction.RollbackAsync(CancellationToken.None);
    }

    [Fact]
    public async Task EmailOutbox_FailureIsSanitizedAndTerminalMessageCanBeReplayed()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "outbox-failure");
        var contact = new ContactMessage
        {
            ID = Guid.NewGuid(),
            User = user,
            Name = "Contact",
            Email = "contact@example.test",
            Subject = "Subject",
            Message = "Message"
        };
        context.AddRange(role, user, contact);
        var notifications = new TestNotificationService { Fail = true };
        var clock = new FixedDateTimeProvider(DateTime.UtcNow);
        var logger = new RecordingLogger<EmailOutboxService>();
        var service = CreateOutboxService(context, notifications, clock, logger);
        service.EnqueueContactNotification(contact);
        await context.SaveChangesAsync();

        var result = await service.DispatchPendingAsync(CancellationToken.None);
        context.ChangeTracker.Clear();
        var failed = await context.EmailOutboxMessage.SingleAsync();
        Assert.Equal(new EmailOutboxDispatchResult(1, 0, 1, 0), result);
        Assert.Equal(1, failed.AttemptCount);
        Assert.Equal("InvalidOperationException: delivery failed", failed.LastError);
        Assert.DoesNotContain("smtp-secret", failed.LastError);
        var retryLog = Assert.Single(logger.Messages);
        Assert.Contains("ContactNotification", retryLog, StringComparison.Ordinal);
        Assert.DoesNotContain(failed.ID.ToString(), retryLog, StringComparison.OrdinalIgnoreCase);

        await context.EmailOutboxMessage.ExecuteUpdateAsync(updates => updates
            .SetProperty(message => message.AttemptCount, 5));
        Assert.True(await service.ReplayTerminalAsync(failed.ID, CancellationToken.None));
        Assert.Contains(logger.Messages, message =>
            message.Contains(failed.ID.ToString(), StringComparison.OrdinalIgnoreCase));
        context.ChangeTracker.Clear();
        var replayed = await context.EmailOutboxMessage.SingleAsync();
        Assert.Equal(0, replayed.AttemptCount);
        Assert.Null(replayed.LastError);
        Assert.Equal(clock.UtcNow, replayed.NextAttemptAt);
        Assert.False(await service.ReplayTerminalAsync(Guid.NewGuid(), CancellationToken.None));
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task EmailOutbox_PartialFailureDoesNotStopRemainingBatch()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "outbox-partial");
        var first = new ContactMessage
        {
            ID = Guid.NewGuid(),
            User = user,
            Name = "First",
            Email = "first@example.test",
            Subject = "First",
            Message = "First"
        };
        var second = new ContactMessage
        {
            ID = Guid.NewGuid(),
            User = user,
            Name = "Second",
            Email = "second@example.test",
            Subject = "Second",
            Message = "Second"
        };
        context.AddRange(role, user, first, second);
        var notifications = new TestNotificationService { FailFirst = true };
        var service = CreateOutboxService(
            context,
            notifications,
            new FixedDateTimeProvider(DateTime.UtcNow));
        service.EnqueueContactNotification(first);
        service.EnqueueContactNotification(second);
        await context.SaveChangesAsync();

        var result = await service.DispatchPendingAsync(CancellationToken.None);
        context.ChangeTracker.Clear();
        var messages = await context.EmailOutboxMessage.OrderBy(item => item.ID).ToListAsync();

        Assert.Equal(new EmailOutboxDispatchResult(2, 1, 1, 0), result);
        Assert.Single(messages, item => item.ProcessedAt != null);
        Assert.Single(messages, item => item.AttemptCount == 1);
        Assert.Equal(1, notifications.ContactNotifications);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task EmailOutbox_DependencyTimeoutCountsAsFailedAttempt()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "outbox-timeout");
        var contact = new ContactMessage
        {
            ID = Guid.NewGuid(),
            User = user,
            Name = "Contact",
            Email = "contact@example.test",
            Subject = "Subject",
            Message = "Message"
        };
        context.AddRange(role, user, contact);
        var notifications = new TestNotificationService { TimeOut = true };
        var clock = new FixedDateTimeProvider(DateTime.UtcNow);
        var service = CreateOutboxService(context, notifications, clock);
        service.EnqueueContactNotification(contact);
        await context.SaveChangesAsync();

        var result = await service.DispatchPendingAsync(CancellationToken.None);
        context.ChangeTracker.Clear();
        var failed = await context.EmailOutboxMessage.SingleAsync();

        Assert.Equal(new EmailOutboxDispatchResult(1, 0, 1, 0), result);
        Assert.Equal(1, failed.AttemptCount);
        Assert.Equal("OperationCanceledException: delivery failed", failed.LastError);
        Assert.Null(failed.LockID);
        Assert.Null(failed.LockedUntil);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task EmailOutbox_CallerCancellationIsNotRecordedAsDeliveryFailure()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "outbox-cancel");
        var contact = new ContactMessage
        {
            ID = Guid.NewGuid(),
            User = user,
            Name = "Contact",
            Email = "contact@example.test",
            Subject = "Subject",
            Message = "Message"
        };
        context.AddRange(role, user, contact);
        using var cancellation = new CancellationTokenSource();
        var notifications = new TestNotificationService { CallerCancellation = cancellation };
        var service = CreateOutboxService(
            context,
            notifications,
            new FixedDateTimeProvider(DateTime.UtcNow));
        service.EnqueueContactNotification(contact);
        await context.SaveChangesAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            service.DispatchPendingAsync(cancellation.Token));
        context.ChangeTracker.Clear();
        var pending = await context.EmailOutboxMessage.SingleAsync();

        Assert.Equal(0, pending.AttemptCount);
        Assert.Null(pending.LastError);
        Assert.NotNull(pending.LockID);
        Assert.NotNull(pending.LockedUntil);
        await transaction.RollbackAsync(CancellationToken.None);
    }

    [Fact]
    public async Task MaintenanceCleanup_PrunesExpiredSessionsAndAgedOutboxWithoutLosingReplayWindow()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var now = DateTime.UtcNow;
        var old = now.Subtract(EmailOutboxPolicy.Retention).AddDays(-1);
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "cleanup-expired");
        var confirmation = new PendingEmailConfirmation
        {
            ID = Guid.NewGuid(),
            User = user,
            TokenHash = $"hash-{Guid.NewGuid():N}",
            ExpiresAt = old,
            RevokedAt = old
        };
        var expiredRefresh = new RefreshToken
        {
            Token = $"hash-{Guid.NewGuid():N}",
            User = user,
            CreatedAt = old,
            ExpiresAt = old,
            CreatedByIp = "127.0.0.1"
        };
        var boundaryRefresh = new RefreshToken
        {
            Token = $"hash-{Guid.NewGuid():N}",
            User = user,
            CreatedAt = old,
            ExpiresAt = now,
            CreatedByIp = "127.0.0.1"
        };
        var revokedUnexpiredRefresh = new RefreshToken
        {
            Token = $"hash-{Guid.NewGuid():N}",
            User = user,
            CreatedAt = old,
            ExpiresAt = now.AddDays(1),
            CreatedByIp = "127.0.0.1",
            IsRevoked = true,
            RevokedAt = old
        };
        var terminalConfirmationDelivery = new EmailOutboxMessage
        {
            Kind = EmailOutboxKind.EmailConfirmation,
            AggregateID = confirmation.ID,
            CreatedAt = old,
            NextAttemptAt = old,
            AttemptCount = EmailOutboxPolicy.MaximumAttempts,
            LastError = "OperationCanceledException: delivery failed"
        };
        var oldProcessedDelivery = new EmailOutboxMessage
        {
            Kind = EmailOutboxKind.ContactNotification,
            AggregateID = Guid.NewGuid(),
            CreatedAt = old,
            NextAttemptAt = old,
            ProcessedAt = old
        };
        var boundaryConfirmation = new PendingEmailConfirmation
        {
            ID = Guid.NewGuid(),
            User = user,
            TokenHash = $"hash-{Guid.NewGuid():N}",
            ExpiresAt = now
        };
        var boundaryProcessedDelivery = new EmailOutboxMessage
        {
            Kind = EmailOutboxKind.ContactNotification,
            AggregateID = Guid.NewGuid(),
            CreatedAt = old,
            NextAttemptAt = old,
            ProcessedAt = now.Subtract(EmailOutboxPolicy.Retention)
        };
        var boundaryTerminalDelivery = new EmailOutboxMessage
        {
            Kind = EmailOutboxKind.ContactNotification,
            AggregateID = Guid.NewGuid(),
            CreatedAt = old,
            NextAttemptAt = now.Subtract(EmailOutboxPolicy.Retention),
            AttemptCount = EmailOutboxPolicy.MaximumAttempts,
            LastError = "InvalidOperationException: delivery failed",
            LockID = Guid.NewGuid(),
            LockedUntil = now
        };
        var recentTerminalDelivery = new EmailOutboxMessage
        {
            Kind = EmailOutboxKind.ContactNotification,
            AggregateID = Guid.NewGuid(),
            CreatedAt = now,
            NextAttemptAt = now,
            AttemptCount = EmailOutboxPolicy.MaximumAttempts,
            LastError = "InvalidOperationException: delivery failed"
        };
        context.AddRange(
            role,
            user,
            confirmation,
            expiredRefresh,
            boundaryRefresh,
            revokedUnexpiredRefresh,
            terminalConfirmationDelivery,
            oldProcessedDelivery,
            boundaryConfirmation,
            boundaryProcessedDelivery,
            boundaryTerminalDelivery,
            recentTerminalDelivery);
        await context.SaveChangesAsync();

        var service = new MaintenanceCleanupService(context, new FixedDateTimeProvider(now));
        var result = await service.CleanupAsync(CancellationToken.None);

        Assert.Equal(new MaintenanceCleanupResult(2, 2, 4), result);
        Assert.False(await context.RefreshToken.AnyAsync(token => token.ID == expiredRefresh.ID));
        Assert.False(await context.RefreshToken.AnyAsync(token => token.ID == boundaryRefresh.ID));
        Assert.True(await context.RefreshToken.AnyAsync(token => token.ID == revokedUnexpiredRefresh.ID));
        Assert.False(await context.PendingEmailConfirmation.AnyAsync(item => item.ID == confirmation.ID));
        Assert.False(await context.PendingEmailConfirmation.AnyAsync(item => item.ID == boundaryConfirmation.ID));
        Assert.False(await context.EmailOutboxMessage.AnyAsync(item => item.ID == terminalConfirmationDelivery.ID));
        Assert.False(await context.EmailOutboxMessage.AnyAsync(item => item.ID == oldProcessedDelivery.ID));
        Assert.False(await context.EmailOutboxMessage.AnyAsync(item => item.ID == boundaryProcessedDelivery.ID));
        Assert.False(await context.EmailOutboxMessage.AnyAsync(item => item.ID == boundaryTerminalDelivery.ID));
        Assert.True(await context.EmailOutboxMessage.AnyAsync(item => item.ID == recentTerminalDelivery.ID));
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task MaintenanceCleanup_BoundsEachRetentionCategoryPerInvocation()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var now = DateTime.UtcNow;
        var expiredAt = now.AddDays(-1);
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "cleanup-bounded");
        context.AddRange(role, user);
        await context.SaveChangesAsync();
        var tokenPrefix = Guid.NewGuid().ToString("N");

        await context.Database.ExecuteSqlInterpolatedAsync(
            $"""
            INSERT INTO "RefreshToken"
                ("ID", "Token", "ExpiresAt", "CreatedAt", "CreatedByIp", "IsRevoked", "RevokedAt", "RememberMe", "UserID")
            SELECT gen_random_uuid(), md5(i::text || {tokenPrefix}), {expiredAt}, {expiredAt},
                '127.0.0.1', false, NULL, false, {user.ID}
            FROM generate_series(1, {MaintenancePolicy.CleanupBatchSize + 1}) AS i;
            """);

        var service = new MaintenanceCleanupService(context, new FixedDateTimeProvider(now));
        var result = await service.CleanupAsync(CancellationToken.None);

        Assert.Equal(MaintenancePolicy.CleanupBatchSize, result.RefreshTokens);
        Assert.Equal(1, await context.RefreshToken.CountAsync(token => token.UserID == user.ID));
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task Logout_UsesOnlyHashedRefreshTokenForAnonymousRevocation()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        const string rawToken = "raw-refresh-token-must-not-be-queried";
        var tokenService = new TestTokenService();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "logout-hash-only");
        var rawLegacyRow = new RefreshToken
        {
            Token = rawToken,
            User = user,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            CreatedByIp = "127.0.0.1"
        };
        var hashedRow = new RefreshToken
        {
            Token = tokenService.HashToken(rawToken),
            User = user,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            CreatedByIp = "127.0.0.1"
        };
        context.AddRange(role, user, rawLegacyRow, hashedRow);
        await context.SaveChangesAsync();
        var cookies = new TestCookieService(rawToken);
        var handler = new LogoutCommandHandler(
            new AnonymousCurrentUser(),
            context,
            cookies,
            tokenService,
            new FixedDateTimeProvider(DateTime.UtcNow));

        await handler.Handle(new LogoutCommand(), CancellationToken.None);
        context.ChangeTracker.Clear();

        Assert.True(cookies.WereCleared);
        Assert.False((await context.RefreshToken.SingleAsync(token => token.ID == rawLegacyRow.ID)).IsRevoked);
        Assert.True((await context.RefreshToken.SingleAsync(token => token.ID == hashedRow.ID)).IsRevoked);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task RefreshToken_ConcurrentReuseRevokesWinningReplacement()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        const string rawToken = "concurrent-original-refresh-token";
        var tokenService = new RotatingTestTokenService();
        Guid roleId;
        Guid userId;
        await using (var setup = CreateContext(connectionString))
        {
            await setup.Database.MigrateAsync();
            var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
            var user = CreateUser(role, "refresh-concurrent");
            setup.AddRange(role, user, new RefreshToken
            {
                Token = tokenService.HashToken(rawToken),
                User = user,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                CreatedByIp = "127.0.0.1",
                RememberMe = true
            });
            await setup.SaveChangesAsync();
            roleId = role.ID;
            userId = user.ID;
        }

        var clock = new FixedDateTimeProvider(DateTime.UtcNow);
        var firstCookies = new TestCookieService(rawToken);
        var secondCookies = new TestCookieService(rawToken);
        var refreshMetrics = new TestOperationalMetrics();
        await using var firstContext = CreateContext(connectionString);
        await using var secondContext = CreateContext(connectionString);
        var first = new TokenRefreshService(
            new AuthService(tokenService, firstCookies),
            firstCookies,
            firstContext,
            clock,
            tokenService,
            refreshMetrics);
        var second = new TokenRefreshService(
            new AuthService(tokenService, secondCookies),
            secondCookies,
            secondContext,
            clock,
            tokenService,
            refreshMetrics);

        var results = await Task.WhenAll(
            first.TryRefreshTokenAsync(CancellationToken.None),
            second.TryRefreshTokenAsync(CancellationToken.None));

        Assert.Single(results, result => result is not null);
        Assert.Equal(1, firstCookies.RefreshCookiesSet + secondCookies.RefreshCookiesSet);
        Assert.Equal(1, Convert.ToInt32(firstCookies.WereCleared) + Convert.ToInt32(secondCookies.WereCleared));
        Assert.Contains("refresh_token_concurrent_reuse", refreshMetrics.AuthenticationFailureReasons);

        await using var verification = CreateContext(connectionString);
        var sessions = await verification.RefreshToken
            .Where(item => item.UserID == userId)
            .ToListAsync();
        Assert.Equal(2, sessions.Count);
        Assert.All(sessions, session => Assert.True(session.IsRevoked));

        await verification.RefreshToken.Where(item => item.UserID == userId).ExecuteDeleteAsync();
        await verification.User.IgnoreQueryFilters().Where(item => item.ID == userId).ExecuteDeleteAsync();
        await verification.Role.IgnoreQueryFilters().Where(item => item.ID == roleId).ExecuteDeleteAsync();
    }

    [Fact]
    public async Task RefreshToken_ExplicitMigrationCompatibilityUpgradesLegacyRawRow()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        const string rawToken = "legacy-raw-refresh-token";
        var tokenService = new RotatingTestTokenService();
        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "refresh-legacy-cutover");
        var legacy = new RefreshToken
        {
            Token = rawToken,
            User = user,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            CreatedByIp = "127.0.0.1"
        };
        context.AddRange(role, user, legacy);
        await context.SaveChangesAsync();
        var cookies = new TestCookieService(rawToken);
        var service = new TokenRefreshService(
            new AuthService(tokenService, cookies),
            cookies,
            context,
            new FixedDateTimeProvider(DateTime.UtcNow),
            tokenService,
            new TestOperationalMetrics(),
            new Application.Common.Configuration.SecuritySettings(
                string.Empty,
                new HashSet<string>(),
                AllowLegacyRefreshTokenLookup: true));

        var refreshedUser = await service.TryRefreshTokenAsync(CancellationToken.None);
        context.ChangeTracker.Clear();
        var upgraded = await context.RefreshToken.SingleAsync(item => item.ID == legacy.ID);

        Assert.NotNull(refreshedUser);
        Assert.True(upgraded.IsRevoked);
        Assert.Equal(tokenService.HashToken(rawToken), upgraded.Token);
        Assert.Equal(1, cookies.RefreshCookiesSet);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task RefreshToken_RejectionIsObservableWithoutRecordingCredentialMaterial()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        var tokenService = new RotatingTestTokenService();
        var cookies = new TestCookieService($"unknown-{Guid.NewGuid():N}");
        var metrics = new TestOperationalMetrics();
        var service = new TokenRefreshService(
            new AuthService(tokenService, cookies),
            cookies,
            context,
            new FixedDateTimeProvider(DateTime.UtcNow),
            tokenService,
            metrics);

        var result = await service.TryRefreshTokenAsync(CancellationToken.None);

        Assert.Null(result);
        Assert.True(cookies.WereCleared);
        Assert.Equal(["refresh_token_rejected"], metrics.AuthenticationFailureReasons);
        Assert.DoesNotContain(metrics.AuthenticationFailureReasons, reason =>
            reason.Contains("unknown-", StringComparison.Ordinal));
    }

    [Fact]
    public async Task MigrationsApplyAndSoftDeleteConventionsWorkAgainstPostgreSql()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return; // CI always supplies PostgreSQL; local runs require TEST_DATABASE_URL to execute this test.
        }

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(
                PostgreSqlConnectionString.Normalize(connectionString),
                builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
            .Options;
        await using var context = new AppDbContext(options);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();

        var role = new Role { Name = $"integration-{Guid.NewGuid():N}" };
        context.Role.Add(role);
        await context.SaveChangesAsync();
        Assert.NotEqual(default, role.CreatedAt);

        context.Role.Remove(role);
        await context.SaveChangesAsync();

        Assert.False(await context.Role.AnyAsync(candidate => candidate.ID == role.ID));
        var persisted = await context.Role.IgnoreQueryFilters()
            .SingleAsync(candidate => candidate.ID == role.ID);
        Assert.True(persisted.IsDeleted);
        Assert.NotNull(persisted.DeletedAt);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task TransactionHelper_ParticipatesInAnExistingUnitOfWork()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        var roleId = Guid.NewGuid();
        await using (var context = CreateContext(connectionString))
        {
            await context.Database.MigrateAsync();
            await using var outerTransaction = await context.Database.BeginTransactionAsync();

            await context.ExecuteInTransactionAsync(async transactionCancellationToken =>
            {
                context.Role.Add(new Role { ID = roleId, Name = $"nested-{Guid.NewGuid():N}" });
                await context.SaveChangesAsync(transactionCancellationToken);
                return true;
            });

            Assert.NotNull(context.Database.CurrentTransaction);
            await outerTransaction.RollbackAsync();
        }

        await using var verification = CreateContext(connectionString);
        Assert.False(await verification.Role.IgnoreQueryFilters().AnyAsync(role => role.ID == roleId));
    }

    [Fact]
    public async Task TransactionHelper_RollsBackOwnedTransactionWhenOperationFails()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        var roleId = Guid.NewGuid();
        await using (var context = CreateContext(connectionString))
        {
            await context.Database.MigrateAsync();
            await Assert.ThrowsAsync<IntentionalTransactionFailure>(() =>
                context.ExecuteInTransactionAsync<bool>(async transactionCancellationToken =>
                {
                    context.Role.Add(new Role { ID = roleId, Name = $"rollback-{Guid.NewGuid():N}" });
                    await context.SaveChangesAsync(transactionCancellationToken);
                    throw new IntentionalTransactionFailure();
                }));
        }

        await using var verification = CreateContext(connectionString);
        Assert.False(await verification.Role.IgnoreQueryFilters().AnyAsync(role => role.ID == roleId));
    }

    [Fact]
    public async Task ActiveUserSkillUniqueness_IsEnforcedByPostgreSql()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();

        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = new User
        {
            Firstname = "Integration",
            Lastname = "User",
            Username = $"integration-{Guid.NewGuid():N}",
            Email = $"integration-{Guid.NewGuid():N}@example.test",
            Password = "not-a-real-password-hash",
            Role = role
        };
        var skill = new LKP_Skill { Name = $"skill-{Guid.NewGuid():N}" };
        context.AddRange(role, user, skill);
        await context.SaveChangesAsync();

        context.UserSkill.AddRange(
            new UserSkill { UserID = user.ID, LKP_SkillID = skill.ID },
            new UserSkill { UserID = user.ID, LKP_SkillID = skill.ID });

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
        var postgresException = Assert.IsType<PostgresException>(exception.InnerException);
        Assert.Equal(PostgresErrorCodes.UniqueViolation, postgresException.SqlState);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task RefreshTokenHashUniqueness_IsEnforcedByPostgreSql()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();

        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = new User
        {
            Firstname = "Integration",
            Lastname = "User",
            Username = $"integration-{Guid.NewGuid():N}",
            Email = $"integration-{Guid.NewGuid():N}@example.test",
            Password = "not-a-real-password-hash",
            Role = role
        };
        context.AddRange(role, user);
        await context.SaveChangesAsync();

        var tokenHash = Convert.ToHexString(Guid.NewGuid().ToByteArray());
        var createdAt = DateTime.UtcNow;
        context.RefreshToken.AddRange(
            new RefreshToken
            {
                Token = tokenHash,
                CreatedAt = createdAt,
                ExpiresAt = createdAt.AddDays(1),
                CreatedByIp = "127.0.0.1",
                UserID = user.ID
            },
            new RefreshToken
            {
                Token = tokenHash,
                CreatedAt = createdAt,
                ExpiresAt = createdAt.AddDays(1),
                CreatedByIp = "127.0.0.1",
                UserID = user.ID
            });

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
        var postgresException = Assert.IsType<PostgresException>(exception.InnerException);
        Assert.Equal(PostgresErrorCodes.UniqueViolation, postgresException.SqlState);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task OnlyOneActiveEmailConfirmationPerUser_IsEnforcedByPostgreSql()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();

        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = new User
        {
            Firstname = "Integration",
            Lastname = "User",
            Username = $"integration-{Guid.NewGuid():N}",
            Email = $"integration-{Guid.NewGuid():N}@example.test",
            Password = "not-a-real-password-hash",
            Role = role
        };
        context.AddRange(role, user);
        await context.SaveChangesAsync();

        var expiresAt = DateTime.UtcNow.AddHours(1);
        context.PendingEmailConfirmation.AddRange(
            new PendingEmailConfirmation
            {
                TokenHash = Convert.ToHexString(Guid.NewGuid().ToByteArray()),
                ExpiresAt = expiresAt,
                UserID = user.ID
            },
            new PendingEmailConfirmation
            {
                TokenHash = Convert.ToHexString(Guid.NewGuid().ToByteArray()),
                ExpiresAt = expiresAt,
                UserID = user.ID
            });

        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
        var postgresException = Assert.IsType<PostgresException>(exception.InnerException);
        Assert.Equal(PostgresErrorCodes.UniqueViolation, postgresException.SqlState);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task RequiredDependent_IsFilteredConsistently_WhenPrincipalIsSoftDeleted()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();

        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = new User
        {
            Firstname = "Integration",
            Lastname = "User",
            Username = $"integration-{Guid.NewGuid():N}",
            Email = $"integration-{Guid.NewGuid():N}@example.test",
            Password = "not-a-real-password-hash",
            Role = role
        };
        context.AddRange(role, user);
        await context.SaveChangesAsync();

        role.IsDeleted = true;
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        Assert.False(await context.User.AnyAsync(candidate => candidate.ID == user.ID));
        Assert.True(await context.User.IgnoreQueryFilters().AnyAsync(candidate => candidate.ID == user.ID));
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task DatabaseRejectsPhysicalPrincipalDelete_WhenDependentsExist()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();

        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = new User
        {
            Firstname = "Integration",
            Lastname = "User",
            Username = $"integration-{Guid.NewGuid():N}",
            Email = $"integration-{Guid.NewGuid():N}@example.test",
            Password = "not-a-real-password-hash",
            Role = role
        };
        context.AddRange(role, user);
        await context.SaveChangesAsync();

        var exception = await Assert.ThrowsAsync<PostgresException>(() =>
            context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM \"Role\" WHERE \"ID\" = {role.ID}"));
        Assert.Equal(PostgresErrorCodes.RestrictViolation, exception.SqlState);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task PublicPortfolioProjection_HasBoundedQueryCount()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        var interceptor = new CommandCountingInterceptor();
        await using var context = CreateContext(connectionString, interceptor);
        var configuration = new MapperConfiguration(
            expression => expression.AddProfile<Application.Client.MappingProfiles.UserMappingProfiles>(),
            NullLoggerFactory.Instance);

        var username = await context.User.AsNoTracking()
            .Where(user => user.IsConfirmed)
            .Select(user => user.Username)
            .FirstOrDefaultAsync();
        if (username is null)
        {
            return;
        }

        interceptor.Reset();
        var portfolio = await context.User.AsNoTracking()
            .Where(user => user.Username == username)
            .AsSplitQuery()
            .ProjectTo<UBUQ_Response>(configuration, new
            {
                currentPublicDate = DateOnly.FromDateTime(DateTime.UtcNow)
            })
            .SingleAsync();

        Assert.NotNull(portfolio.User);
        Assert.InRange(interceptor.CommandCount, 1, 30);
    }

    [Fact]
    public async Task OwnerContactMessagePage_UsesOneAggregateAndOnePageQuery()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        var interceptor = new CommandCountingInterceptor();
        await using var context = CreateContext(connectionString, interceptor);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var owner = CreateUser(role, "contact-page");
        context.AddRange(role, owner);
        context.ContactMessage.AddRange(
            new ContactMessage
            {
                User = owner,
                Name = "Unread sender",
                Email = "unread@example.test",
                Subject = "Unread",
                Message = "Unread message",
                IsRead = false
            },
            new ContactMessage
            {
                User = owner,
                Name = "Read sender",
                Email = "read@example.test",
                Subject = "Read",
                Message = "Read message",
                IsRead = true
            });
        await context.SaveChangesAsync();

        var mapping = new MapperConfiguration(
            expression => expression.AddProfile<Application.Owner.MappingProfiles.ContactMessageMappingProfiles>(),
            NullLoggerFactory.Instance);
        var handler = new Application.Owner.Handlers.ContactMessageHandlers.ContactMessageListQueryHandler(
            context,
            mapping.CreateMapper(),
            new TestCurrentUser(owner.ID));

        interceptor.Reset();
        var response = await handler.Handle(
            new Application.Owner.Queries.ContactMessageQueries.ContactMessageListQuery
            {
                PageNumber = 0,
                PageSize = 20
            },
            CancellationToken.None);

        Assert.Equal(2, response.RowCount);
        Assert.Equal(1, response.UnreadContactMessageCount);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(2, interceptor.CommandCount);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task PublicPortfolioLookup_DoesNotExposeUnconfirmedAccount()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "private-unconfirmed");
        user.IsConfirmed = false;
        context.AddRange(role, user);
        await context.SaveChangesAsync();
        var mapping = new MapperConfiguration(
            expression => expression.AddProfile<Application.Client.MappingProfiles.UserMappingProfiles>(),
            NullLoggerFactory.Instance);
        var handler = new UserByUsernameQueryHandler(
            context,
            mapping.CreateMapper(),
            new FixedDateTimeProvider(DateTime.UtcNow));

        var response = await handler.Handle(
            new UserByUsernameQuery { Username = user.Username },
            CancellationToken.None);

        Assert.Null(response.Data);
        Assert.Contains("Wrong username.", response.lstError);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task OwnerFullInfoProjection_BoundsLegacyCollections()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "bounded-owner");
        context.AddRange(role, user);
        for (var index = 0; index < PublicPortfolioLimits.MaxCollectionItems + 1; index++)
        {
            context.SocialLink.Add(new SocialLink
            {
                User = user,
                Platform = $"platform-{index:D3}",
                Url = $"https://example.test/{index:D3}"
            });
        }
        await context.SaveChangesAsync();

        var mapping = new MapperConfiguration(
            expression => expression.AddProfile<Application.Owner.MappingProfiles.UserMappingProfiles>(),
            NullLoggerFactory.Instance);
        var handler = new UserFullInfoQueryHandler(
            context,
            mapping.CreateMapper(),
            new TestCurrentUser(user.ID));

        var response = await handler.Handle(new UserFullInfoQuery(), CancellationToken.None);

        Assert.NotNull(response.Data);
        Assert.Equal(PublicPortfolioLimits.MaxCollectionItems, response.Data.LstSocialLinks.Count);
        Assert.Equal(
            PublicPortfolioLimits.MaxCollectionItems,
            response.Data.LstSocialLinks.Select(link => link.ID).Distinct().Count());
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task PreferenceEdits_RejectMissingLookupIdsBeforePersistence()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        var currentUser = new TestCurrentUser(Guid.NewGuid());
        var preferenceHandler = new EditUserPreferenceCommandHandler(context, currentUser, mapper: null!);
        var chartHandler = new EditUserChartPreferenceCommandHandler(context, currentUser, mapper: null!);

        var preference = await preferenceHandler.Handle(new EditUserPreferenceCommand
        {
            LKP_PreferenceID = Guid.NewGuid(),
            Value = "true"
        }, CancellationToken.None);
        var chart = await chartHandler.Handle(new EditUserChartPreferenceCommand
        {
            LKP_WidgetID = Guid.NewGuid(),
            LKP_ChartTypeID = Guid.NewGuid(),
            GroupBy = "skill"
        }, CancellationToken.None);

        Assert.Contains("Preference not found.", preference.lstError);
        Assert.Contains("Widget or chart type not found.", chart.lstError);
        Assert.DoesNotContain(context.ChangeTracker.Entries(), entry => entry.State != EntityState.Unchanged);
    }

    [Fact]
    public async Task UserLanguageEdit_ReconcilesCompositeKeyRowsWithoutReplacementConflicts()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "language-owner");
        var firstLanguage = new LKP_Language { Name = $"language-{Guid.NewGuid():N}" };
        var secondLanguage = new LKP_Language { Name = $"language-{Guid.NewGuid():N}" };
        var firstProficiency = new LKP_LanguageProficiency { Level = $"level-{Guid.NewGuid():N}" };
        var secondProficiency = new LKP_LanguageProficiency { Level = $"level-{Guid.NewGuid():N}" };
        context.AddRange(role, user, firstLanguage, secondLanguage, firstProficiency, secondProficiency);
        await context.SaveChangesAsync();
        context.UserLanguage.Add(new UserLanguage
        {
            UserID = user.ID,
            LKP_LanguageID = firstLanguage.ID,
            LKP_LanguageProficiencyID = firstProficiency.ID
        });
        await context.SaveChangesAsync();

        var handler = new EditDeleteUserLanguageCommandHandler(context, new TestCurrentUser(user.ID));
        var response = await handler.Handle(new EditDeleteUserLanguageCommand
        {
            LstLanguages =
            [
                new EDULC_LKP_Language
                {
                    LKP_LanguageID = firstLanguage.ID,
                    LKP_LanguageProficiencyID = secondProficiency.ID
                },
                new EDULC_LKP_Language
                {
                    LKP_LanguageID = secondLanguage.ID,
                    LKP_LanguageProficiencyID = firstProficiency.ID
                }
            ]
        }, CancellationToken.None);

        Assert.Empty(response.lstError);
        var rows = await context.UserLanguage.AsNoTracking()
            .Where(language => language.UserID == user.ID)
            .OrderBy(language => language.LKP_LanguageID)
            .ToListAsync();
        Assert.Equal(2, rows.Count);
        Assert.Contains(rows, language =>
            language.LKP_LanguageID == firstLanguage.ID &&
            language.LKP_LanguageProficiencyID == secondProficiency.ID);
        Assert.Contains(rows, language => language.LKP_LanguageID == secondLanguage.ID);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task UserSkillEdit_RejectsRelationsOwnedByAnotherUser()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();

        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var owner = CreateUser(role, "owner");
        var otherOwner = CreateUser(role, "other");
        var skill = new LKP_Skill { Name = $"skill-{Guid.NewGuid():N}" };
        var otherProject = new Project { Title = "Other owner's project", User = otherOwner };
        context.AddRange(role, owner, otherOwner, skill, otherProject);
        await context.SaveChangesAsync();

        var handler = new EditDeleteUserSkillCommandHandler(
            context,
            new TestCurrentUser(owner.ID));
        var command = new EditDeleteUserSkillCommand
        {
            LstUserSkills =
            [
                new EDUSC_UserSkill
                {
                    LKP_SkillID = skill.ID,
                    ProjectIDs = [otherProject.ID]
                }
            ]
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Contains(result.lstError, error => error.Contains("owned by the current user", StringComparison.Ordinal));
        Assert.False(await context.UserSkill.AnyAsync(candidate => candidate.UserID == owner.ID));
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task UserSkillEdit_ReconcilesSkillsAndCompositeRelationsWithoutGraphReplacement()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var owner = CreateUser(role, "skill-reconcile");
        var retainedSkill = new LKP_Skill { Name = $"skill-{Guid.NewGuid():N}" };
        var removedSkill = new LKP_Skill { Name = $"skill-{Guid.NewGuid():N}" };
        var addedSkill = new LKP_Skill { Name = $"skill-{Guid.NewGuid():N}" };
        var firstProject = new Project { Title = "First project", User = owner };
        var secondProject = new Project { Title = "Second project", User = owner };
        context.AddRange(
            role,
            owner,
            retainedSkill,
            removedSkill,
            addedSkill,
            firstProject,
            secondProject);
        await context.SaveChangesAsync();
        var retained = new UserSkill { UserID = owner.ID, LKP_SkillID = retainedSkill.ID };
        var removed = new UserSkill { UserID = owner.ID, LKP_SkillID = removedSkill.ID };
        retained.LstProjects.Add(new UserSkillProject { ProjectID = firstProject.ID });
        context.UserSkill.AddRange(retained, removed);
        await context.SaveChangesAsync();

        var handler = new EditDeleteUserSkillCommandHandler(context, new TestCurrentUser(owner.ID));
        var response = await handler.Handle(new EditDeleteUserSkillCommand
        {
            LstUserSkills =
            [
                new EDUSC_UserSkill
                {
                    LKP_SkillID = retainedSkill.ID,
                    ProjectIDs = [secondProject.ID]
                },
                new EDUSC_UserSkill
                {
                    LKP_SkillID = addedSkill.ID,
                    ProjectIDs = [firstProject.ID]
                }
            ]
        }, CancellationToken.None);

        Assert.Empty(response.lstError);
        var active = await context.UserSkill.AsNoTracking()
            .Where(skill => skill.UserID == owner.ID)
            .Include(skill => skill.LstProjects)
            .ToListAsync();
        Assert.Equal(2, active.Count);
        Assert.Contains(active, skill =>
            skill.LKP_SkillID == retainedSkill.ID &&
            skill.LstProjects.Single().ProjectID == secondProject.ID);
        Assert.Contains(active, skill =>
            skill.LKP_SkillID == addedSkill.ID &&
            skill.LstProjects.Single().ProjectID == firstProject.ID);
        Assert.True(await context.UserSkill.IgnoreQueryFilters().AsNoTracking().AnyAsync(skill =>
            skill.UserID == owner.ID && skill.LKP_SkillID == removedSkill.ID && skill.IsDeleted));
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task ConcurrentOwnerOrderingUpdate_RejectsStaleWriter()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        Guid roleId;
        Guid userId;
        Guid[] projectIds;
        await using (var setup = CreateContext(connectionString))
        {
            await setup.Database.MigrateAsync();
            var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
            var user = CreateUser(role, "concurrency");
            var projects = new[]
            {
                new Project { Title = "Concurrent project 1", User = user, Order = 1 },
                new Project { Title = "Concurrent project 2", User = user, Order = 2 },
                new Project { Title = "Concurrent project 3", User = user, Order = 3 }
            };
            setup.AddRange(role, user);
            setup.Project.AddRange(projects);
            await setup.SaveChangesAsync();
            roleId = role.ID;
            userId = user.ID;
            projectIds = projects.Select(project => project.ID).ToArray();
        }

        try
        {
            await using var firstContext = CreateContext(connectionString);
            await using var secondContext = CreateContext(connectionString);
            var first = await firstContext.Project
                .Where(project => projectIds.Contains(project.ID))
                .OrderBy(project => project.ID)
                .ToDictionaryAsync(project => project.ID);
            var stale = await secondContext.Project
                .Where(project => projectIds.Contains(project.ID))
                .OrderBy(project => project.ID)
                .ToDictionaryAsync(project => project.ID);
            var orderedIds = first.Keys.OrderBy(id => id).ToArray();
            var originalOrders = stale.ToDictionary(pair => pair.Key, pair => pair.Value.Order);

            first[orderedIds[2]].Order = 4;
            await firstContext.SaveChangesAsync();
            stale[orderedIds[0]].Order = 2;
            stale[orderedIds[2]].Order = 1;

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => secondContext.SaveChangesAsync());

            await using var verification = CreateContext(connectionString);
            var committedOrders = await verification.Project.AsNoTracking()
                .Where(project => projectIds.Contains(project.ID))
                .OrderBy(project => project.ID)
                .Select(project => new { project.ID, project.Order })
                .ToListAsync();
            Assert.Equal(originalOrders[orderedIds[0]], committedOrders.Single(project => project.ID == orderedIds[0]).Order);
            Assert.Equal(originalOrders[orderedIds[1]], committedOrders.Single(project => project.ID == orderedIds[1]).Order);
            Assert.Equal(4, committedOrders.Single(project => project.ID == orderedIds[2]).Order);
        }
        finally
        {
            await using var cleanup = CreateContext(connectionString);
            await cleanup.Project.IgnoreQueryFilters().Where(project => projectIds.Contains(project.ID)).ExecuteDeleteAsync();
            await cleanup.User.IgnoreQueryFilters().Where(user => user.ID == userId).ExecuteDeleteAsync();
            await cleanup.Role.IgnoreQueryFilters().Where(role => role.ID == roleId).ExecuteDeleteAsync();
        }
    }

    [Fact]
    public async Task ProjectReorder_RejectsPartialOwnerCollection()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "reorder");
        var first = new Project { Title = "First", User = user, Order = 1 };
        var second = new Project { Title = "Second", User = user, Order = 2 };
        context.AddRange(role, user, first, second);
        await context.SaveChangesAsync();

        var handler = new SortProjectCommandHandler(context, new TestCurrentUser(user.ID));
        var response = await handler.Handle(
            new SortProjectCommand { ProjectIdsInOrder = [first.ID] },
            CancellationToken.None);

        Assert.Contains(response.lstError, error => error.Contains("every active project", StringComparison.Ordinal));
        Assert.Equal(1, first.Order);
        Assert.Equal(2, second.Order);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task BlogPostCreate_PersistsPublicationAndDraftStatus()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "blog");
        context.AddRange(role, user);
        await context.SaveChangesAsync();

        var mapperConfiguration = new MapperConfiguration(
            expression => expression.AddProfile<BlogPostMappingProfiles>(),
            NullLoggerFactory.Instance);
        var handler = new AddEditBlogPostCommandHandler(
            context,
            new TestCurrentUser(user.ID),
            mapperConfiguration.CreateMapper(),
            new FixedDateTimeProvider(new DateTime(2026, 8, 25, 12, 0, 0, DateTimeKind.Utc)));
        var publishedAt = new DateOnly(2026, 8, 25);

        var result = await handler.Handle(new AddEditBlogPostCommand
        {
            Title = "Production hardening",
            Slug = $"production-hardening-{Guid.NewGuid():N}",
            Content = "Verified content",
            PublishedAt = publishedAt
        }, CancellationToken.None);

        Assert.Empty(result.lstError);
        var blogPost = await context.BlogPost.SingleAsync(post => post.UserID == user.ID);
        Assert.Equal(publishedAt, blogPost.PublishedAt);
        Assert.Equal(BlogPostStatusIdentifiers.Draft, blogPost.LKP_BlogPostStatusID);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task BlogPostPublishing_ExposesOnlyPublishedContentAndRejectsUnsupportedStatus()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var now = new DateTime(2026, 8, 25, 12, 0, 0, DateTimeKind.Utc);
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "published-blog");
        user.IsConfirmed = true;
        context.AddRange(role, user);
        await context.SaveChangesAsync();
        var commandMapping = new MapperConfiguration(
            expression => expression.AddProfile<BlogPostMappingProfiles>(),
            NullLoggerFactory.Instance);
        var commandHandler = new AddEditBlogPostCommandHandler(
            context,
            new TestCurrentUser(user.ID),
            commandMapping.CreateMapper(),
            new FixedDateTimeProvider(now));

        var draftResult = await commandHandler.Handle(new AddEditBlogPostCommand
        {
            Title = "Private draft",
            Slug = $"private-draft-{Guid.NewGuid():N}",
            Content = "Not public",
            PublishedAt = DateOnly.FromDateTime(now)
        }, CancellationToken.None);
        var publishedResult = await commandHandler.Handle(new AddEditBlogPostCommand
        {
            Title = "Public post",
            Slug = $"public-post-{Guid.NewGuid():N}",
            Content = "Public content",
            PublishedAt = DateOnly.FromDateTime(now),
            LKP_BlogPostStatusID = BlogPostStatusIdentifiers.Published
        }, CancellationToken.None);
        var rejectedResult = await commandHandler.Handle(new AddEditBlogPostCommand
        {
            Title = "Unsupported transition",
            Slug = $"unsupported-{Guid.NewGuid():N}",
            Content = "Must not persist",
            PublishedAt = DateOnly.FromDateTime(now),
            LKP_BlogPostStatusID = Guid.NewGuid()
        }, CancellationToken.None);

        Assert.Empty(draftResult.lstError);
        Assert.Empty(publishedResult.lstError);
        Assert.Contains(rejectedResult.lstError, error => error.Contains("Draft and Published", StringComparison.Ordinal));
        var futurePublished = new BlogPost
        {
            UserID = user.ID,
            Title = "Future public post",
            Slug = $"future-public-{Guid.NewGuid():N}",
            Content = "Must remain private",
            PublishedAt = DateOnly.FromDateTime(now.AddDays(1)),
            LKP_BlogPostStatusID = BlogPostStatusIdentifiers.Published
        };
        context.BlogPost.Add(futurePublished);
        await context.SaveChangesAsync();
        var publicMapping = new MapperConfiguration(
            expression => expression.AddProfile<Application.Client.MappingProfiles.UserMappingProfiles>(),
            NullLoggerFactory.Instance);
        var publicHandler = new UserByUsernameQueryHandler(
            context,
            publicMapping.CreateMapper(),
            new FixedDateTimeProvider(now));
        var publicProfile = await publicHandler.Handle(
            new UserByUsernameQuery { Username = user.Username },
            CancellationToken.None);

        Assert.NotNull(publicProfile.Data);
        Assert.Single(publicProfile.Data.LstBlogPosts);
        Assert.Equal("Public post", publicProfile.Data.LstBlogPosts[0].Title);
        Assert.Equal(3, await context.BlogPost.CountAsync(post => post.UserID == user.ID));
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task ProjectCreate_RejectsExperienceOwnedByAnotherUser()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var owner = CreateUser(role, "project-owner");
        var otherOwner = CreateUser(role, "project-other");
        var otherExperience = new Experience
        {
            JobTitle = "Other",
            CompanyName = "Other company",
            Location = "Other location",
            StartDate = new DateOnly(2025, 1, 1),
            User = otherOwner
        };
        context.AddRange(role, owner, otherOwner, otherExperience);
        await context.SaveChangesAsync();

        var mapperConfiguration = new MapperConfiguration(
            expression => expression.AddProfile<ProjectMappingProfiles>(),
            NullLoggerFactory.Instance);
        var handler = new AddEditProjectCommandHandler(
            context,
            new TestCurrentUser(owner.ID),
            mapperConfiguration.CreateMapper(),
            new UserSkillRelationService(context));

        var result = await handler.Handle(new AddEditProjectCommand
        {
            Title = "Invalid cross-owner project",
            ExperienceID = otherExperience.ID
        }, CancellationToken.None);

        Assert.Contains(result.lstError, error => error.Contains("current user", StringComparison.Ordinal));
        Assert.False(await context.Project.AnyAsync(project => project.UserID == owner.ID));
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task CertificateEdit_ReplacesOnlyCurrentOwnersActiveMedia()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var owner = CreateUser(role, "certificate-owner");
        var otherOwner = CreateUser(role, "certificate-other");
        var certificateType = new LKP_Certificate { Name = $"certificate-{Guid.NewGuid():N}" };
        var ownerCertificate = new Certificate
        {
            User = owner,
            LKP_Certificate = certificateType,
            LstCertificateMedias =
            [
                new CertificateMedia { Url = "https://cdn.example/owner-old.png" },
                new CertificateMedia { Url = "https://cdn.example/owner-retained.png" }
            ]
        };
        var otherCertificate = new Certificate
        {
            User = otherOwner,
            LKP_Certificate = certificateType,
            LstCertificateMedias =
            [
                new CertificateMedia { Url = "https://cdn.example/other.png" }
            ]
        };
        context.AddRange(role, owner, otherOwner, certificateType, ownerCertificate, otherCertificate);
        await context.SaveChangesAsync();
        var retainedMediaId = ownerCertificate.LstCertificateMedias.Single(
            media => media.Url.EndsWith("owner-retained.png", StringComparison.Ordinal)).ID;
        context.ChangeTracker.Clear();

        var mapperConfiguration = new MapperConfiguration(
            expression => expression.AddProfile<CertificateMappingProfiles>(),
            NullLoggerFactory.Instance);
        var handler = new AddEditCertificateCommandHandler(
            context,
            new TestCurrentUser(owner.ID),
            mapperConfiguration.CreateMapper(),
            new UserSkillRelationService(context));

        var result = await handler.Handle(new AddEditCertificateCommand
        {
            ID = ownerCertificate.ID,
            LKP_CertificateID = certificateType.ID,
            LstCertificateMedias =
            [
                "https://cdn.example/owner-retained.png",
                "https://cdn.example/owner-new.png"
            ]
        }, CancellationToken.None);

        Assert.Empty(result.lstError);
        context.ChangeTracker.Clear();
        var ownerMedia = await context.CertificateMedia.IgnoreQueryFilters()
            .Where(media => media.CertificateID == ownerCertificate.ID)
            .OrderBy(media => media.Url)
            .ToListAsync();
        Assert.Equal(3, ownerMedia.Count);
        Assert.Contains(ownerMedia, media =>
            media.Url == "https://cdn.example/owner-new.png" && !media.IsDeleted);
        Assert.Contains(ownerMedia, media =>
            media.Url == "https://cdn.example/owner-old.png" && media.IsDeleted);
        var retainedMedia = Assert.Single(ownerMedia, media =>
            media.Url == "https://cdn.example/owner-retained.png");
        Assert.Equal(retainedMediaId, retainedMedia.ID);
        Assert.False(retainedMedia.IsDeleted);
        var otherMedia = await context.CertificateMedia.SingleAsync(
            media => media.CertificateID == otherCertificate.ID);
        Assert.Equal("https://cdn.example/other.png", otherMedia.Url);
        Assert.False(otherMedia.IsDeleted);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task LookupName_CanBeReusedAfterSoftDelete_ButNotWhileActive()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var name = $"Reusable{Guid.NewGuid():N}";
        var original = new Role { Name = name };
        context.Role.Add(original);
        await context.SaveChangesAsync();

        original.IsDeleted = true;
        await context.SaveChangesAsync();
        context.Role.Add(new Role { Name = name });
        await context.SaveChangesAsync();

        context.Role.Add(new Role { Name = name });
        var exception = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
        Assert.Equal(PostgresErrorCodes.UniqueViolation, Assert.IsType<PostgresException>(exception.InnerException).SqlState);
        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task PreferenceDelete_RejectsAssignedPreference()
    {
        var connectionString = Environment.GetEnvironmentVariable("TEST_DATABASE_URL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        await using var context = CreateContext(connectionString);
        await context.Database.MigrateAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var preference = await context.LKP_Preference.FirstAsync();
        var role = new Role { Name = $"role-{Guid.NewGuid():N}" };
        var user = CreateUser(role, "preference");
        context.AddRange(role, user, new UserPreference
        {
            User = user,
            LKP_Preference = preference,
            Value = "true"
        });
        await context.SaveChangesAsync();

        var handler = new DeleteLKP_PreferenceCommandHandler(context);
        var response = await handler.Handle(
            new DeleteLKP_PreferenceCommand { ID = preference.ID },
            CancellationToken.None);

        Assert.Contains(response.lstError, error => error.Contains("assigned to users", StringComparison.Ordinal));
        Assert.False(preference.IsDeleted);
        await transaction.RollbackAsync();
    }

    private static AppDbContext CreateContext(
        string connectionString,
        params IInterceptor[] interceptors)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(
                PostgreSqlConnectionString.Normalize(connectionString),
                builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
            .UsePortfolioQuerySafety();
        if (interceptors.Length > 0)
        {
            optionsBuilder.AddInterceptors(interceptors);
        }

        return new AppDbContext(optionsBuilder.Options);
    }

    private static EmailOutboxService CreateOutboxService(
        AppDbContext context,
        TestNotificationService notifications,
        IDateTimeProvider clock,
        ILogger<EmailOutboxService>? logger = null) => new(
            context,
            notifications,
            new TestTokenService(),
            clock,
            new TestOperationalMetrics(),
            logger ?? NullLogger<EmailOutboxService>.Instance);

    private static User CreateUser(Role role, string prefix) => new()
    {
        Firstname = "Integration",
        Lastname = "User",
        Username = $"{prefix}-{Guid.NewGuid():N}",
        Email = $"{prefix}-{Guid.NewGuid():N}@example.test",
        Password = "not-a-real-password-hash",
        Role = role
    };

    private static ContactMessage CreateContact(User user, string subject) => new()
    {
        ID = Guid.NewGuid(),
        User = user,
        Name = "Contact",
        Email = "contact@example.test",
        Subject = subject,
        Message = "Message"
    };

    private sealed class TestCurrentUser(Guid userId) : ICurrentUserService
    {
        public bool IsAuthenticated => true;
        public Guid? UserID => userId;
        public string? Role => "Owner";
        public string? Username => "integration-user";
        public bool IsConfirmed => true;
        public string? IpAddress => "127.0.0.1";
    }

    private sealed class AnonymousCurrentUser : ICurrentUserService
    {
        public bool IsAuthenticated => false;
        public Guid? UserID => null;
        public string? Role => null;
        public string? Username => null;
        public bool IsConfirmed => false;
        public string? IpAddress => "127.0.0.1";
    }

    private sealed class TestCookieService(string? refreshToken) : ICookieService
    {
        public bool WereCleared { get; private set; }
        public int RefreshCookiesSet { get; private set; }
        public string? GetRefreshToken() => refreshToken;
        public void SetAccessToken(string token) { }
        public void SetRefreshToken(string token, bool rememberMe) => RefreshCookiesSet++;
        public void ClearAuthCookies() => WereCleared = true;
    }

    private sealed class FixedDateTimeProvider(DateTime utcNow) : IDateTimeProvider
    {
        public DateTime UtcNow { get; } = utcNow;
    }

    private sealed class AdvancingDateTimeProvider(DateTime utcNow) : IDateTimeProvider
    {
        public DateTime UtcNow { get; private set; } = utcNow;

        public void Advance(TimeSpan duration) => UtcNow = UtcNow.Add(duration);
    }

    private sealed class TestNotificationService : IUserNotificationService
    {
        public bool Fail { get; init; }
        public bool FailFirst { get; init; }
        public bool TimeOut { get; init; }
        public CancellationTokenSource? CallerCancellation { get; init; }
        public Func<int, Task>? OnContactDelivery { get; init; }
        private int _contactNotifications;
        private int _contactDeliveryCalls;
        private int _deliveryAttempts;
        public int ContactNotifications => Volatile.Read(ref _contactNotifications);

        public Task SendEmailConfirmationAsync(
            User user,
            string rawToken,
            CancellationToken cancellationToken) => Task.CompletedTask;

        public async Task SendContactMessageNotificationEmail(
            Application.Client.Commands.SendEmailCommand contactMessage,
            CancellationToken cancellationToken)
        {
            if (OnContactDelivery is not null)
            {
                await OnContactDelivery(Interlocked.Increment(ref _contactDeliveryCalls));
            }
            if (CallerCancellation is not null)
            {
                CallerCancellation.Cancel();
                throw new OperationCanceledException(CallerCancellation.Token);
            }

            if (TimeOut)
            {
                throw new OperationCanceledException("Dependency delivery timeout");
            }

            if (Fail)
            {
                throw new InvalidOperationException("smtp-secret must never be persisted");
            }

            if (FailFirst && Interlocked.Increment(ref _deliveryAttempts) == 1)
            {
                throw new InvalidOperationException("first delivery fails");
            }

            Interlocked.Increment(ref _contactNotifications);
        }
    }

    private sealed class TestTokenService : ITokenService
    {
        public string GenerateAccessToken(User user) => "access-token";
        public (RefreshToken Entity, string RawToken) GenerateRefreshToken(bool rememberMe)
            => (new RefreshToken(), "refresh-token");
        public string GenerateRawToken() => "raw-token";
        public string DeriveEmailConfirmationToken(Guid confirmationId) => $"confirmation-{confirmationId:N}";
        public string RecoverEmailConfirmationToken(Guid confirmationId, string expectedHash) =>
            DeriveEmailConfirmationToken(confirmationId);
        public string HashToken(string rawToken) => $"hash-{rawToken}";
    }

    private sealed class RotatingTestTokenService : ITokenService
    {
        private int _sequence;

        public string GenerateAccessToken(User user) => $"access-{user.ID}";

        public (RefreshToken Entity, string RawToken) GenerateRefreshToken(bool rememberMe)
        {
            var rawToken = $"replacement-{Interlocked.Increment(ref _sequence)}";
            return (new RefreshToken
            {
                Token = HashToken(rawToken),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                CreatedByIp = "127.0.0.1",
                RememberMe = rememberMe
            }, rawToken);
        }

        public string GenerateRawToken() => $"raw-{Interlocked.Increment(ref _sequence)}";
        public string DeriveEmailConfirmationToken(Guid confirmationId) => $"confirmation-{confirmationId:N}";
        public string RecoverEmailConfirmationToken(Guid confirmationId, string expectedHash) =>
            DeriveEmailConfirmationToken(confirmationId);
        public string HashToken(string rawToken) => $"hash-{rawToken}";
    }

    private sealed class TestOperationalMetrics : IOperationalMetrics
    {
        private readonly System.Collections.Concurrent.ConcurrentQueue<string> _authenticationFailureReasons = new();
        public IReadOnlyCollection<string> AuthenticationFailureReasons => _authenticationFailureReasons.ToArray();
        public void RecordAuthenticationFailure(string reason) => _authenticationFailureReasons.Enqueue(reason);
        public void RecordEmailDelivery(string outcome, string kind) { }
        public void RecordReadinessFailure(string dependency) { }
        public void RecordMaintenanceRun(string job, string outcome) { }
        public void RecordRequestTimeout() { }
        public void RecordRateLimitRejection(string policy) { }
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<string> Messages { get; } = [];
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) =>
            Messages.Add(formatter(state, exception));
    }

    private sealed class CommandCountingInterceptor : DbCommandInterceptor
    {
        private int _commandCount;
        public int CommandCount => Volatile.Read(ref _commandCount);

        public void Reset() => Interlocked.Exchange(ref _commandCount, 0);

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            Interlocked.Increment(ref _commandCount);
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }
    }

    private sealed class IntentionalTransactionFailure : Exception
    {
    }

}
