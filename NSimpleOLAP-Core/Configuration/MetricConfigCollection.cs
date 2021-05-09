using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using NSimpleOLAP.Common.Interfaces;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// A collection of MetricElement(s).
  /// </summary>
  public sealed class MetricConfigCollection : IConfigCollection<MetricConfig>
  {
    #region Properties

    /// <summary>
    /// Retrieve and item in the collection by index.
    /// </summary>
    public MetricConfig this[int index]
    {
      get { throw new System.NotImplementedException(); }
      set
      {
        throw new System.NotImplementedException();
      }
    }

    public new MetricConfig this[string name]
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
    /// Adds a MetricElement to the configuration file.
    /// </summary>
    /// <param name="element">The MetricElement to add.</param>
    public void Add(MetricConfig element)
    {
      throw new System.NotImplementedException();
    }

    public void Clear()
    {
      throw new System.NotImplementedException();
    }

    public bool Contains(MetricConfig item)
    {
      throw new System.NotImplementedException();
    }

    public void CopyTo(MetricConfig[] array, int arrayIndex)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerator<MetricConfig> GetEnumerator()
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Removes a MetricElement with the given name.
    /// </summary>
    /// <param name="name">The name of the MetricElement to remove.</param>
    public void Remove(string name)
    {
      throw new System.NotImplementedException();
    }

    public bool Remove(MetricConfig item)
    {
      throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new System.NotImplementedException();
    }
  }
}