using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PriorityQueue
{
    public class Heap<P, T> : IPriorityQueue<P, T> where P : IComparable
    {
        private HeapNode<P, T> rootNode;
        private object lockObject = new object();

        public object LockObject
        {
            get { return lockObject; }
            set { lockObject = value; }
        }

        public void PrintItems()
        {
            if (rootNode != null)
            {
                Debug.WriteLine(string.Format(" Key: {0}, Value: {1} ",
                    rootNode.Value.PriorityKey, rootNode.Value.PriorityValue));
                if (rootNode.LeftNode != null)
                    PrintSubItems(rootNode.LeftNode);
                if (rootNode.RightNode != null)
                    PrintSubItems(rootNode.RightNode);
            }
            else
            {
                Debug.WriteLine(" - empty - ");
            }
        }

        private void PrintSubItems(HeapNode<P, T> node)
        {
            Debug.WriteLine(string.Format(" Key: {0}, Value: {1} ", 
                node.Value.PriorityKey, node.Value.PriorityValue));
            if (node.LeftNode != null)
                PrintSubItems(node.LeftNode);
            if (node.RightNode != null)
                PrintSubItems(node.RightNode);
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
            if (rootNode == null)
            {
                return counter;
            }
            else
            {
                lock (lockObject)
                {
                    counter++;
                    Counter(rootNode, ref counter);
                    return counter;
                }
            }        
        }

        private void Counter(HeapNode<P, T> heapNode, ref int counter)
        {
            if(heapNode.LeftNode != null)
            {
                counter++;
                Counter(heapNode.LeftNode, ref counter);
            }
            if(heapNode.RightNode != null)
            {
                counter++;
                Counter(heapNode.RightNode, ref counter);
            }
        }

        public IEnumerator<Item<P, T>> GetEnumerator()
        {
            if (rootNode != null)
            {
                lock (lockObject)
                {
                    yield return rootNode.Value;
                    List<Item<P, T>> list = new List<Item<P, T>>();
                    if (rootNode.RightNode != null)
                        Items(rootNode.RightNode, list);
                    if (rootNode.LeftNode != null)
                        Items(rootNode.LeftNode, list);
                    foreach (var item in list)
                    {
                        yield return item;
                    }
                }
            }
            else
            {
                Debug.WriteLine(" - empty - ");
            }
        }

        private void Items(HeapNode<P, T> node, List<Item<P, T>> list)
        {
            list.Add(node.Value);
            if (node.RightNode != null)
                Items(node.RightNode, list);
            if (node.LeftNode != null)
                Items(node.LeftNode, list);
        }

        public Item<P, T> Dequeue()
        {
            if (rootNode == null || this.Count() == 0)
            {
                return default(Item<P, T>);
            }
            lock (lockObject)
            {
                var tempNode = rootNode;
                var tempValue = tempNode.Value;
                var leastNode = FindLeastNode(rootNode);
                if (leastNode == rootNode && rootNode.LeftNode == null && rootNode.RightNode == null)
                {
                    rootNode = null;
                    return tempValue;
                }
                if (leastNode.ParentNode.LeftNode != null &&
                    leastNode == leastNode.ParentNode.LeftNode)
                {
                    leastNode.ParentNode.LeftNode = null;
                }
                if (leastNode.ParentNode.RightNode != null &&
                    leastNode == leastNode.ParentNode.RightNode)
                {
                    leastNode.ParentNode.RightNode = null;
                }
                if (rootNode.LeftNode != null)
                    rootNode.LeftNode.ParentNode = leastNode;
                if (rootNode.RightNode != null)
                    rootNode.RightNode.ParentNode = leastNode;
                leastNode.LeftNode = rootNode.LeftNode;
                leastNode.RightNode = rootNode.RightNode;                
                leastNode.ParentNode = null;                
                if(leastNode.LeftNode == null && leastNode.RightNode == null && leastNode.ParentNode == null)
                {
                    this.rootNode = leastNode;
                }
                else
                {
                    Swap(leastNode);
                }
                return tempValue;
            }
        }

        private void Swap(HeapNode<P, T> rootNode)
        {
            if (rootNode.LeftNode != null && 
                rootNode.LeftNode.Value.PriorityKey.CompareTo(rootNode.Value.PriorityKey) == 1)
            {
                if (rootNode.ParentNode == null)
                {
                    var swapNode = rootNode.LeftNode;
                    var exchangeNode = rootNode;
                    var tempLeftNode = swapNode.LeftNode;
                    var tempRightNode = swapNode.RightNode;
                    swapNode.LeftNode = exchangeNode;
                    rootNode.ParentNode = rootNode.LeftNode;
                    swapNode.ParentNode = null;
                    if (exchangeNode.RightNode != swapNode)
                    {
                        swapNode.RightNode = exchangeNode.RightNode;
                        if (swapNode.RightNode != null)
                            swapNode.RightNode.ParentNode = swapNode;
                    }                   
                    exchangeNode.ParentNode = swapNode;
                    exchangeNode.LeftNode = tempLeftNode;
                    exchangeNode.RightNode = tempRightNode;
                    if (exchangeNode.LeftNode != null)
                    {
                        exchangeNode.LeftNode.ParentNode = exchangeNode;
                    }
                    if (exchangeNode.RightNode != null)
                    {
                        exchangeNode.RightNode.ParentNode = exchangeNode;
                    }
                    this.rootNode = swapNode;
                }
                else
                {
                    var swapNode = rootNode.LeftNode;
                    var exchangeNode = rootNode;
                    if (exchangeNode.ParentNode.LeftNode == exchangeNode)
                        exchangeNode.ParentNode.LeftNode = swapNode;
                    if (exchangeNode.ParentNode.RightNode == exchangeNode)
                        exchangeNode.ParentNode.RightNode = swapNode;
                    var tempLeftNode = swapNode.LeftNode;
                    var tempRightNode = swapNode.RightNode;
                    swapNode.LeftNode = exchangeNode;
                    swapNode.ParentNode = exchangeNode.ParentNode;
                    rootNode.ParentNode = rootNode.LeftNode;
                    if (exchangeNode.RightNode != swapNode)
                    {
                        swapNode.RightNode = exchangeNode.RightNode;
                        if(swapNode.RightNode != null)
                            swapNode.RightNode.ParentNode = swapNode;
                    }                    
                    exchangeNode.ParentNode = swapNode;
                    exchangeNode.LeftNode = tempLeftNode;
                    exchangeNode.RightNode = tempRightNode;
                    if (exchangeNode.LeftNode != null)
                    {
                        exchangeNode.LeftNode.ParentNode = exchangeNode;
                    }
                    if (exchangeNode.RightNode != null)
                    {
                        exchangeNode.RightNode.ParentNode = exchangeNode;
                    }
                }
                Swap(rootNode);
            }
            else if (rootNode.RightNode != null && 
                rootNode.RightNode.Value.PriorityKey.CompareTo(rootNode.Value.PriorityKey) == 1)
            {
                if (rootNode.ParentNode == null)
                {
                    var swapNode = rootNode.RightNode;
                    var exchangeNode = rootNode;
                    var tempLeftNode = swapNode.LeftNode;
                    var tempRightNode = swapNode.RightNode;
                    swapNode.RightNode = exchangeNode;
                    rootNode.ParentNode = rootNode.RightNode;
                    swapNode.ParentNode = null;
                    if (exchangeNode.LeftNode != swapNode)
                    {
                        swapNode.LeftNode = exchangeNode.LeftNode;
                        if (swapNode.LeftNode != null)
                            swapNode.LeftNode.ParentNode = swapNode;
                    }                    
                    exchangeNode.ParentNode = swapNode;
                    exchangeNode.LeftNode = tempLeftNode;
                    exchangeNode.RightNode = tempRightNode;
                    if (exchangeNode.LeftNode != null)
                    {
                        exchangeNode.LeftNode.ParentNode = exchangeNode;
                    }
                    if (exchangeNode.RightNode != null)
                    {
                        exchangeNode.RightNode.ParentNode = exchangeNode;
                    }
                    this.rootNode = swapNode;                    
                }
                else
                {
                    var swapNode = rootNode.RightNode;
                    var exchangeNode = rootNode;
                    if (exchangeNode.ParentNode.LeftNode == exchangeNode)
                        exchangeNode.ParentNode.LeftNode = swapNode;
                    if (exchangeNode.ParentNode.RightNode == exchangeNode)
                        exchangeNode.ParentNode.RightNode = swapNode;
                    var tempLeftNode = swapNode.LeftNode;
                    var tempRightNode = swapNode.RightNode;
                    swapNode.RightNode = exchangeNode;
                    swapNode.ParentNode = exchangeNode.ParentNode;
                    rootNode.ParentNode = rootNode.RightNode;
                    if (exchangeNode.LeftNode != swapNode)
                    {
                        swapNode.LeftNode = exchangeNode.LeftNode;
                        if (swapNode.LeftNode != null)
                            swapNode.LeftNode.ParentNode = swapNode;
                    }                    
                    exchangeNode.ParentNode = swapNode;
                    exchangeNode.LeftNode = tempLeftNode;
                    exchangeNode.RightNode = tempRightNode;
                    if (exchangeNode.LeftNode != null)
                    {
                        exchangeNode.LeftNode.ParentNode = exchangeNode;
                    }
                    if (exchangeNode.RightNode != null)
                    {
                        exchangeNode.RightNode.ParentNode = exchangeNode;
                    }
                }
                Swap(rootNode);
            }
            else if (rootNode.LeftNode != null &&
               rootNode.LeftNode.Value.PriorityKey.CompareTo(rootNode.Value.PriorityKey) == 0)
            {
                if (rootNode.ParentNode == null)
                {
                    this.rootNode = rootNode;
                }
            }
            else if (rootNode.RightNode != null &&
               rootNode.RightNode.Value.PriorityKey.CompareTo(rootNode.Value.PriorityKey) == 0)
            {
                if (rootNode.ParentNode == null)
                {
                    this.rootNode = rootNode;
                }
            }
        }

        private HeapNode<P, T> FindLeastNode(HeapNode<P, T> startItem)
        {
            if (startItem.LeftNode == null && startItem.RightNode == null)
            {
                return startItem;
            }
            else if(startItem.LeftNode != null && startItem.RightNode != null)
            {
                if (startItem.LeftNode.Value.PriorityKey.CompareTo(startItem.RightNode.Value.PriorityKey) == -1 ||
                    startItem.LeftNode.Value.PriorityKey.CompareTo(startItem.RightNode.Value.PriorityKey) == 0)
                {
                    return FindLeastNode(startItem.LeftNode);
                }
                else
                {
                    return FindLeastNode(startItem.RightNode);
                }
            }
            else if(startItem.LeftNode != null)
            {
                return FindLeastNode(startItem.LeftNode);
            }
            else if(startItem.RightNode != null)
            {
                return FindLeastNode(startItem.RightNode);
            }
            return startItem;
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
            if (rootNode == null)
            {
                rootNode = new HeapNode<P, T>() { Value = heapItem };
            }
            else
            {
                lock (lockObject)
                {
                    if (rootNode.Value.PriorityKey.CompareTo(heapItem.PriorityKey) == -1 ||
                        rootNode.Value.PriorityKey.CompareTo(heapItem.PriorityKey) == 0)
                    {
                        if (rootNode.LeftNode != null)
                        {
                            Enqueue(rootNode.LeftNode, heapItem);
                        }
                        else
                        {
                            var tempNode = new HeapNode<P, T>() { Value = heapItem };
                            rootNode.LeftNode = tempNode;
                            tempNode.ParentNode = rootNode;
                            CheckForChange(tempNode.ParentNode, tempNode);
                        }
                    }
                    else if (rootNode.Value.PriorityKey.CompareTo(heapItem.PriorityKey) == 1)
                    {
                        if (rootNode.RightNode != null)
                        {
                            Enqueue(rootNode.RightNode, heapItem);
                        }
                        else
                        {
                            var tempNode = new HeapNode<P, T>() { Value = heapItem };
                            rootNode.RightNode = tempNode;
                            tempNode.ParentNode = rootNode;
                            CheckForChange(tempNode.ParentNode, tempNode);
                        }
                    }
                }
            }
        }

        public void Enqueue(HeapNode<P, T> node, Item<P, T> heapItem)
        {            
            if (node.Value.PriorityKey.CompareTo(heapItem.PriorityKey) == -1 ||
                node.Value.PriorityKey.CompareTo(heapItem.PriorityKey) == 0)
            {
                if (node.LeftNode != null)
                {
                    Enqueue(node.LeftNode, heapItem);
                }
                else
                {                    
                    var tempNode = new HeapNode<P, T>() { Value = heapItem };
                    node.LeftNode = tempNode;
                    tempNode.ParentNode = node;
                    CheckForChange(tempNode.ParentNode, tempNode);
                }
            }
            else if (node.Value.PriorityKey.CompareTo(heapItem.PriorityKey) == 1)
            {
                if (node.RightNode != null)
                {
                    Enqueue(node.RightNode, heapItem);
                }
                else
                {
                    var tempNode = new HeapNode<P, T>() { Value = heapItem };
                    node.RightNode = tempNode;
                    tempNode.ParentNode = node;
                    CheckForChange(tempNode.ParentNode, tempNode);
                }
            }
        }

        private void CheckForChange(HeapNode<P, T> parentNode, HeapNode<P, T> actualNode)
        {
            if (parentNode != null)
            {
                if (actualNode.Value.PriorityKey.CompareTo(parentNode.Value.PriorityKey) == 1)
                {
                    Change(parentNode, actualNode);
                    CheckForChange(actualNode.ParentNode, actualNode);
                }                
            }
        }

        private void Change(HeapNode<P, T> parentNode, HeapNode<P, T> actualNode)
        {
            if (parentNode.ParentNode == null)
            {                   
                actualNode.ParentNode = null;
                rootNode = actualNode;                
                parentNode.ParentNode = actualNode;
            }
            else
            {
                if (parentNode.ParentNode.LeftNode != null && 
                    parentNode.ParentNode.LeftNode.Value.ToString() == actualNode.ParentNode.Value.ToString())
                {
                    parentNode.ParentNode.LeftNode = actualNode;                    
                }
                else if(parentNode.ParentNode.RightNode != null &&
                    parentNode.ParentNode.RightNode.Value.ToString() == actualNode.ParentNode.Value.ToString())
                {
                    parentNode.ParentNode.RightNode = actualNode;                    
                }                
                actualNode.ParentNode = parentNode.ParentNode;
                parentNode.ParentNode = actualNode;
            }
            if (parentNode.LeftNode != null &&
                    parentNode.LeftNode.Value.ToString() == actualNode.Value.ToString())
            {
                parentNode.LeftNode = actualNode.LeftNode;
            }
            else if (parentNode.RightNode != null &&
                    parentNode.RightNode.Value.ToString() == actualNode.Value.ToString())
            {
                parentNode.RightNode = actualNode.RightNode;
            }  
            actualNode.LeftNode = parentNode;
            if (parentNode.LeftNode != null)
                parentNode.LeftNode.ParentNode = parentNode;
            if (parentNode.RightNode != null)
                parentNode.RightNode.ParentNode = parentNode;
            CheckForChange(actualNode.ParentNode, actualNode);
        }
    }
}
