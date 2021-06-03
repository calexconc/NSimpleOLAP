using NSimpleOLAP.Interfaces;
using NSimpleOLAP.Triggers.Interfaces;
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

    public IList<ITrigger<T>> Triggers
    {
      get;
      private set;
    } = new List<ITrigger<T>>();
  }
}