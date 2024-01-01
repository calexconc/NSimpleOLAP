using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.CubeExpressions.Interfaces;
using NSimpleOLAP.Interfaces;
using System;

namespace NSimpleOLAP.CubeExpressions.Builder
{
  public class MetricExpressionBuilder<T>
    where T : struct, IComparable
  {
    private string _name;
    private NamespaceResolver<T> _resolver;
    private ExpressionBuilder<T> _expressionBuilder;


    public MetricExpressionBuilder(NamespaceResolver<T> resolver)
    {
      _resolver = resolver;
    }

    public ExpressionBuilder<T> Metric(string name)
    {
      _name = name;

      _expressionBuilder = new ExpressionBuilder<T>(_resolver);

      return _expressionBuilder;
    }

    public MetricsExpression<T> Create()
    {
      var functor = _expressionBuilder.Create();
      var type = _expressionBuilder.ReturnType;
      Func<IExpressionContext<T, ICell<T>>, object> capsule = x =>
      {
        IExpressionContext<T, ICell<T>> result = functor(x);

        return result.Result;
      };

      return new ImplementationMetricsExpression(_name, type, capsule);
    }

    private class ImplementationMetricsExpression: MetricsExpression<T>
    {
      public ImplementationMetricsExpression(string name, Type type, Func<IExpressionContext<T, ICell<T>>, object> functor)
      {
        this.name = name;
        returnType = type;
        expression = functor;
      }
    }
  }
}