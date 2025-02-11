using System.Collections.Generic;
using System.Linq;

namespace Core.Tools
{
    public static class EnumerableUtils
    {
        public static T RandomElement<T>(this IEnumerable<T> source)
        {
            var elementIndex = UnityEngine.Random.Range(0, source.Count());
            return source.ElementAt(elementIndex);
        }
    }
}
