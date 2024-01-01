using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class CubeConfig : ConfigurationElement
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>CubeElement</c>.
    /// </summary>
    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
    public string Name
    {
      get { return (string)this["name"]; }
      set { this["name"] = value; }
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty("Source", IsRequired = true)]
    public CubeSourceConfig Source
    {
      get { return (CubeSourceConfig)this["Source"]; }
      set { this["Source"] = value; }
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty("DataSources")]
    public DataSourceConfigCollection DataSources
    {
      get { return (DataSourceConfigCollection)this["DataSources"]; }
      set { this["DataSources"] = value; }
    }

    [ConfigurationProperty("MetaData")]
    public MetaDataConfig MetaData
    {
      get { return (MetaDataConfig)this["MetaData"]; }
      set { this["MetaData"] = value; }
    }

    [ConfigurationProperty("Storage")]
    public StorageConfig Storage
    {
      get { return (StorageConfig)this["Storage"]; }
      set { this["Storage"] = value; }
    }
  }
}