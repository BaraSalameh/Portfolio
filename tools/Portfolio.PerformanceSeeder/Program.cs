using DataAccess;
using DataAccess.DbContexts;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Text.Json;

const string Prefix = "perf-";
const string Confirmation = "SEED_ISOLATED_PREVIEW";

var options = Arguments.Parse(args);
if (!options.Confirmed)
{
    return Fail($"Refusing to write. Pass --confirm {Confirmation} after verifying the target is an isolated Preview database.");
}

var rawConnection = Environment.GetEnvironmentVariable("PERFORMANCE_DATABASE_URL_UNPOOLED");
if (string.IsNullOrWhiteSpace(rawConnection))
{
    return Fail("PERFORMANCE_DATABASE_URL_UNPOOLED is required. Generic runtime/production database variables are deliberately ignored.");
}

PostgreSqlConnectionString.EnsureDirectMigrationEndpoint(rawConnection);
PostgreSqlConnectionString.EnsureSecureRemoteTransport(rawConnection);
var connectionString = PostgreSqlConnectionString.Normalize(rawConnection);
var identity = new NpgsqlConnectionStringBuilder(connectionString);
if (!string.Equals(identity.Database, options.ExpectedDatabase, StringComparison.Ordinal))
{
    return Fail($"Connected database name '{identity.Database}' does not match --expected-database '{options.ExpectedDatabase}'.");
}
if (!string.Equals(identity.Host, options.ExpectedHost, StringComparison.OrdinalIgnoreCase))
{
    return Fail($"Connected endpoint '{identity.Host}' does not match --expected-host '{options.ExpectedHost}'.");
}

var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseNpgsql(connectionString, builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
    .UsePortfolioQuerySafety()
    .Options;
await using var db = new AppDbContext(dbOptions);

if ((await db.Database.GetPendingMigrationsAsync()).Any())
{
    return Fail("The target has pending migrations. Apply the reviewed migration range before seeding performance data.");
}

var existingRealUsers = await db.User.CountAsync(user => !user.Username.StartsWith(Prefix));
if (existingRealUsers > options.MaximumExistingUsers)
{
    return Fail($"The target contains {existingRealUsers} non-performance users, above the allowed maximum of {options.MaximumExistingUsers}. Refusing to seed.");
}

var existingOwner = await db.User.SingleOrDefaultAsync(user => user.Username == "perf-owner");
if (existingOwner is null)
{
    await SeedAsync(db, options.UserCount, options.Password);
}

var owner = await db.User.AsNoTracking().SingleAsync(user => user.Username == "perf-owner");
var manifest = new
{
    marker = Prefix,
    database = identity.Database,
    generatedAtUtc = DateTime.UtcNow,
    ownerUsername = owner.Username,
    ownerEmail = owner.Email,
    counts = new
    {
        confirmedUsers = await db.User.CountAsync(user => user.IsConfirmed),
        ownerProjects = await db.Project.CountAsync(entity => entity.UserID == owner.ID),
        ownerExperiences = await db.Experience.CountAsync(entity => entity.UserID == owner.ID),
        ownerEducations = await db.Education.CountAsync(entity => entity.UserID == owner.ID),
        ownerCertificates = await db.Certificate.CountAsync(entity => entity.UserID == owner.ID),
        ownerSkills = await db.UserSkill.CountAsync(entity => entity.UserID == owner.ID),
        ownerContactMessages = await db.ContactMessage.CountAsync(entity => entity.UserID == owner.ID)
    }
};
Console.WriteLine(JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true }));
return 0;

