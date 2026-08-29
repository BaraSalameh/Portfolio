using System.Globalization;
using System.Text;

namespace Application.Common.Identity;

public static class UsernameGenerator
{
    public const int MaxLength = 100;
    private const int SuffixLength = 16;

    public static string Create(string firstName, string lastName)
    {
        var source = $"{firstName.Trim()}-{lastName.Trim()}";
        var slug = new StringBuilder(source.Length);
        var pendingSeparator = false;

        foreach (var rune in source.EnumerateRunes())
        {
            if (Rune.IsLetterOrDigit(rune))
            {
                if (pendingSeparator && slug.Length > 0)
                {
                    slug.Append('-');
                }

                slug.Append(rune.ToString().ToLower(CultureInfo.InvariantCulture));
                pendingSeparator = false;
            }
            else
            {
                pendingSeparator = true;
            }
        }

        if (slug.Length == 0)
        {
            slug.Append("user");
        }

        var maximumBaseLength = MaxLength - SuffixLength - 1;
        if (slug.Length > maximumBaseLength)
        {
            slug.Length = maximumBaseLength;
            while (slug.Length > 0 && char.IsHighSurrogate(slug[^1]))
            {
                slug.Length--;
            }

            while (slug.Length > 0 && slug[^1] == '-')
            {
                slug.Length--;
            }
        }

        var suffix = Guid.NewGuid().ToString("N")[..SuffixLength];
        return $"{slug}-{suffix}";
    }
}
