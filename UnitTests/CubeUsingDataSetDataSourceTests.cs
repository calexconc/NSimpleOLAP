using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;
using NUnit.Framework;
using System;

namespace UnitTests
{
  [TestFixture]
  public class CubeUsingDataSetDataSourceTests
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
            .SetSourceType(DataSourceType.DataSet)
            .SetDataTableConfig(dtbuild =>
            {
              dtbuild.SetDataTable(datset.Tables["Facts"]);
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
            .SetSourceType(DataSourceType.DataSet)
            .AddField("id", typeof(int))
            .AddField("description", typeof(string))
            .SetDataTableConfig(dtbuild =>
            {
              dtbuild.SetDataTable(datset.Tables["Categories"]);
            });
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("genderes")
            .SetSourceType(DataSourceType.DataSet)
            .AddField("id", typeof(int))
            .AddField("description", 1, typeof(string))
            .SetDataTableConfig(dtbuild =>
            {
              dtbuild.SetDataTable(datset.Tables["Gender"]);
            });
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("places")
            .SetSourceType(DataSourceType.DataSet)
            .AddField("id", typeof(int))
            .AddField("description", typeof(string))
            .AddField("idcountry", typeof(int))
            .AddField("country", typeof(string))
            .AddField("idregion", typeof(int))
            .AddField("region", typeof(string))
            .SetDataTableConfig(dtbuild =>
            {
              dtbuild.SetDataTable(datset.Tables["Places"]);
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
    public void Setup_Cube_With_Only_DataTables_Test()
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