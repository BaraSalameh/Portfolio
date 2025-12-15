using System.Text.RegularExpressions;

namespace Application.Common.Functions
{
    public static class AppFunctions
    {
        public static bool IsValidEmail(this string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        public static bool IsValidPhone(this string phone)
        {
            var phonePattern = @"^\+?[1-9]\d{1,14}$";
            return Regex.IsMatch(phone, phonePattern);
        }

        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            var words = Regex.Split(input, @"[\s_]+");
            return char.ToLowerInvariant(words[0][0]) + string.Join("", words.Skip(1).Select(word => char.ToUpperInvariant(word[0]) + word.Substring(1).ToLower()));
        }

        public static string ToPascalCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            var words = Regex.Split(input, @"[\s_]+");
            return string.Join("", words.Select(word => char.ToUpperInvariant(word[0]) + word.Substring(1).ToLower()));
        }

        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            return Regex.Replace(input, @"([a-z])([A-Z])", "$1_$2") // Handle camelCase
                .Replace(" ", "_") // Replace spaces
                .ToLowerInvariant(); // Convert all to lower case
        }
    }
}