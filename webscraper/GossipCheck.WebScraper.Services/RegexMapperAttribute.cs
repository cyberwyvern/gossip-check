using System;
using System.Text.RegularExpressions;

namespace GossipCheck.WebScraper.Services
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class RegexMapperAttribute : Attribute
    {
        private const RegexOptions Options = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled;

        public string Pattern { get; set; }

        public int Group { get; set; }

        public RegexMapperAttribute(string pattern)
        {
            Pattern = pattern;
        }

        public string Map(string input)
        {
            var match = new Regex(Pattern, Options).Match(input);
            if (match.Success)
            {
                return match.Groups[Group].Value;
            }

            return null;
        }
    }
}