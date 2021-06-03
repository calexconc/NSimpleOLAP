using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Common;
using NSimpleOLAP.CubeExpressions.Interfaces;
using NSimpleOLAP.Interfaces;

namespace NSimpleOLAP.Triggers.Interfaces
{
  public interface ITrigger<T>
    where T : struct, IComparable
  {
    Predicate<IExpressionContext<T, ICell<T>>> Condition { get; }

    Action<ITriggerContext<T, ICell<T>>> OnAction { get; }

    Action<ITriggerContext<T, ICell<T>>> OffAction { get; }

    TriggerType TypeOf { get; }

    TriggerExecutionPlanType ExecutionPlanType { get; }
  }
}
