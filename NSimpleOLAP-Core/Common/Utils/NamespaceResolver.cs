using NSimpleOLAP.Schema;
using NSimpleOLAP.Schema.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Common.Utils
{
  public class NamespaceResolver<T>
    where T : struct, IComparable
  {
    private DataSchema<T> _schema;
    private MeasureReferenceTranslator<T> _measureTranslator;
    private DimensionReferenceTranslator<T> _dimensionTranslator;
    private MetricsReferenceTranslator<T> _metricsTranslator;
    private INamespace<T> _namespace;

    public NamespaceResolver(Cube<T> cube)
    {
      _schema = cube.Schema;
      _dimensionTranslator = new DimensionReferenceTranslator<T>(_schema);
      _measureTranslator = new MeasureReferenceTranslator<T>(_schema);
      _metricsTranslator = new MetricsReferenceTranslator<T>(_schema);
      _namespace = cube.NameSpace;
    }

    public NamespaceResolver(DataSchema<T> schema)
    {
      _schema = schema;
      _dimensionTranslator = new DimensionReferenceTranslator<T>(_schema);
      _measureTranslator = new MeasureReferenceTranslator<T>(_schema);
      _metricsTranslator = new MetricsReferenceTranslator<T>(_schema);
    }

    public IDataItem<T> GetDataItemInfo(string name)
    {
      if (_namespace.HasEntity(name))
      {
        return _namespace[name];
      }

      return null;
    }

    public IDataItem<T> GetDataItemInfo(T value)
    {
      if (_namespace.HasEntity(value))
      {
        return _namespace[value];
      }

      return null;
    }

    public KeyValuePair<T, T>[] DimensionTranslate(string value)
    {
      return _dimensionTranslator.Translate(value);
    }

    public T GetDimension(string value)
    {
      return _dimensionTranslator.GetDimension(value);
    }

    public T GetDimensionMember(T dimKey, string value)
    {
      return _dimensionTranslator.GetDimensionMember(dimKey, value);
    }

    public T MeasureTranslate(string value)
    {
      return _measureTranslator.Translate(value);
    }

    public Type MeasureType(T value)
    {
      return _measureTranslator.MeasureType(value);
    }

    public string MeasureName(T value)
    {
      return _measureTranslator.MeasureName(value);
    }

    public T MetricTranslate(string value)
    {
      return _metricsTranslator.Translate(value);
    }

    public Type MetricType(T value)
    {
      return _metricsTranslator.metricType(value);
    }

    public string MetricName(T value)
    {
      return _metricsTranslator.MetricName(value);
    }
  }
}