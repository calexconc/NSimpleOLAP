namespace NSimpleOLAP.Parsers.Tokens
{
  /// <summary>
  /// Description of EofToken.
  /// </summary>
  internal class EofToken : Token
  {
    public EofToken()
    {
      this.Text = "";
    }

    public override TokenType TToken
    {
      get
      {
        return TokenType.EOF;
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