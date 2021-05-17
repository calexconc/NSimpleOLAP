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

#### Fluent API Summary Reference

Method | Description
--- | --- 
AddDataSource | Add a new data source to be loaded into the CubeBuilder.
SetName | Set the name of data source to be referenced through the Metadata section.
SetSourceType | Sets the type of data source and the load mode.
AddField | Add a field mapping.
AddDateField | Add a date field mapping.
SetCSVConfig | Configure CSV file settings.
SetObjectMapperConfig | Add object mapping to data source row.
SetDataTableConfig | Set data source data table.
SetDBConfig | Configures connection to be used and source SQL query.
SetTransformerTableConfig | Configure transformer list to process a continuous variable based in the facts table.

### CSV Data Source

Flat files are the easiest data sources to setup, and there are a lot of open data sets available in this format.

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
Note: This data source requires mapping columns by index.

### Object Mapper Data Source

With Object Mapper you can easily integrate **NSimpleOLAP** on your application code.
You just need to set the collection and map the POCO object to the array row.

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
Note: This data source requires mapping columns by name.

### Ado.Net Dataset Data Source

The Dataset source is the easiest to configure, provided that your column mappings match in terms of name and type.

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

Note: This data source requires mapping columns by name.

### Database Data Source

Loading data through a database is easy to setup in the fluent API, but will require some extra configuration.
This is due that this feature is implemented using the Database Provider pattern, that allows to use any database as long as there is a Data Provider implemented.

```csharp

builder.AddDataSource(dsbuild =>
{
  dsbuild.SetName("genderes")
	.SetSourceType(DataSourceType.DataBase)
	.AddField("id", typeof(int))
	.AddField("description", typeof(string))
	.SetDBConfig(dbBuild => {
	  dbBuild.SetConnection("LITE")
	  .SetQuery("SELECT id, description FROM Genders");
	});
});

```

Don't forget to setup your App.Config, so that you have a valid connection string and working a Database Provider.

```xml
<configuration>
...
  <connectionStrings>
    <add name="LITE" providerName="System.Data.SQLite" connectionString="Data Source=TestData\SQLite\TestData.db3;Version=3;Pooling=True;Max Pool Size=100;Read Only=True;" />
  </connectionStrings>
...
  <system.data>
    <DbProviderFactories>
	  <add name="SQLite Data Provider" invariant="System.Data.SQLite" description=".NET Framework Data Provider for SQLite" type="System.Data.SQLite.SQLiteFactory, System.Data.SQLite" />
	</DbProviderFactories>
  </system.data>
...
</configuration>
```

Note: This data source requires mapping columns by name.

#### Database Data Source .Net Core

Contrary to .Net Framework, .Net Core doesn't have a single configuration schema. It has multiple configuration schemas.
This and the fact that in .Net Core 3.1 the Database Provider pattern is kinda broken, meant that some changes to the fluent API were required.
So the setup is as follows.

```csharp

var confbuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
IConfiguration externalConfig = confbuilder.Build();

builder.AddDbConnection("LITE", dbbuilder =>
{
  dbbuilder
	.SetConnectionString(ConfigurationExtensions.GetConnectionString(externalConfig, "LITE"))
	.SetProviderName("System.Data.SqlClient")
	.SetDbFactory(SQLiteFactory.Instance);
})
.AddDataSource(dsbuild =>
{
  dsbuild.SetName("categories")
	.SetSourceType(DataSourceType.DataBase)
	.AddField("id", typeof(int))
	.AddField("description", typeof(string))
	.SetDBConfig(dbBuild =>
	{
	  dbBuild.SetConnection("LITE", "System.Data.SQLite")
	  .SetQuery("SELECT id, description FROM Categories");
	});
});

```

The `AddDbConnection` method allows to inject the database configuration to create a connection to query the database.

### Dimension Transform Data Source

This more than a datasource it is a transform operation, this will need a chaining of metadata mapping and the facts field mappings.

```csharp

builder.AddDataSource(dsbuild =>
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
});

```