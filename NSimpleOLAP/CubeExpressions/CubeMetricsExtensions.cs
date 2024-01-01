using NSimpleOLAP.CubeExpressions.Builder;
using System;

namespace NSimpleOLAP.CubeExpressions
{
  public static class CubeMetricsExtensions
  {
    public static MetricCubeExpressionBuilder<T> BuildMetrics<T>(this Cube<T> cube)
      where T : struct, IComparable
    {
      return new ImplementationMetricCubeExpressionBuilder<T>(cube);
    }

    private class ImplementationMetricCubeExpressionBuilder<T> : MetricCubeExpressionBuilder<T>
    where T : struct, IComparable
    {
      public ImplementationMetricCubeExpressionBuilder(Cube<T> cube)
      {
        _innerCube = cube;
        Init();
      }
    }
  }
}