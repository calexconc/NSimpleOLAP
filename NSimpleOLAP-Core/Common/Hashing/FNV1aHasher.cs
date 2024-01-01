using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Common.Hashing
{
  /// <summary>
  /// Description of FNV1aHasher.
  /// </summary>
  public class FNV1aHasher<T> : Hasher<T>
    where T : struct, IComparable
  {
    private Func<byte[], T> _hashfunction;

    public FNV1aHasher()
    {
      this.HashStrategy = MolapHashTypes.FNV;
      _hashfunction = this.Init();
    }

    protected override T HashingFunction(byte[] bytes)
    {
      return _hashfunction(bytes);
    }

    #region

    private Func<byte[], T> Init()
    {
      Type type = typeof(T);
      Func<byte[], T> function = null;

      if (type == typeof(int))
        function = (Func<byte[], T>)(object)FuncInt();
      else if (type == typeof(uint))
        function = (Func<byte[], T>)(object)FuncUInt();
      else if (type == typeof(long))
        function = (Func<byte[], T>)(object)FuncLong();
      else if (type == typeof(ulong))
        function = (Func<byte[], T>)(object)FuncULong();

      return function;
    }

    private Func<byte[], int> FuncInt()
    {
      return (bytes) => HashFunctionInt32(bytes);
    }

    private Func<byte[], uint> FuncUInt()
    {
      return (bytes) => HashFunctionUInt32(bytes);
    }

    private Func<byte[], long> FuncLong()
    {
      return (bytes) => HashFunctionInt64(bytes);
    }

    private Func<byte[], ulong> FuncULong()
    {
      return (bytes) => HashFunctionUInt64(bytes);
    }

    #endregion

    #region hash functions

    protected static int HashFunctionInt32(IEnumerable<byte> bytes)
    {
      return unchecked((int)HashFunctionUInt32(bytes));
    }

    protected static uint HashFunctionUInt32(IEnumerable<byte> bytes)
    {
      uint hash = 2166136261;
      foreach (byte b in bytes)
      {
        hash ^= b;
        hash *= 16777619;
      }
      return hash;
    }

    protected static long HashFunctionInt64(IEnumerable<byte> bytes)
    {
      return unchecked((long)HashFunctionUInt64(bytes));
    }

    protected static ulong HashFunctionUInt64(IEnumerable<byte> bytes)
    {
      ulong hash = 14695981039346656037;
      foreach (byte b in bytes)
      {
        hash ^= b;
        hash *= 1099511628211;
      }
      return hash;
    }

    #endregion
  }
}