using System.Configuration;
using System.Collections.Generic;
using NSimpleOLAP.Common.Interfaces;
using System.Collections;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// A collection of CubeElement(s).
  /// </summary>
  public sealed class CubeConfigCollection : IConfigCollection<CubeConfig>
  {
    #region Properties

    
    /// <summary>
    /// Retrieve and item in the collection by index.
    /// </summary>
    public CubeConfig this[int index]
    {
      get { throw new System.NotImplementedException(); }
      set
      {
        throw new System.NotImplementedException();
      }
    }

    public new CubeConfig this[string name]
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
    /// Adds a CubeElement to the configuration file.
    /// </summary>
    /// <param name="element">The CubeElement to add.</param>
    public void Add(CubeConfig element)
    {
      throw new System.NotImplementedException();
    }

    public void Clear()
    {
      throw new System.NotImplementedException();
    }

    public bool Contains(CubeConfig item)
    {
      throw new System.NotImplementedException();
    }

    public void CopyTo(CubeConfig[] array, int arrayIndex)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerator<CubeConfig> GetEnumerator()
    {
      throw new System.NotImplementedException();
    }


    /// <summary>
    /// Removes a CubeElement with the given name.
    /// </summary>
    /// <param name="name">The name of the CubeElement to remove.</param>
    public void Remove(string name)
    {
      throw new System.NotImplementedException();
    }

    public bool Remove(CubeConfig item)
    {
      throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new System.NotImplementedException();
    }
  }
}