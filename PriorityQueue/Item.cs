using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriorityQueue
{
    public class Item<P, T> where P : IComparable
    {
        public P PriorityKey { get; set;}
        public T PriorityValue { get; set; }

        public override string ToString()
        {
            return PriorityKey.ToString() + " " + PriorityValue.ToString();
        }
    }
}
