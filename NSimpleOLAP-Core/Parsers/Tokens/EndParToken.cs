namespace NSimpleOLAP.Parsers.Tokens
{
  /// <summary>
  /// Description of EndParToken.
  /// </summary>
  internal class EndParToken : Token
  {
    public EndParToken()
    {
      this.Text = ")";
    }

    public override TokenType TToken
    {
      get
      {
        return TokenType.ENDPAR;
      }
    }

    public override TokenGroup GToken
    {
      get
      {
        return TokenGroup.Separator;
      }
    }
  }
}