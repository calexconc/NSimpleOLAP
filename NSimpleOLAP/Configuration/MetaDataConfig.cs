using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class MetaDataConfig : ConfigurationElement
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>MetaDataConfigElement</c>.
    /// </summary>
    [ConfigurationProperty("Dimensions", IsRequired = true)]
    public DimensionConfigCollection Dimensions
    {
      get { return (DimensionConfigCollection)this["Dimensions"]; }
      set { this["Dimensions"] = value; }
    }

    [ConfigurationProperty("Measures")]
    public MeasureConfigCollection Measures
    {
      get { return (MeasureConfigCollection)this["Measures"]; }
      set { this["Measures"] = value; }
    }

    [ConfigurationProperty("Metrics")]
    public MetricConfigCollection Metrics
    {
      get { return (MetricConfigCollection)this["Metrics"]; }
      set { this["Metrics"] = value; }
    }
  }
}