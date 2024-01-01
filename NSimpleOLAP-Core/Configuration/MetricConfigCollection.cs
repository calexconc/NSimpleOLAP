using System.Collections.Generic;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// A collection of MetricElement(s).
  /// </summary>
  public sealed class MetricConfigCollection : AbstractConfigCollection<MetricConfig>
  {
    public MetricConfigCollection()
    {
      indexer = new Dictionary<string, int>();
      elements = new List<MetricConfig>();
    }

    /// <summary>
    /// Adds a CubeElement to the configuration file.
    /// </summary>
    /// <param name="element">The CubeElement to add.</param>
    public override void Add(MetricConfig element)
    {
      if (indexer.ContainsKey(element.Name))
        throw new System.Exception($"Element {element.Name} already exists in collection.");

      elements.Add(element);
      indexer.Add(element.Name, elements.Count - 1);
    }

    /// <summary>
    /// Removes a CubeElement with the given name.
    /// </summary>
    /// <param name="name">The name of the CubeElement to remove.</param>
    public override void Remove(string name)
    {
      if (!indexer.ContainsKey(name))
        return;

      var index = indexer[name];

      indexer.Remove(name);
      elements.RemoveAt(index);

      for (var i = 0; i < elements.Count; i++)
      {
        var itemName = elements[i].Name;

        indexer[itemName] = i;
      }
    }

    public override bool Remove(MetricConfig item)
    {
      if (!indexer.ContainsKey(item.Name))
        return false;

      var index = indexer[item.Name];

      indexer.Remove(item.Name);
      elements.RemoveAt(index);

      for (var i = 0; i < elements.Count; i++)
      {
        var name = elements[i].Name;

        indexer[name] = i;
      }

      return true;
    }
  }
}