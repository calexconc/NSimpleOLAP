using System.Data.Common;

namespace NSimpleOLAP.Configuration
{
  public class ConnectionSettings
  {
    public string Name { get; set; }

    public string ConnectionString { get; set; }

    public string ProviderName { get; set; }

    public DbProviderFactory ProviderFactory { get; set; }
  }
}