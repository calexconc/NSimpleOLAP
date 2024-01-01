using NSimpleOLAP.Configuration;

namespace NSimpleOLAP.Data.Interfaces
{
  public interface IDataTransformer
  {
    string Name { get; }

    TransformerItemConfig Config { get; }

    object Transform(object value);
  }
}