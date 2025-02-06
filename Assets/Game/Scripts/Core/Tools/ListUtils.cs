using System.Collections.Generic;

namespace Core.Tools
{
    public static class ListUtils
    {
        public static T RandomElement<T>(this List<T> source)
        {
            var elementIndex = UnityEngine.Random.Range(0, source.Count);
            return source[elementIndex];
        }
    }
}
