using System;

namespace NSimpleOLAP.Common
{
  public class BadTupleReferenceException : Exception
  {
    public BadTupleReferenceException(int index) : base($"Tuple error at index {index}, cannot find member.")
    {
    }
  }
}