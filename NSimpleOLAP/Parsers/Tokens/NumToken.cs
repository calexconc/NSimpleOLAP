using System;

namespace NSimpleOLAP.Parsers.Tokens
{
  /// <summary>
  /// Description of NumToken.
  /// </summary>
  internal class NumToken : Token
  {
    public NumToken(string text)
    {
      this.Text = text;
      this.ParameterType = typeof(double);
    }

    public NumToken(string text, Type type)
    {
      this.Text = text;
      this.ParameterType = type;
    }

    public override TokenType TToken
    {
      get
      {
        return TokenType.NUM;
      }
    }

    public override TokenGroup GToken
    {
      get
      {
        return TokenGroup.Literal;
      }
    }

    public object GetValue()
    {
      return Convert.ChangeType(this.Text, this.ParameterType);
    }

    public Type ParameterType
    {
      get;
      private set;
    }
  }
}