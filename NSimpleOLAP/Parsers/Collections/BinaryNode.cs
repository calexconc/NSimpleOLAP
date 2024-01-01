namespace NSimpleOLAP.Parsers.Collections
{
  /// <summary>
  /// Description of BinaryNode.
  /// </summary>
  internal abstract class BinaryNode<T>
  {
    protected abstract void SetRootNode(BinaryNode<T> node);

    public T Value
    {
      get;
      set;
    }

    public BinaryNode<T> Root
    {
      get;
      set;
    }

    public BinaryNode<T> Left
    {
      get;
      set;
    }

    public BinaryNode<T> Right
    {
      get;
      set;
    }
  }
}