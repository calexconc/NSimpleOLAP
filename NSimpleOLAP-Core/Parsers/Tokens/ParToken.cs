using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Parsers.Tokens
{
  /// <summary>
  /// Description of ParToken.
  /// </summary>
  internal class ParToken : Token
  {
    public ParToken(string text)
    {
      this.Text = text;
      this.Init();
    }

    public override TokenType TToken
    {
      get
      {
        return TokenType.PAR;
      }
    }

    public override TokenGroup GToken
    {
      get
      {
        return TokenGroup.Identifier;
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

    public Type ParameterType
    {
      get;
      set;
    }

    #region private members

    private void Init()
    {
      if (IllegalOperator())
        throw new Exception();

      this.ParameterType = typeof(double);
    }

    private bool IllegalOperator()
    {
      return this.Text.StartsWith("*") || this.Text.StartsWith("/")
        || this.Text.StartsWith("%") || this.Text.StartsWith("^");
    }

    #endregion private members
  }
}