using System.Data;

namespace NSimpleOLAP.Configuration.Fluent
{
  /// <summary>
  /// Description of DataTableConfigBuilder.
  /// </summary>
  public class DataTableConfigBuilder
  {
    private DataTableConfig _element;

    public DataTableConfigBuilder()
    {
      _element = new DataTableConfig();
    }

    #region public methods

    public DataTableConfigBuilder SetDataTable(DataTable table)
    {
      _element.Table = table;
      return this;
    }

    internal DataTableConfig Create()
    {
      return _element;
    }

    #endregion public methods
  }
}