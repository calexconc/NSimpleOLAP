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
  public sealed class OlapConfigSectionSettings : ConfigurationSection
  {
    private System.Configuration.Configuration _Config;

    #region ConfigurationProperties

    /// <summary>
    /// Collection of <c>OlapConfigSectionElement(s)</c>
    /// A custom XML section for an applications configuration file.
    /// </summary>
    [ConfigurationProperty("Cubes")]
    public CubeConfigCollection Cubes
    {
      get { return (CubeConfigCollection)this["Cubes"]; }
      set { this["Cubes"] = value; }
    }

    #endregion ConfigurationProperties

    /// <summary>
    /// Private Constructor used by our factory method.
    /// </summary>
    private OlapConfigSectionSettings() : base()
    {
      // Allow this section to be stored in user.app. By default this is forbidden.
      /*	this.SectionInformation.AllowExeDefinition =
          ConfigurationAllowExeDefinition.MachineToLocalUser;*/
    }

    #region Public Methods

    /// <summary>
    /// Saves the configuration to the config file.
    /// </summary>
    public void Save()
    {
      _Config.Save();
    }

    #endregion Public Methods

    #region Static Members

    /// <summary>
    /// Gets the current applications &lt;OlapConfigSection&gt; section.
    /// </summary>
    /// <param name="ConfigLevel">
    /// The &lt;ConfigurationUserLevel&gt; that the config file
    /// is retrieved from.
    /// </param>
    /// <returns>
    /// The configuration file's &lt;OlapConfigSection&gt; section.
    /// </returns>
    public static OlapConfigSectionSettings GetSection(ConfigurationUserLevel ConfigLevel)
    {
      /*
			 * This class is setup using a factory pattern that forces you to
			 * name the section &lt;OlapConfigSection&gt; in the config file.
			 * If you would prefer to be able to specify the name of the section,
			 * then remove this method and mark the constructor public.
			 */
      System.Configuration.Configuration Config = ConfigurationManager.OpenExeConfiguration
        (ConfigLevel);
      OlapConfigSectionSettings oOlapConfigSectionSettings;

      oOlapConfigSectionSettings =
        (OlapConfigSectionSettings)Config.GetSection("OlapConfigSectionSettings");
      if (oOlapConfigSectionSettings == null)
      {
        oOlapConfigSectionSettings = new OlapConfigSectionSettings();
        Config.Sections.Add("OlapConfigSectionSettings", oOlapConfigSectionSettings);
      }
      oOlapConfigSectionSettings._Config = Config;

      return oOlapConfigSectionSettings;
    }

    #endregion Static Members
  }
}