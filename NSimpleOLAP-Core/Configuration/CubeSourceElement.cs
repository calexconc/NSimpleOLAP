using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class CubeSourceConfig
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>CubeSourceElement</c>.
    /// </summary>
 //   [ConfigurationProperty("name", IsRequired = true)]
 //   [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
    public string Name
    {
      get;
      set;
    }

    public SourceMappingsCollection Fields
    {
      get;
      set;
    } = new SourceMappingsCollection();
  }
}