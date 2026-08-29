using Application.Client;
using Application.Common.Entities;
using Application.Common.Constants;
using Application.Owner.Queries.CertificateQueries;
using Application.Owner.Queries.EducationQueries;
using Application.Owner.Queries.ExperienceQueries;
using Application.Owner.Queries.ProjectQueries;
using Application.Owner.Queries.UserChartPreferenceQueries;
using Application.Owner.Queries.UserLanguageQueries;
using Application.Owner.Queries.UserPreferenceQueries;
using Application.Owner.Queries.UserSkillQueries;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.UnitTests;

public sealed class QueryBoundsTests
{
    [Fact]
    public void OwnerCollectionQueries_DefaultToMaximumBoundedPage()
    {
        var pageSizes = new[]
        {
            new ProjectListQuery().PageSize,
            new EducationListQuery().PageSize,
            new ExperienceListQuery().PageSize,
            new CertificateListQuery().PageSize,
            new UserSkillListQuery().PageSize,
            new UserLanguageListQuery().PageSize,
            new UserPreferenceListQuery().PageSize,
            new UserChartPreferenceListQuery().PageSize
        };

        Assert.All(pageSizes, pageSize => Assert.Equal(100, pageSize));
    }

    [Fact]
    public void PageSizeAboveMaximum_IsRejectedByValidation()
    {
        var query = new ProjectListQuery { PageSize = 101 };
        var results = new List<ValidationResult>();

        Assert.False(Validator.TryValidateObject(query, new ValidationContext(query), results, true));
        Assert.Contains(results, result => result.MemberNames.Contains(nameof(query.PageSize)));
    }

    [Fact]
    public void Offset_SaturatesInsteadOfOverflowing()
    {
        var query = new ProjectListQuery { PageNumber = int.MaxValue, PageSize = 100 };

        Assert.Equal(int.MaxValue, query.Offset);
    }

    [Theory]
    [InlineData(1_000, 100, true)]
    [InlineData(1_001, 100, false)]
    [InlineData(100_000, 100, false)]
    public void CombinedPagination_RejectsDatabaseAmplifyingDeepOffsets(
        int pageNumber,
        int pageSize,
        bool expectedValid)
    {
        var query = new ProjectListQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var results = new List<ValidationResult>();

        var valid = Validator.TryValidateObject(query, new ValidationContext(query), results, true);

        Assert.Equal(expectedValid, valid);
        if (!expectedValid)
        {
            Assert.Contains(results, result =>
                result.MemberNames.Contains(nameof(query.PageNumber)) &&
                result.MemberNames.Contains(nameof(query.PageSize)));
        }
    }

    [Fact]
    public void PublicPortfolioCollections_HaveExplicitMaximum()
    {
        Assert.Equal(100, PublicPortfolioLimits.MaxCollectionItems);
    }

    [Fact]
    public void DailyOutboxRecovery_HasBoundedMultiBatchCapacity()
    {
        Assert.Equal(20, EmailOutboxPolicy.BatchSize);
        Assert.Equal(10, EmailOutboxPolicy.MaximumBatchesPerRecoveryRun);
        Assert.Equal(200, EmailOutboxPolicy.BatchSize * EmailOutboxPolicy.MaximumBatchesPerRecoveryRun);
        Assert.True(MaintenancePolicy.RequestTimeoutMilliseconds < 300_000);
    }

    [Fact]
    public void EmailDeliveryTimeout_FitsInsideLeaseAndMaintenanceDeadline()
    {
        var maximumDeliveryTimeout = TimeSpan.FromMilliseconds(
            EmailOutboxPolicy.MaximumDeliveryTimeoutMilliseconds);

        Assert.True(maximumDeliveryTimeout < EmailOutboxPolicy.ClaimDuration);
        Assert.True(
            EmailOutboxPolicy.MaximumDeliveryTimeoutMilliseconds <
            MaintenancePolicy.RequestTimeoutMilliseconds);
    }
}
