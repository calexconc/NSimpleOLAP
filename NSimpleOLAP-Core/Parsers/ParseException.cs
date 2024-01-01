using NSimpleOLAP.Parsers.Tokens;
using System;

namespace NSimpleOLAP.Parsers
{
  /// <summary>
  /// Description of ParceException.
  /// </summary>
  public class ParseException : Exception
  {
    public ParseException()
    {
    }

    public ParseException(string message) : base(message)
    {
    }

    internal ParseException(Token token) : base(string.Format("Error in \"{0}\"", token.Text))
    {
    }
  }
}