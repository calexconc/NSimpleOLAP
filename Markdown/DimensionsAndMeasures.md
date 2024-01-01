# NSimpleOLAP 

## Working with Dimensions

In the **NSimpleOLAP** you will use categorical dimensions as the main feature to model the data aggregations that will be available within the data Cube.
These will allow you to explore the data, and through the aggregations within the Cube it will be possible to find patterns on how the data self-organizes.
You can discover how a measure is distributed and find hierarchies within the data, this can be useful when you need to what aspects have greater impact.
Like when most sales occur, what groups of people buys the most of a certain product, what type of car has more sales in any given country, etc. ... 


Available types of dimensions you can use:
- Categorical Dimension;
- Categorical Dimension With Levels;
- Date Dimension;
- Time Dimension;
- Continuous Variable Transform Dimension.


### Categorical Dimension
This is the simplest type of categorical dimension that you can use, it will define a set of member tuples with a given or generated id.


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

### Categorical Dimension With Levels
This form of the categorical dimension will be used when in your data source you have extra information about your primary entity.
If you have for example a car model, you can enrich the data with brand name, type of vehicle and other categorical features.
These will then be mapped to their specific dimension, and on the facts mappings the column with the primary dimension will be used to map to the extra dimensions.

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
The date dimenstion is a quasi categorical dimension generated from the facts data, since there are specific rules for how dates behave the members of each date type specific dimension will be automatically generated.
Except in the case when the date itself will be used as dimension member, then the dimension member will be inserted when the facts are being added to the Cube.

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

This dimension type will require you to define which types of date segments you will want to use, this will be important for querying as well for memory management.
Since choosing a dimension that will generate a large number of segments can incur on a heavy memory consumption.

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

The time dimenstion is a quasi categorical dimension generated from the facts data, since there are specific rules for how time information behave the members of each time type specific dimension will be automatically generated.
Except in the case when the time stamp itself will be used as dimension member, then the dimension member will be inserted when the facts are being added to the Cube.

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

This dimension type will require you to define which types of time segments you will want to use, this will be important for querying as well for memory management.
Since choosing a dimension that will generate a large number of segments can incur on a heavy memory consumption.

Segment Type | Description | Example Output
--- | --- | --- 
HOUR | Hours between 0 to 23 | 11
MINUTES | Minutes between 0 to 59 | May
SECONDS | Seconds between 0 to 59 | 2020
TIME | Timetamp format in hh:mm:ss | 13:05:43

### Continuous Variable Transform Dimension

A continuous variable transform dimension is a quasi categorical dimension that will be generated according to sets of rules that map member segments to continuous variables within the facts table.
This will allow you to build extra data features based of intervals, so that you can build age ranges, or group several categorical members into a limited number of segments.
For example, if you want to say that a house that has less that 2 rooms as a small hourse, from 2 to 3 as medium, and everything from 4 is a big house.

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

### Navigating the Schema for Dimensions

You can explore the dimensions on the Cube by accessing the its Schema, you will have there every dimension accessible by name and by ID.
From there you can explore the dimension members, also by name and ID. This can help you if you need to make programmatic queries, or set values for UI interfaces. 

```csharp

var dimenstionID = cube.Schema.Dimensions["gender"].ID;
var dimenstionName = cube.Schema.Dimensions["gender"].Name;
var memberName = cube.Schema.Dimensions["gender"].Members["male"].Name;

```

You can also add new dimension members, just keep in mind to do it in a way that is consistent with your original data source.

```csharp

cube.Schema.Dimensions["cars"]
  .Members.Add(new Member<int>() 
    { ID = cube.Schema.Dimensions["cars"].Members.Count + 1
	  , Name = "Tesla" });

```

If you need to refresh all data in the Cube, all of the new member inserts will be lost if these aren't in the original data source.


## Choosing Measures

Measures are continuous variables that will be the main source of aggregations for the data cubes, only numerical data types can be used at the moment.
Since there are no custom settings for special types of aggregation, besides doing sums on the incoming rows.
When selecting the measures that you want to be part of the data features of your Cube, choose continuous variables that make sense to be aggregated like quantities, sales values, or expenses.
Age values or prices, although continuous variables aren't suited to be aggregated in most cases. Since these aggregations will only make sense as averages, or minimum or maximum values but not as sums.


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

