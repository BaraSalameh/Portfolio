using System.Text.RegularExpressions;
using Application.Common.Identity;

namespace Portfolio.UnitTests;

public sealed class UsernameGeneratorTests
{
    [Fact]
    public void Create_ProducesBoundedRouteSafeUsernameForLongNames()
    {
        var username = UsernameGenerator.Create(new string('A', 100), new string('B', 100));

        Assert.Equal(UsernameGenerator.MaxLength, username.Length);
        Assert.Matches(new Regex("^[a-z0-9-]+-[a-f0-9]{16}$"), username);
    }

    [Fact]
    public void Create_CollapsesUnsafeSeparatorsAndPreservesUnicodeLetters()
    {
        var username = UsernameGenerator.Create("  سارة / ", " O'Neil  ");

        Assert.StartsWith("سارة-o-neil-", username, StringComparison.Ordinal);
        Assert.DoesNotContain('/', username);
        Assert.DoesNotContain(' ', username);
        Assert.True(username.Length <= UsernameGenerator.MaxLength);
    }

    [Fact]
    public void Create_UsesFallbackForNamesWithoutLettersOrDigits()
    {
        var username = UsernameGenerator.Create("---", "///");

        Assert.Matches(new Regex("^user-[a-f0-9]{16}$"), username);
    }
}
