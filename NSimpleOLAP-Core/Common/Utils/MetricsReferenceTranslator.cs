using NSimpleOLAP.Schema;
using System;

namespace NSimpleOLAP.Common.Utils
{
  public class MetricsReferenceTranslator<T>
    where T : struct, IComparable
  {
    private DataSchema<T> _schema;

    public MetricsReferenceTranslator(DataSchema<T> schema)
    {
      _schema = schema;
    }

    public T Translate(string value)
    {
      var metric = GetMetric(value);

      if (metric != null)
        return metric.ID;
      else
        throw new Exception($"metric \'{value}\' does not exist in the schema definition.");
    }

    public Type metricType(T value)
    {
      var metric = GetMetric(value);

      if (metric != null)
        return metric.DataType;
      else
        throw new Exception("metric does not exist in the schema definition.");
    }

    public string MetricName(T value)
    {
      var metric = GetMetric(value);

      if (metric != null)
        return metric.Name;
      else
        return "";
    }

    private Metric<T> GetMetric(string value)
    {
      return _schema.Metrics.Contains(value) ?
        _schema.Metrics[value] : null;
    }

    private Metric<T> GetMetric(T value)
    {
      return _schema.Metrics.ContainsKey(value) ?
        _schema.Metrics[value] : null;
    }
  }
}