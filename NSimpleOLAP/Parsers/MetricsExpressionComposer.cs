using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.CubeExpressions.Builder;
using NSimpleOLAP.CubeExpressions;
using NSimpleOLAP.CubeExpressions.Interfaces;
using NSimpleOLAP.Interfaces;
using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.Parsers.Collections;
using NSimpleOLAP.Parsers.Tokens;

namespace NSimpleOLAP.Parsers
{
  internal class MetricsExpressionComposer<T>
    where T : struct, IComparable
  {
    private NamespaceResolver<T> _resolver;
    public MetricsExpressionComposer(NamespaceResolver<T> resolver)
    {
      _resolver = resolver;
    }

    public MetricsExpression<T> Create(string name, string expression)
    {
      var builder = new MetricExpressionBuilder<T>(_resolver);
      var expBuilder = builder.Metric(name);
      var tree = new TokenTree(GetTokens(expression));

      ProcessToken(tree.Root, expBuilder.SetElementsBuilder());


      return builder.Create();
    }

    private IEnumerable<Token> GetTokens(string expression)
    {
      Tokenizer tokenizer = new Tokenizer(expression);

      foreach (var item in tokenizer.ExtractTokens())
        yield return item;
    }

    private void ProcessToken(BinaryNode<Token> node, ExpressionElementsBuilder<T> builder)
    {
      switch (node.Value.TToken)
      {
        case TokenType.SUM:
        case TokenType.SUB:
        case TokenType.MULT:
        case TokenType.DIV:
          DefineOperation(node, builder);
          break;
        case TokenType.ABS:
        case TokenType.LN:
        case TokenType.SQRT:
        case TokenType.EXP:
        case TokenType.MIN:
        case TokenType.MAX:
        case TokenType.AVG:
          DefineUnaryOperation(node, builder);
          break;
        case TokenType.PAR:
        case TokenType.NUM:
          DefineValue(node, builder);
          break;
        default:
          break;
      }
    }

    private void DefineValue(BinaryNode<Token> node, ExpressionElementsBuilder<T> builder)
    {
      var nodeLeave = node.IsMeasure() ? DefineMeasure(node.Value, builder) : DefineScalar((NumToken)node.Value, builder);

      nodeLeave.Value();
    }

    private void DefineUnaryOperation(BinaryNode<Token> node, ExpressionElementsBuilder<T> builder)
    {
      ProcessToken(node.Right, builder);

      switch (node.Value.TToken)
      {
        case TokenType.MIN:
          builder.Node.Min();
          break;
        case TokenType.MAX:
          builder.Node.Max();
          break;
        case TokenType.AVG:
          builder.Node.Average();
          break;
        case TokenType.ABS:
          builder.Node.ABS();
          break;
        case TokenType.LN:
          builder.Node.Ln();
          break;
        case TokenType.SQRT:
          builder.Node.SQRT();
          break;
        case TokenType.EXP:
          builder.Node.Exp();
          break;
      }
    }


    private void DefineOperation(BinaryNode<Token> node, ExpressionElementsBuilder<T> builder)
    {
      if (node.HasMeasure())
      {
        if (node.Left.IsMeasure() && node.Right.IsMeasure())
          DefineBinaryMeasuresOperation(node, builder);
        
        if (node.HasValue())
          DefineMixedBinaryMeasureLiteralOperation(node, builder);

        if (node.HasOperations())
          DefineMeasuresWithAnotherOperation(node, builder);
      }
      else if (node.HasValue())
      {
        if (node.Left.IsValue() && node.Right.IsValue())
          DefineBinaryLiteralsOperation(node, builder);

        if (node.HasOperations())
          DefineMeasuresWithAnotherOperation(node, builder);
      }
      else if (node.HasOperations())
        DefineBinaryOperation(node, builder);
    }

    private void DefineMeasuresWithAnotherOperation(BinaryNode<Token> node, ExpressionElementsBuilder<T> builder)
    {
      if (node.Left.IsMeasure() || node.Left.IsValue())
      {
        var nodeLeave = node.Left.IsMeasure() ? DefineMeasure(node.Left.Value, builder) : DefineScalar((NumToken)node.Left.Value, builder);
        var root2 = new ExpressionElementsBuilder<T>(_resolver);

        ProcessToken(node.Right, root2);

        switch (node.Value.TToken)
        {
          case TokenType.SUM:
            nodeLeave.Sum(root2);
            break;
          case TokenType.SUB:
            nodeLeave.Subtract(root2);
            break;
          case TokenType.MULT:
            nodeLeave.Multiply(root2);
            break;
          case TokenType.DIV:
            nodeLeave.Divide(root2);
            break;
        }
      }
      else if (node.Right.IsMeasure() || node.Right.IsValue())
      {
        var root2 = new ExpressionElementsBuilder<T>(_resolver);
        var nodeLeave = node.Right.IsMeasure() ?  DefineMeasure(node.Right.Value, root2)  : DefineScalar((NumToken)node.Right.Value, root2);

        nodeLeave.Value();

        ProcessToken(node.Left, builder);

        switch (node.Value.TToken)
        {
          case TokenType.SUM:
            builder.Node.Sum(root2);
            break;
          case TokenType.SUB:
            builder.Node.Subtract(root2);
            break;
          case TokenType.MULT:
            builder.Node.Multiply(root2);
            break;
          case TokenType.DIV:
            builder.Node.Divide(root2);
            break;
        }
      }
    }

    private void DefineBinaryMeasuresOperation(BinaryNode<Token> node, ExpressionElementsBuilder<T> builder)
    {
      var measureRoot = DefineMeasure(node.Left.Value, builder);
      var root2 = new ExpressionElementsBuilder<T>(_resolver);
      var measure2Root = DefineMeasure(node.Right.Value, root2);

      measure2Root.Value();

      switch (node.Value.TToken)
      {
        case TokenType.SUM:
          measureRoot.Sum(root2);
          break;
        case TokenType.SUB:
          measureRoot.Subtract(root2);
          break;
        case TokenType.MULT:
          measureRoot.Multiply(root2);
          break;
        case TokenType.DIV:
          measureRoot.Divide(root2);
          break;
      }
    }

    private void DefineBinaryLiteralsOperation(BinaryNode<Token> node, ExpressionElementsBuilder<T> builder)
    {
      var literalRoot = DefineScalar((NumToken)node.Left.Value, builder);
      var root2 = new ExpressionElementsBuilder<T>(_resolver);
      var literal2Root = DefineScalar((NumToken)node.Right.Value, root2);

      literal2Root.Value();

      switch (node.Value.TToken)
      {
        case TokenType.SUM:
          literalRoot.Sum(root2);
          break;
        case TokenType.SUB:
          literalRoot.Subtract(root2);
          break;
        case TokenType.MULT:
          literalRoot.Multiply(root2);
          break;
        case TokenType.DIV:
          literalRoot.Divide(root2);
          break;
      }
    }

    private void DefineMixedBinaryMeasureLiteralOperation(BinaryNode<Token> node, ExpressionElementsBuilder<T> builder)
    {
      var nodeRoot = node.Left.IsMeasure() ? DefineMeasure(node.Left.Value, builder) : DefineMeasure(node.Right.Value, builder);
      var root2 = new ExpressionElementsBuilder<T>(_resolver);
      var node2Root = node.Left.IsMeasure() ? DefineScalar((NumToken)node.Right.Value, root2) : DefineScalar((NumToken)node.Left.Value, root2);

      node2Root.Value();

      switch (node.Value.TToken)
      {
        case TokenType.SUM:
          nodeRoot.Sum(root2);
          break;
        case TokenType.SUB:
          nodeRoot.Subtract(root2);
          break;
        case TokenType.MULT:
          nodeRoot.Multiply(root2);
          break;
        case TokenType.DIV:
          nodeRoot.Divide(root2);
          break;
      }
    }

    private void DefineBinaryOperation(BinaryNode<Token> node, ExpressionElementsBuilder<T> builder)
    {
      var root2 = new ExpressionElementsBuilder<T>(_resolver);

      ProcessToken(node.Left, builder);
      ProcessToken(node.Right, root2);

      switch (node.Value.TToken)
      {
        case TokenType.SUM:
          builder.Node.Sum(root2);
          break;
        case TokenType.SUB:
          builder.Node.Subtract(root2);
          break;
        case TokenType.MULT:
          builder.Node.Multiply(root2);
          break;
        case TokenType.DIV:
          builder.Node.Divide(root2);
          break;
      }
    }


    private ExpressionNodeBuilder<T> DefineMeasure(Token token, ExpressionElementsBuilder<T> builder)
    {
      return builder.Set(token.Text);
    }

    private ExpressionNodeBuilder<T> DefineScalar(NumToken token, ExpressionElementsBuilder<T> builder)
    {
      return builder.Set((ValueType)token.GetValue());
    }

  }
}
