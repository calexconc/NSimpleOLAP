using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration.Fluent;

namespace NSimpleOLAP.Configuration
{
  /// <summary>
  /// Description of DefaultConfiguration.
  /// </summary>
  internal class DefaultCubeConfiguration
  {
    internal static CubeConfig GetConfig()
    {
      CubeBuilder builder = new CubeBuilder();

      builder.SetName("New_Cube")
        .Storage(store => { store.SetStoreType(StorageType.Molap); });

      return builder.CreateConfig();
    }
  }
}