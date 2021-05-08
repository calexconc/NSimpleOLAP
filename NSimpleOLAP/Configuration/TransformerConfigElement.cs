using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  public sealed class TransformerConfigElement : ConfigurationElement
  {
    [ConfigurationProperty("Transformers")]
    public TransformerItemCollectionConfig Transformers
    {
      get { return (TransformerItemCollectionConfig)this["Transformers"]; }
      set { this["Transformers"] = value; }
    }
  }
}