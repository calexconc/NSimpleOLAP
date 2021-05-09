using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using NSimpleOLAP.Common.Interfaces;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// A collection of SourceMappingsElement(s).
  /// </summary>
  public sealed class SourceMappingsCollection : IConfigCollection<SourceMappingsElement>
  {
    #region Properties

    /// <summary>
    /// Retrieve and item in the collection by index.
    /// </summary>
    public SourceMappingsElement this[int index]
    {
      get { throw new System.NotImplementedException(); }
      set
      {
        throw new System.NotImplementedException();
      }
    }

    public SourceMappingsElement this[string name] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public int Count => throw new System.NotImplementedException();

    public bool IsReadOnly => throw new System.NotImplementedException();

    #endregion Properties

    /// <summary>
    /// Adds a SourceMappingsElement to the configuration file.
    /// </summary>
    /// <param name="element">The SourceMappingsElement to add.</param>
    public void Add(SourceMappingsElement element)
    {
      throw new System.NotImplementedException();
    }

    public void Clear()
    {
      throw new System.NotImplementedException();
    }

    public bool Contains(SourceMappingsElement item)
    {
      throw new System.NotImplementedException();
    }

    public void CopyTo(SourceMappingsElement[] array, int arrayIndex)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerator<SourceMappingsElement> GetEnumerator()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Removes a SourceMappingsElement with the given name.
    /// </summary>
    /// <param name="name">The name of the SourceMappingsElement to remove.</param>
    public void Remove(string name)
    {
      throw new System.NotImplementedException();
    }

    public bool Remove(SourceMappingsElement item)
    {
      throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new System.NotImplementedException();
    }
  }
}