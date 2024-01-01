using NSimpleOLAP.Configuration;
using NSimpleOLAP.Data.Readers;

namespace NSimpleOLAP.Data.Interfaces
{
  /// <summary>
  /// Description of IDataSource.
  /// </summary>
  public interface IDataSource
  {
    string Name { get; set; }

    DataSourceConfig Config { get; set; }

    AbsReader GetReader();
  }
}