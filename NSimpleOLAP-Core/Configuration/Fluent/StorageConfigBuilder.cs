using NSimpleOLAP.Common;
using System;

namespace NSimpleOLAP.Configuration.Fluent
{
  /// <summary>
  /// Description of StorageConfigBuilder.
  /// </summary>
  public class StorageConfigBuilder
  {
    private StorageConfig _element;
    private Action<MolapStorageBuilder> _molapBuilder;

    public StorageConfigBuilder()
    {
      _element = new StorageConfig();
    }

    #region public methods

    public StorageConfigBuilder SetStoreType(StorageType storetype)
    {
      _element.StoreType = storetype;
      return this;
    }

    public StorageConfigBuilder SetMolapStorage(Action<MolapStorageBuilder> molapbuilder)
    {
      _molapBuilder = molapbuilder;
      return this;
    }

    internal StorageConfig Create()
    {
      if (_element.StoreType == StorageType.Molap)
      {
        MolapStorageBuilder builder = new MolapStorageBuilder();

        if (_molapBuilder != null)
          _molapBuilder(builder);

        _element.MolapConfig = builder.Create();
      }

      return _element;
    }

    #endregion public methods
  }
}