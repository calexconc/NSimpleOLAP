using NSimpleOLAP.Interfaces;
using NSimpleOLAP.Triggers;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP
{
  /// <summary>
  /// Description of Cell.
  /// </summary>
  public abstract class Cell<T> : ICell<T>
    where T : struct, IComparable
  {
    public KeyValuePair<T, T>[] Coords
    {
      get;
      protected set;
    }

    public uint Occurrences
    {
      get;
      protected set;
    }

    public IValueCollection<T> Values
    {
      get;
      protected set;
    }

    public event TriggerExecute<T> Triggered;

    public void Trigger()
    {
      if (Triggered != null)
      {
        Triggered(this);
      }
    }
  }
}