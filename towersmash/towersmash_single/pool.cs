#region Copyright (c) 2007 Thomas H. Aylesworth
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using FarseerGames.FarseerPhysics;

namespace towersmash
{
    /// <summary>
    /// Represents a fixed-size pool of available items that can be removed
    /// as needed and returned when finished.
    /// 
    /// </summary>
    public class spool<T> : IEnumerable<T> where T : iunit, new()
    {
        /// <summary>
        /// Represents an entry in a Pool collection.
        /// </summary>
        public class Node: Iid
        {
            public Node(int nodeIndex, UnitID containerID) : base(nodeIndex, containerID)
            {
                _item = new T();
                Iid id = new Iid(nodeIndex, containerID);
                _item.id = id;
            }

            /// <summary>
            /// Item stored in this node
            /// </summary>
            private T _item;

            /// <summary>
            /// Item stored in Pool.
            /// Gets the item as reference since it's a user defined type
            /// Sets the item equal to another
            /// </summary>
            public T Item
            {
                get { return _item; }
                set { _item = value; }
            }
        }

        /// <summary>
        /// The ID of this pool
        /// </summary>
        private UnitID _poolID;

        /// <summary>
        /// Fixed Pool of item nodes.
        /// </summary>
        private Node[] pool;

        public T this[int i]
        {
            get 
            { 
                //todo: add extra logic
                
                return pool[i].Item; 
            }
            set 
            { 
                //todo: add extra logic
                
                pool[i].Item = value; 
            }
        }

        /// <summary>
        /// Array containing the active/available state for each item node 
        /// in the Pool.
        /// <remarks> Only use this if you know what you're doing!</remarks>
        /// </summary>
        public bool[] active;

        /// <summary>
        /// Queue of available item node indices, currently "in" and ready to be checked out
        /// </summary>
        private Queue<int> checkedIn;

        private Stack<int> removalStack;

        /// <summary>
        /// queue of item node indices, currently "out" and in use.
        /// </summary>
        /// This linked list is used like a Queue, but can also remove from the middle of the queue at the same time
        
        private LinkedList<int> checkedOut;

        /// <summary>
        /// Returns the PoolID
        /// </summary>
        public UnitID PoolID
        {
            get { return _poolID; }
        }

        /// <summary>
        /// Gets the number of available items in the Pool.
        /// </summary>
        /// <remarks>
        /// Retrieving this property is an O(1) operation.
        /// </remarks>
        public int AvailableCount
        {
            get { return checkedIn.Count; }
        }


        /// <summary>
        /// Gets the number of active items in the Pool.
        /// </summary>
        /// <remarks>
        /// Retrieving this property is an O(1) operation.
        /// </remarks>
        public int ActiveCount
        {
            get { return pool.Length - checkedIn.Count; }
        }

        /// <summary>
        /// Gets the total number of items in the Pool.
        /// </summary>
        /// <remarks>
        /// Retrieving this property is an O(1) operation.
        /// </remarks>
        public int Capacity
        {
            get { return pool.Length; }
        }

        public bool Active(int index)
        {
            return active[index];
        }

        /// <summary>
        /// Publicly available linked list of all nodes that are currently checked out.
        /// <remarks>
        /// The linked list is kept in the same order as a queue. To get the nodeIndex of the oldest (First) item to be put into the list, use .First, it will return an int value. To get the newest item from the list use .Last, it will return an int value.
        /// </remarks>
        /// </summary>
        public LinkedList<int> CheckedOut
        {
            get { return checkedOut; }
        }

        /// <summary>
        /// Initializes a new instance of the Pool class.
        /// </summary>
        /// <param name="numItems">Total number of items in the Pool.</param>
        /// <exception cref="ArgumentException">
        /// Number of items is less than 1.
        /// </exception>
        /// <remarks>
        /// This constructor is an O(n) operation, where n is capacity.
        /// </remarks>
        public spool(int capacity, UnitID cID)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(
                              "Pool must contain at least one item.");
            }
            
            _poolID = cID;
            pool = new Node[capacity];
            active = new bool[capacity];
            checkedIn = new Queue<int>(capacity);
            removalStack = new Stack<int>();

