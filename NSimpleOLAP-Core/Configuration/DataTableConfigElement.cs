using System.Configuration;
using System.Data;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Represents a single XML tag inside a ConfigurationSection
  /// or a ConfigurationElementCollection.
  /// </summary>
  internal sealed class DataTableConfig
  {
    /// <summary>
    /// The attribute <c>name</c> of a <c>DataTableConfigElement</c>.
    /// </summary>
    internal DataTable Table
    {
      get;
      set;
    }
  }
}