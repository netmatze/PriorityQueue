using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriorityQueue
{
    public class HeapNode<P, T> where P : IComparable
    {
        public Item<P, T> Value { get; set; }
        public HeapNode<P, T> LeftNode { get; set; }
        public HeapNode<P, T> RightNode { get; set; }
        public HeapNode<P, T> ParentNode { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
