using System.Configuration;
using NSimpleOLAP.Common;
using NSimpleOLAP.Common.Converters;
using System.Collections.Generic;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class SourceMappingsElement : ConfigurationElement
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>SourceMappingsElement</c>.
    /// </summary>
    [ConfigurationProperty("field", IsKey = true, IsRequired = true)]
    public string Field
    {
      get { return (string)this["field"]; }
      set { this["field"] = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    [ConfigurationProperty("dimension")]
    public string Dimension
    {
      get { return (string)this["dimension"]; }
      set { this["dimension"] = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    [ConfigurationProperty("labels")]
    public string[] Labels
    {
      get { return (string[])this["labels"]; }
      set { this["labels"] = value; }
    }


  }
}