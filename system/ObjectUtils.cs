using System.Collections.Generic;

namespace LinwoodWorld.WorldSystem
{
    public static class ObjectUtils
    {
        public static List<T> ConcatArrays<T>(params List<T>[] arrays)
        {
            List<T> array = new List<T>();
            foreach (var current in arrays)
            {
                foreach (var item in current)
                {
                    array.Add(item);
                }
            }
            return array;
        }
    }
}