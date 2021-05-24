namespace NSimpleOLAP.Parsers.Tokens
{
  /// <summary>
  /// Description of BoolToken.
  /// </summary>
  internal class BoolToken : Token
  {
    public BoolToken(string value)
    {
      this.Text = value;
    }

    public override TokenType TToken
    {
      get
      {
        return TokenType.BOOL;
      }
    }

    public override TokenGroup GToken
    {
      get
      {
        return TokenGroup.Literal;
      }
    }

    public bool GetValue()
    {
      return bool.Parse(this.Text);
    }
  }
}