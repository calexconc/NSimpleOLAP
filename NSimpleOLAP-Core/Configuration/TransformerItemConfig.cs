using System;
using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  public sealed class TransformerItemConfig
  {
    /// <summary>
    ///
    /// </summary>
    public string Name
    {
      get;
      set;
    }

    /// <summary>
    ///
    /// </summary>
    public Type FieldType
    {
      get;
      set;
    } = typeof(int);

    public object ReturnValue
    {
      get;
      set;
    }

    public bool IsIntervalSetup
    {
      get;
      set;
    }

    public object LowerValue
    {
      get;
      set;
    }

    public object UpperValue
    {
      get;
      set;
    }
  }
}