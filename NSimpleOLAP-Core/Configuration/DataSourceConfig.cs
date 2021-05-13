using NSimpleOLAP.Common;
using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class DataSourceConfig
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>DataSourceConfigElement</c>.
    /// </summary>
  //  [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    public string Name
    {
      get;
      set;
    }

    /// <summary>
    ///
    /// </summary>
    public DataSourceType SourceType
    {
      get;
      set;
    }

    public FieldConfigCollection Fields
    {
      get;
      set;
    } = new FieldConfigCollection();


    public DBConfigElement DBConfig
    {
      get;
      set;
    }

    public CSVConfig CSVConfig
    {
      get;
      set;
    }

    internal DataTableConfig DTableConfig
    {
      get;
      set;
    }

    
    public TransformerConfigElement TransformerConfig
    {
      get;
      set;
    }

    public ObjectMapperConfigElement ObjectMapperConfig
    {
      get;
      set;
    }
    
  }
}