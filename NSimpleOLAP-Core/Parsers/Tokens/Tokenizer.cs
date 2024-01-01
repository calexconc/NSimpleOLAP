using System.Collections.Generic;

namespace NSimpleOLAP.Parsers.Tokens
{
  /// <summary>
  /// Description of Tokenizer.
  /// </summary>
  internal class Tokenizer
  {
    private string _current;
    private bool _start;
    private Queue<char> _queue;

    public Tokenizer(string text)
    {
      this._current = string.Empty;
      this._start = true;
      this._queue = new Queue<char>(text);
    }

    #region new implementation

    private IEnumerable<string> ScanText()
    {
      if (_queue.Count > 0)
      {
        char curr = _queue.Dequeue();
        IEnumerable<string> strtokens = null;

        if (IsOperationSymbol(curr.ToString()))
          strtokens = this.ProcessOperators(curr);
        else if (curr == ' ')
          strtokens = this.ProcessSpace(curr);
        else
          strtokens = this.ProcessChar(curr);

        foreach (string item in strtokens)
          yield return item;
      }
      else if (_queue.Count == 0 && _start)
        throw new ParseException("Invalid expression!");
      else if (_current != string.Empty)
      {
        yield return _current;
        _current = string.Empty;
      }
    }

    private IEnumerable<string> ProcessOperators(char curr)
    {
      IEnumerable<string> strtokens = null;

      if (curr == ')' && _start)
        throw new ParseException("Empty expression within parentesis!");

      if (curr == '(')
        strtokens = this.ProcessStartParentesis(curr);
      else if (_current != string.Empty && IsOperationSymbol(_current + curr))
      {
        yield return _current + curr;
        _current = string.Empty;
      }
      else if (IsOperationSymbol(_current)
               && this.IsSignalSymbol(curr.ToString()))
      {
        if (_current != ")")
          _start = true;

        yield return _current;
        _current = string.Empty;

        strtokens = this.ProcessSignal(curr);
      }
      else if (this.IsSignalSymbol(curr.ToString()))
        strtokens = this.ProcessSignal(curr);
      else if (_current != string.Empty)
      {
        yield return _current;
        _current = curr.ToString();
      }
      else
        _current += curr;

      if (strtokens != null)
      {
        foreach (string item in strtokens)
          yield return item;
      }

      foreach (string item in ScanText())
        yield return item;
    }

    private IEnumerable<string> ProcessStartParentesis(char curr)
    {
      if (_current != string.Empty)
      {
        yield return _current;
        _current = string.Empty;
      }

      _start = true;
      yield return curr.ToString();
    }

    private IEnumerable<string> ProcessSignal(char curr)
    {
      if (_current != string.Empty)
      {
        if (IsFunction(_current))
        {
          _start = true;

          if (_current.StartsWith("-"))
          {
            yield return "-1";
            yield return "*";
            _current = _current.Substring(1);
          }
        }

        yield return _current;

        _current = string.Empty;
      }

      if (_start && curr == '-')
        _current += curr;
      else if (!_start)
        yield return curr.ToString();
    }

    private IEnumerable<string> ProcessSpace(char curr)
    {
      if (_current != string.Empty && !IsSignalSymbol(_current))
      {
        if (IsFunction(_current))
        {
          _start = true;

          if (_current.StartsWith("-"))
          {
            yield return "-1";
            yield return "*";
            _current = _current.Substring(1);
          }
        }

        yield return _current;

        _current = string.Empty;
      }

      foreach (string item in ScanText())
        yield return item;
    }

    private IEnumerable<string> ProcessChar(char curr)
    {
      if (_current != string.Empty &&
          IsOperationSymbol(_current) && !IsSignalSymbol(_current))
      {
        yield return _current;
        _current = string.Empty;
      }
      else if (!_start && IsSignalSymbol(_current))
      {
        yield return _current;
        _current = string.Empty;
      }

      _current += curr;
      _start = false;

      foreach (string item in ScanText())
        yield return item;
    }

    #endregion new implementation

    #region public members

    public IEnumerable<Token> ExtractTokens()
    {
      bool start = true;

      foreach (string item in this.ScanText())
      {
        Token token = Token.Create(item);

        if (start && token.TToken == TokenType.SUM)
          continue; //do nothing in this case
        else if (start && token.TToken == TokenType.SUB)
        {
          yield return Token.Create("-1");
          yield return Token.Create("*");
        }
        else if (token.TToken == TokenType.PAR && ((ParToken)token).HasSignal())
        {
          ParToken ptoken = (ParToken)token;

          foreach (Token xitem in ptoken.Decompose())
            yield return xitem;
        }
        else if ((token.TToken == TokenType.PI || token.TToken == TokenType.E) && ((ConstantToken)token).HasSignal())
        {
          ConstantToken ptoken = (ConstantToken)token;

          foreach (Token xitem in ptoken.Decompose())
            yield return xitem;
        }
        else
          yield return token;

        if (token.TToken == TokenType.STARTPAR)
          start = true;
        else
          start = false;
      }

      yield return Token.Create("");
    }

    #endregion public members

    #region private methods

    private bool IsSignalSymbol(string value)
    {
      switch (value)
      {
        case "+":
        case "-":
          return true;

        default:
          return false;
      }
    }

    private bool IsFunction(string value)
    {
      string xvalue = value.StartsWith("-") ? value.Substring(1) : value;

      switch (xvalue)
      {
        case "EXP":
        case "LN":
        case "COS":
        case "COSH":
        case "SIN":
        case "SINH":
        case "TAN":
        case "TANH":
        case "SQRT":
        case "ABS":
        case "LOG10":
        case "MIN":
        case "MAX":
        case "AVG":
          return true;

        default:
          return false;
      }
    }

    private bool IsConstant(string value)
    {
      string xvalue = value.StartsWith("-") ? value.Substring(1) : value;

      switch (xvalue)
      {
        case "PI":
        case "E":
          return true;

        default:
          return false;
      }
    }

    private bool IsOperationSymbol(string value)
    {
      switch (value)
      {
        case "+":
        case "-":
        case "*":
        case "/":
        case "%":
        case "(":
        case ")":
        case "^":
        case "==":
        case "!=":
        case "<":
        case ">":
        case "<=":
        case ">=":
        case "!":
        case "=":
          return true;

        case "$":
        case "#":
        case "&":
        case "|":
        case "\\":
        case "\"":
        case "\'":
        case "?":
        case "»":
        case "«":
        case "~":
        case ";":
        case ":":
        case "€":
        case "£":
        case "{":
        case "}":
          throw new ParseException(string.Format("Invalid character in expression -> {0}", value));
        default:
          return false;
      }
    }

    #endregion private methods
  }
}