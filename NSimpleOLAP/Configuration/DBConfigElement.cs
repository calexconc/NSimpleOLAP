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
  public sealed class DBConfigElement : ConfigurationElement
  {
    [ConfigurationProperty("connection")]
    public string Connection
    {
      get { return (string)this["connection"]; }
      set { this["connection"] = value; }
    }

    [ConfigurationProperty("query")]
    public string Query
    {
      get { return (string)this["query"]; }
      set { this["query"] = value; }
    }
  }
}