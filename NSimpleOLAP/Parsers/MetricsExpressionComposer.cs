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
          DefineOpearation(node, builder);
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


    private void DefineOpearation(BinaryNode<Token> node, ExpressionElementsBuilder<T> builder)
    {
      
      if (node.HasMeasure())
      {
        var measures = node.GetMeasures().ToArray();
        var measureRoot = DefineMeasure(measures[0], builder);

        if (measures.Length == 1)
        {
          if (node.HasValue())
          {
            var values = node.GetValues().ToArray();

            switch (node.Value.TToken)
            {
              case TokenType.SUM:
                DefineSum((NumToken)values[0], measureRoot);
                break;
              case TokenType.SUB:
                DefineSubtraction((NumToken)values[0], measureRoot);
                break;
              case TokenType.MULT:
                DefineMultiplication((NumToken)values[0], measureRoot);
                break;
              case TokenType.DIV:
                DefineDivision((NumToken)values[0], measureRoot);
                break;
            }
          }
        }
        else
        {
          var root2 = new ExpressionElementsBuilder<T>(_resolver);
          var measure2Root = DefineMeasure(measures[1], root2);

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
      }
      else if (node.HasValue())
      {
        var values = node.GetValues().ToArray();
      }
    }


    private ExpressionNodeBuilder<T> DefineMeasure(Token token, ExpressionElementsBuilder<T> builder)
    {
      return builder.Set(token.Text);
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
