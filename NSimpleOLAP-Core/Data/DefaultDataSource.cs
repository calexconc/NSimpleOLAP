using NSimpleOLAP.Configuration;
using NSimpleOLAP.Data.Interfaces;
using NSimpleOLAP.Data.Readers;

namespace NSimpleOLAP.Data
{
  /// <summary>
  /// Description of DefaultDataSource.
  /// </summary>
  public class DefaultDataSource : IDataSource
  {
    private readonly DatabaseConfig _dbConfig;

    public DefaultDataSource()
    {
    }

    public DefaultDataSource(DataSourceConfig config)
    {
      this.Config = config;
      this.Name = config.Name;
    }

    public DefaultDataSource(DataSourceConfig config, DatabaseConfig dbConfig):this(config)
    {
      _dbConfig = dbConfig;
    }

    public string Name
    {
      get;
      set;
    }

    public DataSourceConfig Config
    {
      get;
      set;
    }

    public AbsReader GetReader()
    {
      return AbsReader.Create(this.Config, _dbConfig);
    }
  }
}