using NSimpleOLAP;
using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;
using NUnit.Framework;

namespace UnitTests
{
  [TestFixture]
  public class CubeInitializationWithDimensionTransformersTests
  {
    [Test]
    public void MolapAddTransformerDimensionInit_Test()
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
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("intervals")
            .SetSourceType(DataSourceType.Transformer)
            .SetTransformerTableConfig(transfbuild =>
            {
              transfbuild
                .AddIntervalSegment("Small Purchase", 0, 20)
                .AddIntervalSegment("Medium Purchase", 21, 50)
                .AddIntervalSegment("Large Purchase", 51, null);
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
          .AddDimension("purchase_size", dimbuild =>
          {
            dimbuild
              .Source("intervals")
              .SetSourceMembersAreGenerated();
          });
        });

      Cube<int> cube = builder.Create<int>();

      cube.Initialize();

      Assert.AreEqual("purchase_size", cube.Schema.Dimensions["purchase_size"].Name);
      Assert.AreEqual(ItemType.Dimension, cube.Schema.Dimensions["purchase_size"].ItemType);
      Assert.Greater(cube.Schema.Dimensions["purchase_size"].ID, 1);
    }

    [Test]
    public void MolapAddTransformerDimensionInitCheckMembers_Test()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello2")
        .SetSourceMappings((sourcebuild) =>
          sourcebuild.SetSource("sales")
          .AddMapping("category", "category")
          .AddMapping("items", "purchase_size")
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
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("intervals")
            .SetSourceType(DataSourceType.Transformer)
            .SetTransformerTableConfig(transfbuild =>
            {
              transfbuild
                .AddIntervalSegment("Small Purchase", 0, 20)
                .AddIntervalSegment("Medium Purchase", 21, 50)
                .AddIntervalSegment("Large Purchase", 51, null);
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
          .AddDimension("purchase_size", dimbuild =>
          {
            dimbuild
              .Source("intervals")
              .SetSourceMembersAreGenerated();
          });
        });

      Cube<int> cube = builder.Create<int>();

      cube.Initialize();
      cube.Process();

      Assert.AreEqual("purchase_size", cube.Schema.Dimensions["purchase_size"].Name);
      Assert.AreEqual(ItemType.Dimension, cube.Schema.Dimensions["purchase_size"].ItemType);
      Assert.Greater(cube.Schema.Dimensions["purchase_size"].ID, 1);
      Assert.AreEqual(3, cube.Schema.Dimensions["purchase_size"].Members.Count);
    }
  }
}