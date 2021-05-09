using System;
using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class MetricConfig
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>MetricElement</c>.
    /// </summary>
    public string Name
    {
      get;
      set;
    }

    /// <summary>
    /// A demonstration of how to use a boolean property.
    /// </summary>
    public bool IsSpecial
    {
      get;
      set;
    }

    public Type DataType
    {
      get;
      set;
    } = typeof(double);

    public string MetricFunction
    {
      get;
      set;
    }
  }
}