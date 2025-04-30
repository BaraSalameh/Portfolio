using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace Portfolio.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NormalizeStringsAttribute : ActionFilterAttribute
    {
        private readonly bool _toLower;

        public NormalizeStringsAttribute(bool toLower = false)
        {
            _toLower = toLower;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null) continue;

                var props = argument.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

                foreach (var prop in props)
                {
                    var currentValue = (string)prop.GetValue(argument);
                    if (currentValue != null)
                    {
                        var value = currentValue.Trim();
                        if (_toLower)
                            value = value.ToLowerInvariant();

                        // Convert empty strings to null if needed
                        prop.SetValue(argument, string.IsNullOrWhiteSpace(value) ? null : value);
                    }
                }
            }
        }
    }
}
