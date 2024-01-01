using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.Query.Interfaces;
using NSimpleOLAP.Query.Predicates;
using System;

namespace NSimpleOLAP.Query.Builder
{
  /// <summary>
  /// Description of WhereBuilder.
  /// </summary>
  public class WhereBuilder<T>
    where T : struct, IComparable
  {
    private NamespaceResolver<T> _resolver;
    private BlockPredicateBuilder<T> _rootBlock;
    private IPredicateBuilder<T> _currentBlock;

    public WhereBuilder(NamespaceResolver<T> resolver)
    {
      _resolver = resolver;
      BuilderFactory = new PredicateBuilderFactory<T>(_resolver);
      _rootBlock = new BlockPredicateBuilder<T>(BuilderFactory);
      _currentBlock = _rootBlock;
    }

    public PredicateBuilderFactory<T> BuilderFactory
    {
      get;
      private set;
    }

    #region fluent interface

    public WhereBuilder<T> Define(Func<BlockPredicateBuilder<T>, IPredicateBuilder<T>> blockBuilder)
    {
      _currentBlock = blockBuilder(_rootBlock);

      return this;
    }

    public IPredicate<T> Build()
    {
      return _rootBlock.Build();
    }

    #endregion fluent interface
  }
}