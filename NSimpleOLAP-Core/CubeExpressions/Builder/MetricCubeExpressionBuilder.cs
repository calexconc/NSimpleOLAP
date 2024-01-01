using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.Parsers;
using NSimpleOLAP.Schema;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.CubeExpressions.Builder
{
  public abstract class MetricCubeExpressionBuilder<T>
    where T : struct, IComparable
  {
    protected Cube<T> _innerCube;
    private NamespaceResolver<T> _resolver;
    private MetricsExpressionComposer<T> _metricExpressionEvaluator;
    protected Dictionary<string, MetricExpressionBuilder<T>> _expressionBuilders;

    protected void Init()
    {
      _resolver = new NamespaceResolver<T>(_innerCube);
      _metricExpressionEvaluator = new MetricsExpressionComposer<T>(_resolver);
      _expressionBuilders = new Dictionary<string, MetricExpressionBuilder<T>>();
    }

    public MetricCubeExpressionBuilder<T> Add(string name, Action<ExpressionBuilder<T>> abuilder)
    {
      var builder = new MetricExpressionBuilder<T>(_resolver);
      var expressionBuilder = builder.Metric(name);

      abuilder(expressionBuilder);
      _expressionBuilders.Add(name, builder);

      return this;
    }

    public Cube<T> AddTextExpression(string name, Type type, string expression)
    {
      var metricConfig = new MetricConfig
      {
        Name = name,
        DataType = type,
        MetricFunction = expression
      };
      var metric = new Metric<T>(metricConfig);

      try
      {
        if (!string.IsNullOrEmpty(metricConfig.MetricFunction))
        {
          metric.MetricExpression = _metricExpressionEvaluator.Create(metricConfig.Name, metricConfig.MetricFunction);
          _innerCube.Schema.Metrics.Add(metric);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        throw;
      }

      return _innerCube;
    }

    public void Create()
    {
      foreach (var item in _expressionBuilders)
      {
        var expression = item.Value.Create();

        _innerCube.Schema.Metrics.Add(new Metric<T>
        {
          MetricExpression = expression,
          Name = item.Key,
          DataType = expression.ReturnType,
          Config = new MetricConfig
          {
            DataType = expression.ReturnType,
            Name = item.Key
          }
        });
      }
    }
  }
}