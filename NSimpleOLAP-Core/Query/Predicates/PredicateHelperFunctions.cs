namespace NSimpleOLAP.Query.Predicates
{
  internal static class PredicateHelperFunctions
  {
    public static bool GreaterThan(this object value, object value2)
    {
      if (value != null && value2 != null)
      {
        switch (value)
        {
          case int t:
            return t.GreaterThan(value2);

          case double d:
            return d.GreaterThan(value2);

          case decimal dex:
            return dex.GreaterThan(value2);

          case float f:
            return f.GreaterThan(value2);
        }
      }

      return false;
    }

    private static bool GreaterThan(this int value, object value2)
    {
      return value > (int)value2;
    }

    private static bool GreaterThan(this double value, object value2)
    {
      return value > (double)value2;
    }

    private static bool GreaterThan(this decimal value, object value2)
    {
      return value > (decimal)value2;
    }

    private static bool GreaterThan(this float value, object value2)
    {
      return value > (float)value2;
    }

    public static bool GreaterOrEquals(this object value, object value2)
    {
      if (value != null && value2 != null)
      {
        switch (value)
        {
          case int t:
            return t.GreaterOrEquals(value2);

          case double d:
            return d.GreaterOrEquals(value2);

          case decimal dex:
            return dex.GreaterOrEquals(value2);

          case float f:
            return f.GreaterOrEquals(value2);
        }
      }

      return false;
    }

    private static bool GreaterOrEquals(this int value, object value2)
    {
      return value >= (int)value2;
    }

    private static bool GreaterOrEquals(this double value, object value2)
    {
      return value >= (double)value2;
    }

    private static bool GreaterOrEquals(this decimal value, object value2)
    {
      return value >= (decimal)value2;
    }

    private static bool GreaterOrEquals(this float value, object value2)
    {
      return value >= (float)value2;
    }

    public static bool LowerThan(this object value, object value2)
    {
      if (value != null && value2 != null)
      {
        switch (value)
        {
          case int t:
            return t.LowerThan(value2);

          case double d:
            return d.LowerThan(value2);

          case decimal dex:
            return dex.LowerThan(value2);

          case float f:
            return f.LowerThan(value2);
        }
      }

      return false;
    }

    private static bool LowerThan(this int value, object value2)
    {
      return value < (int)value2;
    }

    private static bool LowerThan(this double value, object value2)
    {
      return value < (double)value2;
    }

    private static bool LowerThan(this decimal value, object value2)
    {
      return value < (decimal)value2;
    }

    private static bool LowerThan(this float value, object value2)
    {
      return value < (float)value2;
    }

    public static bool LowerOrEquals(this object value, object value2)
    {
      if (value != null && value2 != null)
      {
        switch (value)
        {
          case int t:
            return t.LowerOrEquals(value2);

          case double d:
            return d.LowerOrEquals(value2);

          case decimal dex:
            return dex.LowerOrEquals(value2);

          case float f:
            return f.LowerOrEquals(value2);
        }
      }

      return false;
    }

    private static bool LowerOrEquals(this int value, object value2)
    {
      return value <= (int)value2;
    }

    private static bool LowerOrEquals(this double value, object value2)
    {
      return value <= (double)value2;
    }

    private static bool LowerOrEquals(this decimal value, object value2)
    {
      return value <= (decimal)value2;
    }

    private static bool LowerOrEquals(this float value, object value2)
    {
      return value <= (float)value2;
    }
  }
}