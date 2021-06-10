using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.Data.Readers;
using NSimpleOLAP.Schema;
using NSimpleOLAP.Interfaces;
using NSimpleOLAP.Triggers.Interfaces;
using NSimpleOLAP.Storage.Molap.Graph;


namespace NSimpleOLAP.Triggers
{
  internal class TriggerHelper<T, U>
    where T : struct, IComparable
    where U : class, ICell<T>
  {

    private IList<ITrigger<T>> _triggers;

    public TriggerHelper(IList<ITrigger<T>> triggers)
    {
      _triggers = triggers;
    }

    public void TryRegister(ITrigger<T> trigger, U cell)
    {
      if (CanBind(trigger.Binding, cell.Coords))
      {
        cell.Triggered += trigger.Execute;
      }
    }

    public void TryDeRegister(ITrigger<T> trigger, U cell)
    {
      if (CanBind(trigger.Binding, cell.Coords))
      {
        cell.Triggered -= trigger.Execute;
      }
    }

    public void TryRegister(ITrigger<T> trigger, Graph<T, U> globalGraph)
    {
      var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 3 };

      Parallel.ForEach(globalGraph.NodesEnumerator(), parallelOptions, 
        x => {
          TryRegister(trigger, x.Container);
        });
    }

    public void TryDeRegister(ITrigger<T> trigger, Graph<T, U> globalGraph)
    {
      var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 3 };

      Parallel.ForEach(globalGraph.NodesEnumerator(), parallelOptions,
        x => {
          TryDeRegister(trigger, x.Container);
        });
    }

    public void TryRegisterActiveTriggers(U cell)
    {
      if (_triggers == null)
        return;

      var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 3 };

      Parallel.ForEach(_triggers, parallelOptions,
        x => {
          TryDeRegister(x, cell);
        });
    }

    private bool CanBind(KeyValuePair<T,T>[] trigger, KeyValuePair<T, T>[] coords)
    {
      if (trigger.Length == 0)
        return true;

      return false;
    }
  }
}
