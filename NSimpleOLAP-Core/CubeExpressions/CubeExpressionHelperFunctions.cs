using System;

namespace NSimpleOLAP.CubeExpressions
{
  internal static class CubeExpressionHelperFunctions
  {
    internal static ValueType Sum(this ValueType x, ValueType y)
    {
      switch (x)
      {
        case int i when y is int:
          return ((int)x).Sum((int)y);

        case double i when y is int:
          return ((double)x).Sum((int)y);

        case double i when y is double:
          return ((double)x).Sum((double)y);

        case decimal i when y is int:
          return ((decimal)x).Sum((int)y);

        case decimal i when y is decimal:
          return ((decimal)x).Sum((decimal)y);

        case float i when y is int:
          return ((float)x).Sum((int)y);

        case float i when y is float:
          return ((float)x).Sum((float)y);

        default:
          throw new Exception("Type is not supported.");
      }
    }

    internal static ValueType Subtraction(this ValueType x, ValueType y)
    {
      switch (x)
      {
        case int i when y is int:
          return ((int)x).Subtraction((int)y);

        case double i when y is int:
          return ((double)x).Subtraction((int)y);

        case double i when y is double:
          return ((double)x).Subtraction((double)y);

        case decimal i when y is int:
          return ((decimal)x).Subtraction((int)y);

        case decimal i when y is decimal:
          return ((decimal)x).Subtraction((decimal)y);

        case float i when y is int:
          return ((float)x).Subtraction((int)y);

        case float i when y is float:
          return ((float)x).Subtraction((float)y);

        default:
          throw new Exception("Type is not supported.");
      }
    }

    internal static ValueType Multiplication(this ValueType x, ValueType y)
    {
      switch (x)
      {
        case int i when y is int:
          return ((int)x).Multiplication((int)y);
        case int i when y is double:
          return ((double)y).Multiplication((int)x);
        case int i when y is decimal:
          return ((decimal)y).Multiplication((int)x);

        case double i when y is int:
          return ((double)x).Multiplication((int)y);

        case double i when y is double:
          return ((double)x).Multiplication((double)y);

        case decimal i when y is int:
          return ((decimal)x).Multiplication((int)y);

        case decimal i when y is decimal:
          return ((decimal)x).Multiplication((decimal)y);

        case float i when y is int:
          return ((float)x).Multiplication((int)y);

        case float i when y is float:
          return ((float)x).Multiplication((float)y);

        default:
          throw new Exception("Type is not supported.");
      }
    }

    internal static ValueType Division(this ValueType x, ValueType y)
    {
      switch (x)
      {
        case int i when y is int:
          return ((int)x).Division((int)y);

        case double i when y is int:
          return ((double)x).Division((int)y);

        case double i when y is double:
          return ((double)x).Division((double)y);

        case decimal i when y is int:
          return ((decimal)x).Division((int)y);

        case decimal i when y is decimal:
          return ((decimal)x).Division((decimal)y);

        case float i when y is int:
          return ((float)x).Division((int)y);

        case float i when y is float:
          return ((float)x).Division((float)y);

        default:
          throw new Exception("Type is not supported.");
      }
    }

    internal static ValueType Minimum(this ValueType x, ValueType y)
    {
      switch (x)
      {
        case int i:
          return ((int)x).Min(y);

        case double i:
          return ((double)x).Min(y);

        case decimal i:
          return ((decimal)x).Min(y);

        case float i:
          return ((float)x).Min(y);

        default:
          throw new Exception("Type is not supported.");
      }
    }

    internal static ValueType Abs(this ValueType x)
    {
      
      switch (x)
      {
        case int i:
          return Math.Abs((int)x);

        case double i:
          return Math.Abs((double)x);

        case decimal i:
          return Math.Abs((decimal)x);

        case float i:
          return Math.Abs((float)x);

        default:
          throw new Exception("Type is not supported.");
      }
    }

    internal static ValueType Ln(this ValueType x)
    {

      switch (x)
      {
        case int i:
          return Math.Log((int)x);

        case double i:
          return Math.Log((double)x);

        case decimal i:
          return Convert.ToDecimal(Math.Log(Convert.ToDouble(x)));

        case float i:
          return Math.Log((float)x);

        default:
          throw new Exception("Type is not supported.");
      }
    }

    internal static ValueType Sqrt(this ValueType x)
    {

      switch (x)
      {
        case int i:
          return Math.Sqrt((int)x);

        case double i:
          return Math.Sqrt((double)x);

        case decimal i:
          return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(x)));

