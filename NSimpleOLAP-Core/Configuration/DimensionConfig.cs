using System;
using System.Configuration;
using System.Collections.Generic;
using NSimpleOLAP.Common;
using System.ComponentModel;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class DimensionConfig
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>DimensionElement</c>.
    /// </summary>
  //  [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
  //  [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\.,", MinLength = 0, MaxLength = 120)]
    public string Name
    {
      get;
      set;
    }

    //[ConfigurationProperty("source", IsRequired = true)]
    //[StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
    public string Source
    {
      get;
      set;
    }

    //[ConfigurationProperty("descFieldName")]
    //[StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
    public string DesFieldName
    {
      get;
      set;
    }

    //[ConfigurationProperty("valueFieldName")]
    //[StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
    public string ValueFieldName
    {
      get;
      set;
    }

    public int? DesFieldIndex
    {
      get;
      set;
    }

    public int? ValueFieldIndex
    {
      get;
      set;
    }

    public bool AllowsMembersWithSameName
    {
      get;
      set;
    }

    public DimensionType DimensionType
    {
      get;
      set;
    } = DimensionType.Numeric;

    /// <summary>
    ///
    /// </summary>
    public List<DateLevels> DateLevels
    {
      get;
      set;
    } = new List<DateLevels>();

    /// <summary>
    ///
    /// </summary>
    public List<TimeLevels> TimeLevels
    {
      get;
      set;
    } = new List<TimeLevels>();

    /// <summary>
    ///
    /// </summary>
    public string[] LevelLabels
    {
      get;
      set;
    }

    public string ParentDimension
    {
      get;
      set;
    }

    public bool SourceIsGenerated
    {
      get;
      set;
    }
  }
}