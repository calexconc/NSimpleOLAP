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
using NSimpleOLAP.Triggers.Interfaces;


namespace NSimpleOLAP.Triggers
{
  internal class TriggerHelper<T>
    where T : struct, IComparable
  {
    private DataSchema<T> _schema;
    private CubeSourceConfig _config;

    public TriggerHelper(DataSchema<T> schema, CubeSourceConfig config)
    {
      _schema = schema;
      _config = config;
    }

    public void TryRegister(ITrigger<T> trigger, Cell<T> cell)
    {
      if (CanBind(trigger.Binding, cell.Coords))
      {
        cell.Triggered += trigger.Execute;
      }
    }

    public void TryDeRegister(ITrigger<T> trigger, Cell<T> cell)
    {
      if (CanBind(trigger.Binding, cell.Coords))
      {
        cell.Triggered -= trigger.Execute;
      }
    }

    private bool CanBind(KeyValuePair<T,T>[] trigger, KeyValuePair<T, T>[] coords)
    {
      if (trigger.Length == 0)
        return true;

      return false;
    }
  }
}
