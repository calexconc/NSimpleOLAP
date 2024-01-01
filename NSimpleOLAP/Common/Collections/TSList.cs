using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace NSimpleOLAP.Common.Collections
{
  internal class TSList<TValue> : IList<TValue>, IDisposable
  {
    private readonly List<TValue> _innerList;

    private readonly ReaderWriterLockSlim _listLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

    public TSList()
    {
      this._innerList = new List<TValue>();
    }

    public TSList(IList<TValue> list)
    {
      this._innerList = new List<TValue>(list);
    }

    ~TSList()
    {
      this.Dispose();
    }

    public TValue this[int index]
    {
      get
      {
        this._listLock.EnterReadLock();

        try
        {
          return this._innerList[index];
        }
        finally
        {
          this._listLock.ExitReadLock();
        }
      }
      set
      {
        this._listLock.EnterWriteLock();

        try
        {
          this._innerList[index] = value;
        }
        finally
        {
          this._listLock.ExitWriteLock();
        }
      }
    }

    public int Count
    {
      get
      {
        this._listLock.EnterReadLock();

        try
        {
          return this._innerList.Count;
        }
        finally
        {
          this._listLock.ExitReadLock();
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

    public void Add(TValue item)
    {
      this._listLock.EnterWriteLock();

      try
      {
        this._innerList.Add(item);
      }
      finally
      {
        this._listLock.ExitWriteLock();
      }
    }

    public void Clear()
    {
      this._listLock.EnterWriteLock();

      try
      {
        this._innerList.Clear();
      }
      finally
      {
        this._listLock.ExitWriteLock();
      }
    }

    public bool Contains(TValue item)
    {
      this._listLock.EnterReadLock();

      try
      {
        return _innerList.Exists(x => x.Equals(item));
      }
      finally
      {
        this._listLock.ExitReadLock();
      }
    }

    public void CopyTo(TValue[] array, int arrayIndex)
    {
      this._listLock.EnterReadLock();

      try
      {
        this._innerList.ToArray().CopyTo(array, arrayIndex);
      }
      finally
      {
        this._listLock.ExitReadLock();
      }
    }

    public void Dispose()
    {
      this._listLock.Dispose();

      // This object will be cleaned up by the Dispose method.
      // Therefore, you should call GC.SupressFinalize to
      // take this object off the finalization queue
      // and prevent finalization code for this object
      // from executing a second time.
      GC.SuppressFinalize(this);
    }

    public IEnumerator<TValue> GetEnumerator()
    {
      List<TValue> tempList;

      this._listLock.EnterReadLock();

      try
      {
        tempList = new List<TValue>(this._innerList);
      }
      finally
      {
        this._listLock.ExitReadLock();
      }

      foreach (TValue item in tempList)
        yield return item;
    }

    public int IndexOf(TValue item)
    {
      this._listLock.EnterReadLock();

      try
      {
        return _innerList.FindIndex(x => x.Equals(item));
      }
      finally
      {
        this._listLock.ExitReadLock();
      }
    }

    public void Insert(int index, TValue item)
    {
      this._listLock.EnterWriteLock();

      try
      {
        this._innerList.Insert(index, item);
      }
      finally
      {
        this._listLock.ExitWriteLock();
      }
    }

    public bool Remove(TValue item)
    {
      this._listLock.EnterWriteLock();

      try
      {
        var index = _innerList.FindIndex(x => x.Equals(item));

        if (index >= 0)
        {
          this._innerList.RemoveAt(index);

          return true;
        }

        return false;
      }
      finally
      {
        this._listLock.ExitWriteLock();
      }
    }

    public void RemoveAt(int index)
    {
      this._listLock.EnterWriteLock();

      try
      {
        this._innerList.RemoveAt(index);
      }
      finally
      {
        this._listLock.ExitWriteLock();
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      List<TValue> tempList;

      this._listLock.EnterReadLock();

      try
      {
        tempList = new List<TValue>(this._innerList);
      }
      finally
      {
        this._listLock.ExitReadLock();
      }

      foreach (TValue item in tempList)
        yield return item;
    }
  }
}