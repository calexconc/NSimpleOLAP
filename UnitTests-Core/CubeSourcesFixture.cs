using NSimpleOLAP;
using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;

namespace UnitTests
{
  internal class CubeSourcesFixture
  {
    public static Cube<int> GetBasicCubeTwoDimensionsOneMeasure()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings(
          (sourcebuild) => sourcebuild.SetSource("sales")
            .AddMapping("category", "category")
            .AddMapping("sex", "sex")
        )
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
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("sexes")
            .SetSourceType(DataSourceType.CSV)
            .AddField("id", 0, typeof(int))
            .AddField("description", 1, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//dimension2.csv")
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
          .AddDimension("sex", (dimbuild) =>
          {
            dimbuild.Source("sexes")
                        .ValueField("id")
                        .DescField("description");
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

      return builder.Create<int>();
    }

    public static Cube<int> GetBasicCubeThreeDimensionsTwoMeasures()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) =>
        {
          sourcebuild.SetSource("sales")
            .AddMapping("category", "category")
            .AddMapping("sex", "sex")
            .AddMapping("place", "place");
        })
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
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("sexes")
            .SetSourceType(DataSourceType.CSV)
            .AddField("id", 0, typeof(int))
            .AddField("description", 1, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//dimension2.csv")
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
              csvbuild.SetFilePath("TestData//dimension3.csv")
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
            .AddDimension("sex", (dimbuild) =>
            {
              dimbuild.Source("sexes")
                          .ValueField("id")
                          .DescField("description");
            })
            .AddDimension("place", (dimbuild) =>
            {
              dimbuild.Source("places")
                          .ValueField("id")
                          .DescField("description");
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

      return builder.Create<int>();
    }

    public static Cube<int> GetBasicCubeThreeDimensionsTwoMeasures2()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) =>
        {
          sourcebuild.SetSource("sales")
            .AddMapping("category", "category")
            .AddMapping("sex", "sex")
            .AddMapping("place", "place");
        })
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
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("sexes")
            .SetSourceType(DataSourceType.CSV)
            .AddField("id", 0, typeof(int))
            .AddField("description", 1, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//dimension2.csv")
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
              csvbuild.SetFilePath("TestData//dimension3.csv")
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
          .AddDimension("sex", (dimbuild) =>
          {
            dimbuild.Source("sexes")
              .ValueField("id")
              .DescField("description");
          })
          .AddDimension("place", (dimbuild) =>
          {
            dimbuild.Source("places")
              .ValueField("id")
              .DescField("description");
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

      return builder.Create<int>();
    }

    public static Cube<int> GetBasicCubeThreeSimpleDimensionsTwoMeasuresAndThreeDateDimensions()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) =>
        {
          sourcebuild.SetSource("sales")
            .AddMapping("category", "category")
            .AddMapping("sex", "sex")
            .AddMapping("place", "place")
            .AddMapping("date", "Year", "Month", "Day");
        })
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
          dsbuild.SetName("sexes")
            .SetSourceType(DataSourceType.CSV)
            .AddField("id", 0, typeof(int))
            .AddField("description", 1, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//dimension2.csv")
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
              csvbuild.SetFilePath("TestData//dimension3.csv")
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
          .AddDimension("sex", (dimbuild) =>
          {
            dimbuild.Source("sexes")
              .ValueField("id")
              .DescField("description");
          })
          .AddDimension("place", (dimbuild) =>
          {
            dimbuild.Source("places")
              .ValueField("id")
              .DescField("description");
          })
          .AddDimension("date", dimbuild => {
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

      return builder.Create<int>();
    }

    public static Cube<int> GetCubeThreeSimpleDimensionsTwoMeasuresAndThreeDateDimensionsAndTwoExtraLevels()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) =>
        {
          sourcebuild.SetSource("sales")
            .AddMapping("category", "category")
            .AddMapping("sex", "sex")
            .AddMapping("place", "place", "Country", "Region")
            .AddMapping("date", "Year", "Month", "Day");
        })
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
          dsbuild.SetName("sexes")
            .SetSourceType(DataSourceType.CSV)
            .AddField("id", 0, typeof(int))
            .AddField("description", 1, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//dimension2.csv")
                               .SetHasHeader();
            });
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
          mbuild.AddDimension("category", (dimbuild) =>
          {
            dimbuild.Source("categories")
              .ValueField("id")
              .DescField("description");
          })
          .AddDimension("sex", (dimbuild) =>
          {
            dimbuild.Source("sexes")
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
          .AddDimension("date", dimbuild => {
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

      return builder.Create<int>();
    }

    public static Cube<int> GetCubeThreeSimpleDimensionsTwoMeasuresAndThreeDateDimensionsAndTwoExtraLevelsAndTransformerSegmentDimension()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) =>
        {
          sourcebuild.SetSource("sales")
            .AddMapping("category", "category")
            .AddMapping("sex", "sex")
            .AddMapping("place", "place", "Country", "Region")
            .AddMapping("date", "Year", "Month", "Day")
            .AddMapping("items", "purchase_size");
        })
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
          dsbuild.SetName("sexes")
            .SetSourceType(DataSourceType.CSV)
            .AddField("id", 0, typeof(int))
            .AddField("description", 1, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//dimension2.csv")
                               .SetHasHeader();
            });
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
          .AddDimension("sex", (dimbuild) =>
          {
            dimbuild.Source("sexes")
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
          .AddDimension("date", dimbuild => {
            dimbuild
            .SetToDateSource(DateLevels.YEAR, DateLevels.MONTH, DateLevels.DAY)
            .SetLevelDimensions("Year", "Month", "Day");
          })
          .AddDimension("purchase_size", dimbuild =>
          {
            dimbuild
              .Source("intervals")
              .SetSourceMembersAreGenerated();
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

      return builder.Create<int>();
    }

    public static Cube<int> GetBasicCubeThreeSimpleDimensionsTwoMeasuresAndThreeTimeDimensions()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("hello")
        .SetSourceMappings((sourcebuild) =>
        {
          sourcebuild.SetSource("sales")
            .AddMapping("category", "category")
            .AddMapping("sex", "sex")
            .AddMapping("place", "place")
            .AddMapping("time", "Hour", "Minute", "Second");
        })
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
        .AddDataSource(dsbuild =>
        {
          dsbuild.SetName("sexes")
            .SetSourceType(DataSourceType.CSV)
            .AddField("id", 0, typeof(int))
            .AddField("description", 1, typeof(string))
            .SetCSVConfig(csvbuild =>
            {
              csvbuild.SetFilePath("TestData//dimension2.csv")
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
              csvbuild.SetFilePath("TestData//dimension3.csv")
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
          .AddDimension("sex", (dimbuild) =>
          {
            dimbuild.Source("sexes")
              .ValueField("id")
              .DescField("description");
          })
          .AddDimension("place", (dimbuild) =>
          {
            dimbuild.Source("places")
              .ValueField("id")
              .DescField("description");
          })
          .AddDimension("time", dimbuild => {
            dimbuild
            .SetToTimeSource(TimeLevels.HOUR, TimeLevels.MINUTES, TimeLevels.SECONDS)
            .SetLevelDimensions("Hour", "Minute", "Second");
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

      return builder.Create<int>();
    }
  }
}