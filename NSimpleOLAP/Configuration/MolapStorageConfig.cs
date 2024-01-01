using NSimpleOLAP.Common;
using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class MolapStorageConfig : ConfigurationElement
  {
    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty("hashType", DefaultValue = MolapHashTypes.FNV)]
    public MolapHashTypes HashType
    {
      get { return (MolapHashTypes)this["hashType"]; }
      set { this["hashType"] = value; }
    }

    [ConfigurationProperty("operation", IsRequired = true, DefaultValue = OperationMode.PreAggregate)]
    public OperationMode OperationType
    {
      get { return (OperationMode)this["operation"]; }
      set { this["operation"] = value; }
    }
  }
}