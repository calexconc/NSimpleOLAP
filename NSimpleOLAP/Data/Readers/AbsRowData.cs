using System.Collections.Generic;

namespace NSimpleOLAP.Data.Readers
{
  /// <summary>
  /// Description of AbsRowData.
  /// </summary>
  public abstract class AbsRowData
  {
    protected Dictionary<string, int> _indexes;
    protected object[] _values;

    public object this[string key]
    {
      get { return _values[_indexes[key]]; }
    }

    public object this[int index]
    {
      get { return _values[index]; }
    }

    public object[] Items
    {
      get { return _values; }
    }
  }
}