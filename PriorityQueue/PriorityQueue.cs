using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriorityQueue
{
    public class PriorityQueue<P,T> : IEnumerable<T>, ICollection, IEnumerable 
        where P : IComparable
    {
        private IPriorityQueue<P, T> priorityQueueStructure;
        private PriorityQueueMode priorityQueueMode;

        public PriorityQueue(PriorityQueueMode priorityQueueMode = PriorityQueueMode.LinkedList)
        {
            this.priorityQueueMode = priorityQueueMode;
            if(priorityQueueMode == PriorityQueueMode.LinkedList)
            {
                priorityQueueStructure = new List<P, T>();
            }
            else
            {
                priorityQueueStructure = new Heap<P, T>();
            }
        }

        public int Count 
        {
            get { return priorityQueueStructure.Count(); }
        }

        public void Clear()
        {
            priorityQueueStructure.Clear();
        }

        public bool Contains(T item)
        {
            foreach (var heapItem in priorityQueueStructure)
            {
                if (heapItem != null)
                {
                    if (heapItem.PriorityValue.Equals(heapItem))
                    {
                        return true;
                    }
                }
            }       
            return false;
        }
        
        public void CopyTo(T[] array, int arrayIndex)
        {
            var indexCounter = arrayIndex;
            foreach (var heapItem in priorityQueueStructure)
            {
                if (heapItem != null)
                {
                    array[indexCounter] = heapItem.PriorityValue;
                }
                indexCounter++;
            }       
        }

        public T Dequeue()
        {
            var heapItem = priorityQueueStructure.Dequeue();
            if (heapItem == null)
            {
                return default(T);
            }
            else
            {
                return heapItem.PriorityValue;
            }
        }
        
        public void Enqueue(P priorityKey, T item)
        {
            priorityQueueStructure.Enqueue(new Item<P, T>() { 
                PriorityKey = priorityKey, PriorityValue = item });
        }

        public T Peek()
        {
            var heapItem = priorityQueueStructure.Peek();
            return heapItem.PriorityValue;
        }

        public void PrintItems()
        {
            priorityQueueStructure.PrintItems();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach(var item in priorityQueueStructure)
            {
                yield return item.PriorityValue;
            }           
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in priorityQueueStructure)
            {
                yield return item.PriorityValue;
            }       
        }

        public void CopyTo(Array array, int index)
        {
            var indexCounter = index;
            foreach (var heapItem in priorityQueueStructure)
            {
                if (heapItem != null)
                    array.SetValue(heapItem.PriorityValue, indexCounter);
                indexCounter++;
            }       
        }
        
        public bool IsSynchronized
        {
            get { return true; }
        }

        public object SyncRoot
        {
            get { return priorityQueueStructure.LockObject; }
        }
    }
}
