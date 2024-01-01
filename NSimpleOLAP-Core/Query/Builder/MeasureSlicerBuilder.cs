using NSimpleOLAP.Common;
using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.Query.Interfaces;
using NSimpleOLAP.Query.Predicates;
using System;

namespace NSimpleOLAP.Query.Builder
{
  /// <summary>
  /// Description of MeasureSlicerBuilder.
  /// </summary>
  public class MeasureSlicerBuilder<T> : IPredicateBuilder<T>
    where T : struct, IComparable
  {
    private T _measure;
    private object _value;
    private LogicalOperators _operator;
    private Type _valueType;
    private NamespaceResolver<T> _resolver;
    private string _measureName;

    public MeasureSlicerBuilder(NamespaceResolver<T> resolver)
    {
      _resolver = resolver;
    }

    #region fluent interface

    internal MeasureSlicerBuilder<T> SetMeasure(string measure)
    {
      _measureName = measure;
      _measure = _resolver.MeasureTranslate(measure);
      _valueType = _resolver.MeasureType(_measure);

      return this;
    }

    internal MeasureSlicerBuilder<T> SetMeasure(T measureKey)
    {
      _measure = measureKey;
      _valueType = _resolver.MeasureType(_measure);
      _measureName = _resolver.MeasureName(measureKey);

      return this;
    }

    internal MeasureSlicerBuilder<T> SetOperationValuePair(LogicalOperators loperator, object value)
    {
      if (!value.CompatibleType(_valueType))
        throw new Exception($"Attempting to make operation with incompatible value type on Measure {_measureName}.");

      _operator = loperator;
      _value = value;

      return this;
    }

    #endregion fluent interface

    public IPredicate<T> Build()
    {
      return new SliceByMeasure<T>(_measure, _valueType, _operator, _value);
    }
  }
}