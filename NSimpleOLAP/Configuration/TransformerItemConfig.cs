using System;
using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  public sealed class TransformerItemConfig : ConfigurationElement
  {
    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    public string Name
    {
      get { return (string)this["name"]; }
      set { this["name"] = value; }
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty("type", DefaultValue = typeof(int))]
    public Type FieldType
    {
      get { return (Type)this["type"]; }
      set { this["type"] = value; }
    }

    [ConfigurationProperty("returnValue")]
    public object ReturnValue
    {
      get { return this["returnValue"]; }
      set { this["returnValue"] = value; }
    }

    [ConfigurationProperty("isIntervalSetup", DefaultValue = false)]
    public bool IsIntervalSetup
    {
      get { return (bool)this["isIntervalSetup"]; }
      set { this["isIntervalSetup"] = value; }
    }

    [ConfigurationProperty("lowerValue")]
    public object LowerValue
    {
      get { return this["lowerValue"]; }
      set { this["lowerValue"] = value; }
    }

    [ConfigurationProperty("upperValue")]
    public object UpperValue
    {
      get { return this["upperValue"]; }
      set { this["upperValue"] = value; }
    }
  }
}