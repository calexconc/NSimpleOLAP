using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using NSimpleOLAP.Common.Interfaces;

namespace NSimpleOLAP.Configuration
{
  public sealed class TransformerItemCollectionConfig : IConfigCollection<TransformerItemConfig>
  {
    #region Properties

    /// <summary>
    /// Retrieve and item in the collection by index.
    /// </summary>
    public TransformerItemConfig this[int index]
    {
      get { throw new System.NotImplementedException(); }
      set
      {
        throw new System.NotImplementedException();
      }
    }

    public new TransformerItemConfig this[string name]
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
    /// Adds a FieldElement to the configuration file.
    /// </summary>
    /// <param name="element">The FieldElement to add.</param>
    public void Add(TransformerItemConfig element)
    {
      throw new System.NotImplementedException();
    }

    public void Clear()
    {
      throw new System.NotImplementedException();
    }

    public bool Contains(TransformerItemConfig item)
    {
      throw new System.NotImplementedException();
    }

    public void CopyTo(TransformerItemConfig[] array, int arrayIndex)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerator<TransformerItemConfig> GetEnumerator()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Removes a FieldElement with the given name.
    /// </summary>
    /// <param name="name">The name of the FieldElement to remove.</param>
    public void Remove(string name)
    {
      throw new System.NotImplementedException();
    }

    public bool Remove(TransformerItemConfig item)
    {
      throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new System.NotImplementedException();
    }
  }
}