using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.Common;
using NSimpleOLAP.Triggers.Interfaces;
using NSimpleOLAP.Triggers.Molap;
using NSimpleOLAP.CubeExpressions.Interfaces;
using NSimpleOLAP.Interfaces;

namespace NSimpleOLAP.Triggers.Builder
{
  public class TriggerBuilder<T>
    where T : struct, IComparable
  {
    public TriggerBuilder<T> Bind(params string[] tuples)
    {
      return this;
    }

    public TriggerBuilder<T> ExecuteWhen()
    {
      return this;
    }

    public TriggerBuilder<T> ExecutionMode()
    {
      return this;
    }

    public TriggerBuilder<T> OnCondition(Action<PreConditionBuilder<T>> conditionBuider)
    {
      return this;
    }

    public TriggerBuilder<T> CreateOnAction(Action<ITriggerContext<T, ICell<T>>> action)
    {
      return this;
    }

    public TriggerBuilder<T> CreateOffAction(Action<ITriggerContext<T, ICell<T>>> action)
    {
      return this;
    }

    public void Create()
    {

    }
  }
}
