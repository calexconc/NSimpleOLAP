# NSimpleOLAP 

## Working with Metrics

Metrics are composite functions that calculate new types of indicators based on the original agregations, these can be averages, minimum, maximum, percentages or any other type of function primitive or composition of functions.

Metrics are always calculated after the measure agregations are completed for the Cube cell, in the near future it will be possible to use metrics in other metrics. But this will create an order dependency that will needs to be managed by the user at configuration time.

In terms of querying metrics can be accessed as if these were measures, the output cells don't make a distinction between the two.

### Why Metrics Are Needed

Metrics are needed for the following set of reasons:

- They conveniently allow for retaining some aspects of the original facts, like minimum and maximum values.
- It is possible to create more complex aggregations like averages, percentages, and ratios.
- It allows for querying ready made results without the need to do further calculations at query time.


#### Configuration Example

```csharp

builder.MetaData(mbuild =>
{
  mbuild.AddMetric("teste1", metricBuilder =>
  {
    metricBuilder
      .SetType(typeof(int))
      .SetExpression("quantity + 10");
  });
});

```

#### Adding Metrics To An Existing Cube

```csharp

cube.BuildMetrics()
    .AddTextExpression("testeMultiply3", typeof(int), "quantity * 10");

```

### Metrics Text Expressions

Adding metrics through text expressions is the easiest way to define triggers, and is the only way that is possible to add triggers at configuration time.


#### Parser Limitations

The parser will allow for basic arithmetic operations, like sum, subtraction, division and multiplication. And it's possible to callout measures and use predefined functions.

#### Available Functions To Use in Metrics

Function | Description | Example
--- | --- | --- 
Min | Minimum value on a given measure for a cell | MIN quantity
Max | Maximum value on a given measure for a cell | MAX quantity
Average | Calculated Average value on a given measure for a cell | AVG quantity
Sqrt | Square Root on a given value, aggregated or not, for a cell | SQRT quantity
Abs | Absolute value, aggregated or not, for a cell | ABS quantity
Ln | Natural logarithm of a value, aggregated or not, for a cell | LN quantity
Exp | Exponential value, aggregated or not, for a cell | EXP quantity

### Navigating the Schema for Metrics

```csharp

foreach (var metric in cube.Schema.Metrics)
{
  System.Console.WriteLine(metric.Name);
}

```