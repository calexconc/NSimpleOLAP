using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class MetaDataConfig
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>MetaDataConfigElement</c>.
    /// </summary>
    public DimensionConfigCollection Dimensions
    {
      get;
      set;
    } = new DimensionConfigCollection();

    public MeasureConfigCollection Measures
    {
      get;
      set;
    } = new MeasureConfigCollection();

    public MetricConfigCollection Metrics
    {
      get;
      set;
    } = new MetricConfigCollection();
  }
}