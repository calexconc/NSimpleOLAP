using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;
using NUnit.Framework;
using System;

namespace UnitTests
{
  [TestFixture]
  public class CubeUsingSQLiteDatabaseDataSourceTests
  {
    private CubeBuilder GetCubeConfiguration()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("helloDataTable")
        .SetSourceMappings((sourcebuild) =>
        {
          sourcebuild.SetSource("sales")
            .AddMapping("category", "category")
            .AddMapping("gender", "gender")
            .AddMapping("place", "place", "Country", "Region")
            .AddMapping("date", "Year", "Month", "Day");
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("sales")
            .SetSourceType(DataSourceType.DataBase)
            .SetDBConfig(dbBuild => {
              dbBuild.SetConnection("LITE")
              .SetQuery("SELECT category, gender, place, datetime([date]) as 'date', expenses, items FROM Sales");
            })
            .AddField("category", typeof(int))
            .AddField("gender", typeof(int))
            .AddField("place", typeof(int))
            .AddField("date", typeof(DateTime))
            .AddField("expenses", typeof(double))
            .AddField("items", typeof(int));
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("categories")
            .SetSourceType(DataSourceType.DataBase)
            .AddField("id", typeof(int))
            .AddField("description", typeof(string))
            .SetDBConfig(dbBuild => {
              dbBuild.SetConnection("LITE")
              .SetQuery("SELECT id, description FROM Categories");
            });
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("genderes")
            .SetSourceType(DataSourceType.DataBase)
            .AddField("id", typeof(int))
            .AddField("description", 1, typeof(string))
            .SetDBConfig(dbBuild => {
              dbBuild.SetConnection("LITE")
              .SetQuery("SELECT id, description FROM Genders");
            });
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("places")
            .SetSourceType(DataSourceType.DataBase)
            .AddField("id", typeof(int))
            .AddField("description", typeof(string))
            .AddField("idcountry", typeof(int))
            .AddField("country", typeof(string))
            .AddField("idregion", typeof(int))
            .AddField("region", typeof(string))
            .SetDBConfig(dbBuild => {
              dbBuild.SetConnection("LITE")
              .SetQuery("SELECT id, description, idcountry, country, idregion, region FROM Places");
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
          .AddDimension("gender", (dimbuild) =>
          {
            dimbuild.Source("genderes")
              .ValueField("id")
              .DescField("description");
          })
          .AddDimension("place", (dimbuild) =>
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
          })
          .AddDimension("date", dimbuild =>
          {
            dimbuild
            .SetToDateSource(DateLevels.YEAR, DateLevels.MONTH, DateLevels.DAY)
            .SetLevelDimensions("Year", "Month", "Day");
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
          });
        });

      return builder;
    }

    [Test]
    public void Setup_Cube_With_Only_SQLiteDB_Test()
    {
      var builder = GetCubeConfiguration();
      using (var cube = builder.Create<int>())
      {
        cube.Initialize();
        cube.Process();

        var cellCount = cube.Cells.Count;

        Assert.IsTrue(cellCount > 0 && cellCount > 5000);
      }
    }
  }
}