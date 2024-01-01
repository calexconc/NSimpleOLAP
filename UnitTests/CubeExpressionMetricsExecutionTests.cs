using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP;
using NSimpleOLAP.Query;
using NSimpleOLAP.Query.Builder;
using NUnit.Framework;
using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.CubeExpressions;

namespace UnitTests
{
  [TestFixture]
  public class CubeExpressionMetricsExecutionTests
  {
    [Test]
    public void Basic_Metric_Expression_Execution_Test()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();

        cube.BuildMetrics()
        .Add("teste1", exb => exb.Expression(e => e.Set("quantity").Sum(10)))
        .Create();

        Assert.IsNotNull(cube.Schema.Metrics["teste1"]);

        cube.Process();

        var cell = cube.Cells.Take(1).FirstOrDefault();


        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var value = (int) cell.Values[cube.Schema.Metrics["teste1"].ID];

        Assert.AreEqual(valueMeasure + 10, value);
      }
    }

    [Test]
    public void Basic_Composite_Metric_Expression_Execution_Test()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();

        cube.BuildMetrics()
        .Add("teste2DoubleSum",
          exb => exb.Expression(e => e.Set("quantity").Sum(e2 => e2.Set("quantity").Value())))
        .Create();

        Assert.IsNotNull(cube.Schema.Metrics["teste2DoubleSum"]);

        cube.Process();

        var cell = cube.Cells.Take(1).FirstOrDefault();


        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var value = (int)cell.Values[cube.Schema.Metrics["teste2DoubleSum"].ID];

        Assert.AreEqual(valueMeasure*2, value);
      }
    }

    [Test]
    public void Basic_Multiple_Metric_Expressions_Execution_Test()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();

        cube.BuildMetrics()
        .Add("testeAverage",
          exb => exb.Expression(e => e.Set("quantity").Average()))
        .Add("testeMax",
          exb => exb.Expression(e => e.Set("quantity").Max()))
        .Add("testeMin",
          exb => exb.Expression(e => e.Set("quantity").Min()))
        .Create();

        Assert.IsNotNull(cube.Schema.Metrics["testeAverage"]);
        Assert.IsNotNull(cube.Schema.Metrics["testeMax"]);
        Assert.IsNotNull(cube.Schema.Metrics["testeMin"]);

        cube.Process();

        var cell = cube.Cells.Take(1).FirstOrDefault();


        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var valueAverage = cell.Values[cube.Schema.Metrics["testeAverage"].ID];
        var valueMax = cell.Values[cube.Schema.Metrics["testeMax"].ID];
        var valueMin = cell.Values[cube.Schema.Metrics["testeMin"].ID];

        Assert.AreEqual(valueMeasure / 24, valueAverage);
        Assert.AreEqual(101, valueMax);
        Assert.AreEqual(1, valueMin);
      }
    }

    [Test]
    public void Basic_Metric_Multiplication_Expression_Execution_Test()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();

        cube.BuildMetrics()
        .Add("testeMultiply1", exb => exb.Expression(e => e.Set("quantity").Multiply(3)))
        .Create();

        Assert.IsNotNull(cube.Schema.Metrics["testeMultiply1"]);

        cube.Process();

        var cell = cube.Cells.Take(1).FirstOrDefault();


        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var value = (int)cell.Values[cube.Schema.Metrics["testeMultiply1"].ID];

        Assert.AreEqual(valueMeasure * 3, value);
      }
    }

    [Test]
    public void Basic_Metric_Division_Expression_Execution_Test()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();

        cube.BuildMetrics()
        .Add("testeDivide1", exb => exb.Expression(e => e.Set("quantity").Divide(2)))
        .Create();

        Assert.IsNotNull(cube.Schema.Metrics["testeDivide1"]);

        cube.Process();

        var cell = cube.Cells.Take(1).FirstOrDefault();


        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var value = (double)cell.Values[cube.Schema.Metrics["testeDivide1"].ID];

        Assert.AreEqual(valueMeasure / 2, value);
      }
    }

    [Test]
    public void Basic_Metric_Subtract_Expression_Execution_Test()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();

        cube.BuildMetrics()
        .Add("testeSubtract1", exb => exb.Expression(e => e.Set("quantity").Subtract(10)))
        .Create();

        Assert.IsNotNull(cube.Schema.Metrics["testeSubtract1"]);

        cube.Process();

        var cell = cube.Cells.Take(1).FirstOrDefault();


        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var value = (int)cell.Values[cube.Schema.Metrics["testeSubtract1"].ID];

        Assert.AreEqual(valueMeasure - 10, value);
      }
    }

    [Test]
    public void Composite_Metric_Multiplication_Expression_Execution_Test()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();

        cube.BuildMetrics()
        .Add("testeMultiply2", exb => exb.Expression(e => e.Set("quantity").Multiply(ex => ex.Set("spent").Value())))
        .Create();

        Assert.IsNotNull(cube.Schema.Metrics["testeMultiply2"]);

        cube.Process();

        var cell = cube.Cells.Take(1).FirstOrDefault();


        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var valueMeasure2 = (double)cell.Values[cube.Schema.Measures["spent"].ID];
        var value = (double)cell.Values[cube.Schema.Metrics["testeMultiply2"].ID];

        Assert.AreEqual(valueMeasure * valueMeasure2, value);
      }
    }

    [Test]
    public void Composite_Metric_Division_Expression_Execution_Test()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();

        cube.BuildMetrics()
        .Add("testeDivide2", exb => exb.Expression(e => e.Set("spent").Divide(ex => ex.Set("quantity").Value())))
        .Create();

        Assert.IsNotNull(cube.Schema.Metrics["testeDivide2"]);

        cube.Process();

        var cell = cube.Cells.Take(1).FirstOrDefault();


        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var valueMeasure2 = (double)cell.Values[cube.Schema.Measures["spent"].ID];
        var value = (double)cell.Values[cube.Schema.Metrics["testeDivide2"].ID];

        Assert.AreEqual(valueMeasure2 / valueMeasure, value);
      }
    }

    [Test]
    public void Basic_Multiple_Metric_Expressions_Execution_With_Query_On_Metric_Test()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();

        cube.BuildMetrics()
        .Add("testeAverage",
          exb => exb.Expression(e => e.Set("quantity").Average()))
        .Add("testeMax",
          exb => exb.Expression(e => e.Set("quantity").Max()))
        .Add("testeMin",
          exb => exb.Expression(e => e.Set("quantity").Min()))
        .Create();

        Assert.IsNotNull(cube.Schema.Metrics["testeAverage"]);
        Assert.IsNotNull(cube.Schema.Metrics["testeMax"]);
        Assert.IsNotNull(cube.Schema.Metrics["testeMin"]);

        cube.Process();

        var queryBuilder = cube.BuildQuery()
            .OnColumns("sex.All")
            .AddMeasuresOrMetrics("testeAverage");

        var query = queryBuilder.Create();
        var result = query.StreamRows().ToList();

        Assert.IsTrue(result.Count == 2);
      }
    }

    [Test]
    public void Composite_Metric_Multiplication_Text_Expression_Execution_Test()
    {
      using (var cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2())
      {
        cube.Initialize();

        cube.BuildMetrics()
        .AddTextExpression("testeMultiply3", typeof(double), "quantity * spent");

        Assert.IsNotNull(cube.Schema.Metrics["testeMultiply3"]);

        cube.Process();

        var cell = cube.Cells.Take(1).FirstOrDefault();


        var valueMeasure = (int)cell.Values[cube.Schema.Measures["quantity"].ID];
        var valueMeasure2 = (double)cell.Values[cube.Schema.Measures["spent"].ID];
        var value = (double)cell.Values[cube.Schema.Metrics["testeMultiply3"].ID];

        Assert.AreEqual(valueMeasure * valueMeasure2, value);
      }
    }
  }
}
