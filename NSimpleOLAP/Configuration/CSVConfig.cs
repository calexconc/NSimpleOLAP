using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class CSVConfig : ConfigurationElement
  {
    [ConfigurationProperty("path")]
    public string FilePath
    {
      get { return (string)this["path"]; }
      set { this["path"] = value; }
    }

    [ConfigurationProperty("fieldDelimiter", IsRequired = true, DefaultValue = ',')]
    public char FieldDelimiter
    {
      get { return (char)this["fieldDelimiter"]; }
      set { this["fieldDelimiter"] = value; }
    }

    [ConfigurationProperty("textQuoted", DefaultValue = false)]
    public bool TextIsDoubleQuoted
    {
      get { return (bool)this["textQuoted"]; }
      set { this["textQuoted"] = value; }
    }

    [ConfigurationProperty("header", DefaultValue = false)]
    public bool HasHeader
    {
      get { return (bool)this["header"]; }
      set { this["header"] = value; }
    }

    [ConfigurationProperty("encoding", IsRequired = false, DefaultValue = "")]
    public string Encoding
    {
      get { return (string)this["encoding"]; }
      set { this["encoding"] = value; }
    }
  }
}