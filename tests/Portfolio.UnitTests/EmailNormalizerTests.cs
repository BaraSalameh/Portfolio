using Application.Common.Identity;

namespace Portfolio.UnitTests;

public sealed class EmailNormalizerTests
{
    [Fact]
    public void Normalize_TrimsAndUsesInvariantLowercase()
    {
        Assert.Equal("owner@example.test", EmailNormalizer.Normalize("  OWNER@EXAMPLE.TEST  "));
    }
}
