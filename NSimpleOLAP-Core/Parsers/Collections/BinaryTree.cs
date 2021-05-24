using System.Collections.Generic;

namespace NSimpleOLAP.Parsers.Collections
{
  /// <summary>
  /// Description of BinaryTree.
  /// </summary>
  internal abstract class BinaryTree<T>
  {
    protected Stack<BinaryNode<T>> _roots;

    public BinaryNode<T> Root
    {
      get;
      protected set;
    }

    #region

    protected abstract void Populate(IEnumerable<T> values);

    protected BinaryNode<T> CreateNode(T value)
    {
      return new ImpBinaryNode(value);
    }

    protected BinaryNode<T> CreateNode(BinaryNode<T> root, T value)
    {
      return new ImpBinaryNode(root, value);
    }

    #endregion

    #region BinaryNode<T> implementation

    private class ImpBinaryNode : BinaryNode<T>
    {
      public ImpBinaryNode(T value)
      {
        this.Value = value;
      }

      public ImpBinaryNode(BinaryNode<T> root, T value)
      {
        this.Value = value;
        this.Root = root;
      }

      protected override void SetRootNode(BinaryNode<T> node)
      {
        this.Root = node;
      }
    }

    #endregion
  }
}