static async Task SeedAsync(AppDbContext db, int userCount, string password)
{
    var passwordHasher = new PasswordHasher<User>(Options.Create(new PasswordHasherOptions
    {
        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3,
        IterationCount = 100_000
    }));
    var owner = new User
    {
        ID = Guid.NewGuid(),
        Firstname = "Performance",
        Lastname = "Owner",
        Username = "perf-owner",
        Email = "perf-owner@example.invalid",
        Title = "Representative portfolio owner",
        Bio = new string('x', 1000),
        RoleID = RoleIdentifiers.Owner,
        IsConfirmed = true
    };
    owner.Password = passwordHasher.HashPassword(owner, password);
    db.User.Add(owner);
    await db.SaveChangesAsync();

    // Use bounded batches so the seeder itself does not need production-sized memory.
    for (var start = 1; start < userCount; start += 500)
    {
        var end = Math.Min(userCount, start + 500);
        for (var index = start; index < end; index++)
        {
            db.User.Add(new User
            {
                ID = Guid.NewGuid(),
                Firstname = "Synthetic",
                Lastname = $"User {index:D5}",
                Username = $"perf-user-{index:D5}",
                Email = $"perf-user-{index:D5}@example.invalid",
                Password = owner.Password,
                RoleID = RoleIdentifiers.Owner,
                IsConfirmed = true
            });
        }
        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();
    }

    owner = await db.User.SingleAsync(user => user.Username == "perf-owner");
    var skills = await db.LKP_Skill.OrderBy(skill => skill.Name).Take(30).ToListAsync();
    for (var index = skills.Count; index < 30; index++)
    {
        skills.Add(new LKP_Skill { ID = Guid.NewGuid(), Name = $"Performance Skill {index + 1:D2}", IconUrl = "https://example.invalid/skill.svg" });
    }
    db.LKP_Skill.AddRange(skills.Where(skill => db.Entry(skill).State == EntityState.Detached));

    var institutions = await db.LKP_Institution.Take(1).ToListAsync();
    var degrees = await db.LKP_Degree.Take(1).ToListAsync();
    var fields = await db.LKP_FieldOfStudy.Take(1).ToListAsync();
    var certificateTypes = await db.LKP_Certificate.Take(20).ToListAsync();
    if (institutions.Count == 0 || degrees.Count == 0 || fields.Count == 0 || certificateTypes.Count < 20)
    {
        throw new InvalidOperationException("Required lookup seed data is missing.");
    }

    var experiences = Enumerable.Range(0, 10).Select(index => new Experience
    {
        ID = Guid.NewGuid(),
        UserID = owner.ID,
        JobTitle = $"Role {index + 1}",
        CompanyName = $"Company {index + 1}",
        Location = "Synthetic",
        Description = new string('e', 500),
        StartDate = new DateOnly(2010 + index, 1, 1),
        Order = index
    }).ToList();
    var educations = Enumerable.Range(0, 10).Select(index => new Education
    {
        ID = Guid.NewGuid(),
        UserID = owner.ID,
        LKP_InstitutionID = institutions[0].ID,
        LKP_DegreeID = degrees[0].ID,
        LKP_FieldOfStudyID = fields[0].ID,
        StartDate = new DateOnly(2000 + index, 1, 1),
        Description = new string('d', 500),
        Order = index
    }).ToList();
    var projects = Enumerable.Range(0, 20).Select(index => new Project
    {
        ID = Guid.NewGuid(),
        UserID = owner.ID,
        Title = $"Performance Project {index + 1}",
        Description = new string('p', 1000),
        LiveLink = "https://example.invalid/project",
        SourceCode = "https://example.invalid/source",
        ImageUrl = "https://example.invalid/image.png",
        IsFeatured = index < 5,
        Order = index,
        ExperienceID = experiences[index % experiences.Count].ID,
        EducationID = educations[index % educations.Count].ID
    }).ToList();
    var certificates = Enumerable.Range(0, 20).Select(index => new Certificate
    {
        ID = Guid.NewGuid(),
        UserID = owner.ID,
        LKP_CertificateID = certificateTypes[index].ID,
        IssueDate = new DateOnly(2020, 1, 1).AddMonths(index),
        CredintialID = $"PERF-{index:D3}",
        CredintialUrl = "https://example.invalid/certificate",
        Order = index
    }).ToList();
    var userSkills = skills.Select(skill => new UserSkill
    {
        ID = Guid.NewGuid(),
        UserID = owner.ID,
        LKP_SkillID = skill.ID
    }).ToList();
    db.AddRange(experiences); db.AddRange(educations); db.AddRange(projects); db.AddRange(certificates); db.AddRange(userSkills);
    db.AddRange(Enumerable.Range(0, 100).Select(index => new ContactMessage
    {
        ID = Guid.NewGuid(),
        UserID = owner.ID,
        Name = $"Sender {index}",
        Email = $"sender-{index}@example.invalid",
        Subject = $"Synthetic message {index}",
        Message = new string('m', 500),
        IsRead = index % 2 == 0
    }));
    await db.SaveChangesAsync();

    db.AddRange(projects.SelectMany((project, projectIndex) => userSkills.Take(5).Select(skill =>
        new UserSkillProject { ProjectID = project.ID, UserSkillID = skill.ID })));
    db.AddRange(experiences.SelectMany(experience => userSkills.Take(5).Select(skill =>
        new UserSkillExperience { ExperienceID = experience.ID, UserSkillID = skill.ID })));
    db.AddRange(educations.SelectMany(education => userSkills.Take(5).Select(skill =>
        new UserSkillEducation { EducationID = education.ID, UserSkillID = skill.ID })));
    db.AddRange(certificates.SelectMany(certificate => userSkills.Take(5).Select(skill =>
        new UserSkillCertificate { CertificateID = certificate.ID, UserSkillID = skill.ID })));
    await db.SaveChangesAsync();
}

static int Fail(string message)
{
    Console.Error.WriteLine(message);
    return 1;
}

internal sealed record Arguments(
    bool Confirmed,
    string ExpectedDatabase,
    string ExpectedHost,
    int UserCount,
    int MaximumExistingUsers,
    string Password)
{
    public static Arguments Parse(string[] args)
    {
        string? Value(string name)
        {
            var index = Array.IndexOf(args, name);
            return index >= 0 && index + 1 < args.Length ? args[index + 1] : null;
        }
        var expectedDatabase = Value("--expected-database") ?? string.Empty;
        var expectedHost = Value("--expected-host") ?? string.Empty;
        var password = Environment.GetEnvironmentVariable("PERFORMANCE_OWNER_PASSWORD") ?? string.Empty;
        if (string.IsNullOrWhiteSpace(expectedDatabase)) throw new ArgumentException("--expected-database is required.");
        if (string.IsNullOrWhiteSpace(expectedHost)) throw new ArgumentException("--expected-host is required.");
        if (password.Length < 12) throw new ArgumentException("PERFORMANCE_OWNER_PASSWORD must contain at least 12 characters.");
        return new Arguments(
            Value("--confirm") == "SEED_ISOLATED_PREVIEW",
            expectedDatabase,
            expectedHost,
            int.TryParse(Value("--users"), out var users) ? Math.Clamp(users, 10_000, 100_000) : 10_000,
            int.TryParse(Value("--maximum-existing-users"), out var maximum) ? Math.Clamp(maximum, 0, 100) : 5,
            password);
    }
}
