using NSimpleOLAP.Common.Collections;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.Data.Interfaces;
using System.Collections.Generic;

namespace NSimpleOLAP.Data
{
  /// <summary>
  /// Description of DataSourceCollection.
  /// </summary>
  public class DataSourceCollection : ICollection<IDataSource>
  {
    private TSDictionary<string, IDataSource> _innerdict;

    private readonly DatabaseConfig _dbConfig;

    public DataSourceCollection()
    {
      _innerdict = new TSDictionary<string, IDataSource>();
    }

    public DataSourceCollection(CubeConfig config) : this()
    {
      _dbConfig = config.Database;
      this.Initialize(config.DataSources);
    }

    public IDataSource this[string key]
    {
      get { return _innerdict[key]; }
    }

    #region ICollection<IDataSource> implementation

    public int Count
    {
      get
      {
        return _innerdict.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public void Add(IDataSource item)
    {
      _innerdict.Add(item.Name, item);
    }

    public void Clear()
    {
      _innerdict.Clear();
    }

    public bool ContainsKey(string name)
    {
      return _innerdict.ContainsKey(name);
    }

    public bool Contains(IDataSource item)
    {
      return _innerdict.ContainsKey(item.Name);
    }

    public void CopyTo(IDataSource[] array, int arrayIndex)
    {
      _innerdict.Values.CopyTo(array, arrayIndex);
    }

    public bool Remove(IDataSource item)
    {
      return _innerdict.Remove(item.Name);
    }

    public IEnumerator<IDataSource> GetEnumerator()
    {
      foreach (var item in _innerdict.Values)
        yield return item;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      foreach (var item in _innerdict.Values)
        yield return item;
    }

    #endregion ICollection<IDataSource> implementation

    #region private methods

    private void Initialize(DataSourceConfigCollection collconfig)
    {
      foreach (DataSourceConfig item in collconfig)
        this.Add(this.CreateDataSource(item));
    }

    private IDataSource CreateDataSource(DataSourceConfig config)
    {
      if (config.SourceType != Common.DataSourceType.DataBase)
        return new DefaultDataSource(config);
      else
        return new DefaultDataSource(config, _dbConfig);
    }

    #endregion private methods
  }
}