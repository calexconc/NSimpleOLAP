using System;
using System.Configuration;
using System.Linq.Expressions;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class MeasureConfig
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>MeasureElement</c>.
    /// </summary>
    //[ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    //[StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\.,", MinLength = 0, MaxLength = 120)]
    public string Name
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

    public int? ValueFieldIndex
    {
      get;
      set;
    }

    public Type DataType
    {
      get;
      set;
    } = typeof(double);

    public Expression MergeFunction
    {
      get;
      set;
    }
  }
}