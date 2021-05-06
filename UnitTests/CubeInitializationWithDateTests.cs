using NSimpleOLAP;
using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;
using NUnit.Framework;

namespace UnitTests
{
  [TestFixture]
  public class CubeInitializationWithDateTests
  {
    [Test]
    public void MolapAddDateDimensionInit_Test()
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
          })
          .AddDimension("date", dimbuild =>
          {
            dimbuild
            .SetToDateSource(DateLevels.YEAR, DateLevels.MONTH_WITH_YEAR, DateLevels.MONTH, DateLevels.DAY)
            .SetLevelDimensions("Year", "Year Month", "Month", "Day");
          });
        });

      Cube<int> cube = builder.Create<int>();

      cube.Initialize();

      Assert.AreEqual("Year", cube.Schema.Dimensions["Year"].Name);
      Assert.AreEqual(ItemType.Dimension, cube.Schema.Dimensions["Year"].ItemType);
      Assert.Greater(cube.Schema.Dimensions["Year"].ID, 1);
    }

    [Test]
    public void MolapAddDateDimensionInitCheckMembers_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello2")
        .SetSourceMappings((sourcebuild) =>
          sourcebuild.SetSource("sales")
          .AddMapping("category", "category")
          .AddMapping("date", "Year", "Year Month", "Month", "Day")
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
          })
          .AddDimension("date", dimbuild =>
          {
            dimbuild
            .SetToDateSource(DateLevels.YEAR, DateLevels.MONTH_WITH_YEAR, DateLevels.MONTH, DateLevels.DAY)
            .SetLevelDimensions("Year", "Year Month", "Month", "Day");
          });
        });

      Cube<int> cube = builder.Create<int>();

      cube.Initialize();
      cube.Process();

      Assert.AreEqual("Year", cube.Schema.Dimensions["Year"].Name);
      Assert.AreEqual(ItemType.Dimension, cube.Schema.Dimensions["Year"].ItemType);
      Assert.Greater(cube.Schema.Dimensions["Year"].ID, 1);
      Assert.AreEqual(12, cube.Schema.Dimensions["Month"].Members.Count);
    }
  }
}