/*
 * Created by SharpDevelop.
 * User: calex
 * Date: 27-03-2012
 * Time: 15:59
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class DBConfigElement
  {
    public string Connection
    {
      get;
      set;
    }

    public string Query
    {
      get;
      set;
    }

    public string ProviderName
    {
      get;
      set;
    }

    public string AppSettings
    {
      get;
      set;
    } = "appsettings.json";
  }
}