using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriorityQueue
{
    public interface IPriorityQueue<P, T> where P : IComparable
    {
        object LockObject
        {
            get;
            set;
        }
        void PrintItems();
        void Clear();
        int Count();
        IEnumerator<Item<P, T>> GetEnumerator();        
        Item<P, T> Dequeue();
        Item<P, T> Peek();
        void Enqueue(Item<P, T> heapItem);               
    }
}
