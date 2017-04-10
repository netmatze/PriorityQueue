using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace PriorityQueue
{
    public class List<P, T> : IPriorityQueue<P, T> where P : IComparable
    {
        private ListNode<P, T> rootNode;
        private object lockObject = new object();

        public object LockObject
        {
            get { return lockObject; }
            set { lockObject = value; }
        }

        public void PrintItems()
        {
            if (rootNode != null)
                Print(rootNode);
        }

        public void Clear()
        {
            lock (lockObject)
            {
                rootNode = null;
            }
        }

        public int Count()
        {
            var counter = 0;
            lock (lockObject)
            {
                ListNode<P, T> heapNode = rootNode;
                do
                {
                    if (heapNode.Value != null)
                        counter++;
                    heapNode = heapNode.NextNode;
                }
                while (heapNode != null);
            }
            return counter;
        }

        public IEnumerator<Item<P, T>> GetEnumerator()
        {
            lock(lockObject)
            {
                ListNode<P, T> heapNode = rootNode;
                do
                {
                    if (heapNode.Value != null)
                        yield return heapNode.Value;
                    heapNode = heapNode.NextNode;
                }
                while (heapNode != null);
            }
        }

        public void Print(ListNode<P, T> node)
        {
            if(node.Value != null)
                Debug.WriteLine("Key: {0}, Value: {1}", node.Value.PriorityKey, 
                    node.Value.PriorityValue);
            if (node.NextNode != null)
                Print(node.NextNode);
        }

        public Item<P, T> Dequeue()
        {
            if (rootNode == null)
            {
                return default(Item<P, T>);
            }
            lock (lockObject)
            {
                var tempNode = rootNode;
                rootNode = rootNode.NextNode;
                return tempNode.Value;
            }
        }

        public Item<P, T> Peek()
        {
            if (rootNode == null)
            {
                return default(Item<P, T>);
            }
            lock (lockObject)
            {                
                return rootNode.Value;
            }
        }

        public void Enqueue(Item<P, T> heapItem)
        {
            if(rootNode == null)
            {
                rootNode = new ListNode<P, T>() { Value = heapItem };
            }
            else
            {
                if (rootNode.Value.PriorityKey.CompareTo(heapItem.PriorityKey) == -1 ||
                    rootNode.Value.PriorityKey.CompareTo(heapItem.PriorityKey) == 0)
                {
                    lock (lockObject)
                    {
                        var tempNode = rootNode;
                        rootNode = new ListNode<P, T>() { Value = heapItem };
                        rootNode.NextNode = tempNode;
                        tempNode.ParentNode = rootNode;
                    }
                }
                else if (rootNode.Value.PriorityKey.CompareTo(heapItem.PriorityKey) == 1)
                {
                    Enqueue(rootNode, heapItem);                  
                }                
            }
        }

        public void Enqueue(ListNode<P, T> node, Item<P, T> heapItem)
        {
            if(node.NextNode == null)
            {
                node.NextNode = new ListNode<P, T>() { Value = heapItem };
            }
            else
            {
                if (node.Value.PriorityKey.CompareTo(heapItem.PriorityKey) == -1 ||
                    node.Value.PriorityKey.CompareTo(heapItem.PriorityKey) == 0)
                {
                    lock (lockObject)
                    {
                        var newNode = new ListNode<P, T>() { Value = heapItem };
                        newNode.ParentNode = node.ParentNode;
                        newNode.NextNode = node;
                        node.ParentNode.NextNode = newNode;
                        node.ParentNode = newNode;
                    }
                }
                else if (node.Value.PriorityKey.CompareTo(heapItem.PriorityKey) == 1)
                {
                    if(node.NextNode != null)
                        Enqueue(node.NextNode, heapItem);
                }
            }
        }

        private bool CompareAndSwap(ref ListNode<P, T> destination, ListNode<P, T>
            currentValue, ListNode<P, T> newValue)
        {
            if (currentValue == Interlocked.CompareExchange<ListNode<P, T>>
            (ref destination, newValue, currentValue))
                return true;            
            return false;
        }
    }
}
