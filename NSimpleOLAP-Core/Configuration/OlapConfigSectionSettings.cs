using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Configuration section &lt;OlapConfigSection&gt;
  /// </summary>
  /// <remarks>
  /// Assign properties to your child class that has the attribute
  /// <c>[ConfigurationProperty]</c> to store said properties in the xml.
  /// </remarks>
  public sealed class OlapConfigSectionSettings 
  {

    #region ConfigurationProperties

    /// <summary>
    /// Collection of <c>OlapConfigSectionElement(s)</c>
    /// A custom XML section for an applications configuration file.
    /// </summary>
    public CubeConfigCollection Cubes
    {
      get;
      set;
    } = new CubeConfigCollection();

    #endregion ConfigurationProperties

    #region Public Methods

    #endregion Public Methods

    
  }
}