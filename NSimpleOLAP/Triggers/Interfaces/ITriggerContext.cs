using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Interfaces;

namespace NSimpleOLAP.Triggers.Interfaces
{
  public interface ITriggerContext<T, U>
    where T : struct, IComparable
    where U : class, ICell<T>
  {
    U CurrentCell { get; }

    U RootCell { get; }

    Action<ITriggerContext<T, U>> ExecuteHandle { get; }
  }
}
