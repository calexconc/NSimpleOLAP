using NSimpleOLAP;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnitTests
{
  [TestFixture]
  public class ReadDataSourceTests
  {
    private Stopwatch _watch;

    public ReadDataSourceTests()
    {
      Init();
    }

    public void Init()
    {
      _watch = new Stopwatch();
    }

    [Test]
    public void DimensionMembersTest()
    {
      KeyValuePair<int, int>[] pairs = new KeyValuePair<int, int>[] {
        new KeyValuePair<int, int>(1,2),
        new KeyValuePair<int, int>(2,1),
        new KeyValuePair<int, int>(3,6) };
      KeyValuePair<int, int>[] pairs2 = new KeyValuePair<int, int>[] {
        new KeyValuePair<int, int>(2,1),
        new KeyValuePair<int, int>(3,6) };
      Cube<int> cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures();

      _watch.Reset();
      _watch.Start();

      cube.Initialize();
      cube.Process();

      Cell<int> xcell = cube.Cells[pairs];
      Cell<int> xcell2 = cube.Cells[pairs2];

      _watch.Stop();
      Console.WriteLine();
      Console.WriteLine(_watch.ElapsedMilliseconds);
      Console.WriteLine();

      Assert.AreEqual("male", cube.Schema.Dimensions["sex"].Members["male"].Name);
      Assert.AreEqual("female", cube.Schema.Dimensions["sex"].Members["female"].Name);
      Assert.AreEqual("London", cube.Schema.Dimensions["place"].Members["London"].Name);
      Assert.AreEqual(5, xcell.Values[cube.Schema.Measures["quantity"].ID]);
      Assert.AreEqual(10.10, xcell.Values[cube.Schema.Measures["spent"].ID]);
      Assert.AreEqual(3, xcell2.Occurrences);
    }

    [Test]
    public void CellsEnumeratorTest()
    {
      Cube<int> cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures();

      _watch.Reset();
      _watch.Start();

      cube.Initialize();
      cube.Process();
      // 12, 7
      Console.WriteLine();

      foreach (Cell<int> item in cube.Cells)
      {
        foreach (var pair in item.Coords)
        {
          Console.Write(pair.Key);
          Console.Write(",");
          Console.Write(pair.Value);
          Console.Write("|");
        }
        Console.WriteLine();
      }

      int count = cube.Cells.Count;

      _watch.Stop();
      Console.WriteLine();
      Console.WriteLine(_watch.ElapsedMilliseconds);
      Console.WriteLine();
    }
  }
}