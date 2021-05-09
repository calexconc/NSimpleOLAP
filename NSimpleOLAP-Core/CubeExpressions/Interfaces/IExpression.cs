using NSimpleOLAP.Interfaces;
using System;

namespace NSimpleOLAP.CubeExpressions.Interfaces
{
  public interface IExpression<T, U>
    where T : struct, IComparable
    where U : class, ICell<T>
  {
    T ID { get; }

    Type ReturnType { get; }

    object Evaluate(IExpressionContext<T, U> context);

    V Evaluate<V>(IExpressionContext<T, U> context);
  }
}