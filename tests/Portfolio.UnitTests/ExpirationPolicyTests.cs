using Application.Common.Constants;
using System.Reflection;

namespace Portfolio.UnitTests;

public sealed class ExpirationPolicyTests
{
    [Fact]
    public void RefreshTokenPolicy_HasOneServerLifetimeRegardlessOfCookiePersistence()
    {
        var refreshLifetimeFields = typeof(ExpirationTimes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.Name.Contains("RefreshTokenLifetime", StringComparison.Ordinal))
            .ToArray();

        var field = Assert.Single(refreshLifetimeFields);
        Assert.Equal(nameof(ExpirationTimes.RefreshTokenLifetime), field.Name);
        Assert.Equal(TimeSpan.FromDays(30), (TimeSpan)field.GetValue(null)!);
    }
}
