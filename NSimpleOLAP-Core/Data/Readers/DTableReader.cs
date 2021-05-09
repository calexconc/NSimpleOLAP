using NSimpleOLAP.Configuration;
using NSimpleOLAP.Configuration.Extensions;
using System;
using System.Collections.Generic;
using System.Data;

namespace NSimpleOLAP.Data.Readers
{
  /// <summary>
  /// Description of DataTableReader.
  /// </summary>
  public class DTableReader : AbsReader
  {
    private Row _row;
    private DataTable _table;
    private DataTableReader _reader;

    public DTableReader(DataSourceConfig config)
    {
      this.Config = config;
      this.Init();
    }

    public override bool Next()
    {
      bool ret = false;

      if (_reader.Read())
      {
        _row.SetData(this.GetValues());
        this.Current = _row;
        ret = true;
      }
      else
      {
        _row = null;
        this.Current = null;
      }

      return ret;
    }

    public override void Dispose()
    {
      _reader.Dispose();
    }

    #region private methods

    private void Init()
    {
      _row = new DTableReader.Row(this.Config.Fields.GetFieldIndexes());
      _table = this.Config.DTableConfig.Table;
      _reader = new DataTableReader(_table);
    }

    private object[] GetValues()
    {
      object[] values = new object[this.Config.Fields.Count];

      for (int i = 0; i < this.Config.Fields.Count; i++)
      {
        if (_reader[this.Config.Fields[i].Name] != DBNull.Value)
          values[i] = Convert.ChangeType(_reader[this.Config.Fields[i].Name], this.Config.Fields[i].FieldType);
        else
          values[i] = null;
      }

      return values;
    }

    #endregion private methods

    #region private class

    private class Row : AbsRowData
    {
      public Row(Dictionary<string, int> fields)
      {
        this._indexes = fields;
      }

      public void SetData(object[] values)
      {
        _values = values;
      }
    }

    #endregion private class
  }
}