using System;

namespace NSimpleOLAP.Common
{
  /// <summary>
  /// Description of AbsIdentityKey.
  /// </summary>
  public abstract class AbsIdentityKey<T>
    where T : struct, IComparable
  {
    public T InitialKey
    {
      get;
      set;
    }

    public T LastKey
    {
      get;
      protected set;
    }

    public abstract T GetNextKey();

    public static AbsIdentityKey<T> Create()
    {
      Type type = default(T).GetType();

      if (type == typeof(int))
        return (AbsIdentityKey<T>)(object)new IntIdentityKey();
      else if (type == typeof(uint))
        return (AbsIdentityKey<T>)(object)new UIntIdentityKey();
      else if (type == typeof(long))
        return (AbsIdentityKey<T>)(object)new LongIdentityKey();
      else if (type == typeof(UInt64))
        return (AbsIdentityKey<T>)(object)new UInt64IdentityKey();
      else if (type == typeof(Guid))
        return (AbsIdentityKey<T>)(object)new GuidIdentityKey();
      else
        return null;
    }
  }
}