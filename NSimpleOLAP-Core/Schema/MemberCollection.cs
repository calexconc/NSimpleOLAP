using NSimpleOLAP.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Schema
{
  /// <summary>
  /// Description of MemberCollection.
  /// </summary>
  public class MemberCollection<T> : BaseDataMemberCollection<T, Member<T>>
    where T : struct, IComparable
  {
    public MemberCollection(IMemberStorage<T, Member<T>> storage)
    {
      _storage = storage;
      base.Init();
    }

    public override Member<T> Next(T key)
    {
      var linkedList = new LinkedList<T>(this.Select(x => x.ID));
      var node = linkedList.Find(key);

      if (node != null && node.Next != null)
      {
        return this[node.Next.Value];
      }

      return this[linkedList.First.Value];
    }

    public override Member<T> Previous(T key)
    {
      var linkedList = new LinkedList<T>(this.Select(x => x.ID));
      var node = linkedList.Find(key);

      if (node != null && node.Previous != null)
      {
        return this[node.Previous.Value];
      }

      return this[linkedList.Last.Value];
    }
  }
}