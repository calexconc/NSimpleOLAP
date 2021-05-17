using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class CubeConfig
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>CubeElement</c>.
    /// </summary>
 //   [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
 //   [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
    public string Name
    {
      get;
      set;
    }

    /// <summary>
    ///
    /// </summary>
    public CubeSourceConfig Source
    {
      get;
      set;
    }

    public DatabaseConfig Database
    {
      get;
      set;
    } = new DatabaseConfig();

    /// <summary>
    ///
    /// </summary>
    public DataSourceConfigCollection DataSources
    {
      get;
      set;
    } = new DataSourceConfigCollection();


    public MetaDataConfig MetaData
    {
      get;
      set;
    }

    public StorageConfig Storage
    {
      get;
      set;
    }
  }
}