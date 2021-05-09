using NSimpleOLAP.Common;
using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class MolapStorageConfig
  {
    /// <summary>
    ///
    /// </summary>
    public MolapHashTypes HashType
    {
      get;
      set;
    } = MolapHashTypes.FNV;

    public OperationMode OperationType
    {
      get;
      set;
    } = OperationMode.PreAggregate;
  }
}