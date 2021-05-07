using System;
using System.Collections.Generic;
using System.Globalization;

namespace NSimpleOLAP.Common.Utils
{
  internal static class DateTimeMemberGenerator
  {
    public static IEnumerable<T> GetLevelIds<T>(this DateTime date, DateLevels[] levels)
      where T : struct, IComparable
    {
      foreach (var level in levels)
        yield return TransformToDateId<T>(date, level);
    }

    public static IEnumerable<string> GetLevelNames(this DateTime date, DateLevels[] levels)
    {
      foreach (var level in levels)
        yield return GetLevelName(date, level);
    }

    public static T TransformToDateId<T>(DateTime date, DateLevels level)
      where T : struct, IComparable
    {
      switch (level)
      {
        case DateLevels.DAY:
          return TransformToDay<T>(date);

        case DateLevels.DATE:
          return TransformToDate<T>(date);

        case DateLevels.MONTH_WITH_YEAR:
          return TransformToMonth<T>(date);

        case DateLevels.YEAR:
          return TransformToYear<T>(date);

        case DateLevels.QUARTER:
          return TransformToQuarter<T>(date);

        case DateLevels.WEEK:
          return TransformToWeek<T>(date);

        case DateLevels.MONTH:
          return TransformToMonthOfYear<T>(date);

        default:
          throw new Exception("Type not supported.");
      }
    }

    public static T TransformToTimeId<T>(TimeSpan timespan, TimeLevels level)
      where T : struct, IComparable
    {
      switch (level)
      {
        case TimeLevels.HOUR:
          return TransformToHour<T>(timespan);

        case TimeLevels.MINUTES:
          return TransformToMinutes<T>(timespan);

        case TimeLevels.SECONDS:
          return TransformToSeconds<T>(timespan);

        case TimeLevels.TIME:
          return TransformToTime<T>(timespan);

        default:
          throw new Exception("Type not supported.");
      }
    }

    public static IEnumerable<Tuple<T, string>> GetAllHours<T>()
      where T : struct, IComparable
    {
      for (var i = 0; i <= 23; i++)
      {
        yield return new Tuple<T, string>((T)Convert.ChangeType(i+1, typeof(T)), i.ToString().PadLeft(2, '0'));
      }
    }

    public static IEnumerable<Tuple<T, string>> GetAllMinutes<T>()
      where T : struct, IComparable
    {
      for (var i = 0; i <= 59; i++)
      {
        yield return new Tuple<T, string>((T)Convert.ChangeType(i+1, typeof(T)), i.ToString().PadLeft(2, '0'));
      }
    }

    public static IEnumerable<Tuple<T, string>> GetAllSeconds<T>()
      where T : struct, IComparable
    {
      return GetAllMinutes<T>();
    }

    public static IEnumerable<Tuple<T,string>> GetAllMonthsInYear<T>()
      where T : struct, IComparable
    {
      for (var i = 1; i <= 12; i++)
      {
        var tempDate = new DateTime(2000, i, 1);

        yield return new Tuple<T, string>((T)Convert.ChangeType(i, typeof(T)), tempDate.ToString("MMMM"));
      }
    }

    public static IEnumerable<Tuple<T, string>> GetAllMonthsInYear<T>(DateTime value)
      where T : struct, IComparable
    {
      for (var i = 1; i <= 12; i++)
      {
        var tempDate = new DateTime(value.Year, i, 1);

        yield return new Tuple<T, string>(TransformToDateId<T>(tempDate, DateLevels.MONTH_WITH_YEAR), 
          GetLevelName(tempDate, DateLevels.MONTH_WITH_YEAR));
      }
    }

    public static IEnumerable<Tuple<T, string>> GetAllDays<T>()
      where T : struct, IComparable
    {
      for (var i = 1; i <= 31; i++)
      {
        yield return new Tuple<T, string>((T)Convert.ChangeType(i, typeof(T)), i.ToString());
      }
    }

    public static IEnumerable<Tuple<T, string>> GetAllWeeksInYear<T>()
      where T : struct, IComparable
    {
      for (var i = 1; i <= 52; i++)
      {
        yield return new Tuple<T, string>((T)Convert.ChangeType(i, typeof(T)), i.ToString());
      }
    }

    public static T TransformToDate<T>(DateTime date)
      where T : struct, IComparable
    {
      var value = date.Year * 10000
        + date.Month * 100
        + date.Day;

      return value.SetOutput<T>();
    }

