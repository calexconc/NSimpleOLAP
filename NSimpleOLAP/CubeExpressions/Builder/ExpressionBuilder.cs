using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.CubeExpressions.Interfaces;
using NSimpleOLAP.Interfaces;
using System;

namespace NSimpleOLAP.CubeExpressions.Builder
{
  public class ExpressionBuilder<T>
    where T : struct, IComparable
  {
    private ExpressionElementsBuilder<T> _expressionRoot;
    private NamespaceResolver<T> _resolver;

    public ExpressionBuilder(NamespaceResolver<T> resolver)
    {
      _resolver = resolver;
    }

    public void Expression(Action<ExpressionElementsBuilder<T>> expression)
    {
      _expressionRoot = new ExpressionElementsBuilder<T>(_resolver);

      expression(_expressionRoot);
    }

    internal ExpressionElementsBuilder<T> SetElementsBuilder()
    {
      _expressionRoot = new ExpressionElementsBuilder<T>(_resolver);

      return _expressionRoot;
    }

    internal Type ReturnType
    {
      get
      {
        return _expressionRoot.ReturnType;
      }
    }

    internal Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Create()
    {
      return _expressionRoot.Create();
    }
  }
}