using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriorityQueue
{
    public class ListNode<P, T> where P : IComparable
    {
        public Item<P, T> Value { get; set; }
        public ListNode<P, T> NextNode { get; set; }
        public ListNode<P, T> ParentNode { get; set; }
    }
}
