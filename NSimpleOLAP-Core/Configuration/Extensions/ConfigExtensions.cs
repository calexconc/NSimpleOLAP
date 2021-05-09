using System.Collections.Generic;

namespace NSimpleOLAP.Configuration.Extensions
{
  /// <summary>
  /// Description of ConfigExtensions.
  /// </summary>
  internal static class ConfigExtensions
  {
    public static Dictionary<string, int> GetFieldIndexes(this FieldConfigCollection fields)
    {
      Dictionary<string, int> dict = new Dictionary<string, int>();

      for (int i = 0; i < fields.Count; i++)
        dict.Add(fields[i].Name, i);

      return dict;
    }
  }
}