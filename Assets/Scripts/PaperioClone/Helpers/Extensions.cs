using System.Collections.Generic;

namespace PaperIOClone.Helpers
{
    internal static class Extensions
    {
        public static T CircleNext<T>(this List<T> list, int index)
        {
            if (index == list.Count - 1)
                return list[0];
            return list[index + 1];
        }

        public static T CirclePrev<T>(this List<T> list, int index)
        {
            if (index == 0)
                return list[list.Count - 1];
            return list[index - 1];
        }
    }
}