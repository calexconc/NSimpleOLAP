using NSimpleOLAP.Common.Utils;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.CubeExpressions.Builder
{
  public class ExpressionElementPickerBuilder<T>
    where T : struct, IComparable
  {
    private T _measure;
    private Type _type;
    private List<KeyValuePair<T, T>[]> _tuples;
    private NamespaceResolver<T> _resolver;

    public ExpressionElementPickerBuilder(NamespaceResolver<T> resolver)
    {
      _resolver = resolver;
  }

    internal T Measure
    {
      get
      {
        return _measure;
      }
    }

    internal List<KeyValuePair<T, T>[]> Tuples
    {
      get
      {
        return _tuples;
      }
    }

    internal Type ReturnType
    {
      get
      {
        return _type;
      }
    }

    public ExpressionElementPickerBuilder<T> Set(string measure)
    {
      _measure = _resolver.MeasureTranslate(measure);
      _type = _resolver.MeasureType(_measure);

      return this;
    }

    internal ExpressionElementPickerBuilder<T> Set(T measure)
    {
      _measure = measure;
      _type = _resolver.MeasureType(_measure);

      return this;
    }

    public ExpressionElementPickerBuilder<T> Set(string measure, params string[] tuples)
    {
      Set(measure);

      _tuples = new List<KeyValuePair<T, T>[]>();

      foreach (var item in tuples)
      {
        _tuples.Add(_resolver.DimensionTranslate(item));
      }

      return this;
    }

    internal ExpressionElementPickerBuilder<T> Set(T measure, IEnumerable<KeyValuePair<T, T>[]> tuples)
    {
      Set(measure);

      _tuples = new List<KeyValuePair<T, T>[]>();

      _tuples.AddRange(tuples);

      return this;
    }

    internal Tuple<T, List<KeyValuePair<T, T>[]>> Create()
    {
      return new Tuple<T, List<KeyValuePair<T, T>[]>>(Measure, Tuples);
    }
  }
}