using NSimpleOLAP.Common;
using NSimpleOLAP.Query.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Renderers
{
  internal class ConsoleRenderer<T>
    where T : struct, IComparable
  {
    public void Render(IEnumerable<IOutputCell<T>[]> rows)
    {
      var list = rows.ToList();

      var lengths = CreateHeader(list).ToArray();

      for (var i = 1; i < list.Count; i++)
      {
        CreateRowDataCells(list[i], lengths);
      }
    }

    private IEnumerable<int> CreateHeader(List<IOutputCell<T>[]> rows)
    {
      Console.WriteLine();

      var row = rows[0];

      for (var i = 0; i < row.Length; i++)
      {
        var outTxt = "";
        var initLength = GetMaxColumnLength(i, rows);
        var colLength = initLength + 3 + GetTuplesLength(i, rows);

        if (row[i] == null)
          outTxt = "|" + new String(' ', colLength);
        else if (row[i].Column == null)
          outTxt = "|" + new String(' ', colLength);
        else
          outTxt = "|" + Center(ConcatenateKeyPairs(row[i].Column), colLength);

        Console.Write(outTxt);

        yield return colLength + 1;
      }

      Console.WriteLine();
    }

    private int GetTuplesLength(int index, List<IOutputCell<T>[]> rows)
    {
      var result = 0;

      foreach (var item in rows)
      {
        var cell = item[index];

        if (cell != null)
        {
          if (cell.CellType == OutputCellType.COLUMN_LABEL)
          {
            result = cell.Column.Length;
            break;
          }

          if (cell.CellType == OutputCellType.ROW_LABEL)
          {
            result = cell.Row.Length;
            break;
          }
        }
      }

      return result;
    }

    private void CreateRowLabel(IOutputCell<T> rowLabel, int colLength)
    {
      var outTxt = Center(ConcatenateKeyPairs(rowLabel != null 
        ? rowLabel.Row : new KeyValuePair<string, string>[] { }), colLength);

      Console.Write(outTxt);
    }

    private void CreateRowDataCells(IOutputCell<T>[] row, int[] colLength)
    {
      CreateRowLabel(row[0], colLength[0]);

      for (var i = 1; i < row.Length; i++)
      {
        var outTxt = "";

        if (row[i] == null)
          outTxt = "|" + new String(' ', colLength[i]);
        else
          outTxt = "|" + Center(ConcatenateKeyPairs(row[i] != null
            ? row[i].ToArray() : new KeyValuePair<string, object>[] { }), colLength[i]);

        Console.Write(outTxt);
      }

      Console.WriteLine();
    }

    private int GetMaxColumnLength(int index, List<IOutputCell<T>[]> rows)
    {
      var result = rows
        .Select(x => GetLengthsOfContent(x[index]))
        .Select(x => x.Sum()).Max();

      return result;
    }

    private IEnumerable<int> GetLengthsOfContent(IOutputCell<T> cell)
    {
      if (cell == null)
        yield return 0;
      else
      {
        switch (cell.CellType)
        {
          case OutputCellType.COLUMN_LABEL:
            foreach (var value in cell.Column)
            {
              yield return value.Key.Length;
              yield return value.Value.Length;
            }
            break;

          case OutputCellType.ROW_LABEL:
            foreach (var value in cell.Row)
            {
              yield return value.Key.Length;
              yield return value.Value.Length;
            }
            break;

          case OutputCellType.DATA:
            foreach (var value in cell)
            {
              yield return value.Value.ToString().Length;
            }
            break;
        }
      }
    }

    private string ConcatenateKeyPairs(KeyValuePair<string, string>[] pairs)
    {
      var query = pairs.Select(x => x.Key + " " + x.Value);

      return string.Join(",", query);
    }

    private string ConcatenateKeyPairs(KeyValuePair<string, object>[] pairs)
    {
      var query = pairs.Select(x => x.Value.ToString());

      return string.Join(",", query);
    }

    private string Center(string txt, int length)
    {
      return txt.PadLeft(((length - txt.Length) / 2) + txt.Length).PadRight(length);
    }
  }
}