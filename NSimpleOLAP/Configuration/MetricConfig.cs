using System;
using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class MetricConfig : ConfigurationElement
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>MetricElement</c>.
    /// </summary>
    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    public string Name
    {
      get { return (string)this["name"]; }
      set { this["name"] = value; }
    }

    /// <summary>
    /// A demonstration of how to use a boolean property.
    /// </summary>
    [ConfigurationProperty("special")]
    public bool IsSpecial
    {
      get { return (bool)this["special"]; }
      set { this["special"] = value; }
    }

    [ConfigurationProperty("type", DefaultValue = typeof(double))]
    public Type DataType
    {
      get { return (Type)this["type"]; }
      set { this["type"] = value; }
    }

    [ConfigurationProperty("metricFunction")]
    public string MetricFunction
    {
      get { return (string)this["metricFunction"]; }
      set { this["metricFunction"] = value; }
    }
  }
}