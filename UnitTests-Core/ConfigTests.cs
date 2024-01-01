using NSimpleOLAP;
using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;
using NUnit.Framework;

namespace UnitTests
{
  [TestFixture]
  public class ConfigTests
  {
    [Test]
    public void SetNameConfig_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello");

      Cube<int> cube = builder.Create<int>();

      Assert.AreEqual("hello", cube.Name);
    }

    [Test]
    public void SetSourceConfig_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) => sourcebuild.SetSource("xpto"));

      Cube<int> cube = builder.Create<int>();

      Assert.AreEqual("xpto", cube.Source);
    }

    [Test]
    public void DefaultConfigName_Test()
    {
      Cube<int> cube = new Cube<int>();

      Assert.AreEqual("New_Cube", cube.Name);
    }

    [Test]
    public void DefaultConfigStorage_Test()
    {
      Cube<int> cube = new Cube<int>();

      Assert.AreEqual(StorageType.Molap, cube.Config.Storage.StoreType);
    }

    [Test]
    public void AddDimensionConfig_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) => sourcebuild.SetSource("xpto"))
        .MetaData(mbuild =>
        {
          mbuild.AddDimension("x", (dimbuild) =>
          {
            dimbuild.Source("xtable")
              .ValueField("xkey")
              .DescField("xdesc");
          });
        });

      Cube<int> cube = builder.Create<int>();

      Assert.AreEqual("x", cube.Config.MetaData.Dimensions[0].Name);
      Assert.AreEqual("x", cube.Config.MetaData.Dimensions["x"].Name);
    }

    [Test]
    public void DimensionSetupConfig_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) => sourcebuild.SetSource("xpto"))
        .MetaData(mbuild =>
        {
          mbuild.AddDimension("x", (dimbuild) =>
          {
            dimbuild.Source("xtable")
              .ValueField("xkey")
              .DescField("xdesc");
          });
        });

      Cube<int> cube = builder.Create<int>();

      Assert.AreEqual("x", cube.Config.MetaData.Dimensions["x"].Name);
      Assert.AreEqual("xtable", cube.Config.MetaData.Dimensions["x"].Source);
      Assert.AreEqual("xdesc", cube.Config.MetaData.Dimensions["x"].DesFieldName);
      Assert.AreEqual("xkey", cube.Config.MetaData.Dimensions["x"].ValueFieldName);
    }

    [Test]
    public void AddMoreThanOneDimensionConfig_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) => sourcebuild.SetSource("xpto"))
        .MetaData(mbuild =>
        {
          mbuild.AddDimension("x", (dimbuild) =>
          {
            dimbuild.Source("xtable")
              .ValueField("xkey")
              .DescField("xdesc");
          })
            .AddDimension("y", (dimbuild) =>
            {
              dimbuild.Source("ytable")
                          .ValueField("ykey")
                          .DescField("ydesc");
            });
        });

      Cube<int> cube = builder.Create<int>();

      Assert.AreEqual("x", cube.Config.MetaData.Dimensions[0].Name);
      Assert.AreEqual("x", cube.Config.MetaData.Dimensions["x"].Name);
      Assert.AreEqual("y", cube.Config.MetaData.Dimensions[1].Name);
      Assert.AreEqual("y", cube.Config.MetaData.Dimensions["y"].Name);
    }

    [Test]
    public void AddDataSourceConfig_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) => sourcebuild.SetSource("xpto"))
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("xpto")
            .SetSourceType(DataSourceType.CSV)
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("xpto.csv");
            });
        });

      Cube<int> cube = builder.Create<int>();

      Assert.AreEqual("xpto", cube.Config.DataSources["xpto"].Name);
    }

    [Test]
    public void DataSourceSetupConfig_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) => sourcebuild.SetSource("xpto"))
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("xpto")
            .SetSourceType(DataSourceType.CSV)
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("xpto.csv");
            });
        });

      Cube<int> cube = builder.Create<int>();

      Assert.AreEqual("xpto", cube.Config.DataSources["xpto"].Name);
      Assert.AreEqual(DataSourceType.CSV, cube.Config.DataSources["xpto"].SourceType);
      Assert.AreEqual("xpto.csv", cube.Config.DataSources["xpto"].CSVConfig.FilePath);
    }

    [Test]
    public void AddMoreThanOneDataSourceConfig_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) => sourcebuild.SetSource("xpto"))
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("xpto")
            .SetSourceType(DataSourceType.CSV)
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("xpto.csv");
            });
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("xtable")
            .SetSourceType(DataSourceType.CSV)
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("xtable.csv");
            });
        });

      Cube<int> cube = builder.Create<int>();

      Assert.AreEqual("xpto", cube.Config.DataSources["xpto"].Name);
      Assert.AreEqual("xtable", cube.Config.DataSources["xtable"].Name);
    }

    [Test]
    public void DataSourceFieldsSetupConfig_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) => sourcebuild.SetSource("xpto"))
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("xpto")
            .SetSourceType(DataSourceType.CSV)
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("xpto.csv");
            })
            .AddField("xval", 0, typeof(int))
            .AddField("yval", 1, typeof(int));
        });

      Cube<int> cube = builder.Create<int>();

      Assert.AreEqual("xval", cube.Config.DataSources["xpto"].Fields["xval"].Name);
      Assert.AreEqual(0, cube.Config.DataSources["xpto"].Fields["xval"].Index);
      Assert.AreEqual(typeof(int), cube.Config.DataSources["xpto"].Fields["xval"].FieldType);
      Assert.AreEqual("yval", cube.Config.DataSources["xpto"].Fields["yval"].Name);
      Assert.AreEqual(1, cube.Config.DataSources["xpto"].Fields["yval"].Index);
      Assert.AreEqual(typeof(int), cube.Config.DataSources["xpto"].Fields["yval"].FieldType);
    }

    [Test]
    public void SetStorageConfig_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) => sourcebuild.SetSource("xpto"))
        .Storage(storebuild =>
        {
          storebuild.SetStoreType(StorageType.Molap);
        });

      Cube<int> cube = builder.Create<int>();

      Assert.AreEqual(StorageType.Molap, cube.Config.Storage.StoreType);
    }

    [Test]
    public void AddMeasureConfig_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) => sourcebuild.SetSource("xpto"))
        .MetaData(mbuild =>
        {
          mbuild.AddMeasure("var1", mesbuild =>
          {
            mesbuild.ValueField("varx1");
          });
        });

      Cube<int> cube = builder.Create<int>();

      Assert.AreEqual("var1", cube.Config.MetaData.Measures["var1"].Name);
      Assert.AreEqual("varx1", cube.Config.MetaData.Measures["var1"].ValueFieldName);
    }
  }
}