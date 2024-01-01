using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.CubeExpressions.Interfaces;
using NSimpleOLAP.Interfaces;
using System;

namespace NSimpleOLAP.CubeExpressions.Builder
{
  public class ExpressionElementsBuilder<T>
   where T : struct, IComparable
  {
    private NamespaceResolver<T> _resolver;
    private ExpressionNodeBuilder<T> _node;
    private Type _type;

    public ExpressionElementsBuilder(NamespaceResolver<T> resolver)
    {
      _resolver = resolver;
    }

    public ExpressionNodeBuilder<T> Set(string measure)
    {
      var picker = new ExpressionElementPickerBuilder<T>(_resolver);

      _node = new ExpressionNodeBuilder<T>(picker.Set(measure), _resolver);
      _type = _node.ReturnType;

      return _node;
    }

    public ExpressionNodeBuilder<T> Set(string measure, params string[] tuples)
    {
      var picker = new ExpressionElementPickerBuilder<T>(_resolver);

      _node = new ExpressionNodeBuilder<T>(picker.Set(measure, tuples), _resolver);
      _type = _node.ReturnType;

      return _node;
    }

    public ExpressionNodeBuilder<T> Set(ValueType value)
    {
      _node = new ExpressionNodeBuilder<T>(value);
      _type = _node.ReturnType;

      return _node;
    }

    internal Type ReturnType
    {
      get
      {
        return _type;
      }
    }

    internal ExpressionNodeBuilder<T> Node
    {
      get { return _node; }
    }

    internal Func<IExpressionContext<T, ICell<T>>, IExpressionContext<T, ICell<T>>> Create()
    {
      return _node.Create();
    }
  }
}