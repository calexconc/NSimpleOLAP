using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSimpleOLAP.Schema
{
  public class MemberDateTimeCollection<T> : MemberCollection<T>
    where T : struct, IComparable
  {
    public MemberDateTimeCollection():base(null)
    {
      // change this
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
