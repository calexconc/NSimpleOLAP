using NUnit.Framework;
using System;
using NSimpleOLAP.Data;

namespace UnitTests
{
  [TestFixture]
  public class CubeRefreshDataTests
  {
    [Test]
    public void Cube_Refresh_Data_Only()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();
        cube.Process();

        var cellCount = cube.Cells.Count;

        cube.Refresh();

        Assert.AreEqual(cellCount, cube.Cells.Count);
      }
    }

    [Test]
    public void Cube_Refresh_Data_And_Dimensions()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();
        cube.Process();

        var cellCount = cube.Cells.Count;
        var dim1 = cube.Schema.Dimensions[1].Members.Count;
        var dim2 = cube.Schema.Dimensions[2].Members.Count;
        var dim3 = cube.Schema.Dimensions[3].Members.Count;

        cube.Refresh(true);

        Assert.AreEqual(cellCount, cube.Cells.Count);
        Assert.AreEqual(dim1, cube.Schema.Dimensions[1].Members.Count);
        Assert.AreEqual(dim2, cube.Schema.Dimensions[2].Members.Count);
        Assert.AreEqual(dim3, cube.Schema.Dimensions[3].Members.Count);
      }
    }

    [Test]
    public void Cube_Refresh_Data_And_Dimensions_Verify_When_Member_Added()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();
        cube.Process();

        cube.Schema.Dimensions[1].Members.Add(new NSimpleOLAP.Schema.Member<int>() { ID = cube.Schema.Dimensions[1].Members.Count + 1, Name = "Test" });

        var cellCount = cube.Cells.Count;
        var dim1 = cube.Schema.Dimensions[1].Members.Count;
        var dim2 = cube.Schema.Dimensions[2].Members.Count;
        var dim3 = cube.Schema.Dimensions[3].Members.Count;

        cube.Refresh(true);

        Assert.AreEqual(cellCount, cube.Cells.Count);
        Assert.AreEqual(dim1, cube.Schema.Dimensions[1].Members.Count + 1);
        Assert.AreEqual(dim2, cube.Schema.Dimensions[2].Members.Count);
        Assert.AreEqual(dim3, cube.Schema.Dimensions[3].Members.Count);
      }
    }

    [Test]
    public void Cube_Refresh_Data_With_Add_Row()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();
        cube.Process();

        var cellCountBefore = cube.Cells.Count;

        cube.AppendData("2", "3", "5", "20.00", "4");

        var cellCountAfter = cube.Cells.Count;

        Assert.IsTrue(cellCountAfter > cellCountBefore);

        cube.Refresh();

        Assert.AreEqual(cellCountBefore, cube.Cells.Count);
      }
    }
  }
}