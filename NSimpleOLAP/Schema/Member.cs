using NSimpleOLAP.Common;
using NSimpleOLAP.Schema.Interfaces;
using System;
using System.Collections.Generic;
using NSimpleOLAP.Data.Interfaces;

namespace NSimpleOLAP.Schema
{
  /// <summary>
  /// Description of Member.
  /// </summary>
  public class Member<T> : IMember<T>
    where T : struct, IComparable
  {
    public Member()
    {
    }

    public string Name
    {
      get;
      set;
    }

    public T ID
    {
      get;
      set;
    }

    public ItemType ItemType
    {
      get { return ItemType.Member; }
    }

    public string Alias
    {
      get;
      set;
    }
  }

  public class MemberLevel<T> : Member<T>
    where T : struct, IComparable
  {
    public MemberLevel()
    {
      Levels = new Dictionary<string, T>();
    }

    public IDictionary<string, T> Levels { get; }
  }

  public class MemberGenerated<T> : Member<T>
    where T : struct, IComparable
  {
    public MemberGenerated(IDataTransformer transformer)
    {
      Transformer = transformer;
    }

    public IDataTransformer Transformer { get; private set; }
  }
}