using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExampleConsoleApp
{
  class SimpleInputParser
  {
    public QuerySettings Parse(string text)
    {
      var result = new QuerySettings();

      GetRows(result, text);
      GetColumns(result, text);
      GetMeasures(result, text);

      return result;
    }

    private void GetRows(QuerySettings query, string text)
    {
      if (text.Contains("rows:"))
      {
        if (HasDuplicate("rows:", text))
        {
          query.Error = true;
          query.ErrorMessages.Add("Error duplicate rows keyword entries.");
          return;
        }

        var regex = new Regex("rows:(.*?)(columns|measures)");
        var regex2 = new Regex("rows:.*");
        var result = regex.Match(text).Value;

        if (string.IsNullOrEmpty(result))
          result = regex2.Match(text).Value;

        query.RowTupples.AddRange(GetValues(result));
      }
    }

    private void GetColumns(QuerySettings query, string text)
    {
      if (text.Contains("columns:"))
      {
        if (HasDuplicate("columns:", text))
        {
          query.Error = true;
          query.ErrorMessages.Add("Error duplicate columns keyword entries.");
          return;
        }

        var regex = new Regex("columns:(.*?)(rows|measures)");
        var result = regex.Match(text).Value;
        var regex2 = new Regex("columns:.*");

        if (string.IsNullOrEmpty(result))
          result = regex2.Match(text).Value;

        query.ColumnTupples.AddRange(GetValues(result));
      }
    }

    private void GetMeasures(QuerySettings query, string text)
    {
      if (text.Contains("measures:"))
      {
        if (HasDuplicate("measures:", text))
        {
          query.Error = true;
          query.ErrorMessages.Add("Error duplicate measures keyword entries.");
          return;
        }

        var regex = new Regex("measures:(.*?)(rows|columns)");
        var result = regex.Match(text).Value;
        var regex2 = new Regex("measures:.*");

        if (string.IsNullOrEmpty(result))
          result = regex2.Match(text).Value;

        query.Measures.AddRange(GetValues(result));
      }
    }

    private string[] GetValues(string text)
    {
      return text
        .Replace(":","")
        .Replace("columns", "")
        .Replace("measures", "")
        .Replace("rows", "")
        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.Trim())
        .ToArray();
    }
    private bool HasDuplicate(string word, string text)
    {
      var results = text
        .Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Where(x => x.Contains(word))
        .Count();

      return results > 1;
    }
  }
}
