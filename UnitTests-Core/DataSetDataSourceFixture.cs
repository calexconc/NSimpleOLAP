using System;
using System.Data;

namespace UnitTests
{
  internal class DataSetDataSourceFixture
  {
    public static DataSet GetDataSources()
    {
      var dataSet = new DataSet("Sources");

      dataSet.Tables.Add(GetFactsTable());
      dataSet.Tables.Add(GetCategoriesTable());
      dataSet.Tables.Add(GetGenderTable());
      dataSet.Tables.Add(GetPlacesTable());

      return dataSet;
    }

    private static DataTable GetFactsTable()
    {
      var table = new DataTable("Facts");
      table.Columns.Add(new DataColumn("category", typeof(int)));
      table.Columns.Add(new DataColumn("gender", typeof(int)));
      table.Columns.Add(new DataColumn("place", typeof(int)));
      table.Columns.Add(new DataColumn("Date", typeof(DateTime)));
      table.Columns.Add(new DataColumn("expenses", typeof(double)));
      table.Columns.Add(new DataColumn("items", typeof(int)));

      table.Rows.Add(1, 1, 2, new DateTime(2021, 1, 15), 1000.12, 30);
      table.Rows.Add(2, 2, 2, new DateTime(2021, 3, 5), 200.50, 5);
      table.Rows.Add(4, 2, 5, new DateTime(2021, 10, 17), 11500.00, 101);
      table.Rows.Add(3, 2, 2, new DateTime(2021, 8, 25), 100.00, 20);
      table.Rows.Add(2, 1, 6, new DateTime(2021, 2, 27), 10.10, 5);
      table.Rows.Add(1, 2, 2, new DateTime(2021, 8, 30), 700.10, 36);
      table.Rows.Add(5, 2, 5, new DateTime(2021, 12, 15), 100.40, 31);
      table.Rows.Add(1, 1, 3, new DateTime(2021, 9, 7), 100.12, 12);
      table.Rows.Add(3, 2, 3, new DateTime(2021, 6, 1), 10.12, 30);
      table.Rows.Add(2, 2, 2, new DateTime(2021, 6, 5), 10000.12, 30);
      table.Rows.Add(1, 2, 1, new DateTime(2021, 5, 4), 100.12, 1);
      table.Rows.Add(4, 2, 2, new DateTime(2021, 1, 3), 10.12, 6);
      table.Rows.Add(2, 2, 3, new DateTime(2021, 11, 9), 100.12, 44);
      table.Rows.Add(1, 2, 3, new DateTime(2021, 7, 1), 10.12, 8);
      table.Rows.Add(4, 1, 1, new DateTime(2021, 4, 24), 100.12, 5);
      table.Rows.Add(1, 1, 6, new DateTime(2021, 6, 2), 10.12, 7);
      table.Rows.Add(4, 3, 6, new DateTime(2021, 5, 18), 100.12, 30);
      table.Rows.Add(2, 1, 2, new DateTime(2021, 8, 21), 60.99, 8);
      table.Rows.Add(1, 2, 2, new DateTime(2021, 2, 16), 6000.00, 89);
      table.Rows.Add(4, 3, 6, new DateTime(2021, 3, 7), 600.00, 75);
      table.Rows.Add(1, 1, 6, new DateTime(2021, 1, 1), 10.00, 12);
      table.Rows.Add(4, 2, 2, new DateTime(2021, 7, 28), 2000.00, 30);
      table.Rows.Add(5, 2, 6, new DateTime(2021, 12, 20), 50.10, 11);
      table.Rows.Add(3, 1, 3, new DateTime(2021, 6, 8), 130.50, 2);

      return table;
    }

    private static DataTable GetCategoriesTable()
    {
      var table = new DataTable("Categories");

      table.Columns.Add(new DataColumn("id", typeof(int)));
      table.Columns.Add(new DataColumn("description", typeof(string)));

      table.Rows.Add(1, "toys");
      table.Rows.Add(2, "clothes");
      table.Rows.Add(3, "furniture");
      table.Rows.Add(4, "shoes");
      table.Rows.Add(5, "Video Games");

      return table;
    }

    private static DataTable GetGenderTable()
    {
      var table = new DataTable("Gender");

      table.Columns.Add(new DataColumn("id", typeof(int)));
      table.Columns.Add(new DataColumn("description", typeof(string)));

      table.Rows.Add(1, "male");
      table.Rows.Add(2, "female");
      table.Rows.Add(3, "No Data");

      return table;
    }

    private static DataTable GetPlacesTable()
    {
      var table = new DataTable("Places");

      table.Columns.Add(new DataColumn("id", typeof(int)));
      table.Columns.Add(new DataColumn("description", typeof(string)));
      table.Columns.Add(new DataColumn("idcountry", typeof(int)));
      table.Columns.Add(new DataColumn("country", typeof(string)));
      table.Columns.Add(new DataColumn("idregion", typeof(int)));
      table.Columns.Add(new DataColumn("region", typeof(string)));

      table.Rows.Add(1, "Lisbon", 1, "Portugal", 1, "Europe");
      table.Rows.Add(2, "New York", 2, "USA", 2, "North America");
      table.Rows.Add(3, "Paris", 3, "France", 1, "Europe");
      table.Rows.Add(4, "Munich", 4, "Germany", 1, "Europe");
      table.Rows.Add(5, "Berlin", 4, "Germany", 1, "Europe");
      table.Rows.Add(6, "London", 5, "Uk", 1, "Europe");

      return table;
    }
  }
}