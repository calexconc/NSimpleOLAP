using NSimpleOLAP;
using NSimpleOLAP.Query;
using NSimpleOLAP.Query.Builder;
using NUnit.Framework;
using System.Linq;
using NSimpleOLAP.Renderers;

namespace UnitTests
{
  [TestFixture]
  public class QueryExecutionTests
  {
    private Cube<int> cube;

    public QueryExecutionTests()
    {
      Init();
    }

    public void Init()
    {
      cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2();
      cube.Initialize();
      cube.Process();
    }

    [OneTimeTearDown]
    public void Dispose()
    {
      cube.Dispose();
    }

    [Test]
    public void Query_StreamCells_With_Single_Cell_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("sex.female")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamCells().ToList();

      Assert.IsTrue(result.Count == 1);
    }

    [Test]
    public void Query_StreamCells_With_2_Cells_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("sex.female", "sex.male")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamCells().ToList();

      Assert.IsTrue(result.Count == 2);
    }

    [Test]
    public void Query_StreamCells_With_All_Cells_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("sex.All")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamCells().ToList();

      Assert.IsTrue(result.Count == 3);
    }

    [Test]
    public void Query_StreamCells_With_All_In_Rows_With_Extra_Dims_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("category.All.place.Paris")
        .OnColumns("sex.male")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamCells().ToList();

      Assert.IsTrue(result.Count == 2);
    }

    [Test]
    public void Query_StreamCells_With_All_In_Rows_And_Cols_With_Extra_Dims_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("category.All.place.Paris")
        .OnColumns("sex.All")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamCells().ToList();

      Assert.IsTrue(result.Count == 5);
    }

    [Test]
    public void Query_StreamCells_With_All_In_Rows_And_Cols_With_Extra_All_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("category.All.place.All")
        .OnColumns("sex.All")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamCells().ToList();

      Assert.IsTrue(result.Count == 19);
    }

    [Test]
    public void Query_StreamRows_With_All_In_Rows_And_Cols_With_Extra_Dims_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("category.All.place.Paris")
        .OnColumns("sex.All")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 4);

      result.RenderInConsole();
    }

    [Test]
    public void Query_StreamRows_With_All_In_Rows_With_Extra_Dims_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("category.All.place.Paris")
        .OnColumns("sex.male")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 3);
    }

    [Test]
    public void Query_StreamRows_With_All_Cells_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("sex.All")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 4);
    }

    [Test]
    public void Query_StreamRows_With_All_In_Rows_And_Cols_With_Extra_All_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("category.All.place.All")
        .OnColumns("sex.All")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 16);
    }

    [Test]
    public void Query_StreamRows_With_All_Just_Rows_Cells_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("sex.All")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 4);
    }

    [Test]
    public void Query_StreamRows_With_All_Just_Columns_Cells_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnColumns("sex.All")
        .AddMeasuresOrMetrics("quantity");

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 2);
    }

    [Test]
    public void Query_StreamRows_With_All_In_Rows_And_Cols_With_Extra_All_With_Where_Clause_On_Dimension_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("category.All.place.All")
        .OnColumns("sex.All")
        .AddMeasuresOrMetrics("quantity")
        .Where(b => b.Define(x => x.Dimension("sex").IsEquals("male")));

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 8);
      Assert.IsTrue(result[0].Length == 2);

      result.RenderInConsole();
    }

    [Test]
    public void Query_StreamRows_With_All_In_Rows_And_Cols_With_Extra_All_With_Where_Clause_On_Dimension_Not_Equals_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("category.All.place.All")
        .OnColumns("sex.All")
        .AddMeasuresOrMetrics("quantity")
        .Where(b => b.Define(x => x.Dimension("sex").NotEquals("male")));

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 13);
      Assert.IsTrue(result[0].Length == 3);

      result.RenderInConsole();
    }

    [Test]
    public void Query_StreamRows_With_All_In_Rows_And_Cols_With_Extra_All_With_Where_Clause_On_Measure_Quantity_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("category.All.place.All")
        .OnColumns("sex.All")
        .AddMeasuresOrMetrics("quantity")
        .Where(b => b.Define(x => x.Measure("quantity").IsEquals(5)));

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 4);
    }

    [Test]
    public void Query_StreamRows_With_All_In_Rows_And_Cols_With_Extra_All_With_Where_Clause_And_Measure_Quantity_And_NotEquals_Clothes_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("category.All.place.All")
        .OnColumns("sex.All")
        .AddMeasuresOrMetrics("quantity")
        .Where(b => b.Define(a => a.And(x => x.Dimension("category").NotEquals("clothes"),
        x => x.Measure("quantity").GreaterOrEquals(2))));

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 12);
    }

    [Test]
    public void Query_StreamRows_With_All_In_Rows_And_Cols_With_Extra_All_With_Where_Clause_Or_Dimension_Category_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("category.All.place.All")
        .OnColumns("sex.All")
        .AddMeasuresOrMetrics("quantity")
        .Where(b => b.Define(a => a.Or(x => x.Dimension("category").IsEquals("clothes"),
        x => x.Dimension("category").IsEquals("shoes"))));

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 8);
    }

    [Test]
    public void Query_StreamRows_With_All_In_Rows_And_Cols_With_Extra_All_With_Where_Clause_Or_Dimension_Category_Clothes_WITH_Toys_With_Quant_Greater_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("category.All.place.All")
        .OnColumns("sex.All")
        .AddMeasuresOrMetrics("quantity")
        .Where(b => b.Define(a => a.Or(x => x.Dimension("category").IsEquals("clothes"),
          x => x.Block(x1 => 
            x1.And(x2 => x2.Dimension("category").IsEquals("toys"), 
            x2 => x2.Measure("quantity").GreaterThan(5)))
          )
         )
        );

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 7);
    }

    [Test]
    public void Query_StreamRows_With_All_In_Rows_And_Cols_With_Extra_All_With_Where_Clause_Or_Dimension_Category_Clothes_WITH_Not_Toys_With_Quant_Greater_Test()
    {
      var queryBuilder = cube.BuildQuery()
        .OnRows("category.All.place.All")
        .OnColumns("sex.All")
        .AddMeasuresOrMetrics("quantity")
        .Where(b => b.Define(a => a.Or(x => x.Dimension("category").IsEquals("clothes"),
          x => x.Not(x1 =>
            x1.And(x2 => x2.Dimension("category").IsEquals("toys"),
            x2 => x2.Measure("quantity").GreaterThan(5)))
          )
         )
        );

      var query = queryBuilder.Create();
      var result = query.StreamRows().ToList();

      Assert.IsTrue(result.Count == 13);
    }
  }
}