    public static T TransformToDayOfMonth<T>(DateTime date)
      where T : struct, IComparable
    {
      var value = date.Day;

      return value.SetOutput<T>();
    }

    public static string GetLevelName(DateTime date, DateLevels level)
    {
      switch (level)
      {
        case DateLevels.DAY:
          return date.ToString("dd");
        case DateLevels.DATE:
          return date.ToString("yyyy-MM-dd");
        case DateLevels.YEAR:
          return date.ToString("yyyy");
        case DateLevels.MONTH:
          return date.ToString("MMMM");
        case DateLevels.MONTH_WITH_YEAR:
          return date.ToString("yyyy MMMM");
        case DateLevels.WEEK:
          return string.Format("{0} Week {1}", date.ToString("yyyy"), DateToWeek(date));
        case DateLevels.QUARTER:
          return string.Format("{0} Q{1}", date.ToString("yyyy"), DateToQuarter(date));
        default:
          throw new Exception("Level not supported.");
      }
    }

    public static string GetLevelName(TimeSpan timeSpan, TimeLevels level)
    {
      switch (level)
      {
        case TimeLevels.HOUR:
          return timeSpan.ToString("hh");
        case TimeLevels.MINUTES:
          return timeSpan.ToString("mm");
        case TimeLevels.SECONDS:
          return timeSpan.ToString("ss");
        case TimeLevels.TIME:
          return timeSpan.ToString("hh:mm:ss");
        default:
          throw new Exception("Level not supported.");
      }
    }

    public static T TransformToHour<T>(TimeSpan timespan)
      where T : struct, IComparable
    {
      var value = timespan.Hours + 1;

      return value.SetOutput<T>();
    }

    public static T TransformToMinutes<T>(TimeSpan timespan)
      where T : struct, IComparable
    {
      var value = timespan.Minutes + 1;

      return value.SetOutput<T>();
    }

    public static T TransformToSeconds<T>(TimeSpan timespan)
      where T : struct, IComparable
    {
      var value = timespan.Seconds + 1;

      return value.SetOutput<T>();
    }

    public static T TransformToTime<T>(TimeSpan timespan)
      where T : struct, IComparable
    {
      var value = (timespan.Hours + 1 )* 1000
        + timespan.Minutes * 100
        + timespan.Seconds;

      return value.SetOutput<T>();
    }

    public static int DateToWeek(DateTime date)
    {
      var weekNumber = CultureInfo
        .CurrentCulture.Calendar
        .GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

      return weekNumber;
    }

    public static T TransformToDay<T>(DateTime date)
      where T : struct, IComparable
    {
      var value = date.Day;

      return value.SetOutput<T>();
    }

    public static T TransformToWeek<T>(DateTime date)
      where T : struct, IComparable
    {
      var value = date.Year * 100
        + DateToWeek(date);

      return value.SetOutput<T>();
    }

    public static T TransformToMonth<T>(DateTime date)
      where T : struct, IComparable
    {
      var value = date.Year * 100
        + date.Month;

      return value.SetOutput<T>();
    }

    public static T TransformToMonthOfYear<T>(DateTime date)
      where T : struct, IComparable
    {
      var value = date.Month;

      return value.SetOutput<T>();
    }

    public static int DateToQuarter(DateTime date)
    {
      return (date.Month + 2) / 3;
    }

    public static T TransformToQuarter<T>(DateTime date)
      where T : struct, IComparable
    {
      var value = date.Year * 10
        + DateToQuarter(date);

      // (date.AddMonths(-3).Month + 2)/3;

      return value.SetOutput<T>();
    }

    public static T TransformToQuarterOfYear<T>(DateTime date)
      where T : struct, IComparable
    {
      // (date.AddMonths(-3).Month + 2)/3;

      return DateToQuarter(date).SetOutput<T>();
    }

    public static T TransformToYear<T>(DateTime date)
      where T : struct, IComparable
    {
      var value = date.Year;

      return value.SetOutput<T>();
    }

    public static T SetOutput<T>(this int value)
    {
      var ovalue = default(T);

      switch (ovalue)
      {
        case int i:
          return (T)(object)value;

        case long i:
          return (T)(object)Convert.ToInt64(value);

        case uint i:
          return (T)(object)Convert.ToUInt32(value);

        case ulong i:
          return (T)(object)Convert.ToUInt64(value);

        default:
          throw new Exception("Type not supported.");
      }
    }
  }
}