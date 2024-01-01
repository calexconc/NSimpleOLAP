using NUnit.ConsoleRunner;
using System;
using System.Reflection;

namespace UnitTests
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      string[] my_args = { Assembly.GetExecutingAssembly().Location };

      int returnCode = Runner.Main(my_args);

      if (returnCode != 0)
        Console.Beep();

      Console.Write("Press any key to continue . . . ");
      Console.ReadKey(true);
    }
  }
}