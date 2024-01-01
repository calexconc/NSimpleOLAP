using NSimpleOLAP;
using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;
using NUnit.Framework;

namespace UnitTests
{
  [TestFixture]
  public class CubeInitializationWithTimeTests
  {
    [Test]
    public void MolapAddTimeDimensionInit_Test()
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
              csvbuild.SetFilePath("TestData//tableWithTime.csv")
                               .SetHasHeader();
            })
            .AddField("category", 0, typeof(int))
            .AddField("sex", 1, typeof(int))
            .AddField("place", 2, typeof(int))
            .AddTimeField("time", 3, "hh\\:mm\\:ss")
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
          .AddDimension("time", dimbuild =>
          {
            dimbuild
            .SetToTimeSource(TimeLevels.HOUR, TimeLevels.MINUTES, TimeLevels.SECONDS)
            .SetLevelDimensions("Hour", "Minute", "Second");
          });
        });

      Cube<int> cube = builder.Create<int>();

      cube.Initialize();

      Assert.AreEqual("Hour", cube.Schema.Dimensions["Hour"].Name);
      Assert.AreEqual(ItemType.Dimension, cube.Schema.Dimensions["Hour"].ItemType);
      Assert.Greater(cube.Schema.Dimensions["Hour"].ID, 1);
    }

    [Test]
    public void MolapAddTimeDimensionInitCheckMembers_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello2")
        .SetSourceMappings((sourcebuild) =>
          sourcebuild.SetSource("sales")
          .AddMapping("category", "category")
          .AddMapping("time", "Hour", "Minute", "Second")
          )
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("sales")
            .SetSourceType(DataSourceType.CSV)
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//tableWithTime.csv")
                               .SetHasHeader();
            })
            .AddField("category", 0, typeof(int))
            .AddField("sex", 1, typeof(int))
            .AddField("place", 2, typeof(int))
            .AddTimeField("time", 3, "hh\\:mm\\:ss")
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
          .AddDimension("time", dimbuild =>
          {
            dimbuild
            .SetToTimeSource(TimeLevels.HOUR, TimeLevels.MINUTES, TimeLevels.SECONDS)
            .SetLevelDimensions("Hour", "Minute", "Second");
          });
        });

      Cube<int> cube = builder.Create<int>();

      cube.Initialize();
      cube.Process();

      Assert.AreEqual("Hour", cube.Schema.Dimensions["Hour"].Name);
      Assert.AreEqual(ItemType.Dimension, cube.Schema.Dimensions["Hour"].ItemType);
      Assert.Greater(cube.Schema.Dimensions["Hour"].ID, 1);
      Assert.AreEqual(60, cube.Schema.Dimensions["Minute"].Members.Count);
    }
  }
}