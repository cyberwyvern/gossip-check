using System;
using System.Collections.Generic;
using System.Linq;

namespace GossipCheck.BLL.Extensions
{
    internal static class LinqMathExtensions
    {
        public static IEnumerable<double> Softmax(this IEnumerable<double> values)
        {
            var sum = values.Sum(x => Math.Exp(x));
            return values.Select(x => x / sum).ToArray();
        }
    }
}
