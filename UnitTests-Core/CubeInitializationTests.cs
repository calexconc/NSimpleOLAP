using NSimpleOLAP;
using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;
using NUnit.Framework;

namespace UnitTests
{
  [TestFixture]
  public class CubeInitializationTests
  {
    [Test]
    public void DefaultSettingsInit_Test()
    {
      Cube<int> cube = new Cube<int>();

      cube.Initialize();

      Assert.AreEqual(StorageType.Molap, cube.Storage.StorageType);
    }

    [Test]
    public void MolapAddDimensionInit_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) => sourcebuild.SetSource("sales"))
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
        .MetaData(mbuild =>
        {
          mbuild.AddDimension("category", (dimbuild) =>
          {
            dimbuild.Source("categories")
              .ValueField("id")
              .DescField("description");
          });
        });

      Cube<int> cube = builder.Create<int>();

      cube.Initialize();

      Assert.AreEqual("category", cube.Schema.Dimensions["category"].Name);
      Assert.AreEqual("categories", cube.Schema.Dimensions["category"].DataSource.Name);
      Assert.AreEqual(ItemType.Dimension, cube.Schema.Dimensions["category"].ItemType);
      Assert.Greater(cube.Schema.Dimensions["category"].ID, 0);
    }

    [Test]
    public void MolapAddMeasureInit_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) => sourcebuild.SetSource("xpto"))
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("xpto")
            .SetSourceType(DataSourceType.CSV)
            .AddField("x", 0, typeof(int))
            .AddField("varx1", 2, typeof(int))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("xpto.csv");
            });
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("xtable")
            .SetSourceType(DataSourceType.CSV)
            .AddField("xkey", 0, typeof(int))
            .AddField("xdesc", 1, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("xtable.csv");
            });
        })
        .MetaData(mbuild =>
        {
          mbuild.AddDimension("x", (dimbuild) =>
          {
            dimbuild.Source("xtable")
              .ValueField("xkey")
              .DescField("xdesc");
          })
            .AddMeasure("var1", mesbuild =>
            {
              mesbuild.ValueField("varx1")
                          .SetType(typeof(int));
            });
        });

      Cube<int> cube = builder.Create<int>();

      cube.Initialize();

      Assert.AreEqual("var1", cube.Schema.Measures["var1"].Name);
      Assert.AreEqual(ItemType.Measure, cube.Schema.Measures["var1"].ItemType);
      Assert.AreEqual(typeof(int), cube.Schema.Measures["var1"].DataType);
    }
  }
}