using System;
using System.Collections.Generic;
using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  public class ObjectMapperConfigElement : ConfigurationElement
  {
    /// <summary>
    /// Only for programatic configuration
    /// </summary>
    public Func<object, KeyValuePair<string, object>[]> Mapper
    {
      get;
      set;
    }

    /// <summary>
    /// Only for programatic configuration
    /// </summary>
    public IEnumerable<object> Collection
    {
      get;
      set;
    }
  }
}