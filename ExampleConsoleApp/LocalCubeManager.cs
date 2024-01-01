using NSimpleOLAP;
using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;
using NSimpleOLAP.Query;
using NSimpleOLAP.Renderers;
using System;
using System.Linq;

namespace ExampleConsoleApp
{
  internal class LocalCubeManager : IDisposable
  {
    private Cube<int> _cube;

    public void Dispose()
    {
      _cube.Dispose();
      Console.WriteLine("Cube Disposed");
    }

    public void Initialize()
    {
      var builder = new CubeBuilder();

      builder.SetName("CUBE")
        .SetSourceMappings((sourcebuild) =>
        {
          sourcebuild.SetSource("sales")
            .AddMapping("category", "category")
            .AddMapping("gender", "gender")
            .AddMapping("place", "place")
            .AddMapping("date", "Year", "Month", "Day");
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("sales")
            .SetSourceType(DataSourceType.CSV)
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("Data//facts.csv")
                              .SetHasHeader();
            })
            .AddField("category", 0, typeof(int))
            .AddField("gender", 1, typeof(int))
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
              csvbuild.SetFilePath("Data//dimension1.csv")
                              .SetHasHeader();
            });
        })
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("allgenders")
            .SetSourceType(DataSourceType.CSV)
            .AddField("id", 0, typeof(int))
            .AddField("description", 1, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("Data//dimension2.csv")
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
              csvbuild.SetFilePath("Data//dimension3.csv")
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
          .AddDimension("gender", (dimbuild) =>
          {
            dimbuild.Source("allgenders")
              .ValueField("id")
              .DescField("description");
          })
          .AddDimension("place", (dimbuild) =>
          {
            dimbuild.Source("places")
              .ValueField("id")
              .DescField("description");
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

      _cube = builder.Create<int>();

      Console.WriteLine("Cube Configured.");

      _cube.Initialize();

      Console.WriteLine("Cube Initialized.");
    }

    public void Load()
    {
      Console.WriteLine("Start Loading Cube.");
      _cube.Process();
      Console.WriteLine("Finished Loading Cube.");
    }

    public void QueryCube(QuerySettings querySettings)
    {
      var queryBuilder = _cube.BuildQuery();
      
      if (querySettings.RowTupples.Count > 0)
        queryBuilder.OnRows(querySettings.RowTupples.ToArray());

      if (querySettings.ColumnTupples.Count > 0)
        queryBuilder.OnColumns(querySettings.ColumnTupples.ToArray());

      if (querySettings.Measures.Count > 0)
        queryBuilder.AddMeasuresOrMetrics(querySettings.Measures.ToArray());

      var query = queryBuilder.Create();

      var result = query.StreamRows().ToList();

      result.RenderInConsole();
    }

    public void WriteHelp()
    {
      Console.WriteLine("Query Examples:");
      Console.WriteLine("rows: gender.female, gender.male columns: category.shoes measures: quantity");
      Console.WriteLine("rows: category.All.place.Paris columns: gender.All measures: quantity");

      Console.WriteLine("Available Dimensions:");
      Console.WriteLine(string.Format("{0}.", string.Join(",", _cube.Schema.Dimensions.Select(x => x.Name))));

      Console.WriteLine("Available Measures:");
      Console.WriteLine(string.Format("{0}.", string.Join(",", _cube.Schema.Measures.Select(x => x.Name))));
      Console.WriteLine("");
    }
  }
}