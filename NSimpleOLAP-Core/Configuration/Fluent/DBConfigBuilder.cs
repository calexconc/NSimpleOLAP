namespace NSimpleOLAP.Configuration.Fluent
{
  /// <summary>
  /// Description of DBConfigBuilder.
  /// </summary>
  public class DBConfigBuilder
  {
    private DBConfigElement _element;

    public DBConfigBuilder()
    {
      _element = new DBConfigElement();
    }

    #region public methods

    public DBConfigBuilder SetExternalConfig(string file)
    {
      _element.AppSettings = file;
      return this;
    }

    public DBConfigBuilder SetConnection(string connection, string providerName)
    {
      _element.Connection = connection;
      _element.ProviderName = providerName;
      return this;
    }

    public DBConfigBuilder SetQuery(string query)
    {
      _element.Query = query;
      return this;
    }

    internal DBConfigElement Create()
    {
      return _element;
    }

    #endregion public methods
  }
}