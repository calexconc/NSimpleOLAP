using NSimpleOLAP.Schema;
using System;

namespace NSimpleOLAP.Common.Utils
{
  /// <summary>
  /// Description of MeasureReferenceTranslator.
  /// </summary>
  public class MeasureReferenceTranslator<T>
    where T : struct, IComparable
  {
    private DataSchema<T> _schema;

    public MeasureReferenceTranslator(DataSchema<T> schema)
    {
      _schema = schema;
    }

    public T Translate(string value)
    {
      var measure = GetMeasure(value);

      if (measure != null)
        return measure.ID;
      else
        throw new Exception($"Measure \'{value}\' does not exist in the schema definition.");
    }

    public Type MeasureType(T value)
    {
      var measure = GetMeasure(value);

      if (measure != null)
        return measure.DataType;
      else
        throw new Exception("Measure does not exist in the schema definition.");
    }

    public string MeasureName(T value)
    {
      var measure = GetMeasure(value);

      if (measure != null)
        return measure.Name;
      else
        return "";
    }

    private Measure<T> GetMeasure(string value)
    {
      return _schema.Measures.Contains(value) ?
        _schema.Measures[value] : null;
    }

    private Measure<T> GetMeasure(T value)
    {
      return _schema.Measures.ContainsKey(value) ?
        _schema.Measures[value] : null;
    }
  }
}