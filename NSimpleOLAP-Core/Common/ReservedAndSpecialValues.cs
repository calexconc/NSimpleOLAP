using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Common
{
  internal static class ReservedAndSpecialValues
  {
    public const string ALL = "ALL";

    public const string NEXT_MEMBER = "NEXTMEMBER";

    public const string PREVIOUS_MEMBER = "PREVIOUSMEMBER";

    private static KeyValuePair<int, int> All_Int = new KeyValuePair<int, int>(0, 0);

    private static KeyValuePair<uint, uint> All_UInt = new KeyValuePair<uint, uint>(0, 0);

    private static KeyValuePair<long, long> All_Long = new KeyValuePair<long, long>(0, 0);

    private static KeyValuePair<UInt64, UInt64> All_ULong = new KeyValuePair<UInt64, UInt64>(0, 0);

    private static KeyValuePair<int, int> Next_Int = new KeyValuePair<int, int>(0, 1);

    private static KeyValuePair<uint, uint> Next_UInt = new KeyValuePair<uint, uint>(0, 1);

    private static KeyValuePair<long, long> Next_Long = new KeyValuePair<long, long>(0, 1);

    private static KeyValuePair<UInt64, UInt64> Next_ULong = new KeyValuePair<UInt64, UInt64>(0, 1);

    private static KeyValuePair<int, int> Previous_Int = new KeyValuePair<int, int>(0, 2);

    private static KeyValuePair<uint, uint> Previous_UInt = new KeyValuePair<uint, uint>(0, 2);

    private static KeyValuePair<long, long> Previous_Long = new KeyValuePair<long, long>(0, 2);

    private static KeyValuePair<UInt64, UInt64> Previous_ULong = new KeyValuePair<UInt64, UInt64>(0, 2);

    public static KeyValuePair<T, T> GetAllValue<T>()
      where T : struct, IComparable
    {
      var value = default(T);

      switch (value)
      {
        case int i:
          return (KeyValuePair<T, T>)(object)All_Int;

        case uint i:
          return (KeyValuePair<T, T>)(object)All_UInt;

        case long i:
          return (KeyValuePair<T, T>)(object)All_Long;

        case UInt64 i:
          return (KeyValuePair<T, T>)(object)All_ULong;

        default:
          throw new Exception("Type is not supported.");
      }
    }

    public static KeyValuePair<T, T> GetNextMemberValue<T>()
      where T : struct, IComparable
    {
      var value = default(T);

      switch (value)
      {
        case int i:
          return (KeyValuePair<T, T>)(object)Next_Int;

        case uint i:
          return (KeyValuePair<T, T>)(object)Next_UInt;

        case long i:
          return (KeyValuePair<T, T>)(object)Next_Long;

        case UInt64 i:
          return (KeyValuePair<T, T>)(object)Next_ULong;

        default:
          throw new Exception("Type is not supported.");
      }
    }

    public static KeyValuePair<T, T> GetPreviousValue<T>()
      where T : struct, IComparable
    {
      var value = default(T);

      switch (value)
      {
        case int i:
          return (KeyValuePair<T, T>)(object)Previous_Int;

        case uint i:
          return (KeyValuePair<T, T>)(object)Previous_UInt;

        case long i:
          return (KeyValuePair<T, T>)(object)Previous_Long;

        case UInt64 i:
          return (KeyValuePair<T, T>)(object)Previous_ULong;

        default:
          throw new Exception("Type is not supported.");
      }
    }

    public static bool IsReservedValue<T>(this KeyValuePair<T, T> value)
      where T : struct, IComparable
    {
      var values = new[] { GetAllValue<T>(), GetNextMemberValue<T>(), GetPreviousValue<T>() };

      return value.Key.Equals(values[0].Key) && value.Value.Equals(values[0].Value)
        || value.Key.Equals(values[1].Key) && value.Value.Equals(values[1].Value)
        || value.Key.Equals(values[2].Key) && value.Value.Equals(values[2].Value);
    }

    public static bool IsWildcard<T>(this KeyValuePair<T, T> value)
      where T : struct, IComparable
    {
      var values = new[] { GetAllValue<T>() };

      return value.Key.Equals(values[0].Key) && value.Value.Equals(values[0].Value);
    }

    public static bool IsPositionalHint<T>(this KeyValuePair<T, T> value)
      where T : struct, IComparable
    {
      var values = new[] { GetNextMemberValue<T>(), GetPreviousValue<T>() };

      return value.Key.Equals(values[0].Key) && value.Value.Equals(values[0].Value)
        || value.Key.Equals(values[1].Key) && value.Value.Equals(values[1].Value);
    }

    public static bool IsAll<T>(this KeyValuePair<T, T> value)
      where T : struct, IComparable
    {
      return value.Equals(GetAllValue<T>());
    }

    public static bool IsNext<T>(this KeyValuePair<T, T> value)
      where T : struct, IComparable
    {
      return value.Equals(GetNextMemberValue<T>());
    }

    public static bool IsPrevious<T>(this KeyValuePair<T, T> value)
      where T : struct, IComparable
    {
      return value.Equals(GetPreviousValue<T>());
    }
  }
}