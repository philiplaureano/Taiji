using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taiji.Yin
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            var items = new List<T>(enumerable);
            items.ForEach(action);
        }
    }
}
