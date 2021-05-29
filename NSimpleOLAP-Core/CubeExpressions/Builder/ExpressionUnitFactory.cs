using NSimpleOLAP.Common;
using NSimpleOLAP.CubeExpressions.Interfaces;
using NSimpleOLAP.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.CubeExpressions.Builder
{
  internal abstract class ExpressionUnitFactory
  {
    public static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Build<T>(ExpressionNodeBuilder<T> nodeBuilder)
      where T : struct, IComparable
    {
      if (nodeBuilder.IsListeral)
      {
        switch (nodeBuilder.Operation)
        {
          case OperationType.SUM:
            return Sum<T>(nodeBuilder.ScalarValue, nodeBuilder.RootValue);

          case OperationType.SUBTRACTION:
            return Subtraction<T>(nodeBuilder.ScalarValue, nodeBuilder.RootValue);

          case OperationType.MULTIPLICATION:
            return Multiplication<T>(nodeBuilder.ScalarValue, nodeBuilder.RootValue);

          case OperationType.DIVISION:
            return Division<T>(nodeBuilder.ScalarValue, nodeBuilder.RootValue);

          case OperationType.VALUE:
            return Value<T>(nodeBuilder.RootValue);

          case OperationType.ABS:
            return Abs<T>(nodeBuilder.RootValue);

          case OperationType.SQRT:
            return Sqrt<T>(nodeBuilder.RootValue);

          case OperationType.LN:
            return Ln<T>(nodeBuilder.RootValue);

          case OperationType.EXP:
            return Exp<T>(nodeBuilder.RootValue);

          default:
            throw new Exception("Operation is not supported.");
        }
      }

      switch (nodeBuilder.Operation)
      {
        case OperationType.SUM:
          return Sum(nodeBuilder.ScalarValue, nodeBuilder.Picker);

        case OperationType.SUBTRACTION:
          return Subtraction(nodeBuilder.ScalarValue, nodeBuilder.Picker);

        case OperationType.MULTIPLICATION:
          return Multiplication(nodeBuilder.ScalarValue, nodeBuilder.Picker);

        case OperationType.DIVISION:
          return Division(nodeBuilder.ScalarValue, nodeBuilder.Picker);

        case OperationType.VALUE:
          return Value(nodeBuilder.Picker);

        case OperationType.AVERAGE:
          return Average(nodeBuilder.Picker);

        case OperationType.MAX:
          return Max(nodeBuilder.Picker);

        case OperationType.MIN:
          return Min(nodeBuilder.Picker);

        case OperationType.ABS:
          return Abs(nodeBuilder.Picker);

        case OperationType.SQRT:
          return Sqrt(nodeBuilder.Picker);

        case OperationType.LN:
          return Ln(nodeBuilder.Picker);

        case OperationType.EXP:
          return Exp(nodeBuilder.Picker);

        default:
          throw new Exception("Operation is not supported.");
      }
    }

    public static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Build<T>(ExpressionNodeBuilder<T> nodeBuilder, Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor)
      where T : struct, IComparable
    {

      if (nodeBuilder.IsListeral)
      {
        // to do extra functions
        switch (nodeBuilder.Operation)
        {
          case OperationType.SUM:
            return Sum<T>(functor, nodeBuilder.RootValue);

          case OperationType.SUBTRACTION:
            return Subtraction<T>(functor, nodeBuilder.RootValue);

          case OperationType.MULTIPLICATION:
            return Multiplication<T>(functor, nodeBuilder.RootValue);

          case OperationType.DIVISION:
            return Division<T>(functor, nodeBuilder.RootValue);

          default:
            throw new Exception("Operation is not supported.");
        }
      }

      // to do extra functions
      switch (nodeBuilder.Operation)
      {
        case OperationType.SUM:
          return Sum(functor, nodeBuilder.Picker);

        case OperationType.SUBTRACTION:
          return Subtraction(functor, nodeBuilder.Picker);

        case OperationType.MULTIPLICATION:
          return Multiplication(functor, nodeBuilder.Picker);

        case OperationType.DIVISION:
          return Division(functor, nodeBuilder.Picker);

        case OperationType.ABS:
          return Abs(functor);

        case OperationType.SQRT:
          return Sqrt(functor);

        case OperationType.LN:
          return Ln(functor);

        case OperationType.EXP:
          return Exp(functor);

        default:
          throw new Exception("Operation is not supported.");
      }
    }

    // to do simplify

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Sum<T>(Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> infunctor, Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
       {
         if (x.CurrentCell.Values.ContainsKey(picker.Item1))
         {
           var measureValue = x.CurrentCell.Values[picker.Item1];

           var cxtResult = infunctor(x);

           if (cxtResult.Result != null && cxtResult.Result is ValueType) // change null hanndling by config
           {
             x.Result = measureValue.Sum((ValueType)cxtResult.Result);
           }
         }

         return x;
       };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Sum<T>(Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> infunctor, ValueType value) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        var cxtResult = infunctor(x);

        if (cxtResult.Result != null && cxtResult.Result is ValueType) // change null hanndling by config
        {
          x.Result = value.Sum((ValueType)cxtResult.Result);
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Sum<T>(ValueType value, Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (x.CurrentCell.Values.ContainsKey(picker.Item1))
        {
          var measureValue = x.CurrentCell.Values[picker.Item1];

          // todo tuple filters

          x.Result = measureValue.Sum(value);
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Sum<T>(ValueType value, ValueType value2) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        x.Result = value2.Sum(value);

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Value<T>(Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (x.CurrentCell.Values.ContainsKey(picker.Item1))
        {
          var measureValue = x.CurrentCell.Values[picker.Item1];

          // todo tuple filters

          x.Result = measureValue;
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Value<T>(ValueType value) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        x.Result = value;

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Subtraction<T>(Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> infunctor, Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (x.CurrentCell.Values.ContainsKey(picker.Item1))
        {
          var measureValue = x.CurrentCell.Values[picker.Item1];

          var cxtResult = infunctor(x);

          if (cxtResult.Result != null && cxtResult.Result is ValueType) // change null hanndling by config
          {
            x.Result = measureValue.Subtraction((ValueType)cxtResult.Result);
          }
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Subtraction<T>(Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> infunctor, ValueType value) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        var cxtResult = infunctor(x);

        if (cxtResult.Result != null && cxtResult.Result is ValueType) // change null hanndling by config
        {
          x.Result = value.Subtraction((ValueType)cxtResult.Result);
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Subtraction<T>(ValueType value, Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (x.CurrentCell.Values.ContainsKey(picker.Item1))
        {
          var measureValue = x.CurrentCell.Values[picker.Item1];

          // todo tuple filters

          x.Result = measureValue.Subtraction(value);
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Subtraction<T>(ValueType value, ValueType value2) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        x.Result = value2.Subtraction(value);

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Multiplication<T>(Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> infunctor, Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (x.CurrentCell.Values.ContainsKey(picker.Item1))
        {
          var measureValue = x.CurrentCell.Values[picker.Item1];

          var cxtResult = infunctor(x);

          if (cxtResult.Result != null && cxtResult.Result is ValueType) // change null hanndling by config
          {
            x.Result = measureValue.Multiplication((ValueType)cxtResult.Result);
          }
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Multiplication<T>(Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> infunctor, ValueType value) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        var cxtResult = infunctor(x);

        if (cxtResult.Result != null && cxtResult.Result is ValueType) // change null hanndling by config
        {
          x.Result = value.Multiplication((ValueType)cxtResult.Result);
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Multiplication<T>(ValueType value, Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (x.CurrentCell.Values.ContainsKey(picker.Item1))
        {
          var measureValue = x.CurrentCell.Values[picker.Item1];

          // todo tuple filters

          x.Result = measureValue.Multiplication(value);
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Multiplication<T>(ValueType value, ValueType value2) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        x.Result = value2.Multiplication(value);

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Division<T>(ValueType value, Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (x.CurrentCell.Values.ContainsKey(picker.Item1)
          && !value.IsZero())
        {
          var measureValue = x.CurrentCell.Values[picker.Item1];

          // todo tuple filters

          x.Result = measureValue.Division(value);
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Division<T>(ValueType value, ValueType value2) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        x.Result = value2.Division(value);

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Division<T>(Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> infunctor, Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (x.CurrentCell.Values.ContainsKey(picker.Item1))
        {
          var measureValue = x.CurrentCell.Values[picker.Item1];

          var cxtResult = infunctor(x);

          if (cxtResult.Result != null
            && cxtResult.Result is ValueType
            && !((ValueType)cxtResult.Result).IsZero()) // change null hanndling by config
          {
            x.Result = measureValue.Division((ValueType)cxtResult.Result);
          }
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Division<T>(Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> infunctor, ValueType value) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        var cxtResult = infunctor(x);

        if (cxtResult.Result != null
          && cxtResult.Result is ValueType
          && !((ValueType)cxtResult.Result).IsZero()) // change null hanndling by config
        {
          x.Result = value.Division((ValueType)cxtResult.Result);
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Average<T>(Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (x.CurrentCell.Values.ContainsKey(picker.Item1)
          && x.CurrentCell.Occurrences > 0)
        {
          var measureValue = x.CurrentCell.Values[picker.Item1];

          // todo tuple filters
          // requires recalculation for filtered metric

          x.Result = measureValue.Division(Convert.ToInt32(x.CurrentCell.Occurrences)); // change this
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Abs<T>(Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (x.CurrentCell.Values.ContainsKey(picker.Item1))
        {
          var measureValue = x.CurrentCell.Values[picker.Item1];

          x.Result = measureValue.Abs();
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Abs<T>(Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> infunctor) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        var cxtResult = infunctor(x);

        if (cxtResult.Result != null
          && cxtResult.Result is ValueType) // change null hanndling by config
        {
          x.Result = ((ValueType)cxtResult.Result).Abs();
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Abs<T>(ValueType value) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        x.Result = value.Abs();

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Sqrt<T>(Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (x.CurrentCell.Values.ContainsKey(picker.Item1))
        {
          var measureValue = x.CurrentCell.Values[picker.Item1];

          x.Result = measureValue.Sqrt();
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Sqrt<T>(Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> infunctor) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        var cxtResult = infunctor(x);

        if (cxtResult.Result != null
          && cxtResult.Result is ValueType) // change null hanndling by config
        {
          x.Result = ((ValueType)cxtResult.Result).Sqrt();
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Sqrt<T>(ValueType value) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        x.Result = value.Sqrt();

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Ln<T>(Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (x.CurrentCell.Values.ContainsKey(picker.Item1))
        {
          var measureValue = x.CurrentCell.Values[picker.Item1];

          x.Result = measureValue.Ln();
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Ln<T>(Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> infunctor) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        var cxtResult = infunctor(x);

        if (cxtResult.Result != null
          && cxtResult.Result is ValueType) // change null hanndling by config
        {
          x.Result = ((ValueType)cxtResult.Result).Ln();
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Ln<T>(ValueType value) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        x.Result = value.Ln();

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Exp<T>(Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (x.CurrentCell.Values.ContainsKey(picker.Item1))
        {
          var measureValue = x.CurrentCell.Values[picker.Item1];

          x.Result = measureValue.Exp();
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Exp<T>(Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> infunctor) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        var cxtResult = infunctor(x);

        if (cxtResult.Result != null
          && cxtResult.Result is ValueType) // change null hanndling by config
        {
          x.Result = ((ValueType)cxtResult.Result).Exp();
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Exp<T>(ValueType value) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        x.Result = value.Exp();

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Min<T>(Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (!x.NewValues.ContainsKey(picker.Item1)
          && x.PreviousValues.ContainsKey(x.CurrentMetric)) // to do change this needs extra context
        {
          x.Result = x.PreviousValues[x.CurrentMetric];
        }
        else if (x.NewValues.ContainsKey(picker.Item1)
          && !x.PreviousValues.ContainsKey(x.CurrentMetric))
        {
          x.Result = x.NewValues[picker.Item1];
        }
        else if (x.NewValues.ContainsKey(picker.Item1)
          && x.PreviousValues.ContainsKey(x.CurrentMetric))
        {
          var newValue = x.NewValues[picker.Item1];
          var oldValue = x.PreviousValues[x.CurrentMetric];
          // todo tuple filters
          // requires recalculation for filtered metric

          x.Result = newValue.Minimum(oldValue);
        }

        return x;
      };

      return functor;
    }

    private static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Max<T>(Tuple<T, List<KeyValuePair<T, T>[]>> picker) where T : struct, IComparable
    {
      Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor = x =>
      {
        if (!x.NewValues.ContainsKey(picker.Item1)
          && x.PreviousValues.ContainsKey(x.CurrentMetric))
        {
          x.Result = x.PreviousValues[x.CurrentMetric];
        }
        else if (x.NewValues.ContainsKey(picker.Item1)
          && !x.PreviousValues.ContainsKey(x.CurrentMetric))
        {
          x.Result = x.NewValues[picker.Item1];
        }
        else if (x.NewValues.ContainsKey(picker.Item1)
          && x.PreviousValues.ContainsKey(x.CurrentMetric))
        {
          var newValue = x.NewValues[picker.Item1];
          var oldValue = x.PreviousValues[x.CurrentMetric];
          // todo tuple filters
          // requires recalculation for filtered metric

          x.Result = newValue.Maximum(oldValue);
        }

        return x;
      };

      return functor;
    }
  }
}