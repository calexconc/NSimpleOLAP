using System;

namespace NSimpleOLAP.Common.Hashing
{
  internal class MurmurHash2<T> : Hasher<T>
  where T : struct, IComparable
  {
    private const UInt32 _m = 0x5bd1e995;
    private const UInt64 _m2 = 0xc6a4a7935bd1e995;
    private const Int32 _r = 24;
    private const Int32 _r2 = 47;

    private Func<byte[], T> _hashfunction;

    public MurmurHash2()
    {
      this.HashStrategy = MolapHashTypes.MURMUR2;
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

    protected static int HashFunctionInt32(byte[] bytes)
    {
      return unchecked((int)HashFunctionUInt32(bytes));
    }

    protected static uint HashFunctionUInt32(byte[] data)
    {
      return Hash32(data, 0xc58f1a7b);
    }

    protected static long HashFunctionInt64(byte[] bytes)
    {
      return unchecked((long)HashFunctionUInt64(bytes));
    }

    protected static ulong HashFunctionUInt64(byte[] data)
    {
      return Hash64(data, 0xe17a1465);
    }

    public static UInt32 Hash32(byte[] data, UInt32 seed)
    {
      Int32 length = data.Length;
      if (length == 0)
        return 0;
      UInt32 h = seed ^ (UInt32)length;
      Int32 currentIndex = 0;
      while (length >= 4)
      {
        UInt32 k = (UInt32)(data[currentIndex++] | data[currentIndex++] << 8 | data[currentIndex++] << 16 | data[currentIndex++] << 24);
        k *= _m;
        k ^= k >> _r;
        k *= _m;

        h *= _m;
        h ^= k;
        length -= 4;
      }
      switch (length)
      {
        case 3:
          h ^= (UInt16)(data[currentIndex++] | data[currentIndex++] << 8);
          h ^= (UInt32)(data[currentIndex] << 16);
          h *= _m;
          break;

        case 2:
          h ^= (UInt16)(data[currentIndex++] | data[currentIndex] << 8);
          h *= _m;
          break;

        case 1:
          h ^= data[currentIndex];
          h *= _m;
          break;

        default:
          break;
      }

      // Do a few final mixes of the hash to ensure the last few
      // bytes are well-incorporated.

      h ^= h >> 13;
      h *= _m;
      h ^= h >> 15;

      return h;
    }

    public static UInt64 Hash64(byte[] data, UInt64 seed)
    {
      Int64 length = data.Length;
      if (length == 0)
        return 0;
      UInt64 h = seed ^ (UInt64)length;
      Int64 currentIndex = 0;

      while (length >= 8)
      {
        UInt64 k = (UInt64)(data[currentIndex++] | data[currentIndex++] << 8 | data[currentIndex++] << 16 | data[currentIndex++] << 24
                         | data[currentIndex++] << 32 | data[currentIndex++] << 40 | data[currentIndex++] << 48
                        | data[currentIndex++] << 56);
        k *= _m2;
        k ^= k >> _r2;
        k *= _m2;

        h *= _m2;
        h ^= k;
        length -= 8;
      }
      switch (length)
      {
        case 7:
          h ^= (UInt16)(data[currentIndex++] | data[currentIndex++] << 8);
          h ^= (UInt32)(data[currentIndex] << 16 | data[currentIndex++] << 24);
          h ^= (UInt64)(data[currentIndex] << 32 | data[currentIndex] << 40 | data[currentIndex] << 48);
          h *= _m2;
          break;

        case 6:
          h ^= (UInt16)(data[currentIndex++] | data[currentIndex++] << 8);
          h ^= (UInt32)(data[currentIndex] << 16 | data[currentIndex++] << 24);
          h ^= (UInt64)(data[currentIndex] << 32 | data[currentIndex] << 40);
          h *= _m2;
          break;

        case 5:
          h ^= (UInt16)(data[currentIndex++] | data[currentIndex++] << 8);
          h ^= (UInt32)(data[currentIndex] << 16 | data[currentIndex++] << 24);
          h ^= (UInt64)(data[currentIndex] << 32);
          h *= _m2;
          break;

        case 4:
          h ^= (UInt16)(data[currentIndex++] | data[currentIndex++] << 8);
          h ^= (UInt32)(data[currentIndex] << 16 | data[currentIndex++] << 24);
          h *= _m2;
          break;

        case 3:
          h ^= (UInt16)(data[currentIndex++] | data[currentIndex++] << 8);
          h ^= (UInt32)(data[currentIndex] << 16);
          h *= _m2;
          break;

        case 2:
          h ^= (UInt16)(data[currentIndex++] | data[currentIndex] << 8);
          h *= _m2;
          break;

        case 1:
          h ^= data[currentIndex];
          h *= _m2;
          break;

        default:
          break;
      }

      // Do a few final mixes of the hash to ensure the last few
      // bytes are well-incorporated.

      h ^= h >> _r2;
      h *= _m2;
      h ^= h >> _r2;

      return h;
    }

    #endregion
  }
}