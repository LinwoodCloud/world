using Godot.Collections;

namespace LinwoodWorld.System
{
    public static class ObjectUtils 
    {
        public static Array<T> ConcatArrays<T>(params Array<T>[] arrays)
        {
            Array<T> array = new Array<T>();
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