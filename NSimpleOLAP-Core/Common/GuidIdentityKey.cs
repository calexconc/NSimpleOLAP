using System;

namespace NSimpleOLAP.Common
{
  /// <summary>
  /// Description of GuidIdentityKey.
  /// </summary>
  public class GuidIdentityKey : AbsIdentityKey<Guid>
  {
    public GuidIdentityKey()
    {
      this.InitialKey = Guid.Empty;
      this.LastKey = Guid.Empty;
    }

    public override Guid GetNextKey()
    {
      this.LastKey = Guid.NewGuid();

      return this.LastKey;
    }
  }
}