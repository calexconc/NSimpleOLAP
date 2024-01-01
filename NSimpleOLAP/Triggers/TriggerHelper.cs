using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
using NSimpleOLAP.Query;


namespace NSimpleOLAP.Triggers
{
  internal class TriggerHelper<T, U>
    where T : struct, IComparable
    where U : class, ICell<T>
  {

    private IList<ITrigger<T>> _triggers;
    private AllKeysComparer<T> _allKeysComparer;
    private KeyComparer<T> _keyComparer;
    private ConcurrentQueue<ITriggerContext<T, U>> _queue;

    public TriggerHelper(IList<ITrigger<T>> triggers)
    {
      _triggers = triggers;
      _allKeysComparer = new AllKeysComparer<T>();
      _keyComparer = new KeyComparer<T>();
      _queue = new ConcurrentQueue<ITriggerContext<T, U>>();
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

    public void DropAllTriggers(Graph<T, U> globalGraph)
    {
      foreach (var trigger in _triggers)
        TryDeRegister(trigger, globalGraph);
    }

    public void Queue(ITriggerContext<T, U> context)
    {
      if (!_queue.Contains(context)) // use different equality comparer
      {
        _queue.Enqueue(context);
      }
    }

    public void ServiceQueue()
    {
      if (_queue.Count > 0)
      {
        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 3 };

        Parallel.ForEach(DeQueue(), parallelOptions, x => ExecuteTrigger(x));
      }
    }

    private IEnumerable<ITriggerContext<T, U>> DeQueue()
    {
      while (_queue.TryDequeue(out var context))
      {
        yield return context;
      }
    }

    private void ExecuteTrigger(ITriggerContext<T, U> context)
    {
      context.ExecuteHandle(context);
    }

    private bool CanBind(KeyValuePair<T,T>[] trigger, KeyValuePair<T, T>[] coords)
    {
      if (trigger.Length == 0)
        return true;

      var hasWidldCards = trigger.Any(x => x.IsWildcard());

      if (!hasWidldCards && 
        _allKeysComparer.Compare(trigger, coords) == 0)
        return true;

      if (hasWidldCards)
      {
        var indexes =  trigger
          .Select((x,i) => new { wildcard = x.IsWildcard(), index = i - 1 })
          .Where(x => x.wildcard)
          .Select(x=> x.index)
          .ToList();

        var result = false;
        var matchedIndexes = new List<int>();

        foreach (var wild in indexes)
        {
          var item = trigger[wild];
          var cindex = Array.FindIndex(coords, x => _keyComparer.Compare(item, x) == 0);

          if (cindex >= 0)
          {
            result = true;
            matchedIndexes.Add(cindex);
          }
          else
          {
            result = false;
            break;
          }
        }

        if (result &&
          coords.Length == indexes.Count * 2)
          return true;

        if (result)
        {
          var remTrgCoords = trigger
          .Select((x, i) => new { wildcard = x.IsWildcard(), index = i, Pair = x })
          .Where(x => !x.wildcard && !indexes.Contains(x.index))
          .Select(x => x.Pair).ToArray();

          if (remTrgCoords.Length == 0)
            return true;

          var remCoords = coords
            .Where((x, i) => !matchedIndexes.Contains(i))
            .ToArray();

          if (remTrgCoords.Length == remCoords.Length)
          {
            if (_allKeysComparer.Compare(remTrgCoords, remCoords) == 0)
              return true;
          }
        }
      }
      
      return false;
    }
  }
}
