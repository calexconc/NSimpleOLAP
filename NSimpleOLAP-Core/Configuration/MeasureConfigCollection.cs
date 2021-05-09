using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using NSimpleOLAP.Common.Interfaces;


namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// A collection of MeasureElement(s).
  /// </summary>
  public sealed class MeasureConfigCollection : IConfigCollection<MeasureConfig>
  {
    #region Properties

    /// <summary>
    /// Retrieve and item in the collection by index.
    /// </summary>
    public MeasureConfig this[int index]
    {
      get { throw new System.NotImplementedException(); }
      set
      {
        throw new System.NotImplementedException();
      }
    }

    public new MeasureConfig this[string name]
    {
      get { throw new System.NotImplementedException(); }
      set
      {
        throw new System.NotImplementedException();
      }
    }

    public int Count => throw new System.NotImplementedException();

    public bool IsReadOnly => throw new System.NotImplementedException();

    #endregion Properties

    /// <summary>
    /// Adds a MeasureElement to the configuration file.
    /// </summary>
    /// <param name="element">The MeasureElement to add.</param>
    public void Add(MeasureConfig element)
    {
      throw new System.NotImplementedException();
    }

    public void Clear()
    {
      throw new System.NotImplementedException();
    }

    public bool Contains(MeasureConfig item)
    {
      throw new System.NotImplementedException();
    }

    public void CopyTo(MeasureConfig[] array, int arrayIndex)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerator<MeasureConfig> GetEnumerator()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Removes a MeasureElement with the given name.
    /// </summary>
    /// <param name="name">The name of the MeasureElement to remove.</param>
    public void Remove(string name)
    {
      throw new System.NotImplementedException();
    }

    public bool Remove(MeasureConfig item)
    {
      throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new System.NotImplementedException();
    }
  }
}