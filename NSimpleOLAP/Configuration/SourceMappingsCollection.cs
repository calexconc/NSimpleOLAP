using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// A collection of SourceMappingsElement(s).
  /// </summary>
  public sealed class SourceMappingsCollection : ConfigurationElementCollection
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
      get { return "SourceMappings"; }
    }

    /// <summary>
    /// Retrieve and item in the collection by index.
    /// </summary>
    public SourceMappingsElement this[int index]
    {
      get { return (SourceMappingsElement)BaseGet(index); }
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
    /// Adds a SourceMappingsElement to the configuration file.
    /// </summary>
    /// <param name="element">The SourceMappingsElement to add.</param>
    public void Add(SourceMappingsElement element)
    {
      BaseAdd(element);
    }

    /// <summary>
    /// Creates a new SourceMappingsElement.
    /// </summary>
    /// <returns>A new <c>SourceMappingsElement</c></returns>
    protected override ConfigurationElement CreateNewElement()
    {
      return new SourceMappingsElement();
    }

    /// <summary>
    /// Gets the key of an element based on it's Id.
    /// </summary>
    /// <param name="element">Element to get the key of.</param>
    /// <returns>The key of <c>element</c>.</returns>
    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((SourceMappingsElement)element).Field;
    }

    /// <summary>
    /// Removes a SourceMappingsElement with the given name.
    /// </summary>
    /// <param name="name">The name of the SourceMappingsElement to remove.</param>
    public void Remove(string name)
    {
      base.BaseRemove(name);
    }
  }
}