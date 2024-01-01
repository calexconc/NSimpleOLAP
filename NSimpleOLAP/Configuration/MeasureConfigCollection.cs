using System.Configuration;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// A collection of MeasureElement(s).
  /// </summary>
  public sealed class MeasureConfigCollection : ConfigurationElementCollection
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
      get { return "Measure"; }
    }

    /// <summary>
    /// Retrieve and item in the collection by index.
    /// </summary>
    public MeasureConfig this[int index]
    {
      get { return (MeasureConfig)BaseGet(index); }
      set
      {
        if (BaseGet(index) != null)
        {
          BaseRemoveAt(index);
        }
        BaseAdd(index, value);
      }
    }

    public new MeasureConfig this[string name]
    {
      get { return (MeasureConfig)BaseGet(name); }
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
    /// Adds a MeasureElement to the configuration file.
    /// </summary>
    /// <param name="element">The MeasureElement to add.</param>
    public void Add(MeasureConfig element)
    {
      BaseAdd(element);
    }

    /// <summary>
    /// Creates a new MeasureElement.
    /// </summary>
    /// <returns>A new <c>MeasureElement</c></returns>
    protected override ConfigurationElement CreateNewElement()
    {
      //ConfigurationProperty dd = new ConfigurationProperty(
      return new MeasureConfig();
    }

    /// <summary>
    /// Gets the key of an element based on it's Id.
    /// </summary>
    /// <param name="element">Element to get the key of.</param>
    /// <returns>The key of <c>element</c>.</returns>
    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((MeasureConfig)element).Name;
    }

    /// <summary>
    /// Removes a MeasureElement with the given name.
    /// </summary>
    /// <param name="name">The name of the MeasureElement to remove.</param>
    public void Remove(string name)
    {
      base.BaseRemove(name);
    }
  }
}