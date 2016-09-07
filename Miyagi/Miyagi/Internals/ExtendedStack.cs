using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miyagi.Internals
{
    internal class ExtendedStack<T> : List<T>
    {
        public void Push(T item)
        {
            this.Add(item);
        }
        public T Pop()
        {
            if (this.Count > 0)
            {
                T temp = this[this.Count - 1];
                this.RemoveAt(this.Count - 1);
                return temp;
            }
            else
                return default(T);
        }
        public void Remove(int itemAtPosition)
        {
            this.RemoveAt(itemAtPosition);
        }
    }
}
