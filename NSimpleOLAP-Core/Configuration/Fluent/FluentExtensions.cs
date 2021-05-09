using System;

namespace NSimpleOLAP.Configuration.Fluent
{
  /// <summary>
  /// Description of FluentExtensions.
  /// </summary>
  public static class FluentExtensions
  {
    public static CubeBuilder SetupConfig<T>(this Cube<T> cube)
      where T : struct, IComparable
    {
      cube.Config = new CubeConfig();
      CubeBuilder builder = new CubeBuilder(cube.Config);

      return builder;
    }

    public static Cube<T> Create<T>(this CubeBuilder cubebuilder)
      where T : struct, IComparable
    {
      CubeConfig cube = cubebuilder.CreateConfig();

      return new Cube<T>(cube);
    }
  }
}