using System.ComponentModel.DataAnnotations;
using Application.Owner.Commands.Profile;
using Application.Owner.Commands.SocialLinkCommands;
using Application.Owner.Commands.UserChartPreferenceCommands;
using Application.Owner.Commands.UserPreferenceCommands;
using Application.Common.Services.Interface;
using Application.Owner.Commands.UserLanguageCommands;
using Application.Owner.Commands.UserSkillCommands;
using Application.Owner.Commands.CertificaeCommands;

namespace Portfolio.UnitTests;

public sealed class CommandValidationTests
{
    [Fact]
    public void Profile_RejectsFutureBirthDateAndUnsafeOrOversizedFields()
    {
        var unsafeFields = new EditProfileCommand
        {
            Firstname = new string('a', 101),
            ProfilePicture = "javascript:alert(1)",
            Gender = 99
        };

        var errors = Validate(unsafeFields);

        Assert.Contains(errors, error => error.MemberNames.Contains(nameof(unsafeFields.Firstname)));
        Assert.Contains(errors, error => error.MemberNames.Contains(nameof(unsafeFields.ProfilePicture)));
        Assert.Contains(errors, error => error.MemberNames.Contains(nameof(unsafeFields.Gender)));
        var futureDate = new EditProfileCommand
        {
            BirthDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
        };
        Assert.Contains(
            Validate(futureDate),
            error => error.MemberNames.Contains(nameof(futureDate.BirthDate)));
    }

    [Fact]
    public void PreviouslyUnboundedCommands_RejectInvalidValues()
    {
        Assert.NotEmpty(Validate(new AddEditSocialLinkCommand
        {
            Platform = new string('p', 101),
            Url = "not-a-url"
        }));
        Assert.NotEmpty(Validate(new EditUserPreferenceCommand
        {
            Value = new string('v', 1001)
        }));
        Assert.NotEmpty(Validate(new EditUserChartPreferenceCommand
        {
            GroupBy = new string('g', 101),
            ValueSource = new string('v', 201)
        }));
    }

    [Fact]
    public void CertificateMedia_RejectsEmbeddedCredentialsAndUnsafeSchemes()
    {
        var command = new AddEditCertificateCommand
        {
            LstCertificateMedias =
            [
                "https://user:password@cdn.example/certificate.png",
                "javascript:alert(1)"
            ]
        };

        var errors = Validate(command);

        Assert.Contains(errors, error =>
            error.MemberNames.Contains(nameof(command.LstCertificateMedias)));
    }

    [Fact]
    public void ProfileBirthDateValidation_UsesInjectedUtcClockAtYearBoundary()
    {
        var command = new EditProfileCommand { BirthDate = new DateOnly(2027, 1, 1) };
        var clock = new FixedClock(new DateTime(2026, 12, 31, 23, 59, 59, DateTimeKind.Utc));

        var errors = Validate(command, clock);

        Assert.Contains(errors, error => error.MemberNames.Contains(nameof(command.BirthDate)));
    }

    [Fact]
    public void SkillGraph_RejectsDuplicateEmptyAndExcessiveRelations()
    {
        var duplicateSkillId = Guid.NewGuid();
        var command = new EditDeleteUserSkillCommand
        {
            LstUserSkills =
            [
                new EDUSC_UserSkill
                {
                    LKP_SkillID = duplicateSkillId,
                    EducationIDs = Enumerable.Range(0, 100).Select(_ => Guid.NewGuid()).ToList(),
                    ExperienceIDs = Enumerable.Range(0, 100).Select(_ => Guid.NewGuid()).ToList(),
                    ProjectIDs = Enumerable.Range(0, 100).Select(_ => Guid.NewGuid()).ToList(),
                    CertificateIDs = Enumerable.Range(0, 100).Select(_ => Guid.NewGuid()).ToList()
                },
                new EDUSC_UserSkill
                {
                    LKP_SkillID = duplicateSkillId,
                    EducationIDs = Enumerable.Range(0, 100).Select(_ => Guid.NewGuid()).Append(Guid.Empty).ToList()
                }
            ]
        };

        var errors = Validate(command);

        Assert.Contains(errors, error => error.ErrorMessage == "Duplicate skill IDs are not allowed.");
        Assert.Contains(errors, error => error.ErrorMessage?.Contains("maximum of 500", StringComparison.Ordinal) == true);
        Assert.Contains(errors, error => error.ErrorMessage?.Contains("non-empty and unique", StringComparison.Ordinal) == true);
    }

    [Fact]
    public void LanguageGraph_RejectsDuplicateAndEmptyIdentifiers()
    {
        var languageId = Guid.NewGuid();
        var command = new EditDeleteUserLanguageCommand
        {
            LstLanguages =
            [
                new EDULC_LKP_Language
                {
                    LKP_LanguageID = languageId,
                    LKP_LanguageProficiencyID = Guid.Empty
                },
                new EDULC_LKP_Language
                {
                    LKP_LanguageID = languageId,
                    LKP_LanguageProficiencyID = Guid.NewGuid()
                }
            ]
        };

        var errors = Validate(command);

        Assert.Contains(errors, error => error.ErrorMessage == "Language and proficiency IDs must not be empty.");
        Assert.Contains(errors, error => error.ErrorMessage == "Duplicate language IDs are not allowed.");
    }

    private static List<ValidationResult> Validate(object value, IDateTimeProvider? clock = null)
    {
        var results = new List<ValidationResult>();
        var context = clock is null
            ? new ValidationContext(value)
            : new ValidationContext(value, new ClockServiceProvider(clock), items: null);
        Validator.TryValidateObject(value, context, results, validateAllProperties: true);
        return results;
    }

    private sealed class ClockServiceProvider(IDateTimeProvider clock) : IServiceProvider
    {
        public object? GetService(Type serviceType) =>
            serviceType == typeof(IDateTimeProvider) ? clock : null;
    }

    private sealed class FixedClock(DateTime utcNow) : IDateTimeProvider
    {
        public DateTime UtcNow { get; } = utcNow;
    }
}
