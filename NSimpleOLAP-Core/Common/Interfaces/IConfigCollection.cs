using System;
using System.Collections.Generic;
using System.Text;

namespace NSimpleOLAP.Common.Interfaces
{
  public interface IConfigCollection<T> : ICollection<T>
  {
    T this[int index] {get; set;}

    T this[string name] { get; set; }

    void Remove(string name);
  }
}
