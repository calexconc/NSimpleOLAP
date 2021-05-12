using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration;
using System;

namespace NSimpleOLAP.Data.Readers
{
  /// <summary>
  /// Description of AbsReader.
  /// </summary>
  public abstract class AbsReader : IDisposable
  {
    protected DataSourceConfig Config
    {
      get;
      set;
    }

    #region public members

    public AbsRowData Current
    {
      get;
      protected set;
    }

    public abstract bool Next();

    public static AbsReader Create(DataSourceConfig config)
    {
      AbsReader reader = null;

      switch (config.SourceType)
      {
        case DataSourceType.CSV:
          reader = new CSVReader(config);
          break;

        case DataSourceType.DataBase:
          reader = new DBReader(config);
          break;

        case DataSourceType.DataSet:
          reader = new DTableReader(config);
          break;
        case DataSourceType.Transformer:
          reader = new TransformerReader(config);
          break;
        case DataSourceType.ObjectMapper:
          reader = new ObjectMapperReader(config);
          break;
      }

      return reader;
    }

    #endregion public members

    public abstract void Dispose();
  }
}