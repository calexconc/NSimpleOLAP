using NSimpleOLAP.Configuration;
using NSimpleOLAP.Configuration.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Data.Readers
{
  public class ObjectMapperReader : AbsReader
  {
    private Row _row;
    private IEnumerator<object> _enumerator;
    private Func<object, KeyValuePair<string, object>[]> _mapper;

    public ObjectMapperReader(DataSourceConfig config)
    {
      this.Config = config;
      this.Init();
    }

    public override bool Next()
    {
      bool ret = false;

      if (_enumerator.MoveNext())
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
      _enumerator.Dispose();
    }

    #region private methods

    private void Init()
    {
      _row = new Row(this.Config.Fields.GetFieldIndexes());
      _enumerator = this.Config.ObjectMapperConfig.Collection.GetEnumerator();
      _mapper = this.Config.ObjectMapperConfig.Mapper;
    }

    private object[] GetValues()
    {
      object[] values = new object[this.Config.Fields.Count];
      var results = _mapper(_enumerator.Current);

      if (results != null)
      {
        var dict = results.ToDictionary(x => x.Key, x => x.Value);

        for (int i = 0; i < this.Config.Fields.Count; i++)
        {
          if (dict.ContainsKey(this.Config.Fields[i].Name)
            && dict[this.Config.Fields[i].Name] != null)
            values[i] = Convert.ChangeType(dict[this.Config.Fields[i].Name], this.Config.Fields[i].FieldType);
          else
            values[i] = null;
        }
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