using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Common.Hashing
{
  /// <summary>
  /// Description of Hasher.
  /// </summary>
  public abstract class Hasher<T>
    where T : struct, IComparable
  {
    public T HashTuples(params KeyValuePair<T, T>[] tuples)
    {
      return HashingFunction(GetTuplesBytes(tuples).ToArray());
    }

    public MolapHashTypes HashStrategy
    {
      get;
      protected set;
    }

    protected abstract T HashingFunction(byte[] bytes);

    public static Hasher<T> Create(MolapHashTypes type)
    {
      Hasher<T> hasher = null;

      switch (type)
      {
        case MolapHashTypes.FNV:
          hasher = new FNVHasher<T>();
          break;

        case MolapHashTypes.FNV1A:
          hasher = new FNV1aHasher<T>();
          break;

        case MolapHashTypes.MURMUR2:
          hasher = new MurmurHash2<T>();
          break;

        case MolapHashTypes.CITY:
          hasher = new CityHash<T>();
          break;
      }

      return hasher;
    }

    private IEnumerable<byte> GetTuplesBytes(KeyValuePair<T, T>[] tuples)
    {
      foreach (byte item in KeyStreamer.TransformKeys<T>(tuples))
        yield return item;
    }
  }
}