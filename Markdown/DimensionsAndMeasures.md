# NSimpleOLAP 

## Working with Dimensions




### Categorical Dimension

#### Configuration Example

```csharp

builder
.SetSourceMappings((sourcebuild) =>
{
  sourcebuild.SetSource("sales")
	.AddMapping("category", "category");
}).AddDataSource(dsbuild =>
{
  dsbuild.SetName("categories")
	.SetSourceType(DataSourceType.CSV)
	.AddField("id", 0, typeof(int))
	.AddField("description", 1, typeof(string))
	.SetCSVConfig(csvbuild =>
	{
	  csvbuild
		.SetFilePath("TestData//dimension1.csv")
		.SetHasHeader();
	});
}).MetaData(mbuild =>
{
  mbuild.AddDimension("category", (dimbuild) =>
  {
	dimbuild.Source("categories")
	  .ValueField("id")
	  .DescField("description");
  });
});

```

### Categorical Dimension Levels

#### Configuration Example

```csharp

builder
.SetSourceMappings((sourcebuild) =>
{
  sourcebuild.SetSource("sales")
	.AddMapping("place", "place", "Country", "Region");
}).AddDataSource(dsbuild =>
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
	  csvbuild
	    .SetFilePath("TestData//dimension3.csv")
		.SetHasHeader();
	});
}).MetaData(mbuild =>
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

```

### Date Dimension

#### Configuration Example

```csharp

builder
.SetSourceMappings((sourcebuild) =>
{
  sourcebuild.SetSource("sales")
	.AddMapping("date", "Year", "Month", "Day");
}).MetaData(mbuild =>
{
  mbuild.AddDimension("date", dimbuild => {
	dimbuild
	.SetToDateSource(DateLevels.YEAR, DateLevels.MONTH, DateLevels.DAY)
	.SetLevelDimensions("Year", "Month", "Day");
  });
});

```

#### Available Date Segments

Segment Type | Description | Example Output
--- | --- | --- 
DAY | Days of the month between 1 and 31 | 11
MONTH | Months of the Year | May
YEAR | Year | 2020
WEEK | Week of the Year between 1 to 52 | 2020 Week 51
QUARTER | Quarter of the year | 2020 Q1
MONTH_WITH_YEAR | Months of the Year with Year | 2020 June
DATE | Date Format representation | 2020-03-15


### Time Dimension

#### Configuration Example

```csharp

builder
.AddDataSource(dsbuild =>
{
  dsbuild.SetName("sales")
	.SetSourceType(DataSourceType.CSV)
	.SetCSVConfig(csvbuild =>
	{
	  csvbuild
		.SetFilePath("TestData//tableWithTime.csv")
		.SetHasHeader();
	})
	.AddTimeField("time", 3, "hh\\:mm\\:ss");
})
.SetSourceMappings((sourcebuild) =>
{
  sourcebuild.SetSource("sales")
	.AddMapping("time", "Hour", "Minute", "Second");
}).MetaData(mbuild =>
{
  mbuild.AddDimension("time", dimbuild => {
		dimbuild
		.SetToTimeSource(TimeLevels.HOUR, TimeLevels.MINUTES, TimeLevels.SECONDS)
		.SetLevelDimensions("Hour", "Minute", "Second");
	  });
});

```

#### Available Time Segments

Segment Type | Description | Example Output
--- | --- | --- 
HOUR | Hours between 0 to 23 | 11
MINUTES | Minutes between 0 to 59 | May
SECONDS | Seconds between 0 to 59 | 2020
TIME | Timetamp format in hh:mm:ss | 13:05:43

### Transformed Dimension

#### Configuration Example

```csharp

builder
.SetSourceMappings((sourcebuild) =>
{
  sourcebuild.SetSource("sales")
	.AddMapping("items", "purchase_size");
}).AddDataSource(dsbuild =>
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
}).MetaData(mbuild =>
{
  mbuild.AddDimension("category", (dimbuild) =>
  {
	dimbuild
	  .Source("intervals")
	  .SetSourceMembersAreGenerated();
  });
});

```

## Choosing Measures



#### Configuration Example

```csharp

builder
.AddDataSource(dsbuild =>
{
  dsbuild.SetName("sales")
	.SetSourceType(DataSourceType.CSV)
	.SetCSVConfig(csvbuild =>
	{
	  csvbuild
		.SetFilePath("TestData//tableWithTime.csv")
		.SetHasHeader();
	})
	.AddField("category", 0, typeof(int))
	.AddField("expenses", 4, typeof(double))
	.AddField("items", 5, typeof(int));
}).MetaData(mbuild =>
{
  mbuild.AddMeasure("spent", mesbuild =>
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

```

