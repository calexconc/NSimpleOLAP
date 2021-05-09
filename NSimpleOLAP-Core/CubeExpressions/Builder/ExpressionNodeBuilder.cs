using NSimpleOLAP.Common;
using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.CubeExpressions.Interfaces;
using NSimpleOLAP.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.CubeExpressions.Builder
{
  public class ExpressionNodeBuilder<T>
   where T : struct, IComparable
  {
    private NamespaceResolver<T> _resolver;
    private ExpressionElementPickerBuilder<T> _picker;
    private OperationType _operation;
    private ValueType _value;
    private ExpressionElementsBuilder<T> _leftNodeBuilder;
    private Type _type;

    public ExpressionNodeBuilder(ExpressionElementPickerBuilder<T> picker, NamespaceResolver<T> resolver)
    {
      _resolver = resolver;
      _picker = picker;
      _type = picker.ReturnType;
    }

    internal Tuple<T, List<KeyValuePair<T, T>[]>> Picker
    {
      get
      {
        return _picker.Create();
      }
    }

    internal OperationType Operation
    {
      get
      {
        return _operation;
      }
    }

    internal ValueType ScalarValue
    {
      get
      {
        return _value;
      }
    }

    internal Type ReturnType
    {
      get
      {
        return _type;
      }
    }

    public void Sum<V>(V value)
      where V : struct
    {
      _operation = OperationType.SUM;
      _value = value;
    }

    public void Sum(Action<ExpressionElementsBuilder<T>> builder)
    {
      _operation = OperationType.SUM;
      _leftNodeBuilder = new ExpressionElementsBuilder<T>(_resolver);

      builder(_leftNodeBuilder);
    }

    public void Subtract<V>(V value)
      where V : struct
    {
      _operation = OperationType.SUBTRACTION;
      _value = value;
    }

    public void Subtract(Action<ExpressionElementsBuilder<T>> builder)
    {
      _operation = OperationType.SUBTRACTION;
      _leftNodeBuilder = new ExpressionElementsBuilder<T>(_resolver);

      builder(_leftNodeBuilder);
    }

    public void Multiply<V>(V value)
      where V : struct
    {
      _operation = OperationType.MULTIPLICATION;
      _value = value;
    }

    public void Multiply(Action<ExpressionElementsBuilder<T>> builder)
    {
      _operation = OperationType.MULTIPLICATION;
      _leftNodeBuilder = new ExpressionElementsBuilder<T>(_resolver);

      builder(_leftNodeBuilder);
    }

    public void Divide<V>(V value)
      where V : struct
    {
      V defvalue = default(V);

      if (defvalue.Equals(value))
        throw new Exception("Division by 0 is not allowed!");

      _operation = OperationType.DIVISION;
      _value = value;
    }

    public void Divide(Action<ExpressionElementsBuilder<T>> builder)
    {
      _operation = OperationType.DIVISION;
      _leftNodeBuilder = new ExpressionElementsBuilder<T>(_resolver);

      builder(_leftNodeBuilder);
    }

    public void Average()
    {
      _operation = OperationType.AVERAGE;
    }

    public void Value()
    {
      _operation = OperationType.VALUE;
    }

    public void Max()
    {
      _operation = OperationType.MAX;
    }

    public void Min()
    {
      _operation = OperationType.MIN;
    }

    internal Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Create()
    {
      if (_leftNodeBuilder == null)
        return ExpressionUnitFactory.Build(this);

      return ExpressionUnitFactory.Build(this, _leftNodeBuilder.Create());
    }
  }
}