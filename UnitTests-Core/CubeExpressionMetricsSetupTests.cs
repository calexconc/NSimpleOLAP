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
  public class CubeExpressionMetricsSetupTests
  {
    private Cube<int> cube;

    public CubeExpressionMetricsSetupTests()
    {
      Init();
    }

    public void Init()
    {
      cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2();
      cube.Initialize();
    }

    [OneTimeTearDown]
    public void Dispose()
    {
      cube.Dispose();
    }

    [Test]
    public void Basic_Metric_Expression_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("teste1", exb => exb.Expression(e => e.Set("quantity").Sum(10)))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["teste1"]);
    }

    [Test]
    public void Basic_Composite_Metric_Expression_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("teste2DoubleSum", 
          exb => exb.Expression(e => e.Set("quantity").Sum(e2 => e2.Set("quantity").Value())))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["teste2DoubleSum"]);
    }

    [Test]
    public void Basic_Multiple_Metric_Expressions_Setup_Test()
    {
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
    }

    [Test]
    public void Basic_Metric_Multiplication_Expression_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("testeMultiply1", exb => exb.Expression(e => e.Set("quantity").Multiply(3)))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["testeMultiply1"]);
    }

    [Test]
    public void Basic_Metric_Division_Expression_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("testeDivide1", exb => exb.Expression(e => e.Set("quantity").Divide(2)))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["testeDivide1"]);
    }

    [Test]
    public void Basic_Metric_Subtract_Expression_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("testeSubtract1", exb => exb.Expression(e => e.Set("quantity").Subtract(10)))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["testeSubtract1"]);
    }

    [Test]
    public void Composite_Metric_Multiplication_Expression_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("testeMultiply2", exb => exb.Expression(e => e.Set("quantity").Multiply(ex => ex.Set("spent").Value())))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["testeMultiply2"]);
    }

    [Test]
    public void Composite_Metric_Division_Expression_Setup_Test()
    {
      cube.BuildMetrics()
        .Add("testeDivide2", exb => exb.Expression(e => e.Set("spent").Divide(ex => ex.Set("quantity").Value())))
        .Create();

      Assert.IsNotNull(cube.Schema.Metrics["testeDivide2"]);
    }

  }
}
