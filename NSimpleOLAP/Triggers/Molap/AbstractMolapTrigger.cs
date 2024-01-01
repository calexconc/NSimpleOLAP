using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Common;
using NSimpleOLAP.CubeExpressions.Interfaces;
using NSimpleOLAP.Interfaces;
using NSimpleOLAP.Triggers.Interfaces;

namespace NSimpleOLAP.Triggers.Molap
{
  public abstract class AbstractMolapTrigger<T> : ITrigger<T>
    where T : struct, IComparable
  {
    protected T id;
    protected KeyValuePair<T, T>[] coordsBinding;
    protected Predicate<IExpressionContext<T, ICell<T>>> preCondition;
    protected Action<ITriggerContext<T, ICell<T>>> actionOnPreconditionTrue;
    protected Action<ITriggerContext<T, ICell<T>>> actionOnPreconditionTurnsOff;
    protected TriggerType typeOf;
    protected TriggerExecutionPlanType executionPlan;

    public T ID { get { return id; } }

    public KeyValuePair<T, T>[] Binding { get { return coordsBinding; } }

    public Predicate<IExpressionContext<T, ICell<T>>> Condition { get { return preCondition; } }

    public Action<ITriggerContext<T, ICell<T>>> OnAction { get { return actionOnPreconditionTrue; } }

    public Action<ITriggerContext<T, ICell<T>>> OffAction { get { return actionOnPreconditionTurnsOff; } }

    public TriggerType TypeOf { get { return typeOf; } }

    public TriggerExecutionPlanType ExecutionPlanType { get { return executionPlan; } }

    public void Execute(ICell<T> cell)
    {
      switch (this.ExecutionPlanType)
      {
        case TriggerExecutionPlanType.Queue:
          HandleQueuedTriggerExecution(cell);
          break;
        case TriggerExecutionPlanType.Priority:
          HandleImmediateTriggerExecution(cell);
          break;
      }
    }

    private void HandleQueuedTriggerExecution(ICell<T> cell)
    {
      var triggerContext = BuildTriggerContext(cell);


      if (Condition(BuildConditionContext(cell)))
      {
        if (TypeOf == TriggerType.Always)
        {
          Queue(triggerContext);
          return;
        }

        if (IsChangingState(triggerContext))
        {
          Queue(triggerContext);
          return;
        }
      }

      if (TypeOf == TriggerType.ChangeState &&
        IsChangingState(triggerContext))
      {
        Queue(triggerContext);
        return;
      }
    }

    private void HandleImmediateTriggerExecution(ICell<T> cell)
    {
      var triggerContext = BuildTriggerContext(cell);


      if (Condition(BuildConditionContext(cell)))
      {
        if (TypeOf == TriggerType.Always)
        {
          OnAction(triggerContext);
          return;
        }

        if (IsChangingState(triggerContext))
        {
          OnAction(triggerContext);
          return;
        }
      }

      if (TypeOf == TriggerType.ChangeState &&
        IsChangingState(triggerContext))
      {
        OffAction(triggerContext);
        return;
      }
    }

    protected abstract IExpressionContext<T, ICell<T>> BuildConditionContext(ICell<T> cell);

    protected abstract ITriggerContext<T, ICell<T>> BuildTriggerContext(ICell<T> cell);

    protected abstract bool IsChangingState(ITriggerContext<T, ICell<T>> context);

    protected abstract void Queue(ITriggerContext<T, ICell<T>> context);
  }
}
