using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace GossipCheck.BLL.Extensions
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

        public static string ToAuthorityUrl(this string text)
        {
            text.EnsureWebUrl();
            return new Uri(text).GetLeftPart(UriPartial.Authority);
        }

        public static string ToSentenceCase<T>(this T obj) where T : Enum
        {
            var words = Regex.Split(obj.ToString(), @"(?<=[a-z])(?=[A-Z\d])|(?<=[a-z\d])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])")
                .Select(x => Regex.IsMatch(x, @"^[A-Z]+$") ? x : x.ToLower())
                .ToList();

            return words[0][0].ToString().ToUpper() + string.Join(' ', words)[1..];
        }
    }
}