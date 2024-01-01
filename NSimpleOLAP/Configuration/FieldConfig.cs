using System;
using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class FieldConfig : ConfigurationElement
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>FieldElement</c>.
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
    [ConfigurationProperty("type")]
    public Type FieldType
    {
      get { return (Type)this["type"]; }
      set { this["type"] = value; }
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty("index", DefaultValue = -1)]
    public int Index
    {
      get { return (int)this["index"]; }
      set { this["index"] = value; }
    }

    [ConfigurationProperty("format", IsRequired = false)]
    public string Format
    {
      get { return (string)this["format"]; }
      set { this["format"] = value; }
    }
  }
}