/*
 * Created by SharpDevelop.
 * User: calex
 * Date: 21-03-2012
 * Time: 16:33
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// A collection of DimensionElement(s).
  /// </summary>
  public sealed class DimensionConfigCollection : ConfigurationElementCollection
  {
    #region Properties

    /// <summary>
    /// Gets the CollectionType of the ConfigurationElementCollection.
    /// </summary>
    public override ConfigurationElementCollectionType CollectionType
    {
      get { return ConfigurationElementCollectionType.BasicMap; }
    }

    /// <summary>
    /// Gets the Name of Elements of the collection.
    /// </summary>
    protected override string ElementName
    {
      get { return "DimensionElementCollection"; }
    }

    public new DimensionConfig this[string name]
    {
      get { return (DimensionConfig)BaseGet(name); }
      set
      {
        if (BaseGet(name) != null)
        {
          BaseRemove(name);
        }
        BaseAdd(value);
      }
    }

    /// <summary>
    /// Retrieve and item in the collection by index.
    /// </summary>
    public DimensionConfig this[int index]
    {
      get { return (DimensionConfig)BaseGet(index); }
      set
      {
        if (BaseGet(index) != null)
        {
          BaseRemoveAt(index);
        }
        BaseAdd(index, value);
      }
    }

    #endregion Properties

    /// <summary>
    /// Adds a DimensionElement to the configuration file.
    /// </summary>
    /// <param name="element">The DimensionElement to add.</param>
    public void Add(DimensionConfig element)
    {
      BaseAdd(element);
    }

    /// <summary>
    /// Creates a new DimensionElement.
    /// </summary>
    /// <returns>A new <c>DimensionElement</c></returns>
    protected override ConfigurationElement CreateNewElement()
    {
      return new DimensionConfig();
    }

    /// <summary>
    /// Gets the key of an element based on it's Id.
    /// </summary>
    /// <param name="element">Element to get the key of.</param>
    /// <returns>The key of <c>element</c>.</returns>
    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((DimensionConfig)element).Name;
    }

    /// <summary>
    /// Removes a DimensionElement with the given name.
    /// </summary>
    /// <param name="name">The name of the DimensionElement to remove.</param>
    public void Remove(string name)
    {
      base.BaseRemove(name);
    }
  }
}