using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Configuration.Fluent
{
  /// <summary>
  /// Description of ConfigBuilder.
  /// </summary>
  public class CubeBuilder
  {
    private string _name = string.Empty;
    private StorageConfigBuilder _storeconfig;
    private List<DataSourceBuilder> _datasourceconfigs;
    private MetaDataBuilder _metadataconfig;
    private CubeSourceBuilder _cubeSource;
    private Dictionary<string, DatabaseConnectionBuilder> _dbBuilder;
    private CubeConfig _root;

    public CubeBuilder()
    {
      _root = new CubeConfig();
      _storeconfig = new StorageConfigBuilder();
      _datasourceconfigs = new List<DataSourceBuilder>();
      _metadataconfig = new MetaDataBuilder();
      _cubeSource = new CubeSourceBuilder();
      _dbBuilder = new Dictionary<string, DatabaseConnectionBuilder>();
    }

    internal CubeBuilder(CubeConfig root)
    {
      _root = root;
    }

    #region public methods

    public CubeBuilder SetName(string name)
    {
      _name = name;
      return this;
    }

    public CubeBuilder SetSourceMappings(Action<CubeSourceBuilder> sourceconfig)
    {
      sourceconfig(_cubeSource);
      return this;
    }

    public CubeBuilder Storage(Action<StorageConfigBuilder> storeconfig)
    {
      storeconfig(_storeconfig);
      return this;
    }

    public CubeBuilder AddDataSource(Action<DataSourceBuilder> datasourceconfig)
    {
      DataSourceBuilder builder = new DataSourceBuilder();

      datasourceconfig(builder);
      _datasourceconfigs.Add(builder);

      return this;
    }

    public CubeBuilder MetaData(Action<MetaDataBuilder> medataconfig)
    {
      medataconfig(_metadataconfig);
      return this;
    }

    public CubeBuilder AddDbConnection(string name, Action<DatabaseConnectionBuilder> connectionBuilder)
    {
      var builder = new DatabaseConnectionBuilder(name);
      connectionBuilder(builder);
      _dbBuilder.Add(name, builder);

      return this;
    }

    internal CubeConfig CreateConfig()
    {
      CubeConfig cube = _root;

      cube.Name = _name;
      cube.Storage = _storeconfig.Create();
      cube.MetaData = _metadataconfig.Create();
      cube.DataSources = new DataSourceConfigCollection();
      cube.Source = _cubeSource.Create();

      foreach (var item in _datasourceconfigs)
        cube.DataSources.Add(item.Create());

      foreach (var item in _dbBuilder)
        cube.Database.Add(item.Value.Create());

      return cube;
    }

    #endregion public methods
  }
}