using NSimpleOLAP;
using NSimpleOLAP.Query;
using NSimpleOLAP.Renderers;
using NUnit.Framework;
using System.Linq;

namespace UnitTests
{
  [TestFixture]
  public class QueryExecutionWithTranformerDimensionsTests
  {
    private Cube<int> cube;

    public QueryExecutionWithTranformerDimensionsTests()
    {
      Init();
    }

    public void Init()
    {
      cube = CubeSourcesFixture.GetCubeThreeSimpleDimensionsTwoMeasuresAndThreeDateDimensionsAndTwoExtraLevelsAndTransformerSegmentDimension();
      cube.Initialize();
      cube.Process();
    }

    [OneTimeTearDown]
    public void Dispose()
    {
      cube.Dispose();
    }

    [Test]
    public void Simple_Query_Month_Purchase_Size_On_Measure_Quantity()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("Month.All")
        .OnColumns("purchase_size.All")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      // output for checking, temporary
      result.RenderInConsole();

      Assert.IsTrue(result[0].Length == 4);
      Assert.IsTrue(result.Count == 13);

      
    }

    [Test]
    public void Simple_Query_Purchase_Size_Extra_All_On_Measure_Quantity()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("purchase_size.All")
        .OnColumns("place.Paris.sex.All")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      result.RenderInConsole();

      Assert.IsTrue(result[0].Length == 3);
      Assert.IsTrue(result.Count == 3);
    }

    [Test]
    public void Simple_Query_Purchase_Size_Extra_All_On_Reverse_Measure_Quantity()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("purchase_size.All")
        .OnColumns("sex.All.place.Paris")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      result.RenderInConsole();

      Assert.IsTrue(result[0].Length == 3);
      Assert.IsTrue(result.Count == 3);
    }

    [Test]
    public void Simple_Query_Month_Purchase_Size_Country_Extra_All_Measure_Quantity()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("purchase_size.All.Country.All")
        .OnColumns("Month.January", "Month.February", "Month.March")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      result.RenderInConsole();

      Assert.IsTrue(result[0].Length == 4);
      Assert.IsTrue(result.Count == 6);
    }
  }
}