using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace GossipCheck.API.Extensions
{
    public static class EnumExtensions
    {
        public static string ToSentenceCase<T>(this T obj) where T : Enum
        {
            var words = Regex.Split(obj.ToString(), @"(?<=[a-z])(?=[A-Z\d])|(?<=[a-z\d])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])")
                .Select(x => Regex.IsMatch(x, @"^[A-Z]+$") ? x : x.ToLower())
                .ToList();

            return words[0][0].ToString().ToUpper() + string.Join(' ', words)[1..];
        }
    }
}
