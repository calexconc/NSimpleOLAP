using System;
using System.Configuration;
using System.Collections.Generic;
using NSimpleOLAP.Common;
using NSimpleOLAP.Common.Converters;
using System.ComponentModel;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class DimensionConfig : ConfigurationElement
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>DimensionElement</c>.
    /// </summary>
    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\.,", MinLength = 0, MaxLength = 120)]
    public string Name
    {
      get { return (string)this["name"]; }
      set { this["name"] = value; }
    }

    [ConfigurationProperty("source", IsRequired = true)]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
    public string Source
    {
      get { return (string)this["source"]; }
      set { this["source"] = value; }
    }

    [ConfigurationProperty("descFieldName")]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
    public string DesFieldName
    {
      get { return (string)this["descFieldName"]; }
      set { this["descFieldName"] = value; }
    }

    [ConfigurationProperty("valueFieldName")]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
    public string ValueFieldName
    {
      get { return (string)this["valueFieldName"]; }
      set { this["valueFieldName"] = value; }
    }

    [ConfigurationProperty("descFieldIndex")]
    public int? DesFieldIndex
    {
      get { return (int?)this["descFieldIndex"]; }
      set { this["descFieldIndex"] = value; }
    }

    [ConfigurationProperty("valueFieldIndex")]
    public int? ValueFieldIndex
    {
      get { return (int?)this["valueFieldIndex"]; }
      set { this["valueFieldIndex"] = value; }
    }

    [ConfigurationProperty("allowsNameDuplicates", DefaultValue = false)]
    public bool AllowsMembersWithSameName
    {
      get { return (bool)this["allowsNameDuplicates"]; }
      set { this["allowsNameDuplicates"] = value; }
    }

    [ConfigurationProperty("dimensionType", DefaultValue = DimensionType.Numeric)]
    public DimensionType DimensionType
    {
      get { return (DimensionType) this["dimensionType"]; }
      set { this["dimensionType"] = value; }
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty("dateLevels", IsRequired = false)]
    [TypeConverter(typeof(DateLevelListFieldConverter))]
    public List<DateLevels> DateLevels
    {
      get { return (List<DateLevels>)this["dateLevels"]; }
      set { this["dateLevels"] = value; }
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty("timeLevels", IsRequired = false)]
    [TypeConverter(typeof(TimeLevelListFieldConverter))]
    public List<TimeLevels> TimeLevels
    {
      get { return (List<TimeLevels>)this["timeLevels"]; }
      set { this["timeLevels"] = value; }
    }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationProperty("levelLabels", IsRequired = false)]
    public string[] LevelLabels
    {
      get { return (string[])this["levelLabels"]; }
      set { this["levelLabels"] = value; }
    }

    [ConfigurationProperty("parentDimension")]
    public string ParentDimension
    {
      get { return (string)this["parentDimension"]; }
      set { this["parentDimension"] = value; }
    }

    [ConfigurationProperty("sourceIsGenerated", DefaultValue = false)]
    public bool SourceIsGenerated
    {
      get { return (bool)this["sourceIsGenerated"]; }
      set { this["sourceIsGenerated"] = value; }
    }
  }
}