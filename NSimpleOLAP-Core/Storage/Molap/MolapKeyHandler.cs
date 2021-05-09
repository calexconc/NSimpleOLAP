using NSimpleOLAP.Common;
using NSimpleOLAP.Common.Hashing;
using NSimpleOLAP.Configuration;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Storage.Molap
{
  /// <summary>
  /// Description of MolapKeyHandler.
  /// </summary>
  internal class MolapKeyHandler<T>
    where T : struct, IComparable
  {
    private Hasher<T> _hasher;
    private MolapHashTypes _hashingtype;

    public MolapKeyHandler(MolapStorageConfig config)
    {
      _hashingtype = config.HashType;
      this.Init();
    }

    public T GetKey(params KeyValuePair<T, T>[] pairs)
    {
      return this._hasher.HashTuples(pairs);
    }

    #region private members

    private void Init()
    {
      _hasher = Hasher<T>.Create(this._hashingtype);
    }

    #endregion private members
  }
}