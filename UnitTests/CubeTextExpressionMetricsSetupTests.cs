using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;
using NUnit.Framework;
using System;
using System.Linq;

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
          .AddMetric("teste1", metricBuilder =>
          {
            metricBuilder
              .SetType(typeof(int))
              .SetExpression("quantity + 10");
          });
        });

      using (var cube = builder.Create<int>())
      {
        cube.Initialize();
        cube.Process();

        Assert.IsNotNull(cube.Schema.Metrics["teste1"]);

        var cell = cube.Cells.Take(1).FirstOrDefault();

        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var value = (int)cell.Values[cube.Schema.Metrics["teste1"].ID];

        Assert.AreEqual(valueMeasure + 10, value);
      }
    }

    [Test]
    public void Composite_Metric_Multiplication_Expression_Setup_Test()
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
          .AddMetric("testeMultiply2", metricBuilder =>
          {
            metricBuilder
              .SetType(typeof(int))
              .SetExpression("quantity * spent");
          });
        });

      using (var cube = builder.Create<int>())
      {
        cube.Initialize();
        cube.Process();

        Assert.IsNotNull(cube.Schema.Metrics["testeMultiply2"]);

        var cell = cube.Cells.Take(1).FirstOrDefault();

        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var valueMeasure2 = (double)cell.Values[cube.Schema.Measures["spent"].ID];
        var value = (double)cell.Values[cube.Schema.Metrics["testeMultiply2"].ID];

        Assert.AreEqual(valueMeasure * valueMeasure2, value);
      }
    }

    [Test]
    public void Basic_Composite_Metric_Expression_Setup_Test()
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
          .AddMetric("teste2DoubleSum", metricBuilder =>
          {
            metricBuilder
              .SetType(typeof(int))
              .SetExpression("quantity + quantity");
          });
        });

      using (var cube = builder.Create<int>())
      {
        cube.Initialize();
        cube.Process();

        Assert.IsNotNull(cube.Schema.Metrics["teste2DoubleSum"]);

        var cell = cube.Cells.Take(1).FirstOrDefault();

        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var value = (int)cell.Values[cube.Schema.Metrics["teste2DoubleSum"].ID];

        Assert.AreEqual(valueMeasure + valueMeasure, value);
      }
    }

    [Test]
    public void Basic_Metric_Multiplication_Expression_Setup_Test()
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
          .AddMetric("testeMultiply1", metricBuilder =>
          {
            metricBuilder
              .SetType(typeof(int))
              .SetExpression("3 * quantity");
          });
        });

      using (var cube = builder.Create<int>())
      {
        cube.Initialize();
        cube.Process();

        Assert.IsNotNull(cube.Schema.Metrics["testeMultiply1"]);

        var cell = cube.Cells.Take(1).FirstOrDefault();

        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var value = (int)cell.Values[cube.Schema.Metrics["testeMultiply1"].ID];

        Assert.AreEqual(valueMeasure * 3, value);
      }
    }

    [Test]
    public void Basic_Metric_Division_Expression_Setup_Test()
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
          .AddMetric("testeDivide1", metricBuilder =>
          {
            metricBuilder
              .SetType(typeof(double))
              .SetExpression("quantity / 2");
          });
        });

      using (var cube = builder.Create<int>())
      {
        cube.Initialize();
        cube.Process();

        Assert.IsNotNull(cube.Schema.Metrics["testeDivide1"]);

        var cell = cube.Cells.Take(1).FirstOrDefault();

        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var value = (double)cell.Values[cube.Schema.Metrics["testeDivide1"].ID];

        Assert.AreEqual(Convert.ToDouble(valueMeasure) / 2, value);
      }
    }

    [Test]
    public void Basic_Metric_Subtract_Expression_Setup_Test()
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
          .AddMetric("testeSubtract1", metricBuilder =>
          {
            metricBuilder
              .SetType(typeof(int))
              .SetExpression("quantity - 10");
          });
        });

      using (var cube = builder.Create<int>())
      {
        cube.Initialize();
        cube.Process();

        Assert.IsNotNull(cube.Schema.Metrics["testeSubtract1"]);

        var cell = cube.Cells.Take(1).FirstOrDefault();

        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var value = (int)cell.Values[cube.Schema.Metrics["testeSubtract1"].ID];

        Assert.AreEqual(valueMeasure - 10, value);
      }
    }

    [Test]
    public void Composite_Metric_Division_Expression_Setup_Test()
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
          .AddMetric("testeDivide2", metricBuilder =>
          {
            metricBuilder
              .SetType(typeof(int))
              .SetExpression("spent / quantity");
          });
        });

      using (var cube = builder.Create<int>())
      {
        cube.Initialize();
        cube.Process();

        Assert.IsNotNull(cube.Schema.Metrics["testeDivide2"]);

        var cell = cube.Cells.Take(1).FirstOrDefault();

        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var valueMeasure2 = (double)cell.Values[cube.Schema.Measures["spent"].ID];
        var value = (double)cell.Values[cube.Schema.Metrics["testeDivide2"].ID];

        Assert.AreEqual(valueMeasure2 / valueMeasure, value);
      }
    }

    [Test]
    public void Basic_Multiple_Metric_Expressions_Setup_Test()
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
          .AddMetric("testeAverage", metricBuilder =>
          {
            metricBuilder
              .SetType(typeof(double))
              .SetExpression("AVG quantity");
          })
          .AddMetric("testeMax", metricBuilder =>
          {
            metricBuilder
              .SetType(typeof(int))
              .SetExpression("MAX quantity");
          })
          .AddMetric("testeMin", metricBuilder =>
          {
            metricBuilder
              .SetType(typeof(int))
              .SetExpression("MIN quantity");
          })
          .AddMetric("testeSqrt", metricBuilder =>
          {
            metricBuilder
              .SetType(typeof(double))
              .SetExpression("SQRT quantity");
          });
        });

      using (var cube = builder.Create<int>())
      {
        cube.Initialize();
        cube.Process();

        Assert.IsNotNull(cube.Schema.Metrics["testeAverage"]);
        Assert.IsNotNull(cube.Schema.Metrics["testeMax"]);
        Assert.IsNotNull(cube.Schema.Metrics["testeMin"]);
        Assert.IsNotNull(cube.Schema.Metrics["testeSqrt"]);

        var cell = cube.Cells.Take(1).FirstOrDefault();

        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var valueAverage = cell.Values[cube.Schema.Metrics["testeAverage"].ID];
        var valueMax = cell.Values[cube.Schema.Metrics["testeMax"].ID];
        var valueMin = cell.Values[cube.Schema.Metrics["testeMin"].ID];
        var valueSqrt = cell.Values[cube.Schema.Metrics["testeSqrt"].ID];

        Assert.AreEqual(valueMeasure / 24, valueAverage);
        Assert.AreEqual(101, valueMax);
        Assert.AreEqual(1, valueMin);
        Assert.AreEqual(Math.Sqrt(valueMeasure), valueSqrt);
      }
    }
  }
}