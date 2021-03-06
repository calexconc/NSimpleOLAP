﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace NSimpleOLAP.Common.Converters
{
  public class TimeLevelListFieldConverter : ConfigurationConverterBase
  {
    public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo culture, object value)
    {
      if (value == null)
        return new List<TimeLevels>();

      var str = (string)value;
      var list = new List<TimeLevels>();

      foreach (var item in str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
      {
        TimeLevels result;

        if (!Enum.TryParse(item.ToUpper().Trim(), true, out result))
        {
          throw new InvalidOperationException("Invalid Date Level value");
        }

        list.Add(result);
      }

      return list;
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (value is List<TimeLevels>)
      {
        var arr = (List<TimeLevels>)value;

        return string.Join(",", arr.Select(x => x.ToString()));
      }

      return base.ConvertTo(context, culture, value, destinationType);
    }
  }
}
