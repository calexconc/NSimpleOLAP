/*
 * Created by SharpDevelop.
 * User: Calex
 * Date: 15-05-2011
 * Time: 21:05
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NSimpleOLAP.Common.Collections
{
  /// <summary>
  /// Description of TSDictionary.
  /// </summary>
  internal class TSDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDisposable
    where TKey : System.IComparable
  {
    private readonly Dictionary<TKey, TValue> _innerDictionary;

    private readonly ReaderWriterLockSlim _dictionaryLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

    #region ctors

    public TSDictionary()
    {
      this._innerDictionary = new Dictionary<TKey, TValue>();
    }

    /// <summary>
    /// Initializes the dictionary object
    /// </summary>
    /// <param name="dictionary">the dictionary whose keys and values are copied to this object</param>
    public TSDictionary(IDictionary<TKey, TValue> dictionary)
    {
      this._innerDictionary = new Dictionary<TKey, TValue>(dictionary);
    }

    /// <summary>
    /// Initializes the dictionary object
    /// </summary>
    /// <param name="capacity">initial capacity of the dictionary</param>
    /// <param name="comparer">the comparer to use when comparing keys</param>
    public TSDictionary(int capacity, IEqualityComparer<TKey> comparer)
    {
      this._innerDictionary = new Dictionary<TKey, TValue>(capacity, comparer);
    }

    /// <summary>
    /// Initializes the dictionary object
    /// </summary>
    /// <param name="dictionary">the dictionary whose keys and values are copied to this object</param>
    /// <param name="comparer">the comparer to use when comparing keys</param>
    public TSDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
    {
      this._innerDictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
    }

    ~TSDictionary()
    {
      this.Dispose();
    }

    #endregion ctors

    #region GetValueAddIfNotExist

    /// <summary>
    /// Returns the value of <paramref name="key"/>. If <paramref name="key"/>
    /// does not exist, <paramref name="func"/> is performed and added to the
    /// dictionary
    /// </summary>
    /// <param name="key">the key to check</param>
    /// <param name="func">the delegate to call if key does not exist</param>
    public TValue GetValueAddIfNotExist(TKey key, Func<TValue> func)
    {
      // enter a write lock, to make absolutely sure that the key
      // is added/deleted from the time we check if it exists
      // to the time we add it if it doesn't exist
      _dictionaryLock.EnterWriteLock();

      try
      {
        TValue rVal;

        // if we have the value, get it and exit
        if (_innerDictionary.TryGetValue(key, out rVal))
          return rVal;

        // not found, so do the function to get the value
        rVal = func.Invoke();

        // add to the dictionary
        _innerDictionary.Add(key, rVal);

        // return the value
        return rVal;
      }
      finally
      {
        _dictionaryLock.ExitWriteLock();
      }
    }

    #endregion GetValueAddIfNotExist

    #region AddIfNotExists

    /// <summary>
    /// Adds the value if it's key does not already exist. Returns
    /// true if the value was added
    /// </summary>
    /// <param name="key">the key to check, add</param>
    /// <param name="value">the value to add if the key does not already exist</param>
    public bool AddIfNotExists(TKey key, TValue value)
    {
      bool rVal = false;

      _dictionaryLock.EnterWriteLock();

      try
      {
        // if not exist, then add it
        if (!_innerDictionary.ContainsKey(key))
        {
          // add the value and set the flag
          _innerDictionary.Add(key, value);
          rVal = true;
        }
      }
      finally
      {
        _dictionaryLock.ExitWriteLock();
      }

      return rVal;
    }

    /// <summary>
    /// Adds the list of value if the keys do not already exist.
    /// </summary>
    /// <param name="keys">the keys to check, add</param>
    /// <param name="defaultValue">the value to add if the key does not already exist</param>
    public void AddIfNotExists(IEnumerable<TKey> keys, TValue defaultValue)
    {
      _dictionaryLock.EnterWriteLock();

      try
      {
        foreach (TKey key in keys)
        {
          // if not exist, then add it
          if (!_innerDictionary.ContainsKey(key))
            _innerDictionary.Add(key, defaultValue);
        }
      }
      finally
      {
        _dictionaryLock.ExitWriteLock();
      }
    }

    #endregion AddIfNotExists

    #region UpdateValueIfKeyExists

    /// <summary>
    /// Updates the value of the key if the key exists. Returns true if updated
    /// </summary>
    /// <param name="key"></param>
    /// <param name="NewValue"></param>
    public bool UpdateValueIfKeyExists(TKey key, TValue NewValue)
    {
      bool rVal = false;

      _dictionaryLock.EnterWriteLock();

      try
      {
        // if we have the key, then update it
        if (_innerDictionary.ContainsKey(key))
        {
          _innerDictionary[key] = NewValue;
          rVal = true;
        }
      }
      finally
      {
        _dictionaryLock.ExitWriteLock();
      }

      return rVal;
    }

    #endregion UpdateValueIfKeyExists

    #region IDictionary implementation

    public TValue this[TKey key]
    {
      get
      {
        this._dictionaryLock.EnterReadLock();

        try
        {
          return this._innerDictionary[key];
        }
        finally
        {
          this._dictionaryLock.ExitReadLock();
        }
      }
      set
      {
        this._dictionaryLock.EnterWriteLock();

        try
        {
          this._innerDictionary[key] = value;
        }
        finally
        {
          this._dictionaryLock.ExitWriteLock();
        }
      }
    }

    public ICollection<TKey> Keys
    {
      get
      {
        this._dictionaryLock.EnterReadLock();

        try
        {
          List<TKey> retList = new List<TKey>(this._innerDictionary.Keys);

          return retList;
        }
        finally
        {
          this._dictionaryLock.ExitReadLock();
        }
      }
    }

    public ICollection<TValue> Values
    {
      get
      {
        this._dictionaryLock.EnterReadLock();

        try
        {
          List<TValue> retList = new List<TValue>(this._innerDictionary.Values);

          return retList;
        }
        finally
        {
          this._dictionaryLock.ExitReadLock();
        }
      }
    }

    public int Count
    {
      get
      {
        this._dictionaryLock.EnterReadLock();

        try
        {
          return this._innerDictionary.Count;
        }
        finally
        {
          this._dictionaryLock.ExitReadLock();
        }
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public bool ContainsKey(TKey key)
    {
      this._dictionaryLock.EnterReadLock();

      try
      {
        return this._innerDictionary.ContainsKey(key);
      }
      finally
      {
        this._dictionaryLock.ExitReadLock();
      }
    }

    public void Add(TKey key, TValue value)
    {
      this._dictionaryLock.EnterWriteLock();

      try
      {
        this._innerDictionary.Add(key, value);
      }
      finally
      {
        this._dictionaryLock.ExitWriteLock();
      }
    }

    public bool Remove(TKey key)
    {
      this._dictionaryLock.EnterWriteLock();

      try
      {
        return this._innerDictionary.Remove(key);
      }
      finally
      {
        this._dictionaryLock.ExitWriteLock();
      }
    }

    /// <summary>
    /// Removes items from the dictionary that match a pattern. Returns true
    /// on success
    /// </summary>
    /// <param name="predKey">Optional expression based on the keys</param>
    /// <param name="predValue">Option expression based on the values</param>
    public bool Remove(Predicate<TKey> predKey, Predicate<TValue> predValue)
    {
      bool retVal = false;

      this._dictionaryLock.EnterWriteLock();

      try
      {
        if (this._innerDictionary.Keys.Count == 0)
          retVal = true;
        else
        {
          List<TKey> listKeys = new List<TKey>();
          IEnumerator<TKey> key_enum = this._innerDictionary.Keys.GetEnumerator();

          while (key_enum.MoveNext())
          {
            bool isMatch = false;

            if (predKey != null)
              isMatch = predKey(key_enum.Current);

            if ((!isMatch) && (predValue != null) && (predValue(this._innerDictionary[key_enum.Current])))
              isMatch = true;

            if (isMatch)
              listKeys.Add(key_enum.Current);
          }

          for (int i = 0; i < listKeys.Count; i++)
            this._innerDictionary.Remove(listKeys[i]);

          retVal = true;
        }
      }
      finally
      {
        this._dictionaryLock.ExitWriteLock();
      }

      return retVal;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      this._dictionaryLock.EnterReadLock();

      try
      {
        return this._innerDictionary.TryGetValue(key, out value);
      }
      finally
      {
        this._dictionaryLock.ExitReadLock();
      }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
      this._dictionaryLock.EnterWriteLock();

      try
      {
        this._innerDictionary.Add(item.Key, item.Value);
      }
      finally
      {
        this._dictionaryLock.ExitWriteLock();
      }
    }

    public void Clear()
    {
      this._dictionaryLock.EnterWriteLock();

      try
      {
        this._innerDictionary.Clear();
      }
      finally
      {
        this._dictionaryLock.ExitWriteLock();
      }
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
      this._dictionaryLock.EnterReadLock();

      try
      {
        return (this._innerDictionary.ContainsKey(item.Key) && this._innerDictionary.ContainsValue(item.Value) &&
                this._innerDictionary[item.Key].Equals(item.Value));
      }
      finally
      {
        this._dictionaryLock.ExitReadLock();
      }
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      this._dictionaryLock.EnterReadLock();

      try
      {
        this._innerDictionary.ToArray().CopyTo(array, arrayIndex);
      }
      finally
      {
        this._dictionaryLock.ExitReadLock();
      }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
      this._dictionaryLock.EnterWriteLock();

      try
      {
        TValue tempValue;

        if (!this._innerDictionary.TryGetValue(item.Key, out tempValue))
          return false;

        if (!item.Value.Equals(tempValue))
          return false;

        return this._innerDictionary.Remove(item.Key);
      }
      finally
      {
        this._dictionaryLock.ExitWriteLock();
      }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      Dictionary<TKey, TValue> tempDict;

      this._dictionaryLock.EnterReadLock();

      try
      {
        tempDict = new Dictionary<TKey, TValue>(this._innerDictionary);
      }
      finally
      {
        this._dictionaryLock.ExitReadLock();
      }

      foreach (KeyValuePair<TKey, TValue> item in tempDict)
        yield return item;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      Dictionary<TKey, TValue> tempDict;

      this._dictionaryLock.EnterReadLock();

      try
      {
        tempDict = new Dictionary<TKey, TValue>(this._innerDictionary);
      }
      finally
      {
        this._dictionaryLock.ExitReadLock();
      }

      foreach (KeyValuePair<TKey, TValue> item in tempDict)
        yield return item;
    }

    #endregion IDictionary implementation

    #region IDisposable

    public void Dispose()
    {
      this._dictionaryLock.Dispose();

      // This object will be cleaned up by the Dispose method.
      // Therefore, you should call GC.SupressFinalize to
      // take this object off the finalization queue
      // and prevent finalization code for this object
      // from executing a second time.
      GC.SuppressFinalize(this);
    }

    #endregion IDisposable
  }
}