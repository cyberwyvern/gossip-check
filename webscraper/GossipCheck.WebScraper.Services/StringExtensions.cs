using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace GossipCheck.WebScraper
{
    public static class StringExtensions
    {
        public static bool IsWebUrl(this string text)
        {
            return Uri.TryCreate(text, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static void EnsureWebUrl(this string text)
        {
            if (!text.IsWebUrl())
            {
                throw new ArgumentException($"The scheme of the Url must be either {Uri.UriSchemeHttp} or {Uri.UriSchemeHttps}");
            }
        }

        public static string ToPascalCase(this string text)
        {
            var words = Regex.Split(text.Trim(), @"\W")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.ToSentenceCase());

            return string.Join(string.Empty, words);
        }

        public static string ToSentenceCase(this string text)
        {
            var result = text[0].ToString().ToUpper();
            if (text.Length > 1)
            {
                result += text.ToLower()[1..];
            }

            return result;
        }
    }
}