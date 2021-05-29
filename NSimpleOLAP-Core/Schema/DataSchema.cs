using NSimpleOLAP.Common.Interfaces;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.Data;
using NSimpleOLAP.Common;
using NSimpleOLAP.Storage.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using NSimpleOLAP.Parsers;
using NSimpleOLAP.Common.Utils;

namespace NSimpleOLAP.Schema
{
  /// <summary>
  /// Description of DataSchema.
  /// </summary>
  public class DataSchema<T> : IDisposable, IProcess
    where T : struct, IComparable
  {
    private DataSourceCollection _datasources;
    private MetricsExpressionComposer<T> _metricExpressionEvaluator;

    public DataSchema(IMemberStorage<T, Dimension<T>> dimstorage,
                      IMemberStorage<T, Measure<T>> messtorage,
                     IMemberStorage<T, Metric<T>> metstorage)
    {
      Dimensions = new DimensionCollection<T>(dimstorage);
      Measures = new MeasuresCollection<T>(messtorage);
      Metrics = new MetricsCollection<T>(metstorage);
    }

    public DataSchema(CubeConfig config,
                      DataSourceCollection datasources,
                      IMemberStorage<T, Dimension<T>> dimstorage,
                      IMemberStorage<T, Measure<T>> messtorage,
                      IMemberStorage<T, Metric<T>> metstorage) : this(dimstorage, messtorage, metstorage)
    {
      _datasources = datasources;
      this.Config = config.MetaData;
      _metricExpressionEvaluator = new MetricsExpressionComposer<T>(new NamespaceResolver<T>(this));
      this.Initialize();
    }

    public MetaDataConfig Config
    {
      get;
      set;
    }

    public DimensionCollection<T> Dimensions
    {
      get;
      private set;
    }

    public MeasuresCollection<T> Measures
    {
      get;
      private set;
    }

    public MetricsCollection<T> Metrics
    {
      get;
      private set;
    }

    #region IDisposable implementation

    public void Dispose()
    {
      this.Dimensions.Dispose();
      this.Measures.Dispose();
      this.Metrics.Dispose();
    }

    #endregion IDisposable implementation

    #region private members

    private void Initialize()
    {
      this.InitializeDimensions();
      this.InitializeMeasures();
      this.InitializeMetrics();
    }

    private void InitializeDimensions()
    {
      var levels = new Dictionary<string, string[]>();

      foreach (DimensionConfig item in this.Config.Dimensions)
      {
        if (item.DimensionType == DimensionType.Numeric 
          && !_datasources.ContainsKey(item.Source))
          throw new Exception($"Datasource {item.Source} does not exist in sources definitions.");

        switch (item.DimensionType)
        {
          case DimensionType.Numeric:
            HandleInitNumericDimension(item);
            break;
          case DimensionType.Date:
            HandleInitDateDimension(item);
            break;
          case DimensionType.Time:
            HandleInitTimeDimension(item);
            break;
          case DimensionType.Levels:
            HandleInitLevelDimension(item, levels);
            break;
        }
      }

      foreach (var parentLevel in levels.Keys)
      {
        var parentDim = (DimensionLevel<T>)this.Dimensions[parentLevel];

        foreach (var child in parentDim.LevelDimensions)
        {
          foreach (var child2 in parentDim.LevelDimensions)
          {
            if (!child.ID.Equals(child2.ID)
              && !child2.ID.Equals(parentDim)
              && child.LevelPosition <= child2.LevelPosition)
              child.LevelDimensions.Add(child2);
          }
        }
      }
    }

    private void HandleInitNumericDimension(DimensionConfig item)
    {
      Dimension<T> ndim = new Dimension<T>(item, _datasources[item.Source]) { Name = item.Name };
      this.Dimensions.Add(ndim);
    }

    private void HandleInitDateDimension(DimensionConfig item)
    {
      var dtLevels = new List<DimensionDate<T>>();

      for (var i = 0; i < item.DateLevels.Count; i++)
      {
        var ndim = new DimensionDate<T>(item, item.DateLevels[i], i)
        {
          Name = item.LevelLabels[i],
        };
        dtLevels.Add(ndim);
        this.Dimensions.Add(ndim);
      }

      foreach (var dtItem in dtLevels)
      {
        var query = dtLevels
          .Where(x => x.LevelPosition != dtItem.LevelPosition)
          .ToList();

        query.ForEach(x => dtItem.LevelDimensions.Add(x));
      }
    }

    private void HandleInitTimeDimension(DimensionConfig item)
    {
      var dtLevels = new List<DimensionTime<T>>();

      for (var i = 0; i < item.TimeLevels.Count; i++)
      {
        var ndim = new DimensionTime<T>(item, item.TimeLevels[i], i)
        {
          Name = item.LevelLabels[i],
        };
        dtLevels.Add(ndim);
        this.Dimensions.Add(ndim);
      }

      foreach (var dtItem in dtLevels)
      {
        var query = dtLevels
          .Where(x => x.LevelPosition != dtItem.LevelPosition)
          .ToList();

        query.ForEach(x => dtItem.LevelDimensions.Add(x));
      }
    }

    private void HandleInitLevelDimension(DimensionConfig item, Dictionary<string, string[]> levels)
    {
      if (string.IsNullOrEmpty(item.ParentDimension))
      {
        var ldim = new DimensionLevel<T>(item, 0, _datasources[item.Source]) { Name = item.Name };

        levels.Add(item.Name, item.LevelLabels);
        this.Dimensions.Add(ldim);
      }
      else
      {
        var dLevels = levels[item.ParentDimension];
        var index = Array.FindIndex(dLevels, x => x.Equals(item.Name));
        var ldim = new DimensionLevel<T>(item, index + 1, _datasources[item.Source]) { Name = item.Name };
        var parentDim = (DimensionLevel<T>)this.Dimensions[item.ParentDimension];

        parentDim.LevelDimensions.Add(ldim);
        this.Dimensions.Add(ldim);
      }
    }

    private void InitializeMeasures()
    {
      foreach (MeasureConfig item in this.Config.Measures)
      {
        Measure<T> nmes = new Measure<T>(item);
        this.Measures.Add(nmes);
      }
    }

    private void InitializeMetrics()
    {
      foreach (MetricConfig item in this.Config.Metrics)
      {
        Metric<T> nmes = new Metric<T>(item);
        this.Metrics.Add(nmes);

        try
        {
          if (!string.IsNullOrEmpty(item.MetricFunction))
            nmes.MetricExpression = _metricExpressionEvaluator.Create(item.Name, item.MetricFunction);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }
    }

    private void ProcessDimensions()
    {
      foreach (Dimension<T> item in this.Dimensions)
        item.Process();
    }

    #endregion private members

    #region IProcess implementation

    public void Process()
    {
      this.ProcessDimensions();
    }

    public void Refresh(bool all)
    {
      if (all)
      {
        foreach (var dimension in Dimensions)
          dimension.Members.Clear();

        this.ProcessDimensions();
      }
    }

    #endregion IProcess implementation
  }
}