            for (int i = 0; i < capacity; i++)
            {
                pool[i] = new Node(i, cID);
                active[i] = false;               
                
                checkedIn.Enqueue(i);
                checkedOut = new LinkedList<int>();
            }            
        }

        public void Update()
        {
            int nodeID;
            while (removalStack.Count > 0)
            {
                nodeID = removalStack.Pop();
                Return(nodeID);
            }
            if (removalStack.Count != 0)
                throw new Exception("Not all units deleted!");
        }


        /// <summary>
        /// Makes all items in the Pool available.
        /// </summary>
        /// <remarks>
        /// This method is an O(n) operation, where n is Capacity.
        /// </remarks>
        public void Clear()
        {
            checkedIn.Clear();

            for (int i = 0; i < pool.Length; i++)
            {
                active[i] = false;                
                checkedIn.Enqueue(i);                
            }
            checkedOut.Clear();
        }


        /// <summary>
        /// Removes an available item from the Pool and makes it active.
        /// </summary>
        /// <returns>The node that is removed from the available Pool.</returns>
        /// <exception cref="InvalidOperationException">
        /// There are no available items in the Pool.
        /// </exception>
        /// <remarks>
        /// This method is an O(1) operation.
        /// </remarks>
        public Node Retrieve()
        {
            //If there are no more nodes to return
            if (checkedIn.Count == 0)
            {
                //Console.WriteLine("There are no more resources to check out!");
                //Check to see if there are any waiting to be removed
                if (removalStack.Count > 0)
                {
                    //Get from the stack
                    int nodeIndex = removalStack.Pop();
                    //It's not in the checked in queue, and it's in the checked out queue, it's still set to active so don't need to modify them
                    //However, need to requeue it to the back
                    checkedOut.Remove(nodeIndex);
                    checkedOut.AddLast(nodeIndex);
                    //Console.WriteLine("Was able to get a resource from the removal stack");
                    return pool[nodeIndex];
                }
                else
                {
                    //First value is oldest
                    int nodeIndex = checkedOut.First.Value;
                    //Remove from checked out linked list
                    //Normally this is a O(N) operation, but the list checks from front to back so it'll be fast
                    checkedOut.Remove(nodeIndex);
                    //Re-add to check out list, but at the back
                    checkedOut.AddLast(nodeIndex);
                    //Return value
                    //Console.WriteLine("Unable to get resource from removal stack. Returned the first checked-out resource.");
                    return pool[nodeIndex];
                }
            }
            else
            {
                int nodeIndex = checkedIn.Dequeue();
                active[nodeIndex] = true;
                checkedOut.AddLast(nodeIndex);                
                return pool[nodeIndex];
            }  
        }

        public Node Get(int index)
        {
            try
            {
                if ((index < 0) || (index > pool.Length))
                {
                    throw new ArgumentException("Invalid item node.");
                }
                else if (!active[index])
                {
                    throw new InvalidOperationException("Attempt to return an inactive node.");
                }

                return pool[index];
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
                return default(Node);
            }

        }
        /// <summary>
        /// Returns an active item to the available Pool.
        /// </summary>
        /// <param name="item">The node to return to the available Pool.</param>
        /// <exception cref="ArgumentException">
        /// The node being returned is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The node being returned was not active.
        /// This probably means the node was previously returned.
        /// </exception>
        /// <remarks>
        /// Update must be called regularly for the stack to be emptied.
        /// The stack is used to prevent the removal of the object more than once every update.
        /// This method is an O(1) operation.
        /// </remarks>
        private void Return(int NodeIndex)
        {
            if ((NodeIndex < 0) || (NodeIndex > pool.Length))
            {
                throw new ArgumentException("Invalid item node.");
            }
            else if (!active[NodeIndex])
            {
                throw new ArgumentOutOfRangeException("cant");
            }
            else
            {
                //Set node to not active in the active array
                active[NodeIndex] = false;
                //Add to checked in queue
                checkedIn.Enqueue(NodeIndex);
                //Remove from checked out linked list
                checkedOut.Remove(NodeIndex);
            }
        }

        public void Remove(int NodeIndex)
        {
            if (!removalStack.Contains(NodeIndex))
                removalStack.Push(NodeIndex);
        }

        /// <summary>
        /// Sets the value of the item in the Pool associated with the 
        /// given node.
        /// Will not be used much as all objects in the resource pool are used as reference.
        /// </summary>
        /// <param name="item">The node whose item value is to be set.</param>
        /// <exception cref="ArgumentException">
        /// The node being returned is invalid.
        /// </exception>
        /// <remarks>
        /// This method is necessary to modify the value of a value type stored
        /// in the Pool.  It copies the value of the node's Item field into the
        /// Pool.
        /// This method is an O(1) operation.
        /// </remarks>
        public void SetItemValue(Node item)
        {
            if ((item.NodeID < 0) || (item.NodeID > pool.Length))
            {
                throw new ArgumentException("Invalid item node.");
            }

            pool[item.NodeID].Item = item.Item;
        }


        /// <summary>
        /// Copies the active items to an existing one-dimensional Array, 
        /// starting at the specified array index. 
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array to which active Pool items will be 
        /// copied.
        /// </param>
        /// <param name="arrayIndex">
        /// The index in array at which copying begins.
        /// </param>
        /// <returns>The number of items copied.</returns>
        /// <remarks>
        /// This method is an O(n) operation, where n is the smaller of 
        /// capacity or the array length.
        /// </remarks>
        public int CopyTo(T[] array, int arrayIndex)
        {
            int index = arrayIndex;

            foreach (Node item in pool)
            {
                if (active[item.NodeID])
                {
                    array[index++] = item.Item;

                    if (index == array.Length)
                    {
                        return index - arrayIndex;
                    }
                }
            }

            return index - arrayIndex;
        }


        /// <summary>
        /// Gets an enumerator that iterates through the active items 
        /// in the Pool.
        /// </summary>
        /// <returns>Enumerator for the active items.</returns>
        /// <remarks>
        /// This method is an O(n) operation, 
        /// where n is Capacity divided by ActiveCount. 
        /// 
        /// to be noted that this enumerator is to be
        /// used for going through the items in the pool 
        ///
        /// </remarks>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (int index in checkedOut)
            {
                yield return pool[index].Item;

            }            
        }


        /// <summary>
        /// Gets an enumerator that iterates through the active nodes 
        /// in the Pool.
        /// </summary>
        /// <remarks>
        /// This method is an O(n) operation, 
        /// where n is Capacity divided by ActiveCount. 
        /// 
        /// To be noted that this enumerator is to be used
        /// for editing the pool
        /// 
        /// 
        /// </remarks>
        public IEnumerable<Node> ActiveNodes
        {
            get
            {                
                foreach (int index in checkedOut)
                {                    
                        yield return pool[index];
                    
                }
            }
        }


        /// <summary>
        /// Gets an enumerator that iterates through all of the nodes 
        /// in the Pool.
        /// </summary>
        /// <remarks>
        /// This method is an O(1) operation. 
        /// </remarks>
        public IEnumerable<Node> AllNodes
        {
            get
            {
                foreach (Node item in pool)
                {
                    yield return item;
                }
            }
        }


        /// <summary>
        /// Implementation of the IEnumerable interface.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
