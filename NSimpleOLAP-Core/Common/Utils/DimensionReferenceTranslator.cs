using NSimpleOLAP.Common;
using NSimpleOLAP.Schema;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Common.Utils
{
  /// <summary>
  /// Description of ReferenceTranslator.
  /// </summary>
  public class DimensionReferenceTranslator<T>
    where T : struct, IComparable
  {
    private DataSchema<T> _schema;
    private readonly KeyValuePair<T, T> AllValue;

    public DimensionReferenceTranslator(DataSchema<T> schema)
    {
      _schema = schema;
      AllValue = ReservedAndSpecialValues.GetAllValue<T>();
    }

    public KeyValuePair<T, T>[] Translate(string value)
    {
      var values = this.GetStreamValues(value);
      int index = 0;
      List<KeyValuePair<T, T>> tuples = new List<KeyValuePair<T, T>>();

      while (index < values.Length)
      {
        T key = default(T);
        T member = default(T);

        if (TryGetDimension(values[index], out key))
        {
          index++;

          if (index < values.Length
            && IsReservedWord(values[index]))
          {
            tuples.Add(new KeyValuePair<T, T>(key, member));
            tuples.Add(GetReservedWordMappedTuple(values[index]));
            index++;
            continue;
          }

          if (TryGetDimensionMember(key, values, index, out member))
            index++;

          tuples.Add(new KeyValuePair<T, T>(key, member));
        }
        else
          throw new BadTupleReferenceException(index);
      }

      return tuples.ToArray();
    }

    public T GetDimension(string value)
    {
      T key = default(T);

      if (TryGetDimension(value, out key))
        return key;
      else
        throw new Exception($"Dimension \'{value}\' does not exist in schema.");
    }

    public T GetDimensionMember(T dimKey, string value)
    {
      T key = default(T);

      if (TryGetDimensionMember(dimKey, value, out key))
        return key;
      else
        throw new Exception($"Segment \'{value}\' does not exist in the specified dimension.");
    }

    #region private members

    private string[] GetStreamValues(string value)
    {
      return value.Split('.');
    }

    private bool TryGetDimension(string value, out T key)
    {
      bool ret = false;
      var dimension = _schema.Dimensions[value];

      if (dimension != null)
      {
        key = dimension.ID;
        ret = true;
      }
      else
        key = default(T);

      return ret;
    }

    private bool TryGetDimensionMember(T dimKey, string value, out T key)
    {
      bool ret = false;
      var member = _schema.Dimensions[dimKey].Members[value];

      if (member != null)
      {
        key = member.ID;
        ret = true;
      }
      else
        key = default(T);

      return ret;
    }

    private bool TryGetDimensionMember(T dimKey, string[] values, int index, out T key)
    {
      if (index >= values.Length ||
        !_schema.Dimensions[dimKey].Members.Contains(values[index]))
      {
        key = default(T);

        return false;
      }

      bool ret = false;
      var member = _schema.Dimensions[dimKey].Members[values[index]];

      if (member != null)
      {
        key = member.ID;
        ret = true;
      }
      else
        key = default(T);

      return ret;
    }

    private bool IsReservedWord(string value)
    {
      if (value.Equals(ReservedAndSpecialValues.ALL, StringComparison.InvariantCultureIgnoreCase))
        return true;

      return false;
    }

    private KeyValuePair<T, T> GetReservedWordMappedTuple(string value)
    {
      if (value.Equals(ReservedAndSpecialValues.ALL, StringComparison.InvariantCultureIgnoreCase))
        return AllValue;
      else
        throw new Exception($"No mapped tupple member exists for word {value}.");
    }

    #endregion private members
  }
}