using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// A collection of CubeElement(s).
  /// </summary>
  public sealed class CubeConfigCollection : ConfigurationElementCollection
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
      get { return "Cube"; }
    }

    /// <summary>
    /// Retrieve and item in the collection by index.
    /// </summary>
    public CubeConfig this[int index]
    {
      get { return (CubeConfig)BaseGet(index); }
      set
      {
        if (BaseGet(index) != null)
        {
          BaseRemoveAt(index);
        }
        BaseAdd(index, value);
      }
    }

    public new CubeConfig this[string name]
    {
      get { return (CubeConfig)BaseGet(name); }
      set
      {
        if (BaseGet(name) != null)
        {
          BaseRemove(name);
        }
        BaseAdd(value);
      }
    }

    #endregion Properties

    /// <summary>
    /// Adds a CubeElement to the configuration file.
    /// </summary>
    /// <param name="element">The CubeElement to add.</param>
    public void Add(CubeConfig element)
    {
      BaseAdd(element);
    }

    /// <summary>
    /// Creates a new CubeElement.
    /// </summary>
    /// <returns>A new <c>CubeElement</c></returns>
    protected override ConfigurationElement CreateNewElement()
    {
      return new CubeConfig();
    }

    /// <summary>
    /// Gets the key of an element based on it's Id.
    /// </summary>
    /// <param name="element">Element to get the key of.</param>
    /// <returns>The key of <c>element</c>.</returns>
    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((CubeConfig)element).Name;
    }

    /// <summary>
    /// Removes a CubeElement with the given name.
    /// </summary>
    /// <param name="name">The name of the CubeElement to remove.</param>
    public void Remove(string name)
    {
      base.BaseRemove(name);
    }
  }
}