using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  public sealed class TransformerConfigElement
  {
    public TransformerItemCollectionConfig Transformers
    {
      get;
      set;
    } = new TransformerItemCollectionConfig();
  }
}