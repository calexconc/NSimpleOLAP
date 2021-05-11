namespace NSimpleOLAP.Common.Interfaces
{
  /// <summary>
  /// Description of IProcess.
  /// </summary>
  public interface IProcess
  {
    void Process();

    void Refresh(bool all);
  }
}