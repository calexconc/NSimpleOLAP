using System.Collections.Generic;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// A collection of SourceMappingsElement(s).
  /// </summary>
  public sealed class SourceMappingsCollection : AbstractConfigCollection<SourceMappingsElement>
  {
    public SourceMappingsCollection()
    {
      indexer = new Dictionary<string, int>();
      elements = new List<SourceMappingsElement>();
    }

    /// <summary>
    /// Adds a CubeElement to the configuration file.
    /// </summary>
    /// <param name="element">The CubeElement to add.</param>
    public override void Add(SourceMappingsElement element)
    {
      elements.Add(element);
    }

    /// <summary>
    /// Removes a CubeElement with the given name.
    /// </summary>
    /// <param name="name">The name of the CubeElement to remove.</param>
    public override void Remove(string name)
    {
    }

    public override bool Remove(SourceMappingsElement item)
    {
      return elements.Remove(item);
    }
  }
}