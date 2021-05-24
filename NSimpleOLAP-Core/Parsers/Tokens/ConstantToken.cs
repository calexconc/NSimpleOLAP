using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Parsers.Tokens
{
  /// <summary>
  /// Description of ConstantToken.
  /// </summary>
  internal class ConstantToken : Token
  {
    public ConstantToken(TokenType ttype, string text)
    {
      this._ttoken = ttype;
      this.Text = text;
      this.ParameterType = typeof(double);
    }

    private TokenType _ttoken;

    public override TokenType TToken
    {
      get { return _ttoken; }
    }

    public override TokenGroup GToken
    {
      get
      {
        return TokenGroup.Literal;
      }
    }

    public bool HasSignal()
    {
      return this.Text.StartsWith("-") || this.Text.StartsWith("+");
    }

    public IEnumerable<Token> Decompose()
    {
      if (!HasSignal())
        yield break;
      else
      {
        yield return Token.Create("(");
        yield return Token.Create(this.Text[0] + "1");
        yield return Token.Create("*");
        yield return Token.Create(this.Text.Substring(1));
        yield return Token.Create(")");
      }
    }

    public double GetValue()
    {
      switch (this.Text)
      {
        case "E": return Math.E;
        case "PI": return Math.PI;
        default:
          throw new ParseException("Invalid constant!");
      }
    }

    public Type ParameterType
    {
      get;
      private set;
    }
  }
}