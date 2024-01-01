# NSimpleOLAP 

The goal of this project is to build an embeddable .Net **OLAP** library that can be used within the context of console, desktop, or other types of applications.

One of the key motivations for this project is to educate more developers on the utility of aggregation engines beyond the field of Business Intelligence and Finance.

At the present moment this project is still in alpha stage and unstable, and only allows for some basic querying and modes of aggregation.
Some of its implementations features are experimental, or are there to allow easy testing of different opportunities for optimization or feature enhancement.
More on that later.

## Contents
- [Quick Start](##Quick-Start)
- [Configuring a Cube](Markdown/Configuration.md)
- [Loading Data with Data Sources](Markdown/Datasources.md)
- [Working with Dimensions](Markdown/DimensionsAndMeasures.md)
- [Choosing Measures](Markdown/DimensionsAndMeasures.md##Choosing-Measures)
- [Working with Metrics](Markdown/Metrics.md)
- [Working with Queries](Markdown/Querying.md)

## Quick Start

Building a Cube will require some intial setup to identify the data sources, mappings and defining the metadata.
In the following example we will build a Cube from data that is contained in CSV files, and these will be used to define the Cube dimensions and measures.


```csharp
CubeBuilder builder = new CubeBuilder();

builder.SetName("Hello World")
.SetSourceMappings((sourcebuild) =>
{
  sourcebuild.SetSource("sales")
  .AddMapping("category", "category")
  .AddMapping("sex", "sex"));
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
``` 


Creating the Cube and processing the data will be done as follows.


```csharp

var cube = builder.Create<int>();

cube.Initialize();
cube.Process();

``` 


Querying the Cube will be done by using the querying interface, here's a basic example:


```csharp

var queryBuilder = cube.BuildQuery()
  .OnRows("sex.female")
  .OnColumns("category.shoes")
  .AddMeasuresOrMetrics("quantity");

var query = queryBuilder.Create();
var result = query.StreamCells().ToList();

``` 

Also you can add some basic expressions to filter on the table facts: 

```csharp

var queryBuilder = cube.BuildQuery()
  .OnRows("sex.All")
  .OnColumns("category.All")
  .AddMeasuresOrMetrics("quantity")
  .Where(b => b.Define(x => x.Measure("quantity").IsEquals(5)));

var query = queryBuilder.Create();
var result = query.StreamCells().ToList();

``` 

Or you can add some basic expressions to filter on dimension members: 

```csharp

var queryBuilder = cube.BuildQuery()
  .OnRows("sex.All")
  .OnColumns("category.All")
  .AddMeasuresOrMetrics("quantity")
  .Where(b => b.Define(x => x.Dimension("sex").NotEquals("male")));

var query = queryBuilder.Create();
var result = query.StreamCells().ToList();

``` 
