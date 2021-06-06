using System;
using System.Collections.Generic;
using System.Linq;

namespace GossipCheck.BLL.Extensions
{
    internal static class LinqMathExtensions
    {
        public static IEnumerable<double> Softmax(this IEnumerable<double> values)
        {
            var exp = values.Select(Math.Exp);
            var sumExp = exp.Sum();
            return exp.Select(i => i / sumExp);
        }
    }
}
