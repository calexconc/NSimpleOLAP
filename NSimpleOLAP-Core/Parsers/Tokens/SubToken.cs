namespace NSimpleOLAP.Parsers.Tokens
{
  /// <summary>
  /// Description of SubToken.
  /// </summary>
  internal class SubToken : Token
  {
    public SubToken()
    {
      this.Text = "-";
    }

    public override TokenType TToken
    {
      get
      {
        return TokenType.SUB;
      }
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