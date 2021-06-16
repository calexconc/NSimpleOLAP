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
  internal class DifferentialExecutionTrigger<T> : AbstractMolapTrigger<T>
    where T : struct, IComparable
  {
    private TriggerHelper<T, ICell<T>> _triggerHelper;
    public DifferentialExecutionTrigger(TriggerHelper<T, ICell<T>> triggerHelper)
    {
      _triggerHelper = triggerHelper;
      typeOf = TriggerType.ChangeState;
    }

    protected override IExpressionContext<T, ICell<T>> BuildConditionContext(ICell<T> cell)
    {
      throw new NotImplementedException();
    }

    protected override ITriggerContext<T, ICell<T>> BuildTriggerContext(ICell<T> cell)
    {
      throw new NotImplementedException();
    }

    protected override bool IsChangingState(ITriggerContext<T, ICell<T>> context)
    {
      throw new NotImplementedException();
    }

    protected override void Queue(ITriggerContext<T, ICell<T>> context)
    {
      _triggerHelper.Queue(context);
    }
  }
}
