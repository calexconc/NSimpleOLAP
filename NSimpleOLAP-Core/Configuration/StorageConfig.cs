using NSimpleOLAP.Common;
using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class StorageConfig
  {
    /// <summary>
    ///
    /// </summary>
    public StorageType StoreType
    {
      get;
      set;
    } = StorageType.Molap;

    public MolapStorageConfig MolapConfig
    {
      get;
      set;
    }
  }
}