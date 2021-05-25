using System;

namespace GossipCheck.WebScraper
{
    public static class Util
    {
        public static bool IsUrl(string text) => Uri.TryCreate(text, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}