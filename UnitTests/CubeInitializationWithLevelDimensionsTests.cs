using NSimpleOLAP;
using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;
using NUnit.Framework;

namespace UnitTests
{
  [TestFixture]
  public class CubeInitializationWithLevelDimensionsTests
  {
    [Test]
    public void MolapAddLevelDimensionInit_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello2")
        .SetSourceMappings((sourcebuild) => sourcebuild.SetSource("sales"))
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("sales")
            .SetSourceType(DataSourceType.CSV)
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//tableWithDate.csv")
                               .SetHasHeader();
            })
            .AddField("category", 0, typeof(int))
            .AddField("sex", 1, typeof(int))
            .AddField("place", 2, typeof(int))
            .AddDateField("date", 3, "yyyy-MM-dd")
            .AddField("expenses", 4, typeof(double))
            .AddField("items", 5, typeof(int));
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("places")
            .SetSourceType(DataSourceType.CSV)
            .AddField("id", 0, typeof(int))
            .AddField("description", 1, typeof(string))
            .AddField("idcountry", 2, typeof(int))
            .AddField("country", 3, typeof(string))
            .AddField("idregion", 4, typeof(int))
            .AddField("region", 5, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//dimension3.csv")
                               .SetHasHeader();
            });
        })
        .MetaData(mbuild =>
        {
          mbuild.AddDimension("place", (dimbuild) =>
          {
            dimbuild.Source("places")
              .ValueField("id")
              .DescField("description")
              .SetLevelDimensions("Country", "Region");
          })
          .AddDimension("Country", (dimbuild) =>
          {
            dimbuild.Source("places")
              .ValueField("idcountry")
              .DescField("country");
          })
          .AddDimension("Region", (dimbuild) =>
          {
            dimbuild.Source("places")
              .ValueField("idregion")
              .DescField("region");
          });
        });

      Cube<int> cube = builder.Create<int>();

      cube.Initialize();

      Assert.AreEqual("Country", cube.Schema.Dimensions["Country"].Name);
      Assert.AreEqual(ItemType.Dimension, cube.Schema.Dimensions["Country"].ItemType);
      Assert.Greater(cube.Schema.Dimensions["Country"].ID, 1);
    }

    [Test]
    public void MolapAddDateDimensionInitCheckMembers_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello2")
        .SetSourceMappings((sourcebuild) =>
          sourcebuild.SetSource("sales")
          .AddMapping("place", "place", "Country", "Region")
          )
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("sales")
            .SetSourceType(DataSourceType.CSV)
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//tableWithDate.csv")
                               .SetHasHeader();
            })
            .AddField("category", 0, typeof(int))
            .AddField("sex", 1, typeof(int))
            .AddField("place", 2, typeof(int))
            .AddDateField("date", 3, "yyyy-MM-dd")
            .AddField("expenses", 4, typeof(double))
            .AddField("items", 5, typeof(int));
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("places")
            .SetSourceType(DataSourceType.CSV)
            .AddField("id", 0, typeof(int))
            .AddField("description", 1, typeof(string))
            .AddField("idcountry", 2, typeof(int))
            .AddField("country", 3, typeof(string))
            .AddField("idregion", 4, typeof(int))
            .AddField("region", 5, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//dimension3.csv")
                               .SetHasHeader();
            });
        })
        .MetaData(mbuild =>
        {
          mbuild.AddDimension("place", (dimbuild) =>
          {
            dimbuild.Source("places")
              .ValueField("id")
              .DescField("description")
              .SetLevelDimensions("Country", "Region");
          })
          .AddDimension("Country", (dimbuild) =>
          {
            dimbuild.Source("places")
              .ValueField("idcountry")
              .DescField("country");
          })
          .AddDimension("Region", (dimbuild) =>
          {
            dimbuild.Source("places")
              .ValueField("idregion")
              .DescField("region");
          });
        });

      Cube<int> cube = builder.Create<int>();

      cube.Initialize();
      cube.Process();

      Assert.AreEqual("Country", cube.Schema.Dimensions["Country"].Name);
      Assert.AreEqual(ItemType.Dimension, cube.Schema.Dimensions["Country"].ItemType);
      Assert.Greater(cube.Schema.Dimensions["Country"].ID, 1);
      Assert.AreEqual(5, cube.Schema.Dimensions["Country"].Members.Count);
    }
  }
}