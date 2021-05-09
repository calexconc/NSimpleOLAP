/*
 * Created by SharpDevelop.
 * User: calex
 * Date: 21-03-2012
 * Time: 16:33
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using NSimpleOLAP.Common.Interfaces;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// A collection of DimensionElement(s).
  /// </summary>
  public sealed class DimensionConfigCollection : IConfigCollection<DimensionConfig>
  {
    #region Properties

    
    public new DimensionConfig this[string name]
    {
      get { throw new System.NotImplementedException(); }
      set
      {
        throw new System.NotImplementedException();
      }
    }

    /// <summary>
    /// Retrieve and item in the collection by index.
    /// </summary>
    public DimensionConfig this[int index]
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
    /// Adds a DimensionElement to the configuration file.
    /// </summary>
    /// <param name="element">The DimensionElement to add.</param>
    public void Add(DimensionConfig element)
    {
      throw new System.NotImplementedException();
    }

    public void Clear()
    {
      throw new System.NotImplementedException();
    }

    public bool Contains(DimensionConfig item)
    {
      throw new System.NotImplementedException();
    }

    public void CopyTo(DimensionConfig[] array, int arrayIndex)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerator<DimensionConfig> GetEnumerator()
    {
      throw new System.NotImplementedException();
    }


    /// <summary>
    /// Removes a DimensionElement with the given name.
    /// </summary>
    /// <param name="name">The name of the DimensionElement to remove.</param>
    public void Remove(string name)
    {
      throw new System.NotImplementedException();
    }

    public bool Remove(DimensionConfig item)
    {
      throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new System.NotImplementedException();
    }
  }
}