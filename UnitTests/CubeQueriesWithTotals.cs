using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Data;
using NUnit.Framework;
using NSimpleOLAP;
using NSimpleOLAP.Query;
using NSimpleOLAP.Query.Builder;
using NSimpleOLAP.Renderers;

namespace UnitTests
{
  [TestFixture]
  public class CubeQueriesWithTotals
  {
    private Cube<int> cube;

    public CubeQueriesWithTotals()
    {
      Init();
    }

    public void Init()
    {
      cube = CubeSourcesFixture.GetCubeThreeSimpleDimensionsTwoMeasuresAndThreeDateDimensionsAndTwoExtraLevels();
      cube.Initialize();
      cube.Process();
    }

    [OneTimeTearDown]
    public void Dispose()
    {
      cube.Dispose();
    }

    [Test]
    public void Query_StreamRows_With_RowTotals_Cell_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("sex.All")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity")
        .GetRowTotals();

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 4);
      Assert.AreEqual(result[1][1][0], result[1][2][0]);
    }

    [Test]
    public void Query_StreamRows_With_ColumnTotals_Cell_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("sex.All")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity")
        .GetColumnTotals();

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 5);
      var values = result.Skip(1).Select(x => (KeyValuePair<string, object>) x[1][0]).ToArray();
      Assert.AreEqual((int)values[0].Value + (int)values[1].Value + (int)values[2].Value, (int)values[3].Value);
    }

    [Test]
    public void Query_StreamRows_With_ColumnTotals_And_RowTotals_Cell_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("sex.All")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity")
        .GetColumnTotals()
        .GetRowTotals();

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 5);
      var values = result.Skip(1).Select(x => (KeyValuePair<string, object>)x[1][0]).ToArray();
      Assert.AreEqual((int)values[0].Value + (int)values[1].Value + (int)values[2].Value, (int)values[3].Value);
      Assert.AreEqual(result[1][1][0], result[1][2][0]);
      Assert.AreEqual(result[4][1][0], result[4][2][0]);

      result.RenderInConsole();
    }


    [Test]
    public void Query_StreamRows_With_RowBaseTotals_Cell_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("sex.All")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity")
        .GetBaseRowTotals();

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      var valuesCol1 = result.Skip(1).Select(x => (KeyValuePair<string, object>)x[1][0]).ToArray();
      var valuesCol2 = result.Skip(1).Select(x => (KeyValuePair<string, object>)x[2][0]).ToArray();
      Assert.IsTrue(result.Count == 4);
      Assert.IsTrue((int)valuesCol1[0].Value <= (int)valuesCol2[0].Value);
    }

    [Test]
    public void Query_StreamRows_With_ColumnBaseTotals_Cell_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("sex.All")
        .OnColumns("category.All")
        .AddMeasuresOrMetrics("quantity")
        .GetColumnTotals()
        .GetBaseColumnTotals();

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      result.RenderInConsole();

      var valuesRow1 = result[4].Skip(1).Select(x => (KeyValuePair<string, object>)x[0]).Select(x => (int) x.Value).ToArray();
      var valuesRow2 = result[5].Skip(1).Select(x => (KeyValuePair<string, object>)x[0]).Select(x => (int)x.Value).ToArray();
      Assert.IsTrue(result.Count == 6);
      CollectionAssert.AreEqual(valuesRow1, valuesRow2);
    }
  }
}
