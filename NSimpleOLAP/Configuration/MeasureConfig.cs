using System;
using System.Configuration;
using System.Linq.Expressions;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class MeasureConfig : ConfigurationElement
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>MeasureElement</c>.
    /// </summary>
    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\.,", MinLength = 0, MaxLength = 120)]
    public string Name
    {
      get { return (string)this["name"]; }
      set { this["name"] = value; }
    }

    [ConfigurationProperty("valueFieldName")]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
    public string ValueFieldName
    {
      get { return (string)this["valueFieldName"]; }
      set { this["valueFieldName"] = value; }
    }

    [ConfigurationProperty("valueFieldIndex")]
    public int? ValueFieldIndex
    {
      get { return (int?)this["valueFieldIndex"]; }
      set { this["valueFieldIndex"] = value; }
    }

    [ConfigurationProperty("type", DefaultValue = typeof(double))]
    public Type DataType
    {
      get { return (Type)this["type"]; }
      set { this["type"] = value; }
    }

    [ConfigurationProperty("mergeExpression")]
    public Expression MergeFunction
    {
      get { return (Expression)this["mergeExpression"]; }
      set { this["mergeExpression"] = value; }
    }
  }
}