using System;

namespace GossipCheck.WebScraper
{
    public static class StringExtensions
    {
        public static bool IsUrl(this string text)
        {
            return Uri.TryCreate(text, UriKind.Absolute, out Uri uriResult)
&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}