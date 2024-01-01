# NSimpleOLAP 

## Working with Queries

A data cube is nothing if it cannot be queried, the **NSimpleOlap** fluent query API borrows many concepts from the MDX query language. You will need to get familiarized to specify your rows and columns as tuples. In general that is no different as setting paths or using something like xpath in XSL or any DOM XML API. You are not only slicing the cube but you are also defining what data hierarchies you want to visualize.

### Getting Started
Defining a simple query and sending the output to the text console.

```csharp

cube.Process();
 
var queryBuilder = cube.BuildQuery()
    .OnRows("category.All.place.Paris")
    .OnColumns("sex.All")
    .AddMeasuresOrMetrics("quantity");
 
var query = queryBuilder.Create();
 
query.StreamRows().RenderInConsole();

```

Outputs to the console the following result:

```text
|                                | sex male  | sex female
    category toys,place Paris    |     12     |      8
 category furniture,place Paris  |     2      |      30
  category clothes,place Paris   |            |      44
```

Understand that you will need to select what dimensions are part of the rows and what are part of the columns, this is taken from the MDX.

You can also select both measures and metrics at the same time in a query.

```csharp

var queryBuilder = cube.BuildQuery()
    .OnColumns("sex.All")
    .AddMeasuresOrMetrics("quantity", "MaxOnQuantity", "MinOnQuantity");
 
var query = queryBuilder.Create();
var result = query.StreamRows().ToList();

```

When querying datasets with **NSimpleOlap** you will not be getting a table output but a stream of cells. Although you can use the *RenderInConsole* extension method to output to a tabular form in the console.

The reason for this choice is that in most cases the output of a query is not a table but a Tensor, or a matrix that can have scalars, vectors and other matrices as its elements. So for ease of use with Linq I decided to keep the output as a sequence of cells.
This can be changed in any other format by applying transformations, since each cell has the necessary metadat to identify what are its row and column dimensions.

### Querying with the *Where* clause

We can make filters on the aggregate values and also on the values in the facts. That means on the first case we are filtering on the aggregated values in the cells, and in the second case we are filtering the source data and creating new aggregations.

First we will filter on the aggregates.

```csharp

var queryBuilder = cube.BuildQuery()
    .OnRows("category.All.place.All")
    .OnColumns("sex.All")
    .AddMeasuresOrMetrics("quantity")
    .Where(b => b.Define(x => x.Dimension("sex").NotEquals("male")));
 
var query = queryBuilder.Create();
var result = query.StreamRows().ToList();

```

Then we will reduce the scope of the data by filtering on a measure.

```csharp

var queryBuilder = cube.BuildQuery()
    .OnRows("category.All.place.All")
    .OnColumns("sex.All")
    .AddMeasuresOrMetrics("quantity")
    .Where(b => b.Define(x => x.Measure("quantity").IsEquals(5)));
 
var query = queryBuilder.Create();
var result = query.StreamRows().ToList();

```

Making filters on the facts will generate a cube with a smaller subset of data. This makes sense since the main Cube will have aggregated away the context of the facts, and any operation that requires digging on the source facts will require generating a new Cube to represent those aggregations.

#### Using **AND** and **OR** logical Operators

It is also possible to use **AND** and **OR** operators to perform logical filters both on dimension and on measures.

```csharp

var queryBuilder = cube.BuildQuery()
    .OnRows("category.All.place.All")
    .OnColumns("sex.All")
    .AddMeasuresOrMetrics("quantity")
    .Where(b => b.Define(a => a.And(x => x.Dimension("category").NotEquals("clothes"),
    x => x.Measure("quantity").GreaterOrEquals(2))));
 
var query = queryBuilder.Create();
var result = query.StreamRows().ToList();

```

This is done through the fluent query API that allows for composition of complex logical expressions. 

```csharp

var queryBuilder = cube.BuildQuery()
    .OnRows("category.All.place.All")
    .OnColumns("sex.All")
    .AddMeasuresOrMetrics("quantity")
    .Where(b => b.Define(a => a.Or(x => x.Dimension("category").IsEquals("clothes"),
    x => x.Dimension("category").IsEquals("shoes"))));
 
var query = queryBuilder.Create();
var result = query.StreamRows().ToList();

```

