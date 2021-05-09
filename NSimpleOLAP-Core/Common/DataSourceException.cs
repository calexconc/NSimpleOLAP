using System;

namespace NSimpleOLAP.Common
{
  /// <summary>
  /// Description of DataSourceException.
  /// </summary>
  public class DataSourceException : Exception
  {
    public DataSourceException(string datasource, string message) : base(message)
    {
      this.DataSource = datasource;
    }

    public DataSourceException(string message) : base(message)
    {
    }

    public DataSourceException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public string DataSource
    {
      get;
      private set;
    }
  }
}