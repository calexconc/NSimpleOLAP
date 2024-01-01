using NSimpleOLAP;
using NSimpleOLAP.Query;
using NSimpleOLAP.Renderers;
using NUnit.Framework;
using System.Linq;

namespace UnitTests
{
  [TestFixture]
  public class QueryExecutionWithTimeDimensionsTests
  {
    private Cube<int> cube;

    public QueryExecutionWithTimeDimensionsTests()
    {
      Init();
    }

    public void Init()
    {
      cube = CubeSourcesFixture.GetBasicCubeThreeSimpleDimensionsTwoMeasuresAndThreeTimeDimensions();
      cube.Initialize();
      cube.Process();
    }

    [OneTimeTearDown]
    public void Dispose()
    {
      cube.Dispose();
    }

    [Test]
    public void Simple_Query_Hour_Minute_On_Measure_Quantity()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("Hour.All")
        .OnColumns("Minute.All")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      // output for checking, temporary
      result.RenderInConsole();

      Assert.IsTrue(result[0].Length == 13);
      Assert.IsTrue(result.Count == 18);
    }

    [Test]
    public void Simple_Query_Hour_Extra_All_On_Measure_Quantity()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("Hour.All")
        .OnColumns("place.Paris.sex.All")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      result.RenderInConsole();

      Assert.IsTrue(result[0].Length == 3);
      Assert.IsTrue(result.Count == 6);
    }

    [Test]
    public void Simple_Query_Hour_Extra_All_On_Reverse_Measure_Quantity()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("Hour.All")
        .OnColumns("sex.All.place.Paris")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      result.RenderInConsole();

      Assert.IsTrue(result[0].Length == 3);
      Assert.IsTrue(result.Count == 6);
    }

    [Test]
    public void Simple_Query_Hour_Minute_Extra_All_Measure_Quantity()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("sex.All.Minute.All")
        .OnColumns("Hour.01", "Hour.03", "Hour.05")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      result.RenderInConsole();

      Assert.IsTrue(result[0].Length == 2);
      Assert.IsTrue(result.Count == 2);
    }
  }
}