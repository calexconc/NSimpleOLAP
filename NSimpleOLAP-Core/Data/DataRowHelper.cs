using NSimpleOLAP.Configuration;
using NSimpleOLAP.Data.Readers;
using NSimpleOLAP.Schema;
using System;
using System.Linq;
using System.Collections.Generic;
using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.Common;

namespace NSimpleOLAP.Data
{
  /// <summary>
  /// Description of DataRowHelper.
  /// </summary>
  internal class DataRowHelper<T>
    where T : struct, IComparable
  {
    private DataSchema<T> _schema;
    private CubeSourceConfig _config;

    public DataRowHelper(DataSchema<T> schema, CubeSourceConfig config)
    {
      _schema = schema;
      _config = config;
    }

    public KeyValuePair<T, T>[] GetDimensions(AbsRowData rowdata)
    {
      List<KeyValuePair<T, T>> retlist = new List<KeyValuePair<T, T>>();

      foreach (SourceMappingsElement item in _config.Fields)
      {
        if (rowdata[item.Field] == null)
          continue;

        if (!string.IsNullOrEmpty(item.Dimension))
          HandleSimpleDimension(rowdata, item, retlist);
        else if (item.Labels?.Length > 0)
          HandleLevels(rowdata, item, retlist);
      }

      retlist.Sort(ComparePairs);

      return retlist.ToArray();
    }

    public MeasureValuesCollection<T> GetMeasureData(AbsRowData rowdata)
    {
      MeasureValuesCollection<T> vars = new MeasureValuesCollection<T>();

      foreach (var item in _schema.Measures)
      {
        if (rowdata[item.Config.ValueFieldName] != null)
          vars.Add(item.ID, rowdata[item.Config.ValueFieldName]);
      }

      return vars;
    }

    private void HandleSimpleDimension(AbsRowData rowdata, SourceMappingsElement item, List<KeyValuePair<T, T>> retlist)
    {
      Dimension<T> dimension = _schema.Dimensions[item.Dimension];

      if (!dimension.Config.SourceIsGenerated)
      {
        T segment = (T)Convert.ChangeType(rowdata[item.Field], typeof(T));

        if (dimension.Members.ContainsKey(segment))
        {
          KeyValuePair<T, T> pair = new KeyValuePair<T, T>(dimension.ID, segment);
          retlist.Add(pair);
        }
      }
      else
      {
        var value = rowdata[item.Field];

        if (value != null)
        {
          foreach (var member in dimension.Members.Cast<MemberGenerated<T>>())
          {
            var result = member.Transformer.Transform(value);

            if (result != null) // todo change this to handle different transformations
            {
              KeyValuePair<T, T> pair = new KeyValuePair<T, T>(dimension.ID, member.ID);
              retlist.Add(pair);
              break;
            }
          }
        }
      }
    }

    private void HandleLevels(AbsRowData rowdata, SourceMappingsElement item, List<KeyValuePair<T, T>> retlist)
    {

      var firstDimension = _schema.Dimensions[item.Labels[0]];

      switch (firstDimension.TypeOf)
      {
        case DimensionType.Date:
          HandleDateLevels(rowdata, item, retlist);
          break;
        case DimensionType.Time:
          HandleTimeLevels(rowdata, item, retlist);
          break;
        case DimensionType.Levels:
          HandleNumericLevels(firstDimension, rowdata, item, retlist);
          break;
      }
    }

    private void HandleDateLevels(AbsRowData rowdata, SourceMappingsElement item, List<KeyValuePair<T, T>> retlist)
    {
      for (var i = 0; i < item.Labels.Length; i++)
      {
        var dtDimension = (DimensionDate<T>)_schema.Dimensions[item.Labels[i]];
        var value = ((DateTime?)rowdata[item.Field]).Value;
        T segment = DateTimeMemberGenerator.TransformToDateId<T>(value, dtDimension.DateLevel);

        KeyValuePair<T, T> pair = new KeyValuePair<T, T>(dtDimension.ID, segment);
        retlist.Add(pair);

        if (!dtDimension.Members.ContainsKey(segment))
        {
          dtDimension.Members.Add(new Member<T>
          {
            ID = segment,
            Name = DateTimeMemberGenerator.GetLevelName(value, dtDimension.DateLevel)
          });
        }
      }
    }

    private void HandleTimeLevels(AbsRowData rowdata, SourceMappingsElement item, List<KeyValuePair<T, T>> retlist)
    {
      for (var i = 0; i < item.Labels.Length; i++)
      {
        var dtDimension = (DimensionTime<T>)_schema.Dimensions[item.Labels[i]];
        var value = ((TimeSpan?)rowdata[item.Field]).Value;
        T segment = DateTimeMemberGenerator.TransformToTimeId<T>(value, dtDimension.TimeLevel);

        KeyValuePair<T, T> pair = new KeyValuePair<T, T>(dtDimension.ID, segment);
        retlist.Add(pair);

        if (!dtDimension.Members.ContainsKey(segment))
        {
          dtDimension.Members.Add(new Member<T>
          {
            ID = segment,
            Name = DateTimeMemberGenerator.GetLevelName(value, dtDimension.TimeLevel)
          });
        }
      }
    }

    private void HandleNumericLevels(Dimension<T> firstDimension, AbsRowData rowdata, SourceMappingsElement item, List<KeyValuePair<T, T>> retlist)
    {
      T segment = (T)Convert.ChangeType(rowdata[item.Field], typeof(T));
      var lvDimension = (DimensionLevel<T>)firstDimension;

      if (firstDimension.Members.ContainsKey(segment))
      {
        var member = (MemberLevel<T>)lvDimension.Members[segment];
        KeyValuePair<T, T> pair = new KeyValuePair<T, T>(lvDimension.ID, segment);
        retlist.Add(pair);

        for (var i = 1; i < item.Labels.Length; i++)
        {
          var xdimension = (DimensionLevel<T>)_schema.Dimensions[item.Labels[i]];
          var xpair = new KeyValuePair<T, T>(xdimension.ID, member.Levels[item.Labels[i]]);

          retlist.Add(xpair);
        }
      }
    }

    private int ComparePairs(KeyValuePair<T, T> a, KeyValuePair<T, T> b)
    {
      return a.Key.CompareTo(b.Key);
    }
  }
}