        case float i:
          return Math.Sqrt((float)x);

        default:
          throw new Exception("Type is not supported.");
      }
    }

    internal static ValueType Exp(this ValueType x)
    {

      switch (x)
      {
        case int i:
          return Math.Exp((int)x);

        case double i:
          return Math.Exp((double)x);

        case decimal i:
          return Convert.ToDecimal(Math.Exp(Convert.ToDouble(x)));

        case float i:
          return Math.Exp((float)x);

        default:
          throw new Exception("Type is not supported.");
      }
    }

    internal static ValueType Maximum(this ValueType x, ValueType y)
    {
      switch (x)
      {
        case int i:
          return ((int)x).Max(y);

        case double i:
          return ((double)x).Max(y);

        case decimal i:
          return ((decimal)x).Max(y);

        case float i:
          return ((float)x).Max(y);

        default:
          throw new Exception("Type is not supported.");
      }
    }

    internal static bool IsZero(this ValueType x)
    {
      switch (x)
      {
        case int i:
          return (int)x == 0;

        case double i:
          return (double)x == 0.00d;

        case decimal i:
          return (decimal)x == 0.00m;

        case float i:
          return (float)x == 0.00f;

        default:
          return false;
      }
    }

    private static int Max(this int value, object value2)
    {
      return (value > (int)value2) ? value : (int)value2;
    }

    private static double Max(this double value, object value2)
    {
      return (value > (double)value2) ? value : (double)value2;
    }

    private static decimal Max(this decimal value, object value2)
    {
      return (value > (decimal)value2) ? value : (decimal)value2;
    }

    private static float Max(this float value, object value2)
    {
      return (value > (float)value2) ? value : (float)value2;
    }

    private static int Min(this int value, object value2)
    {
      return (value < (int)value2) ? value : (int)value2;
    }

    private static double Min(this double value, object value2)
    {
      return (value < (double)value2) ? value : (double)value2;
    }

    private static decimal Min(this decimal value, object value2)
    {
      return (value < (decimal)value2) ? value : (decimal)value2;
    }

    private static float Min(this float value, object value2)
    {
      return (value < (float)value2) ? value : (float)value2;
    }

    private static float? Division(this float value, int value2)
    {
      return value2 > 0 ? (float?)(value / value2) : null;
    }

    private static double? Division(this double value, int value2)
    {
      return value2 > 0 ? (double?)(value / value2) : null;
    }

    private static decimal? Division(this decimal value, int value2)
    {
      return value2 > 0 ? (decimal?)(value / value2) : null;
    }

    private static double? Division(this int value, int value2)
    {
      return value2 > 0 ? (double?)(value / value2) : null;
    }

    private static double? Division(this double value, double value2)
    {
      return value2 > 0 ? (double?)(value / value2) : null;
    }

    private static decimal? Division(this decimal value, decimal value2)
    {
      return value2 > 0 ? (decimal?)(value / value2) : null;
    }

    private static float Multiplication(this float value, int value2)
    {
      return value * value2;
    }

    private static double Multiplication(this double value, int value2)
    {
      return value * value2;
    }

    private static decimal Multiplication(this decimal value, int value2)
    {
      return value * value2;
    }

    private static int Multiplication(this int value, int value2)
    {
      return value * value2;
    }

    private static double Multiplication(this double value, double value2)
    {
      return value * value2;
    }

    private static decimal Multiplication(this decimal value, decimal value2)
    {
      return value * value2;
    }

    private static float Subtraction(this float value, int value2)
    {
      return value - value2;
    }

    private static double Subtraction(this double value, int value2)
    {
      return value - value2;
    }

    private static decimal Subtraction(this decimal value, int value2)
    {
      return value - value2;
    }

    private static int Subtraction(this int value, int value2)
    {
      return value - value2;
    }

    private static double Subtraction(this double value, double value2)
    {
      return value - value2;
    }

    private static decimal Subtraction(this decimal value, decimal value2)
    {
      return value - value2;
    }

    private static float Sum(this float value, int value2)
    {
      return value + value2;
    }

    private static double Sum(this double value, int value2)
    {
      return value + value2;
    }

    private static decimal Sum(this decimal value, int value2)
    {
      return value + value2;
    }

    private static int Sum(this int value, int value2)
    {
      return value + value2;
    }

    private static double Sum(this double value, double value2)
    {
      return value + value2;
    }

    private static decimal Sum(this decimal value, decimal value2)
    {
      return value + value2;
    }
  }
}