using NSimpleOLAP.Query.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Renderers
{
  public static class RenderingExtensions
  {
    public static void RenderInConsole<T>(this IEnumerable<IOutputCell<T>[]> rows)
      where T : struct, IComparable
    {
      var renderer = new ConsoleRenderer<T>();

      renderer.Render(rows);
    }
  }
}