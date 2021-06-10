using System;

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
    }
}