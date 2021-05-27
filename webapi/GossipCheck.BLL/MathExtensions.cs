using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GossipCheck.BLL
{
    internal static class LinqMathExtensions
    {
        public static IEnumerable<T> SoftmaxOnProperty<T>(this IEnumerable<T> objects, Expression<Func<T, double>> selectorExpr) where T : class
        {
            var property = (selectorExpr.Body as MemberExpression ?? ((UnaryExpression)selectorExpr.Body).Operand as MemberExpression).Member as PropertyInfo;
            var selector = selectorExpr.Compile();


            var sum = objects.Select(x => Math.Exp(selector(x))).Sum();
            objects.ToList().ForEach(x => property.SetValue(x, selector(x) / sum, null));
            return objects;
        }
    }
}
