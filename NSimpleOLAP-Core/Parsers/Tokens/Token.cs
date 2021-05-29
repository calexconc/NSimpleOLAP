using System;
using System.Text.RegularExpressions;

namespace NSimpleOLAP.Parsers.Tokens
{
  public enum TokenType
  {
    NUM = 0, PAR = 1, SUM = 2, SUB = 3, MULT = 4, DIV = 5, REM = 6, POW = 7, STARTPAR = 8, ENDPAR = 9,
    AND = 10, OR = 11, NOT = 12, EQUALS = 13, NOTEQUALS = 14, LOWER = 15, GREATER = 16, LOWEROREQUALS = 17, GREATEROREQUALS = 18
      , BOOL = 19, EXP = 20, LN = 21, COS = 22, SIN = 23, TAN = 24, COSH = 25, SINH = 26, TANH = 27, ABS = 28, SQRT = 29, LOG10 = 30,
    PI = 31, E = 32, MIN = 33, MAX = 34, AVG = 35,
    EOF = 256
  }

  public enum TokenGroup { Operator = 0, Identifier = 1, Literal = 2, Separator = 3 }

  /// <summary>
  /// Description of Token.
  /// </summary>
  internal abstract class Token
  {
    private static Regex _isnumeric = new Regex(@"(^(\+?\-? *[0-9]+)([,0-9 ]*)([0-9 ])*$)|(^ *$)");
    private static Regex _isbool = new Regex("(true|false|True|False|FALSE|TRUE)");

    public string Text
    {
      get;
      protected set;
    }

    public abstract TokenType TToken { get; }

    public abstract TokenGroup GToken { get; }

    public static Token Create(string value)
    {
      TokenType ttype = GetTokenType(value);
      Token token = null;

      switch (ttype)
      {
        case TokenType.SUM:
        case TokenType.SUB:
        case TokenType.MULT:
        case TokenType.DIV:
        case TokenType.POW:
        case TokenType.REM:
        case TokenType.AND:
        case TokenType.OR:
        case TokenType.NOT:
        case TokenType.EQUALS:
        case TokenType.NOTEQUALS:
        case TokenType.GREATER:
        case TokenType.GREATEROREQUALS:
        case TokenType.LOWER:
        case TokenType.LOWEROREQUALS:
        case TokenType.EXP:
        case TokenType.LN:
        case TokenType.COS:
        case TokenType.COSH:
        case TokenType.SIN:
        case TokenType.SINH:
        case TokenType.TAN:
        case TokenType.TANH:
        case TokenType.SQRT:
        case TokenType.ABS:
        case TokenType.LOG10:
        case TokenType.MIN:
        case TokenType.MAX:
        case TokenType.AVG:
          token = new OperatorToken(ttype);
          break;

        case TokenType.STARTPAR:
          token = new StartParToken();
          break;

        case TokenType.ENDPAR:
          token = new EndParToken();
          break;

        case TokenType.PAR:
          token = new ParToken(value);
          break;

        case TokenType.NUM:
          token = new NumToken(value, InferNumericType(value));
          break;

        case TokenType.BOOL:
          token = new BoolToken(value);
          break;

        case TokenType.PI:
        case TokenType.E:
          token = new ConstantToken(ttype, value);
          break;

        case TokenType.EOF:
          token = new EofToken();
          break;
      }

      return token;
    }

    private static bool IsNumeric(string value)
    {
      return _isnumeric.IsMatch(value);
    }

    private static bool IsBoolean(string value)
    {
      return _isbool.IsMatch(value);
    }

    private static Type InferNumericType(string value)
    {
      if (value.Contains(".") || value.Contains("."))
        return typeof(double);
      else
        return typeof(int);
    }

    private static TokenType GetTokenType(string value)
    {
      switch (value)
      {
        case "+":
          return TokenType.SUM;

        case "-":
          return TokenType.SUB;

        case "*":
          return TokenType.MULT;

        case "/":
          return TokenType.DIV;

        case "%":
          return TokenType.REM;

        case "(":
          return TokenType.STARTPAR;

        case ")":
          return TokenType.ENDPAR;

        case "^":
          return TokenType.POW;

        case "==":
          return TokenType.EQUALS;

        case "!=":
          return TokenType.NOTEQUALS;

        case "<":
          return TokenType.LOWER;

        case ">":
          return TokenType.GREATER;

        case "<=":
          return TokenType.LOWEROREQUALS;

        case ">=":
          return TokenType.GREATEROREQUALS;

        case "AND":
          return TokenType.AND;

        case "OR":
          return TokenType.OR;

        case "NOT":
          return TokenType.NOT;

        case "EXP":
          return TokenType.EXP;

        case "LN":
          return TokenType.LN;

        case "COS":
          return TokenType.COS;

        case "COSH":
          return TokenType.COSH;

        case "SIN":
          return TokenType.SIN;

        case "SINH":
          return TokenType.SINH;

        case "TAN":
          return TokenType.TAN;

        case "TANH":
          return TokenType.TANH;

        case "SQRT":
          return TokenType.SQRT;

        case "LOG10":
          return TokenType.LOG10;

        case "ABS":
          return TokenType.ABS;

        case "MIN":
          return TokenType.MIN;

        case "MAX":
          return TokenType.MAX;

        case "AVG":
          return TokenType.AVG;

        case "-PI":
        case "PI":
          return TokenType.PI;

        case "-E":
        case "E":
          return TokenType.E;

        case "":
          return TokenType.EOF;

        default:
          if (IsNumeric(value))
            return TokenType.NUM;
          else if (IsBoolean(value))
            return TokenType.BOOL;
          else
            return TokenType.PAR;
      }
    }
  }
}