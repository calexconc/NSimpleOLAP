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

        default:
          throw new Exception("Operation is not supported.");
      }
    }

    public static Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Build<T>(ExpressionNodeBuilder<T> nodeBuilder, Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> functor)
      where T : struct, IComparable
    {
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