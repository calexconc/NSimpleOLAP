using NSimpleOLAP.Common;
using NSimpleOLAP.CubeExpressions.Interfaces;
using NSimpleOLAP.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Triggers.Interfaces
{
  public interface ITrigger<T>
    where T : struct, IComparable
  {
    T ID { get; }

    KeyValuePair<T, T>[] Binding { get; }

    Predicate<IExpressionContext<T, ICell<T>>> Condition { get; }

    Action<ITriggerContext<T, ICell<T>>> OnAction { get; }

    Action<ITriggerContext<T, ICell<T>>> OffAction { get; }

    TriggerType TypeOf { get; }

    TriggerExecutionPlanType ExecutionPlanType { get; }

    void Execute(ICell<T> cell);
  }
}