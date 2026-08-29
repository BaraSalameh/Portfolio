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

            var words = Words(input);
            if (words.Length == 0) return string.Empty;

            return LowerFirst(words[0]) + string.Concat(words.Skip(1).Select(UpperFirst));
        }

        public static string ToPascalCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            var words = Words(input);
            return string.Concat(words.Select(UpperFirst));
        }

        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            return Regex.Replace(input, @"([a-z])([A-Z])", "$1_$2") // Handle camelCase
                .Replace(" ", "_") // Replace spaces
                .ToLowerInvariant(); // Convert all to lower case
        }

        private static string[] Words(string input) => Regex.Split(input.Trim(), @"[\s_]+")
            .Where(word => word.Length > 0)
            .ToArray();

        private static string LowerFirst(string word) =>
            char.ToLowerInvariant(word[0]) + word[1..].ToLowerInvariant();

        private static string UpperFirst(string word) =>
            char.ToUpperInvariant(word[0]) + word[1..].ToLowerInvariant();
    }
}
