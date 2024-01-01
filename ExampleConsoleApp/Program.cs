using System;

namespace ExampleConsoleApp
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      var parser = new SimpleInputParser();

      using (var localCube = new LocalCubeManager())
      {
        var hitEnter = 0;
        localCube.Initialize();
        localCube.Load();

        while (hitEnter < 1)
        {
          Console.WriteLine("Enter your query or type help:");
          var queryText = Console.ReadLine();

          if (queryText.Equals("help"))
          {
            localCube.WriteHelp();
            continue;
          }

          if (!string.IsNullOrEmpty(queryText))
          {
            var querySettings = parser.Parse(queryText);

            if (!querySettings.Error)
            {
              try
              {
                localCube.QueryCube(querySettings);
              }
              catch (Exception ex)
              {
                Console.WriteLine(ex.Message);
              }
            }
            else
            {
              foreach (var message in querySettings.ErrorMessages)
                Console.WriteLine(message);
            }
          }
          else
            hitEnter++;
        }

        Console.WriteLine("Exiting application.");
        Console.WriteLine("Enter again to close application.");
        Console.ReadKey();
      }
    }
  }
}