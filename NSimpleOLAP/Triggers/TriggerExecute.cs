using NSimpleOLAP.Interfaces;
using System;

namespace NSimpleOLAP.Triggers
{
  public delegate void TriggerExecute<T>(ICell<T> cell) where T : struct, IComparable;
}