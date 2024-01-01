using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  public sealed class CSVConfig
  {
    public string FilePath
    {
      get;
      set;
    }

    public char FieldDelimiter
    {
      get;
      set;
    } = ',';


    public bool TextIsDoubleQuoted
    {
      get;
      set;
    }

    public bool HasHeader
    {
      get;
      set;
    }

    public string Encoding
    {
      get;
      set;
    } = "";
  }
}