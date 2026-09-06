using Application.Client;
using Application.Client.Queries;
using Application.Client.MappingProfiles;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace Portfolio.UnitTests;

public sealed class PublicProfilePrivacyTests
{
    [Fact]
    public void Apply_HidesSensitiveFieldsUnlessExplicitlyEnabled()
    {
        var profile = CreateProfile();
        profile.LstUserPreferences.Add(new UBUQ_UserPreference
        {
            Preference = new UBUQ_LKP_Preference { Name = "show-email-address" },
            Value = "true"
        });

        PublicProfilePrivacy.Apply(profile);

        Assert.Equal("owner@example.com", profile.User.Email);
        Assert.Null(profile.User.Phone);
        Assert.Null(profile.User.BirthDate);
        Assert.Null(profile.User.Gender);
        Assert.Null(profile.User.WhatsAppNumber);
        Assert.Null(profile.User.CvUrl);
        Assert.Empty(profile.LstSocialLinks);
    }

    [Fact]
    public void Apply_HidesAllSensitiveFieldsWhenPreferencesAreMissing()
    {
        var profile = CreateProfile();

        PublicProfilePrivacy.Apply(profile);

        Assert.Null(profile.User.Email);
        Assert.Null(profile.User.Phone);
        Assert.Null(profile.User.BirthDate);
        Assert.Null(profile.User.Gender);
        Assert.Null(profile.User.WhatsAppNumber);
        Assert.Null(profile.User.CvUrl);
        Assert.Empty(profile.LstSocialLinks);
    }

    [Fact]
    public void Apply_EnablesWhatsAppCvAndSiteLinksIndependently()
    {
        var profile = CreateProfile();
        profile.LstUserPreferences.AddRange([
            new UBUQ_UserPreference { Preference = new UBUQ_LKP_Preference { Name = PublicProfilePrivacy.ShowWhatsAppPreference }, Value = "true" },
            new UBUQ_UserPreference { Preference = new UBUQ_LKP_Preference { Name = PublicProfilePrivacy.ShowCvPreference }, Value = "true" },
            new UBUQ_UserPreference { Preference = new UBUQ_LKP_Preference { Name = PublicProfilePrivacy.ShowSiteLinksPreference }, Value = "true" }
        ]);

        PublicProfilePrivacy.Apply(profile);

        Assert.Equal("+962790000000", profile.User.WhatsAppNumber);
        Assert.Equal("https://example.com/cv.pdf", profile.User.CvUrl);
        Assert.Single(profile.LstSocialLinks);
    }

    [Fact]
    public void MappingSuppressesSensitiveFieldsBeforePostProjectionPrivacyScrub()
    {
        var configuration = new MapperConfiguration(
            expression => expression.AddProfile<UserMappingProfiles>(),
            NullLoggerFactory.Instance);
        var mapper = configuration.CreateMapper();
        var user = new User
        {
            Email = "owner@example.com",
            Phone = "+962790000000",
            BirthDate = new DateOnly(1990, 1, 1),
            Gender = 1,
            WhatsAppNumber = "+962790000000",
            CvUrl = "https://example.com/cv.pdf"
        };

        var hidden = mapper.Map<UBUQ_User>(user);
        Assert.Null(hidden.Email);
        Assert.Null(hidden.Phone);
        Assert.Null(hidden.BirthDate);
        Assert.Null(hidden.Gender);
        Assert.Null(hidden.WhatsAppNumber);
        Assert.Null(hidden.CvUrl);

        user.LstUserPreferences.Add(new UserPreference
        {
            LKP_Preference = new LKP_Preference { Name = PublicProfilePrivacy.ShowEmailPreference },
            Value = "TRUE"
        });
        var enabled = mapper.Map<UBUQ_User>(user);
        Assert.Equal("owner@example.com", enabled.Email);
        Assert.Null(enabled.Phone);
    }

    private static UBUQ_Response CreateProfile() => new()
    {
        User = new UBUQ_User
        {
            Email = "owner@example.com",
            Phone = "+962790000000",
            BirthDate = new DateOnly(1990, 1, 1),
            Gender = 1,
            WhatsAppNumber = "+962790000000",
            CvUrl = "https://example.com/cv.pdf"
        },
        LstSocialLinks = [new UBUQ_SocialLink { Platform = "GitHub", Url = "https://github.com/example" }]
    };
}
