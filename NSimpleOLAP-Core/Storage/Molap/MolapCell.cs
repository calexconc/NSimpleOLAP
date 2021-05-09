using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Storage.Molap
{
  /// <summary>
  /// Description of MolapCell.
  /// </summary>
  public class MolapCell<T> : Cell<T>
    where T : struct, IComparable
  {
    public MolapCell(KeyValuePair<T, T>[] coords)
    {
      this.Coords = coords;
      this.Occurrences = 0;
      this.Values = new MolapValuesCollection<T>();
    }

    public void IncrementOcurrences()
    {
      this.Occurrences += 1;
    }

    public void Reset()
    {
      this.Occurrences = 0;
      this.Values = new MolapValuesCollection<T>();
    }
  }
}