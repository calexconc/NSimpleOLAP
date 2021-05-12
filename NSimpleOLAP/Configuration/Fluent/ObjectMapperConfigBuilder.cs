using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Configuration.Fluent
{
  public class ObjectMapperConfigBuilder
  {
    private ObjectMapperConfigElement _element;

    public ObjectMapperConfigBuilder()
    {
      _element = new ObjectMapperConfigElement();
    }

    public ObjectMapperConfigBuilder SetCollection<T>(IEnumerable<T> collection)
      where T : class
    {
      _element.Collection = collection;

      return this;
    }

    public ObjectMapperConfigBuilder SetMapper<T>(Func<T, KeyValuePair<string, object>[]> mapper)
      where T : class
    {
      Func<object, KeyValuePair<string, object>[]> functor = x => mapper((T)x);

      _element.Mapper = functor;

      return this;
    }

    internal ObjectMapperConfigElement Create()
    {
      return _element;
    }
  }
}