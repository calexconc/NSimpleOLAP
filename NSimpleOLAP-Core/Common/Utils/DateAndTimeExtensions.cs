using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace NSimpleOLAP.Common.Utils
{
  internal static class DateAndTimeExtensions
  {
    public static DateTime? GetDate(this string value, string format)
    {
      var provider = CultureInfo.InvariantCulture;
      DateTime result;

      if (DateTime.TryParseExact(value, format, provider, DateTimeStyles.None, out result))
        return result;

      return null;
    }

    public static TimeSpan? GetTimeSpan(this string value, string format)
    {
      var provider = CultureInfo.InvariantCulture;
      TimeSpan result;

      if (TimeSpan.TryParseExact(value, format, provider,TimeSpanStyles.None, out result))
        return result;

      return null;
    }
  }
}
