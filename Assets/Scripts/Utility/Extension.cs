using System;
using System.Collections.Generic;

namespace Utility {
    public static class Extension {
        public static void Shuffle<T>(this IList<T> list, Random random) {
            var n = list.Count;

            for (var i = list.Count - 1; i > 1; i--) {
                var rnd = random.Next(i + 1);

                var value = list[rnd];
                list[rnd] = list[i];
                list[i] = value;
            }
        }
    }
}