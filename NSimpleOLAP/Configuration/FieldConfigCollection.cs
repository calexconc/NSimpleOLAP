using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// A collection of FieldElement(s).
  /// </summary>
  public sealed class FieldConfigCollection : ConfigurationElementCollection
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
      get { return "Field"; }
    }

    /// <summary>
    /// Retrieve and item in the collection by index.
    /// </summary>
    public FieldConfig this[int index]
    {
      get { return (FieldConfig)BaseGet(index); }
      set
      {
        if (BaseGet(index) != null)
        {
          BaseRemoveAt(index);
        }
        BaseAdd(index, value);
      }
    }

    public new FieldConfig this[string name]
    {
      get { return (FieldConfig)BaseGet(name); }
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
    /// Adds a FieldElement to the configuration file.
    /// </summary>
    /// <param name="element">The FieldElement to add.</param>
    public void Add(FieldConfig element)
    {
      BaseAdd(element);
    }

    /// <summary>
    /// Creates a new FieldElement.
    /// </summary>
    /// <returns>A new <c>FieldElement</c></returns>
    protected override ConfigurationElement CreateNewElement()
    {
      return new FieldConfig();
    }

    /// <summary>
    /// Gets the key of an element based on it's Id.
    /// </summary>
    /// <param name="element">Element to get the key of.</param>
    /// <returns>The key of <c>element</c>.</returns>
    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((FieldConfig)element).Name;
    }

    /// <summary>
    /// Removes a FieldElement with the given name.
    /// </summary>
    /// <param name="name">The name of the FieldElement to remove.</param>
    public void Remove(string name)
    {
      base.BaseRemove(name);
    }
  }
}