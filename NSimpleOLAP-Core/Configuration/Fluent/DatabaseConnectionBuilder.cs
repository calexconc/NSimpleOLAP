using System.Data.Common;

namespace NSimpleOLAP.Configuration.Fluent
{
  public class DatabaseConnectionBuilder
  {
    private ConnectionSettings _element;

    public DatabaseConnectionBuilder(string name)
    {
      _element = new ConnectionSettings() { Name = name };
    }

    public DatabaseConnectionBuilder SetConnectionString(string connectionString)
    {
      _element.ConnectionString = connectionString;
      return this;
    }

    public DatabaseConnectionBuilder SetProviderName(string providerName)
    {
      _element.ProviderName = providerName;
      return this;
    }

    public DatabaseConnectionBuilder SetDbFactory(DbProviderFactory dbFactory)
    {
      _element.ProviderFactory = dbFactory;
      return this;
    }

    internal ConnectionSettings Create()
    {
      return _element;
    }
  }
}