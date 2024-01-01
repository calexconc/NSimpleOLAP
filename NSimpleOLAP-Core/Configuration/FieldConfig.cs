using System;
using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class FieldConfig
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>FieldElement</c>.
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
    }

    /// <summary>
    ///
    /// </summary>
    public int Index
    {
      get;
      set;
    } = -1;

    public string Format
    {
      get;
      set;
    }
  }
}