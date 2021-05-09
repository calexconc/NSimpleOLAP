using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Interfaces
{
  /// <summary>
  /// Description of IValueCollection.
  /// </summary>
  public interface IValueCollection<T> : IDictionary<T, ValueType>
        where T : struct, IComparable
  {
    /*		S GetValue<S>(T key);
            bool InsertOrUpdate(T key, int value);
            bool InsertOrUpdate(T key, long value);
            bool InsertOrUpdate(T key, decimal value);
            bool InsertOrUpdate(T key, float value);
            bool InsertOrUpdate(T key, double value);*/
  }
}