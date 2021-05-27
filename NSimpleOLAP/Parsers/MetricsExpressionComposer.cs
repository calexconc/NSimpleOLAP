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
        default:
          break;
      }
    }

    /*
    private Expression ProcessToken(BinaryNode<Token> node)
    {
      switch (node.Value.TToken)
      {
        case TokenType.SUM:
          return DefineSum(node);
        case TokenType.SUB:
          return DefineSub(node);
        case TokenType.MULT:
          return DefineMult(node);
        case TokenType.DIV:
          return DefineDiv(node);
        case TokenType.POW:
          return DefinePow(node);
        case TokenType.AND:
          return DefineAnd(node);
        case TokenType.OR:
          return DefineOr(node);
        case TokenType.NOT:
          return DefineNot(node);
        case TokenType.EQUALS:
          return DefineEqual(node);
        case TokenType.NOTEQUALS:
          return DefineNotEqual(node);
        case TokenType.GREATER:
          return DefineGreater(node);
        case TokenType.GREATEROREQUALS:
          return DefineGreaterOrEqual(node);
        case TokenType.LOWER:
          return DefineLower(node);
        case TokenType.LOWEROREQUALS:
          return DefineLowerOrEqual(node);
        case TokenType.REM:
          return DefineRemainder(node);
        case TokenType.EXP:
          return DefineExp(node);
        case TokenType.LN:
          return DefineLN(node);
        case TokenType.COS:
          return DefineCos(node);
        case TokenType.COSH:
          return DefineCosh(node);
        case TokenType.SIN:
          return DefineSin(node);
        case TokenType.TAN:
          return DefineTan(node);
        case TokenType.TANH:
          return DefineTanh(node);
        case TokenType.SQRT:
          return DefineSQRT(node);
        case TokenType.LOG10:
          return DefineLOG10(node);
        case TokenType.ABS:
          return DefineAbs(node);
        case TokenType.PAR:
          return CreateParameter(node.Value);
        case TokenType.NUM:
        case TokenType.BOOL:
          return CreateLiteral(node.Value);
        case TokenType.PI:
        case TokenType.E:
          return CreateConstant(node.Value);
        case TokenType.STARTPAR:
          break;
        case TokenType.ENDPAR:
          break;
      }

      return null;
    }*/


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
      {

      }
    }

    private void DefineMeasuresWithAnotherOperation(BinaryNode<Token> node, ExpressionElementsBuilder<T> builder)
    {
      if (node.Left.IsMeasure())
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
      else
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
      var nodeRoot = node.Left.IsMeasure() ? DefineMeasure(node.Left.Value, builder) : DefineScalar((NumToken)node.Left.Value, builder);
      var root2 = new ExpressionElementsBuilder<T>(_resolver);
      var node2Root = node.Right.IsMeasure() ? DefineMeasure(node.Right.Value, builder) : DefineScalar((NumToken)node.Right.Value, root2);

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

    private void DefineSum(NumToken token, ExpressionNodeBuilder<T> builder)
    {
      if (token.ParameterType == typeof(int))
        builder.Sum<int>((int)token.GetValue());
      else if (token.ParameterType == typeof(double))
        builder.Sum<double>((double)token.GetValue());
      else if (token.ParameterType == typeof(float))
        builder.Sum<float>((float)token.GetValue());
      else if (token.ParameterType == typeof(decimal))
        builder.Sum<decimal>((decimal)token.GetValue());
    }

    private void DefineSubtraction(NumToken token, ExpressionNodeBuilder<T> builder)
    {
      if (token.ParameterType == typeof(int))
        builder.Subtract<int>((int)token.GetValue());
      else if (token.ParameterType == typeof(double))
        builder.Subtract<double>((double)token.GetValue());
      else if (token.ParameterType == typeof(float))
        builder.Subtract<float>((float)token.GetValue());
      else if (token.ParameterType == typeof(decimal))
        builder.Subtract<decimal>((decimal)token.GetValue());
    }

    private void DefineMultiplication(NumToken token, ExpressionNodeBuilder<T> builder)
    {
      if (token.ParameterType == typeof(int))
        builder.Multiply<int>((int)token.GetValue());
      else if (token.ParameterType == typeof(double))
        builder.Multiply<double>((double)token.GetValue());
      else if (token.ParameterType == typeof(float))
        builder.Multiply<float>((float)token.GetValue());
      else if (token.ParameterType == typeof(decimal))
        builder.Multiply<decimal>((decimal)token.GetValue());
    }

    private void DefineDivision(NumToken token, ExpressionNodeBuilder<T> builder)
    {
      if (token.ParameterType == typeof(int))
        builder.Divide<int>((int)token.GetValue());
      else if (token.ParameterType == typeof(double))
        builder.Divide<double>((double)token.GetValue());
      else if (token.ParameterType == typeof(float))
        builder.Divide<float>((float)token.GetValue());
      else if (token.ParameterType == typeof(decimal))
        builder.Divide<decimal>((decimal)token.GetValue());
    }
  }
}
