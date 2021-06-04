using System.Collections.Generic;
using System.Linq;

namespace GossipCheck.WebScraper.Services
{
    public static class RegexMapperHelper
    {
        public static T Map<T>(string input) where T : class, new()
        {
            var globalAttributes = typeof(T)
                .GetCustomAttributes(typeof(RegexMapperAttribute), false)
                .Cast<RegexMapperAttribute>()
                .ToArray();

            var processedInput = ApplyAttributes(input, globalAttributes);

            var result = new T();
            foreach (var property in typeof(T).GetProperties())
            {
                var attributes = property
                    .GetCustomAttributes(typeof(RegexMapperAttribute), false)
                    .Cast<RegexMapperAttribute>()
                    .ToArray();

                var value = ApplyAttributes(processedInput, attributes);
                property.SetValue(result, value);
            }

            return result;
        }

        private static string ApplyAttributes(string input, IEnumerable<RegexMapperAttribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                var value = attribute.Map(input);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }
    }
}