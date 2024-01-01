using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Common
{
  /// <summary>
  /// Description of CanonicFormater.
  /// </summary>
  internal class CanonicFormater<T>
    where T : struct, IComparable
  {
    public CanonicFormater()
    {
    }

    public KeyValuePair<T, T>[] Format(KeyValuePair<T, T>[] pairs)
    {
      Array.Sort<KeyValuePair<T, T>>(pairs, ComparePairs);

      if (pairs.Length > 0 && !pairs[0].Value.Equals(default(T)))
      {
        List<KeyValuePair<T, T>> xpairs = new List<KeyValuePair<T, T>>(pairs);
        xpairs.Insert(0, new KeyValuePair<T, T>(pairs[0].Key, default(T)));

        return xpairs.ToArray();
      }
      else
        return pairs;
    }

    private int ComparePairs(KeyValuePair<T, T> a, KeyValuePair<T, T> b)
    {
      return a.Key.CompareTo(b.Key);
    }
  }
}