It is also possible to compose more complex logical expressions mixing several **OR** and **AND** clauses.

```csharp

// this is equivalent to: category = clothes OR (category = toys AND quantity > 5)

var queryBuilder = cube.BuildQuery()
.OnRows("category.All.place.All")
.OnColumns("sex.All")
.AddMeasuresOrMetrics("quantity")
.Where(b => b.Define(a => a.Or(x => x.Dimension("category").IsEquals("clothes"),
    x => x.Block(x1 => 
    x1.And(x2 => x2.Dimension("category").IsEquals("toys"), 
    x2 => x2.Measure("quantity").GreaterThan(5)))
    )
    )
);
 
var query = queryBuilder.Create();
var result = query.StreamRows().ToList();

```


#### Using the **NOT** logical Operator to Negate

In case there is need to, a complete logical expression can be negated using the **NOT** operator.

```csharp

// this is equivalent to: category = clothes OR NOT(category = toys AND quantity > 5)

var queryBuilder = cube.BuildQuery()
.OnRows("category.All.place.All")
.OnColumns("sex.All")
.AddMeasuresOrMetrics("quantity")
.Where(b => b.Define(a => a.Or(x => x.Dimension("category").IsEquals("clothes"),
    x => x.Not(x1 => 
    x1.And(x2 => x2.Dimension("category").IsEquals("toys"), 
    x2 => x2.Measure("quantity").GreaterThan(5)))
    )
    )
);
 
var query = queryBuilder.Create();
var result = query.StreamRows().ToList();

```

#### Get Row and Column Totals

When querying aggregate values it is also important to get the values of the Row and Column totals, this will be sum of the measures or metrics that were selected in the query.
 
```csharp

// this is equivalent to: category = clothes OR NOT(category = toys AND quantity > 5)

var queryBuilder = cube.BuildQuery()
        .OnRows("sex.All")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity")
        .GetColumnTotals();
 
var query = queryBuilder.Create();
var result = query.StreamRows().ToList();

```

This renders for column totals as an extra row with the total for that column.
```text
|                | category shoes
   sex female    |       137
   sex No Data   |       105
    sex male     |        5
  Sum Row Total  |       247
```

When trying to get the row totals this will add an extra column for the row totals.

```csharp

// this is equivalent to: category = clothes OR NOT(category = toys AND quantity > 5)

var queryBuilder = cube.BuildQuery()
        .OnRows("sex.All")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity")
        .GetRowTotals();
 
var query = queryBuilder.Create();
var result = query.StreamRows().ToList();

```


```text
|              | category shoes  |category Total
  sex female   |       137        |  137
  sex No Data  |       105        |  105
   sex male    |        5         |   5
```


You can join it all together on a single output.

```csharp

// this is equivalent to: category = clothes OR NOT(category = toys AND quantity > 5)

var queryBuilder = cube.BuildQuery()
        .OnRows("sex.All")
        .OnColumns("category.shoes")
        .AddMeasuresOrMetrics("quantity")
        .GetColumnTotals()
        .GetRowTotals();
 
var query = queryBuilder.Create();
var result = query.StreamRows().ToList();

```


```text
|                | category shoes  |category Total
   sex female    |       137        |  137
   sex No Data   |       105        |  105
    sex male     |        5         |   5
  Sum Row Total  |       247        |  247
```

#### Get Row and Column Base Totals

These totals are the sample totals from the facts that can spans more than the value of the sums of the columns and rows that were selected.
This can be important when calculating ratios based on the full sample totals and not just on the outputted values.

```csharp

// this is equivalent to: category = clothes OR NOT(category = toys AND quantity > 5)

var queryBuilder = cube.BuildQuery()
    .OnRows("sex.All")
    .OnColumns("category.shoes", "category.toys")
    .AddMeasuresOrMetrics("quantity")
    .GetColumnTotals()
    .GetBaseColumnTotals()
    .GetBaseRowTotals();
 
var query = queryBuilder.Create();
var result = query.StreamRows().ToList();

```


```text
|                 | category shoes  | category toys  |
    sex female    |       137        |       134       |442
   sex No Data    |       105        |                 |105
     sex male     |        5         |       61        | 81
  Sum Row Total   |       247        |       195       |628
  Base Row Total  |       247        |       195       |
```