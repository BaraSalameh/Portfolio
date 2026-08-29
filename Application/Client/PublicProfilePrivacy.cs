using Application.Client.Queries;

namespace Application.Client;

public static class PublicProfilePrivacy
{
    public const string ShowEmailPreference = "show-email-address";
    public const string ShowPhonePreference = "show-phone-number";
    public const string ShowBirthDatePreference = "show-birthdate";
    public const string ShowGenderPreference = "show-gender";

    public static void Apply(UBUQ_Response profile)
    {
        var enabledPreferences = profile.LstUserPreferences
            .Where(preference => bool.TryParse(preference.Value, out var enabled) && enabled)
            .Select(preference => preference.Preference.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (!enabledPreferences.Contains(ShowEmailPreference)) profile.User.Email = null;
        if (!enabledPreferences.Contains(ShowPhonePreference)) profile.User.Phone = null;
        if (!enabledPreferences.Contains(ShowBirthDatePreference)) profile.User.BirthDate = null;
        if (!enabledPreferences.Contains(ShowGenderPreference)) profile.User.Gender = null;
    }
}
