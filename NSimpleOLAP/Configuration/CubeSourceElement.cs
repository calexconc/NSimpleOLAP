using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class CubeSourceConfig : ConfigurationElement
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>CubeSourceElement</c>.
    /// </summary>
    [ConfigurationProperty("name", IsRequired = true)]
    [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
    public string Name
    {
      get { return (string)this["name"]; }
      set { this["name"] = value; }
    }

    [ConfigurationProperty("SourceMaps")]
    public SourceMappingsCollection Fields
    {
      get { return (SourceMappingsCollection)this["SourceMaps"]; }
      set { this["SourceMaps"] = value; }
    }
  }
}