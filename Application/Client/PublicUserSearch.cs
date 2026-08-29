using Domain.Entities;

namespace Application.Client;

public static class PublicUserSearch
{
    public static IQueryable<User> Apply(IQueryable<User> query, string search)
    {
        var terms = search
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Take(10)
            .ToArray();

        return query.Where(user =>
            user.Username.Contains(search) ||
            (user.Email.Contains(search) && user.LstUserPreferences.Any(preference =>
                !preference.IsDeleted &&
                preference.LKP_Preference.Name == PublicProfilePrivacy.ShowEmailPreference &&
                preference.Value.ToLower() == "true")) ||
            terms.All(term =>
                user.Firstname.Contains(term) ||
                user.Lastname.Contains(term)));
    }
}
