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
  internal static class ParserExtensions
  {
    public static bool HasMeasure(this BinaryNode<Token> node)
    {
      if(node.Left == null)
        return node.Right.Value?.TToken == TokenType.PAR;

      if (node.Right == null)
        return node.Left.Value?.TToken == TokenType.PAR;

      return node.Left.Value.TToken == TokenType.PAR || node.Right.Value.TToken == TokenType.PAR;
    }

    public static bool HasValue(this BinaryNode<Token> node)
    {
      if (node.Left == null)
        return node.Right.Value?.TToken == TokenType.NUM;

      if (node.Right == null)
        return node.Left.Value?.TToken == TokenType.NUM;

      return node.Left.Value.TToken == TokenType.NUM || node.Right.Value.TToken == TokenType.NUM;
    }

    public static IEnumerable<Token> GetMeasures(this BinaryNode<Token> node)
    {
      if (node.Left != null && node.Left.Value.TToken == TokenType.PAR)
        yield return node.Left.Value;

      if (node.Right != null && node.Right.Value.TToken == TokenType.PAR)
        yield return node.Right.Value;
    }

    public static IEnumerable<Token> GetValues(this BinaryNode<Token> node)
    {
      if (node.Left != null && node.Left.Value.TToken == TokenType.NUM)
        yield return node.Left.Value;

      if (node.Right != null && node.Right.Value.TToken == TokenType.NUM)
        yield return node.Right.Value;
    }
  }
}
