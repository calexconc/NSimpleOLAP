using NSimpleOLAP.Data;
using NSimpleOLAP.Storage.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Storage.FactsCache
{
  public class FactsRow<T> : IFactRow<T>
    where T : struct, IComparable
  {
    private readonly KeyValuePair<T, T>[] _pairs;
    private readonly MeasureValuesCollection<T> _data;

    public FactsRow(T hashCode, KeyValuePair<T, T>[] pairs, MeasureValuesCollection<T> data)
    {
      _pairs = pairs;
      _data = data;
      HashCode = hashCode;
    }

    public FactsRow(int index, T hashCode, KeyValuePair<T, T>[] pairs, MeasureValuesCollection<T> data) : this(hashCode, pairs, data)
    {
      Index = index;
    }

    public int Index { get; private set; }

    public T HashCode { get; private set; }

    public KeyValuePair<T, T>[] Pairs { get { return _pairs; } }

    public MeasureValuesCollection<T> Data { get { return _data; } }

    public override bool Equals(object obj)
    {
      var value = obj as FactsRow<T>;

      return value.Pairs.Equals(this.Pairs)
        && value.Data.Equals(this.Data);
    }

    public override int GetHashCode()
    {
      return this.Pairs.GetHashCode()
        ^ this.Data.GetHashCode();
    }
  }
}