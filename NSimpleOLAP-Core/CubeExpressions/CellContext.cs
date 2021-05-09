using NSimpleOLAP.CubeExpressions.Interfaces;
using NSimpleOLAP.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.CubeExpressions
{
  public abstract class CellContext<T> : IExpressionContext<T, ICell<T>>
    where T : struct, IComparable
  {
    protected ICell<T> currentCell;
    protected ICell<T> rootCell;
    protected IDictionary<T, ValueType> previousValues;
    protected IDictionary<T, ValueType> newValues;

    public ICell<T> CurrentCell
    {
      get
      {
        return currentCell;
      }
    }

    public ICell<T> RootCell 
    { get
      {
        return rootCell;
      } 
    }

    public object Result { get; set; }

    public T CurrentMetric { get; set; }

    public IDictionary<T, ValueType> PreviousValues
    {
      get
      {
        return previousValues;
      }
    }

    public IDictionary<T, ValueType> NewValues
    {
      get
      {
        return newValues;
      }
    }

    internal void UpdateOldValue(T measureKey, ValueType value)
    {
      if (PreviousValues.ContainsKey(measureKey))
        PreviousValues[measureKey] = value;
      else
        PreviousValues.Add(measureKey, value);
    }

    internal void UpdateNewValue(T measureKey, ValueType value)
    {
      if (NewValues.ContainsKey(measureKey))
        NewValues[measureKey] = value;
      else
        NewValues.Add(measureKey, value);
    }
  }
}