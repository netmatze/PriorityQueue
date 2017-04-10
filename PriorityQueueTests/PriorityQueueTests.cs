using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PriorityQueue;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace PriorityQueueTests
{
    [TestClass]
    public class PriorityQueueTests
    {
        [TestMethod]
        public void PriorityQueueListModeTest()
        {
            PriorityQueue<int, string> priorityQueueList = 
                new PriorityQueue<int, string>(PriorityQueueMode.LinkedList);
            priorityQueueList.Enqueue(1, "A");
            priorityQueueList.Enqueue(2, "B");            
            priorityQueueList.Enqueue(3, "C");
            priorityQueueList.Enqueue(4, "D");
            priorityQueueList.Enqueue(5, "E");
            priorityQueueList.Enqueue(6, "F");
            var count = priorityQueueList.Count;
            var result = priorityQueueList.Dequeue();
            Assert.AreEqual(result, "F");
            result = priorityQueueList.Dequeue();
            Assert.AreEqual(result, "E");
            result = priorityQueueList.Dequeue();
            Assert.AreEqual(result, "D");
            result = priorityQueueList.Dequeue();
            Assert.AreEqual(result, "C");
            result = priorityQueueList.Dequeue();
            Assert.AreEqual(result, "B");
            result = priorityQueueList.Dequeue();
            Assert.AreEqual(result, "A");
        }

        [TestMethod]
        public void PriorityQueueBinaryHeapModeTest()
        {
            PriorityQueue<int, string> priorityQueueHeap =
                new PriorityQueue<int, string>(PriorityQueueMode.BinaryHeap);
            priorityQueueHeap.Enqueue(1, "A");
            priorityQueueHeap.Enqueue(2, "B");
            priorityQueueHeap.Enqueue(3, "C");
            priorityQueueHeap.Enqueue(4, "D");
            priorityQueueHeap.Enqueue(5, "E");
            priorityQueueHeap.Enqueue(6, "F");
            var count = priorityQueueHeap.Count;
            var result = priorityQueueHeap.Dequeue();
            Assert.AreEqual(result, "F");
            result = priorityQueueHeap.Dequeue();
            Assert.AreEqual(result, "E");
            result = priorityQueueHeap.Dequeue();
            Assert.AreEqual(result, "D");
            result = priorityQueueHeap.Dequeue();
            Assert.AreEqual(result, "C");
            result = priorityQueueHeap.Dequeue();
            Assert.AreEqual(result, "B");
            result = priorityQueueHeap.Dequeue();
            Assert.AreEqual(result, "A");
        }

        [TestMethod]
        public void PriorityQueueTest()
        {
            PriorityQueue<int, string> priorityQueue = new PriorityQueue<int, string>();
            priorityQueue.Enqueue(1, "A");
            priorityQueue.Enqueue(2, "B");
            priorityQueue.Enqueue(3, "C");
            priorityQueue.Enqueue(4, "D");
            priorityQueue.Enqueue(5, "E");
            priorityQueue.Enqueue(6, "F");
            var count = priorityQueue.Count;            
            var result = priorityQueue.Dequeue();
            Assert.AreEqual(result, "F");
            result = priorityQueue.Dequeue();
            Assert.AreEqual(result, "E");
            result = priorityQueue.Dequeue();
            Assert.AreEqual(result, "D");
            result = priorityQueue.Dequeue();
            Assert.AreEqual(result, "C");
            result = priorityQueue.Dequeue();
            Assert.AreEqual(result, "B");
            result = priorityQueue.Dequeue();
            Assert.AreEqual(result, "A");
            result = priorityQueue.Dequeue();
            priorityQueue.Enqueue(3, "G");
            priorityQueue.Enqueue(4, "H");
            priorityQueue.Enqueue(2, "I");
            priorityQueue.Enqueue(1, "J");
            priorityQueue.PrintItems();
            result = priorityQueue.Dequeue();
            priorityQueue.PrintItems();
            result = priorityQueue.Dequeue();
            priorityQueue.PrintItems();
            result = priorityQueue.Dequeue();
            priorityQueue.PrintItems();
            result = priorityQueue.Dequeue();
            priorityQueue.PrintItems();
            result = priorityQueue.Dequeue();
            priorityQueue.PrintItems();
            var contains = priorityQueue.Contains("J");
            foreach (var item in priorityQueue)
            {
                var localItem = item;
            }
            Array array = new string[100];
            priorityQueue.CopyTo(array, 0);
            var newArray = new string[100];
            priorityQueue.CopyTo(newArray, 1);
        }

        [TestMethod]
        public void PriorityQueueHeapProducerConsumerTest()
        {
            PriorityQueue<int, string> priorityQueue = new PriorityQueue<int, string>(PriorityQueueMode.BinaryHeap);
            Task producerTask = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 30; i++)
                {
                    if (i % 4 == 0)
                    {
                        priorityQueue.Enqueue(4, i.ToString());
                    }
                    else if (i % 2 == 0)
                    {
                        priorityQueue.Enqueue(i, i.ToString());
                    }
                    else if (i % 3 == 0)
                    {
                        priorityQueue.Enqueue(3, i.ToString());
                    }
                    else
                    {
                        priorityQueue.Enqueue(1, i.ToString());
                    }
                }
            });
            Thread.Sleep(1000);
            Task consumerTask = Task.Factory.StartNew(() =>
            {
                var result = String.Empty;
                do
                {
                    result = priorityQueue.Dequeue();
                    Debug.WriteLine(string.Format(" Item {0} ", result));
                }
                while (!string.IsNullOrEmpty(result));
            });
            consumerTask.Wait();
            Debug.WriteLine("End");
        }

        [TestMethod]
        public void PriorityQueueHeapTest()
        {
            PriorityQueue<int, string> priorityQueue = new PriorityQueue<int, string>(PriorityQueueMode.BinaryHeap);
            for (int i = 0; i < 30; i++)
            {
                if (i % 4 == 0)
                {
                    priorityQueue.Enqueue(4, i.ToString());
                }
                else if (i % 2 == 0)
                {
                    priorityQueue.Enqueue(i, i.ToString());
                }
                else if (i % 3 == 0)
                {
                    priorityQueue.Enqueue(3, i.ToString());
                }
                else
                {
                    priorityQueue.Enqueue(1, i.ToString());
                }
            }
            //priorityQueue.Enqueue(1, "H");            
            //priorityQueue.Enqueue(8, "A");
            //priorityQueue.Enqueue(3, "F");
            //priorityQueue.Enqueue(5, "D");
            //priorityQueue.Enqueue(7, "B");
            //priorityQueue.Enqueue(2, "G");
            //priorityQueue.Enqueue(6, "C");
            //priorityQueue.Enqueue(4, "E");
            foreach (var item in priorityQueue)
            {
                var localItem = item;
                Debug.WriteLine(string.Format(" Item {0} ", localItem));
            }
            //priorityQueue.Enqueue(1, "A");
            //priorityQueue.Enqueue(1, "B");
            //priorityQueue.Enqueue(1, "C");
            //priorityQueue.Enqueue(2, "D");
            //priorityQueue.Enqueue(2, "E");
            //priorityQueue.Enqueue(3, "F");
            //priorityQueue.Enqueue(4, "G");
            //priorityQueue.Enqueue(4, "H");
            var counter = 30;
            for (int i = 0; i < 30; i++)
            {
                Debug.WriteLine("----------------- " + i.ToString());
                var result = priorityQueue.Dequeue();
                priorityQueue.PrintItems();
                counter--;
                Assert.AreEqual(priorityQueue.Count, counter);
            }           
            priorityQueue.Enqueue(5, "A");
            priorityQueue.Enqueue(4, "B");
            priorityQueue.Enqueue(3, "C");
            priorityQueue.Enqueue(2, "D");
            priorityQueue.Enqueue(1, "E");
            priorityQueue.Enqueue(2, "F");
            priorityQueue.Enqueue(3, "G");
            priorityQueue.Enqueue(4, "H");
            priorityQueue.Enqueue(5, "I");
            priorityQueue.Enqueue(6, "J");
            priorityQueue.PrintItems();
            counter = 10;
            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine("----------------- " + i.ToString());
                var result = priorityQueue.Dequeue();
                priorityQueue.PrintItems();
                counter--;
                Assert.AreEqual(priorityQueue.Count, counter);
            }     
            //result = priorityQueue.Dequeue();
            //priorityQueue.PrintItems();
            //result = priorityQueue.Dequeue();
            //priorityQueue.PrintItems();
            //result = priorityQueue.Dequeue();
            //priorityQueue.PrintItems();
            //result = priorityQueue.Dequeue();
            //priorityQueue.PrintItems();
            //result = priorityQueue.Dequeue();
            //priorityQueue.PrintItems();
            //priorityQueue.Enqueue(2, "D");
            //priorityQueue.Enqueue(1, "E");
            //priorityQueue.Enqueue(2, "F");
            //priorityQueue.PrintItems();
            //result = priorityQueue.Dequeue();
            //priorityQueue.PrintItems();
            //priorityQueue.Enqueue(3, "G");
            //priorityQueue.Enqueue(4, "H");
            //priorityQueue.Enqueue(5, "I");
            ////priorityQueue.Enqueue(6, "J");
            //priorityQueue.PrintItems();
            //var count = priorityQueue.Count;
            ////priorityQueue.PrintItems();
            ////result = priorityQueue.Dequeue();
            ////result = priorityQueue.Dequeue();
            ////result = priorityQueue.Dequeue();
            //priorityQueue.Enqueue(2, "K");
            //priorityQueue.Enqueue(3, "L");
            //priorityQueue.Enqueue(1, "M");
            //result = priorityQueue.Dequeue();
            //result = priorityQueue.Dequeue();
            //priorityQueue.PrintItems();
            //result = priorityQueue.Dequeue();
            //priorityQueue.PrintItems();
            //result = priorityQueue.Dequeue();
            //priorityQueue.PrintItems();
            //result = priorityQueue.Dequeue();
            //priorityQueue.PrintItems();
            //result = priorityQueue.Dequeue();
            //priorityQueue.PrintItems();
            //var contains = priorityQueue.Contains("J");
            //foreach (var item in priorityQueue)
            //{
            //    var localItem = item;
            //}
            //Array array = new string[100];
            //priorityQueue.CopyTo(array, 0);
            //var newArray = new string[100];
            //priorityQueue.CopyTo(newArray, 1);
        }
    }
}
