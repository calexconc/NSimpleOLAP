namespace NSimpleOLAP.Parsers.Tokens
{
  /// <summary>
  /// Description of OperatorToken.
  /// </summary>
  internal class OperatorToken : Token
  {
    public OperatorToken(TokenType ttype)
    {
      this._ttoken = ttype;
      this.Text = ttype.ToString();
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
        return TokenGroup.Operator;
      }
    }
  }
}