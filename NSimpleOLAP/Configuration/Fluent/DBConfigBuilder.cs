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

    public DBConfigBuilder SetConnection(string connection)
    {
      _element.Connection = connection;
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