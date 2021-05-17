# NSimpleOLAP 

## Loading Data with Data Sources

OLAP engines are a type of database for aggregations, but you require to have a source of data that will be the feedstock for any aggregation that you can configure.
Given that data Cubes often works better with denormalized tables it is necessary load the data in a format that fits this need.
What in the BI field people call ETL (Extract, Transform and Load) will be something you will also be doing for the **NSimpleOLAP** albeit in a smaller scale.
You can load the data in a form that is pratically raw, except for the necessary ids for the dimension members, or use the dimension transformer facility to set dimensions from continuous variables.
But you will need to be aware that not all data fits well, specially when you have cases of many-to-many relationships. Like having a movie that can be in several genres at the same time.
In this case you cannot list the same movie for every genre with a sales figure, because the OLAP engine will aggregate every record and multiply the same value.
Instead you will need to transform each genre into it's own column, and work with it as a dimension.
This is in fact an example of what an ETL would have to do, and there are many more possible cases where data needs to be transformed.

### Adding a Data Source
Adding a data source is very simple, you can use the *CubeBuilder* and set into the *AddDataSource* method the source definition that you want.
**NSimpleOLAP** allows for a variety of data sources to allow for flexibility and different use case. At the moment these are the supported data sources:

- CSV files : Simple Comma Separated text files.
- Databases : Connect to a database and query the tables you require.
- Object Mapper : Use POCO collections and map the object properties to the columns your require.
- Ado.Net Tables : Use Ado.Net data sets to load data.
- Dimension Transform : Use when you need to change a continuous variable in your facts table into a categoric dimension.

### CSV Data Source

```csharp
var builder = new CubeBuilder();

builder.AddDataSource(dsbuild =>
{
  dsbuild.SetName("sales")
	.SetSourceType(DataSourceType.CSV)
	.SetCSVConfig(csvbuild =>
	{
	  csvbuild.SetFilePath("TestData//facts.csv")
	  .SetHasHeader();
	})
	.AddField("category", 0, typeof(int))
	.AddField("expenses", 3, typeof(double))
	.AddField("items", 4, typeof(int));
});

```

### Object Mapper Data Source

```csharp

builder.AddDataSource(dsbuild =>
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
});

```


### Ado.Net Dataset Data Source

```csharp

builder.AddDataSource(dsbuild =>
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
});

```


### Database Data Source

```csharp

builder.AddDataSource(dsbuild =>
{
  dsbuild.SetName("genderes")
	.SetSourceType(DataSourceType.DataBase)
	.AddField("id", typeof(int))
	.AddField("description", 1, typeof(string))
	.SetDBConfig(dbBuild => {
	  dbBuild.SetConnection("LITE")
	  .SetQuery("SELECT id, description FROM Genders");
	});
});

```