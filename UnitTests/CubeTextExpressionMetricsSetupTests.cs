using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP;
using NSimpleOLAP.Query;
using NSimpleOLAP.Query.Builder;
using NUnit.Framework;
using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.CubeExpressions;
using NSimpleOLAP;
using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;

namespace UnitTests
{
  [TestFixture]
  public class CubeTextExpressionMetricsSetupTests
  {


    [Test]
    public void Basic_Metric_Expression_Setup_Test()
    {
      var builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) =>
        {
          sourcebuild.SetSource("sales")
            .AddMapping("category", "category")
            .AddMapping("sex", "sex")
            .AddMapping("place", "place");
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("sales")
            .SetSourceType(DataSourceType.CSV)
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//table.csv")
                              .SetHasHeader();
            })
            .AddField("category", 0, typeof(int))
            .AddField("sex", 1, typeof(int))
            .AddField("place", 2, typeof(int))
            .AddField("expenses", 3, typeof(double))
            .AddField("items", 4, typeof(int));
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("categories")
            .SetSourceType(DataSourceType.CSV)
            .AddField("id", 0, typeof(int))
            .AddField("description", 1, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//dimension1.csv")
                              .SetHasHeader();
            });
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("sexes")
            .SetSourceType(DataSourceType.CSV)
            .AddField("id", 0, typeof(int))
            .AddField("description", 1, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//dimension2.csv")
                               .SetHasHeader();
            });
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("places")
            .SetSourceType(DataSourceType.CSV)
            .AddField("id", 0, typeof(int))
            .AddField("description", 1, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//dimension3.csv")
                               .SetHasHeader();
            });
        })
        .MetaData(mbuild =>
        {
          mbuild.AddDimension("category", (dimbuild) =>
          {
            dimbuild.Source("categories")
              .ValueField("id")
              .DescField("description");
          })
          .AddDimension("sex", (dimbuild) =>
          {
            dimbuild.Source("sexes")
              .ValueField("id")
              .DescField("description");
          })
          .AddDimension("place", (dimbuild) =>
          {
            dimbuild.Source("places")
              .ValueField("id")
              .DescField("description");
          })
          .AddMeasure("spent", mesbuild =>
          {
            mesbuild.ValueField("expenses")
              .SetType(typeof(double));
          })
          .AddMeasure("quantity", mesbuild =>
          {
            mesbuild.ValueField("items")
              .SetType(typeof(int));
          })
          .AddMetric("teste1", metricBuilder => {
            metricBuilder
              .SetType(typeof(int))
              .SetExpression("quantity + 10");
          });
        });

      var cube = builder.Create<int>();

      cube.Initialize();
      cube.Process();


      Assert.IsNotNull(cube.Schema.Metrics["teste1"]);

      var cell = cube.Cells.Take(1).FirstOrDefault();


      var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
      var value = (int)cell.Values[cube.Schema.Metrics["teste1"].ID];

      Assert.AreEqual(valueMeasure + 10, value);
    }
    /*
    [Test]
    public void Basic_Composite_Metric_Expression_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("teste2DoubleSum", 
          exb => exb.Expression(e => e.Set("quantity").Sum(e2 => e2.Set("quantity").Value())))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["teste2DoubleSum"]);
    }

    [Test]
    public void Basic_Multiple_Metric_Expressions_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("testeAverage",
          exb => exb.Expression(e => e.Set("quantity").Average()))
        .Add("testeMax",
          exb => exb.Expression(e => e.Set("quantity").Max()))
        .Add("testeMin",
          exb => exb.Expression(e => e.Set("quantity").Min()))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["testeAverage"]);
      Assert.IsNotNull(cube.Schema.Metrics["testeMax"]);
      Assert.IsNotNull(cube.Schema.Metrics["testeMin"]);
    }

    [Test]
    public void Basic_Metric_Multiplication_Expression_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("testeMultiply1", exb => exb.Expression(e => e.Set("quantity").Multiply(3)))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["testeMultiply1"]);
    }

    [Test]
    public void Basic_Metric_Division_Expression_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("testeDivide1", exb => exb.Expression(e => e.Set("quantity").Divide(2)))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["testeDivide1"]);
    }

    [Test]
    public void Basic_Metric_Subtract_Expression_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("testeSubtract1", exb => exb.Expression(e => e.Set("quantity").Subtract(10)))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["testeSubtract1"]);
    }

    [Test]
    public void Composite_Metric_Multiplication_Expression_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("testeMultiply2", exb => exb.Expression(e => e.Set("quantity").Multiply(ex => ex.Set("spent").Value())))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["testeMultiply2"]);
    }

    [Test]
    public void Composite_Metric_Division_Expression_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("testeDivide2", exb => exb.Expression(e => e.Set("spent").Divide(ex => ex.Set("quantity").Value())))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["testeDivide2"]);
    }
    */
  }
}
