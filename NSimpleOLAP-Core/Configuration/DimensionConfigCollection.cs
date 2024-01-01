/*
 * Created by SharpDevelop.
 * User: calex
 * Date: 21-03-2012
 * Time: 16:33
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Collections.Generic;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// A collection of DimensionElement(s).
  /// </summary>
  public sealed class DimensionConfigCollection : AbstractConfigCollection<DimensionConfig>
  {
    public DimensionConfigCollection()
    {
      indexer = new Dictionary<string, int>();
      elements = new List<DimensionConfig>();
    }

    /// <summary>
    /// Adds a CubeElement to the configuration file.
    /// </summary>
    /// <param name="element">The CubeElement to add.</param>
    public override void Add(DimensionConfig element)
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

    public override bool Remove(DimensionConfig item)
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