using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.Configuration.Extensions;
using NSimpleOLAP.Data.Readers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NSimpleOLAP.Data
{
  public static class DataExtensions
  {
    public static void AppendData<T>(this Cube<T> cube, params string[] values)
      where T : struct, IComparable
    {
      var dataSource = cube.Config.DataSources[cube.Source];

      if (dataSource.Fields.Count > values.Length)
        throw new Exception("Number of values does not match the number of mapped fields in the original datasource.");

      if (dataSource.SourceType == Common.DataSourceType.CSV)
      {
        var row = new Row(dataSource.Fields.GetFieldIndexes());

        row.SetData(GetValues(dataSource, values));

        cube.Storage.AddRowData(cube.RowHelper.GetDimensions(row),
            cube.RowHelper.GetMeasureData(row));
      }
      else
      {
        throw new Exception("Use this data appender when the original data source is a CSV file.");
      }
    }

    public static void AppendData<T>(this Cube<T> cube,  IEnumerable<string[]> rows)
      where T : struct, IComparable
    {
      var dataSource = cube.Config.DataSources[cube.Source];

      if (dataSource.SourceType != Common.DataSourceType.CSV)
        throw new Exception("Use this data appender when the original data source is a CSV file.");

      var row = new Row(dataSource.Fields.GetFieldIndexes());

      foreach (var values in rows)
      {
        if (dataSource.Fields.Count > values.Length)
          throw new Exception("Number of values does not match the number of mapped fields in the original datasource.");

        row.SetData(GetValues(dataSource, values));

        cube.Storage.AddRowData(cube.RowHelper.GetDimensions(row),
            cube.RowHelper.GetMeasureData(row));
      }
    }

    public static void AppendData<T>(this Cube<T> cube, params object[] values)
      where T : struct, IComparable
    {
      var dataSource = cube.Config.DataSources[cube.Source];

      if (dataSource.Fields.Count > values.Length)
        throw new Exception("Number of values does not match the number of mapped fields in the original datasource.");

      var row = new Row(dataSource.Fields.GetFieldIndexes());

      row.SetData(GetValues(dataSource, values));

      cube.Storage.AddRowData(cube.RowHelper.GetDimensions(row),
          cube.RowHelper.GetMeasureData(row));
    }

    public static void AppendData<T>(this Cube<T> cube, IEnumerable<object[]> rows)
      where T : struct, IComparable
    {
      var dataSource = cube.Config.DataSources[cube.Source];
      var row = new Row(dataSource.Fields.GetFieldIndexes());

      foreach (var values in rows)
      {
        if (dataSource.Fields.Count > values.Length)
          throw new Exception("Number of values does not match the number of mapped fields in the original datasource.");

        row.SetData(GetValues(dataSource, values));

        cube.Storage.AddRowData(cube.RowHelper.GetDimensions(row),
            cube.RowHelper.GetMeasureData(row));
      }
    }

    public static void AppendData<T>(this Cube<T> cube, params KeyValuePair<string, object>[] values)
      where T : struct, IComparable
    {
      var dataSource = cube.Config.DataSources[cube.Source];

      if (dataSource.Fields.Count > values.Length)
        throw new Exception("Number of values does not match the number of mapped fields in the original datasource.");

      var row = new Row(dataSource.Fields.GetFieldIndexes());

      row.SetData(GetValues(dataSource, values));

      cube.Storage.AddRowData(cube.RowHelper.GetDimensions(row),
          cube.RowHelper.GetMeasureData(row));
    }

    public static void AppendData<T>(this Cube<T> cube, IEnumerable<KeyValuePair<string, object>[]> rows)
      where T : struct, IComparable
    {
      var dataSource = cube.Config.DataSources[cube.Source];
      var row = new Row(dataSource.Fields.GetFieldIndexes());

      foreach (var values in rows)
      {
        if (dataSource.Fields.Count > values.Length)
          throw new Exception("Number of values does not match the number of mapped fields in the original datasource.");

        row.SetData(GetValues(dataSource, values));

        cube.Storage.AddRowData(cube.RowHelper.GetDimensions(row),
            cube.RowHelper.GetMeasureData(row));
      }
    }

    private static object[] GetValues(DataSourceConfig config, string[] strs)
    {
      object[] values = new object[config.Fields.Count];

      for (int i = 0; i < config.Fields.Count; i++)
      {
        string value = strs[config.Fields[i].Index].Trim();

        if (string.IsNullOrEmpty(value))
        {
          values[i] = null;
          continue;
        }

        if (!value.Contains(".") && string.IsNullOrEmpty(config.Fields[i].Format))
          values[i] = Convert.ChangeType(value, config.Fields[i].FieldType);
        else if (value.Contains(".") && string.IsNullOrEmpty(config.Fields[i].Format))
        {
          double val = 0;

          if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
            values[i] = Convert.ChangeType(val, config.Fields[i].FieldType);
          else
            values[i] = null;
        }
        else if (!string.IsNullOrEmpty(config.Fields[i].Format)
          && config.Fields[i].FieldType == typeof(DateTime))
          values[i] = value.GetDate(config.Fields[i].Format);
        else if (!string.IsNullOrEmpty(config.Fields[i].Format)
          && config.Fields[i].FieldType == typeof(TimeSpan))
          values[i] = value.GetTimeSpan(config.Fields[i].Format);
      }

      return values;
    }

    private static object[] GetValues(DataSourceConfig config, object[] values)
    {
      object[] rvalues = new object[config.Fields.Count];

      for (int i = 0; i < config.Fields.Count; i++)
      {
        var value = values[config.Fields[i].Index];

        if (value == null)
        {
          rvalues[i] = null;
          continue;
        }

        rvalues[i] = Convert.ChangeType(value, config.Fields[i].FieldType);
      }

      return rvalues;
    }

    private static object[] GetValues(DataSourceConfig config, KeyValuePair<string, object>[] values)
    {
      object[] rvalues = new object[config.Fields.Count];
      var dict = values.ToDictionary(x => x.Key, x => x.Value);

      for (int i = 0; i < config.Fields.Count; i++)
      {
        if (!dict.ContainsKey(config.Fields[i].Name))
        {
          rvalues[i] = null;
          continue;
        }

        var value = dict[config.Fields[i].Name];

        if (value == null)
        {
          rvalues[i] = null;
          continue;
        }

        rvalues[i] = Convert.ChangeType(value, config.Fields[i].FieldType);
      }

      return rvalues;
    }

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