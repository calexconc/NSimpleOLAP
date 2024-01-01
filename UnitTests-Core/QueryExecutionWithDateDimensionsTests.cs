using NSimpleOLAP;
using NSimpleOLAP.Query;
using NSimpleOLAP.Renderers;
using NUnit.Framework;
using System.Linq;

namespace UnitTests
{
  [TestFixture]
  public class QueryExecutionWithDateDimensionsTests
  {
    private Cube<int> cube;

    public QueryExecutionWithDateDimensionsTests()
    {
      Init();
    }

    public void Init()
    {
      cube = CubeSourcesFixture.GetBasicCubeThreeSimpleDimensionsTwoMeasuresAndThreeDateDimensions();
      cube.Initialize();
      cube.Process();
    }

    [OneTimeTearDown]
    public void Dispose()
    {
      cube.Dispose();
    }

    [Test]
    public void Simple_Query_Month_Year_On_Measure_Quantity()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("Month.All")
        .OnColumns("Year.All")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result[0].Length == 2);
      Assert.IsTrue(result.Count == 13);

      // output for checking, temporary
      result.RenderInConsole();
    }

    [Test]
    public void Simple_Query_Month_Extra_All_On_Measure_Quantity()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("Month.All")
        .OnColumns("place.Paris.sex.All")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      result.RenderInConsole();

      Assert.IsTrue(result[0].Length == 3);
      Assert.IsTrue(result.Count == 5);
    }

    [Test]
    public void Simple_Query_Month_Extra_All_On_Reverse_Measure_Quantity()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("Month.All")
        .OnColumns("sex.All.place.Paris")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      result.RenderInConsole();

      Assert.IsTrue(result[0].Length == 3);
      Assert.IsTrue(result.Count == 5);
    }

    [Test]
    public void Simple_Query_Month_Day_Extra_All_Measure_Quantity()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("sex.All.Day.All")
        .OnColumns("Month.January", "Month.February", "Month.March")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      result.RenderInConsole();

      Assert.IsTrue(result[0].Length == 4);
      Assert.IsTrue(result.Count == 8);
    }
  }
}