using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests
{
  [TestFixture]
  public class CubeUsingObjectMapperDataSourceTests
  {
    private CubeBuilder GetCubeConfiguration()
    {
      CubeBuilder builder = new CubeBuilder();
      var datset = DataSetDataSourceFixture.GetDataSources();

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
            .SetSourceType(DataSourceType.ObjectMapper)
            .SetObjectMapperConfig(obMapper =>
            {
              obMapper.SetCollection<Fact>(ObjectMapperDataSourceFixture.GetFacts())
              .SetMapper<Fact>(x =>
              {
                var row = new KeyValuePair<string, object>[6] {
                  new KeyValuePair<string, object>("category", x.Category),
                  new KeyValuePair<string, object>("gender", x.Gender),
                  new KeyValuePair<string, object>("place", x.Place),
                  new KeyValuePair<string, object>("date", x.Date),
                  new KeyValuePair<string, object>("expenses", x.Expenses),
                  new KeyValuePair<string, object>("items", x.Items)
                };

                return row;
              });
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
            .SetSourceType(DataSourceType.ObjectMapper)
            .AddField("id", typeof(int))
            .AddField("description", typeof(string))
            .SetObjectMapperConfig(obMapper =>
            {
              obMapper.SetCollection<Category>(ObjectMapperDataSourceFixture.GetCategories())
              .SetMapper<Category>(x =>
              {
                var row = new KeyValuePair<string, object>[2] {
                  new KeyValuePair<string, object>("id", x.Id),
                  new KeyValuePair<string, object>("description", x.Description)
                };

                return row;
              });
            });
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("genderes")
            .SetSourceType(DataSourceType.ObjectMapper)
            .AddField("id", typeof(int))
            .AddField("description", 1, typeof(string))
            .SetObjectMapperConfig(obMapper =>
            {
              obMapper.SetCollection<Gender>(ObjectMapperDataSourceFixture.GetGender())
              .SetMapper<Gender>(x =>
              {
                var row = new KeyValuePair<string, object>[2] {
                  new KeyValuePair<string, object>("id", x.Id),
                  new KeyValuePair<string, object>("description", x.Description)
                };

                return row;
              });
            });
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("places")
            .SetSourceType(DataSourceType.ObjectMapper)
            .AddField("id", typeof(int))
            .AddField("description", typeof(string))
            .AddField("idcountry", typeof(int))
            .AddField("country", typeof(string))
            .AddField("idregion", typeof(int))
            .AddField("region", typeof(string))
            .SetObjectMapperConfig(obMapper =>
            {
              obMapper.SetCollection<Place>(ObjectMapperDataSourceFixture.GetPlaces())
              .SetMapper<Place>(x =>
              {
                var row = new KeyValuePair<string, object>[6] {
                  new KeyValuePair<string, object>("id", x.Id),
                  new KeyValuePair<string, object>("description", x.Description),
                  new KeyValuePair<string, object>("idcountry", x.IdCountry),
                  new KeyValuePair<string, object>("country", x.Country),
                  new KeyValuePair<string, object>("idregion", x.IdRegion),
                  new KeyValuePair<string, object>("region", x.Region)
                };

                return row;
              });
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
    public void Setup_Cube_With_Only_ObjectMapper_Test()
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