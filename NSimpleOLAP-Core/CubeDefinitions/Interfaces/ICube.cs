using NSimpleOLAP.Common.Interfaces;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.Data;
using NSimpleOLAP.Schema;
using NSimpleOLAP.Schema.Interfaces;
using NSimpleOLAP.Storage.Interfaces;
using System;

namespace NSimpleOLAP.Interfaces
{
  /// <summary>
  /// Description of ICube.
  /// </summary>
  public interface ICube<T, U> : IDisposable, IProcess
    where T : struct, IComparable
    where U : class, ICell<T>
  {
    T Key { get; set; }
    string Name { get; set; }
    INamespace<T> NameSpace { get; }
    DataSchema<T> Schema { get; }
    IStorage<T, U> Storage { get; }
    ICellCollection<T, U> Cells { get; }
    DataSourceCollection DataSources { get; }
    bool IsProcessing { get; }
    CubeConfig Config { get; }
    string Source { get; set; }

    void Initialize();
  }
}