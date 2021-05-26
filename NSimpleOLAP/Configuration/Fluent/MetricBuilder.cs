using System;
using System.Linq.Expressions;

namespace NSimpleOLAP.Configuration.Fluent
{
  /// <summary>
  /// Description of MetricBuilder.
  /// </summary>
  public class MetricBuilder
  {
    private MetricConfig _element;

    public MetricBuilder()
    {
      _element = new MetricConfig();
    }

    #region public methods

    internal MetricBuilder SetName(string name)
    {
      _element.Name = name;
      return this;
    }

    public MetricBuilder SetType(Type type)
    {
      _element.DataType = type;
      return this;
    }

    public MetricBuilder SetExpression(string expression)
    {
      _element.MetricFunction = expression;
      return this;
    }

    internal MetricConfig Create()
    {
      return _element;
    }

    #endregion public methods
  }
}