using System.Configuration;
using NSimpleOLAP.Common;
using System.Collections.Generic;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class SourceMappingsElement
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>SourceMappingsElement</c>.
    /// </summary>
    public string Field
    {
      get;
      set;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Dimension
    {
      get;
      set;
    }

    /// <summary>
    /// 
    /// </summary>
    public string[] Labels
    {
      get;
      set;
    }
  }
}