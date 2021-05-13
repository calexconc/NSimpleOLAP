using NSimpleOLAP.Configuration;
using NSimpleOLAP.Configuration.Extensions;
using NSimpleOLAP.Common.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace NSimpleOLAP.Data.Readers
{
  /// <summary>
  /// Description of CVSReader.
  /// </summary>
  public class CSVReader : AbsReader
  {
    private Row _row;
    private bool _firstline = true;
    private TextReader _reader;

    public CSVReader(DataSourceConfig config)
    {
      this.Config = config;
      this.Init();
    }

    public override bool Next()
    {
      bool ret = false;

      string line = _reader.ReadLine();

      if (this._firstline && this.Config.CSVConfig.HasHeader)
        line = _reader.ReadLine();

      if (line != null)
      {
        string[] strs = line.Split(this.Config.CSVConfig.FieldDelimiter);

        _row.SetData(this.GetValues(strs));
        this.Current = _row;
        ret = true;
      }
      else
        this.Current = null;

      this._firstline = false;

      return ret;
    }

    public override void Dispose()
    {
      this._reader.Close();
      this._reader.Dispose();
    }

    #region private members

    private void Init()
    {
      _row = new CSVReader.Row(this.Config.Fields.GetFieldIndexes());
      FileStream file_stream = File.OpenRead(this.Config.CSVConfig.FilePath);

      if (this.Config.CSVConfig.Encoding == string.Empty)
        this._reader = new StreamReader(file_stream, true);
      else
        this._reader = new StreamReader(file_stream, Encoding.GetEncoding(this.Config.CSVConfig.Encoding));
    }

    private object[] GetValues(string[] strs)
    {
      object[] values = new object[this.Config.Fields.Count];

      for (int i = 0; i < this.Config.Fields.Count; i++)
      {
        string value = strs[Config.Fields[i].Index].Trim();

        if (string.IsNullOrEmpty(value))
        {
          values[i] = null;
          continue;
        }

        if (!value.Contains(".") && string.IsNullOrEmpty(Config.Fields[i].Format))
          values[i] = Convert.ChangeType(value, this.Config.Fields[i].FieldType);
        else if (value.Contains(".") && string.IsNullOrEmpty(Config.Fields[i].Format))
        {
          double val = 0;

          if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
            values[i] = Convert.ChangeType(val, this.Config.Fields[i].FieldType);
          else
            values[i] = null;
        }
        else if (!string.IsNullOrEmpty(Config.Fields[i].Format) 
          && Config.Fields[i].FieldType == typeof(DateTime))
          values[i] = value.GetDate(Config.Fields[i].Format);
        else if (!string.IsNullOrEmpty(Config.Fields[i].Format)
          && Config.Fields[i].FieldType == typeof(TimeSpan))
          values[i] = value.GetTimeSpan(Config.Fields[i].Format);
      }

      return values;
    }

    #endregion private members

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