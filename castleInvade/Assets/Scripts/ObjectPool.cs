using System.Collections.Generic;

namespace ObjectPool
{
    public class ObjectPool<T> where T : class
    {
        short count;
        public delegate T Func();

        Func create_fn;
        Stack<T> objects;

        public ObjectPool(short initCount, Func fn)
        {
            count = initCount;
            create_fn = fn;
            objects = new Stack<T>(count);

            allocate();
        }

        void allocate()
        {
            for (int i = 0; i < count; ++i)
            {
                objects.Push(create_fn());
            }
        }

        public T pop()
        {
            if (objects.Count <= 0)
            {
                allocate();
            }
            return objects.Pop();
        }

        public void push(T obj)
        {
            objects.Push(obj);
        }
    }
}
