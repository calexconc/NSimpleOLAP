using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.Interfaces;
using NSimpleOLAP.Storage.Interfaces;
using NSimpleOLAP.Storage.Molap;
using NSimpleOLAP.Triggers.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Storage
{
  /// <summary>
  /// Description of StorageFactory.
  /// </summary>
  public abstract class StorageFactory<T, U>
    where T : struct, IComparable
    where U : class, ICell<T>
  {
    public static IStorage<T, U> Create(T cubeid, StorageConfig config, IList<ITrigger<T>> triggers = null)
    {
      IStorage<T, U> storage = null;

      switch (config.StoreType)
      {
        case StorageType.Molap:
          storage = new MolapStorage<T, U>(cubeid, config, triggers);
          break;

        case StorageType.Rolap:
          break;
      }

      return storage;
    }
  }
}