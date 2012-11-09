// -----------------------------------------------------------------------
// <copyright file="ExpireDictionary.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DeleteList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;

    /// <summary>
    /// Dictionary that keeps track of the items lifetime and removes the expired ones.
    /// </summary>
    /// <typeparam name="T1">The key of the KeyValuePair</typeparam>
    /// <typeparam name="T2">The value of the KeyValuePair</typeparam>
    public class ExpireDictionary<T1, T2> : IDictionary<T1, T2>
    {
        /// <summary>
        /// The collection that holds the dictionary paired with a DateTime value to track expiration
        /// </summary>
        private System.Collections.Concurrent.ConcurrentDictionary<DateTime, KeyValuePair<T1, T2>> collection = new System.Collections.Concurrent.ConcurrentDictionary<DateTime, KeyValuePair<T1, T2>>();

        /// <summary>
        /// The TimeSpan after which an item in the dictionary is expired and can be removed.
        /// </summary>
        private TimeSpan expiration;

        /// <summary>
        /// The timer used for removing expired elements
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Initializes a new instance of the ExpireDictionary class.
        /// </summary>
        /// <param name="intervalField">The interval at which the list test for old objects.</param>
        /// <param name="expirationField">The TimeSpan an object stay valid inside the list.</param>
        public ExpireDictionary(int intervalField, TimeSpan expirationField)
        {
            this.timer = new Timer();
            this.timer.Interval = intervalField;
            this.timer.Elapsed += new ElapsedEventHandler(this.Tick);
            this.timer.Enabled = true;
            this.timer.Start();

            this.expiration = expirationField;
        }

        /// <summary>
        /// Gets the number of KeyValuePairs in the collection
        /// </summary>
        public int Count
        {
            get { return this.collection.Count; }
        }

        /// <summary>
        /// The TimeSpan after which an item in the dictionary is expired and can be removed.
        /// </summary>
        public TimeSpan Expiration
        {
            get { return this.expiration; }
            set { this.expiration = value; }
        }

        /// <summary>
        /// The interval in milliseconds used for verifying the list and removing expired items.
        /// </summary>
        public int Interval
        {
            get { return (int)this.timer.Interval; }
            set { this.timer.Interval = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the collection  is read-only
        /// </summary>
        /// <returns> True if the collection is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get
            {
                return (this.collection as ICollection<KeyValuePair<T1, T2>>).IsReadOnly;
            }
        }

        /// <summary>
        /// Gets an System.Collections.Generic.ICollection containing the keys of the collection
        /// </summary>
        public ICollection<T1> Keys
        {
            get
            {
                List<T1> result = new List<T1>();
                foreach (var kvp in this.collection)
                {
                    result.Add(kvp.Value.Key);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets an System.Collections.Generic.ICollection containing the values in the collection
        /// </summary>
        public ICollection<T2> Values
        {
            get
            {
                List<T2> result = new List<T2>();
                foreach (var kvp in this.collection)
                {
                    result.Add(kvp.Value.Value);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get or set.</param>
        /// <returns>The element with the specified key.</returns>
        public T2 this[T1 key]
        {
            get
            {
                var keyValuePair = this.collection.Values.FirstOrDefault(kvp => kvp.Key.Equals(key));
                if (!keyValuePair.Equals(default(KeyValuePair<T1, T2>)))
                {
                    return keyValuePair.Value;
                }

                return default(T2);
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }

        /// <summary>
        /// Adds an element with the provided key and value to the collection.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(T1 key, T2 value)
        {
            this.collection.TryAdd(DateTime.Now, new KeyValuePair<T1, T2>(key, value));
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The item to be added to the collection</param>
        public void Add(KeyValuePair<T1, T2> item)
        {
            this.collection.TryAdd(DateTime.Now, item);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            this.collection.Clear();
        }

        /// <summary>
        ///  Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the collection</param>
        /// <returns>true if item is found in the collection otherwise,false.</returns>
        public bool Contains(KeyValuePair<T1, T2> item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the collection contains an item with the specified key.
        /// </summary>
        /// <param name="key">The key of the item to be located in the collection</param>
        /// <returns>true if item is found in the collection otherwise false.</returns>
        public bool ContainsKey(T1 key)
        {
            foreach (var it in this.collection)
            {
                if (it.Value.Key.Equals(key))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Copies the elements of the collection to an
        /// System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements
        /// copied from System.Collections.Generic.ICollection. The System.Array must
        /// have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(KeyValuePair<T1, T2>[] array, int arrayIndex)
        {
            this.collection.Values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An System.Collections.IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return this.collection.Values.GetEnumerator();
        }

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="key">The key of the item to be removed</param>
        /// <returns>true if item was successfully removed from the collection
        /// otherwise, false. This method also returns false if item is not found in
        /// the original collection..</returns>
        public bool Remove(T1 key)
        {
            foreach (var kvp in this.collection)
            {
                if (kvp.Value.Key.Equals(key))
                {
                    KeyValuePair<T1, T2> removedItem;
                    return this.collection.TryRemove(kvp.Key, out removedItem);
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the System.Collections.Generic.ICollection.
        /// </summary>
        /// <param name="item">The object to remove from the collection</param>
        /// <returns>true if item was successfully removed from the collection
        /// otherwise, false. This method also returns false if item is not found in
        /// the original collection.</returns>
        public bool Remove(KeyValuePair<T1, T2> item)
        {
            return this.collection.Values.Remove(item);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise,
        /// the default value for the type of the value
        /// parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the collection
        /// contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(T1 key, out T2 value)
        {
            KeyValuePair<T1, T2> result = this.collection.Values.FirstOrDefault(kvp => kvp.Key.Equals(key));
            if (result.Equals(default(KeyValuePair<T1, T2>)))
            {
                value = default(T2);
                return false;
            }

            value = result.Value;
            return true;
        }

        /// <summary>
        /// The handler for the event Elapsed of the timer
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void Tick(object sender, EventArgs e)
        {
            foreach (var kp in this.collection.Keys.ToList())
            {
                if (DateTime.Now - kp >= this.expiration)
                {
                    KeyValuePair<T1, T2> removedKeyValuePair;
                    this.collection.TryRemove(kp, out removedKeyValuePair);
                    Console.WriteLine("removed element '{0}'", kp.ToString("yyyy.MM.dd HH:mm:ss:ffff"));
                }
            }
        }
    }
}