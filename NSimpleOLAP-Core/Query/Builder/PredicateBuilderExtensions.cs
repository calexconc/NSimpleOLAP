using NSimpleOLAP.Common;
using System;

namespace NSimpleOLAP.Query.Builder
{
  public static class PredicateBuilderExtensions
  {
    public static MeasureSlicerBuilder<T> IsEquals<T>(this MeasureSlicerBuilder<T> builder, object value)
      where T : struct, IComparable
    {
      return builder.SetOperationValuePair(LogicalOperators.EQUALS, value);
    }

    public static MeasureSlicerBuilder<T> NotEquals<T>(this MeasureSlicerBuilder<T> builder, object value)
      where T : struct, IComparable
    {
      return builder.SetOperationValuePair(LogicalOperators.NOTEQUALS, value);
    }

    public static MeasureSlicerBuilder<T> GreaterThan<T>(this MeasureSlicerBuilder<T> builder, object value)
      where T : struct, IComparable
    {
      return builder.SetOperationValuePair(LogicalOperators.GREATERTHAN, value);
    }

    public static MeasureSlicerBuilder<T> GreaterOrEquals<T>(this MeasureSlicerBuilder<T> builder, object value)
      where T : struct, IComparable
    {
      return builder.SetOperationValuePair(LogicalOperators.GREATEROREQUALS, value);
    }

    public static MeasureSlicerBuilder<T> LowerThan<T>(this MeasureSlicerBuilder<T> builder, object value)
      where T : struct, IComparable
    {
      return builder.SetOperationValuePair(LogicalOperators.LOWERTHAN, value);
    }

    public static MeasureSlicerBuilder<T> LowerOrEquals<T>(this MeasureSlicerBuilder<T> builder, object value)
      where T : struct, IComparable
    {
      return builder.SetOperationValuePair(LogicalOperators.LOWEROREQUALS, value);
    }

    public static DimensionSlicerBuilder<T> IsEquals<T>(this DimensionSlicerBuilder<T> builder, string member)
      where T : struct, IComparable
    {
      return builder.SetOperationSegments(LogicalOperators.EQUALS, member);
    }

    public static DimensionSlicerBuilder<T> NotEquals<T>(this DimensionSlicerBuilder<T> builder, string member)
      where T : struct, IComparable
    {
      return builder.SetOperationSegments(LogicalOperators.NOTEQUALS, member);
    }

    public static DimensionSlicerBuilder<T> In<T>(this DimensionSlicerBuilder<T> builder, params string[] members)
      where T : struct, IComparable
    {
      return builder.SetOperationSegments(LogicalOperators.IN, members);
    }

    internal static bool CompatibleType(this object value, Type type)
    {
      switch (value)
      {
        case int i when (type == typeof(int)
        || (type == typeof(double))
        || type == typeof(decimal)
        || type == typeof(float)):
          return true;

        case long l when (type == typeof(long)
        || type == typeof(int)
        || (type == typeof(double))
        || type == typeof(decimal)
        || type == typeof(float)):
          return true;

        case decimal l when (type == typeof(decimal)
        || type == typeof(double)
        || type == typeof(int)):
          return true;

        case double db when (type == typeof(double)
        || type == typeof(decimal)
        || type == typeof(float)):
          return true;

        default:
          return false;
      }
    }
  }
}