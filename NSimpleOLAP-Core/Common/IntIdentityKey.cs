using System;

namespace NSimpleOLAP.Common
{
  /// <summary>
  /// Description of IntIdentityKey.
  /// </summary>
  public class IntIdentityKey : AbsIdentityKey<int>
  {
    public IntIdentityKey()
    {
      this.InitialKey = 0;
      this.LastKey = 0;
    }

    public override int GetNextKey()
    {
      this.LastKey += 1;

      return this.LastKey;
    }
  }

  public class UIntIdentityKey : AbsIdentityKey<uint>
  {
    public UIntIdentityKey()
    {
      this.InitialKey = 0;
      this.LastKey = 0;
    }

    public override uint GetNextKey()
    {
      this.LastKey += 1;

      return this.LastKey;
    }
  }

  public class LongIdentityKey : AbsIdentityKey<long>
  {
    public LongIdentityKey()
    {
      this.InitialKey = 0;
      this.LastKey = 0;
    }

    public override long GetNextKey()
    {
      this.LastKey += 1;

      return this.LastKey;
    }
  }

  public class UInt64IdentityKey : AbsIdentityKey<UInt64>
  {
    public UInt64IdentityKey()
    {
      this.InitialKey = 0;
      this.LastKey = 0;
    }

    public override UInt64 GetNextKey()
    {
      this.LastKey += 1;

      return this.LastKey;
    }
  }
}