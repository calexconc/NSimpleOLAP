using System.Collections.Generic;

namespace ExampleConsoleApp
{
  internal class QuerySettings
  {
    public List<string> RowTupples { get; set; } = new List<string>();

    public List<string> ColumnTupples { get; set; } = new List<string>();

    public List<string> Measures { get; set; } = new List<string>();

    public bool Error { get; set; }

    public List<string> ErrorMessages { get; set; } = new List<string>();
  }
}