using NSimpleOLAP;
using NSimpleOLAP.Query;
using NSimpleOLAP.Query.Builder;
using NUnit.Framework;
using NSimpleOLAP.Common.Utils;

namespace UnitTests
{
  [TestFixture]
  public class QueryBuildTests
  {
    private Cube<int> cube;
    private NamespaceResolver<int> _resolver;

    public QueryBuildTests()
    {
      Init();
    }

    public void Init()
    {
      cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2();
      cube.Initialize();
      cube.Process();

      _resolver = new NamespaceResolver<int>(cube);
    }

    [OneTimeTearDown]
    public void Dispose()
    {
      cube.Dispose();
    }

    [Test]
    public void WhereBuilder_Setup_Test()
    {
      Assert.DoesNotThrow(() =>
      {
        WhereBuilder<int> builder = new WhereBuilder<int>(_resolver);
      });
    }

    [Test]
    public void WhereBuilder_Add_MeasureSlicer_Test()
    {
      WhereBuilder<int> builder = new WhereBuilder<int>(_resolver);

      builder.Define(b => b.Measure("quantity").GreaterOrEquals(2));

      var predicate = builder.Build();

      Assert.IsNotNull(predicate);
      Assert.IsFalse(predicate.FiltersOnAggregation());
    }

    [Test]
    public void WhereBuilder_Add_DimensionSlicer_Test()
    {
      WhereBuilder<int> builder = new WhereBuilder<int>(_resolver);

      builder.Define(b => b.Dimension("category").IsEquals("clothes"));

      var predicate = builder.Build();

      Assert.IsNotNull(predicate);
      Assert.IsTrue(predicate.FiltersOnAggregation());
    }

    [Test]
    public void WhereBuilder_Add_And_Expression_With_DimensionSlicers_Test()
    {
      WhereBuilder<int> builder = new WhereBuilder<int>(_resolver);

      builder.Define(b =>
        b.And(x => x.Dimension("category").IsEquals("clothes"),
        x => x.Dimension("category").IsEquals("shoes")));

      var predicate = builder.Build();

      Assert.IsNotNull(predicate);
      Assert.IsTrue(predicate.FiltersOnAggregation());
    }

    [Test]
    public void WhereBuilder_Add_Or_Expression_With_DimensionSlicers_Test()
    {
      WhereBuilder<int> builder = new WhereBuilder<int>(_resolver);

      builder.Define(b =>
        b.Or(x => x.Dimension("category").IsEquals("clothes"),
        x => x.Dimension("category").IsEquals("shoes")));

      var predicate = builder.Build();

      Assert.IsNotNull(predicate);
      Assert.IsTrue(predicate.FiltersOnAggregation());
    }

    [Test]
    public void WhereBuilder_Add_And_Expression_With_DimensionSlicers_And_MeasureSlicer_Test()
    {
      WhereBuilder<int> builder = new WhereBuilder<int>(_resolver);

      builder.Define(b =>
        b.And(x => x.Dimension("category").IsEquals("clothes"),
        x => x.Measure("quantity").GreaterOrEquals(2))
      );

      var predicate = builder.Build();

      Assert.IsNotNull(predicate);
      Assert.IsTrue(predicate.FiltersOnAggregation());
      Assert.IsTrue(predicate.FiltersOnFacts());
    }

    [Test]
    public void QueryBuilder_Create_Test()
    {
      var query = cube.BuildQuery()
        .OnRows("sex.male")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity");

      var result = query.Create();

      Assert.IsNotNull(result);
    }

    [Test]
    public void QueryBuilder_With_Where_Create_Test()
    {
      var query = cube.BuildQuery()
        .OnRows("sex.male")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity")
        .Where(b => b.Define(x => x.Measure("spent").GreaterOrEquals(100)));

      var result = query.Create();

      Assert.IsNotNull(result);
    }

    [Test]
    public void QueryBuilder_With_Where_Create_2_Test()
    {
      var query = cube.BuildQuery()
        .OnRows("sex.male", "sex.female")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity")
        .Where(b => b.Define(x => x.Measure("spent").GreaterOrEquals(100)));

      var result = query.Create();

      Assert.IsNotNull(result);
    }

    [Test]
    public void QueryBuilder_With_Where_Create_3_Test()
    {
      var query = cube.BuildQuery()
        .OnRows("sex.male", "sex.female")
        .OnColumns("category.shoes", "category.toys")
        .AddMeasuresOrMetrics("quantity")
        .Where(b => b.Define(x => x.Measure("spent").GreaterOrEquals(100)));

      var result = query.Create();

      Assert.IsNotNull(result);
    }

    [Test]
    public void QueryBuilder_With_Where_Create_4_Test()
    {
      var query = cube.BuildQuery()
        .OnRows("sex.male", "sex.female")
        .OnColumns("category.shoes", "category.toys")
        .AddMeasuresOrMetrics("quantity", "spent")
        .Where(b => b.Define(x => x.Measure("spent").GreaterOrEquals(100)));

      var result = query.Create();

      Assert.IsNotNull(result);
    }
  }
}