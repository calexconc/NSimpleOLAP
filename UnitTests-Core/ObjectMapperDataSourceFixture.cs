using System;
using System.Collections.Generic;

namespace UnitTests
{
  public class Fact
  {
    public int Category { get; set; }

    public int Gender { get; set; }

    public int Place { get; set; }

    public DateTime Date { get; set; }

    public double Expenses { get; set; }

    public int Items { get; set; }
  }

  public class Category
  {
    public int Id { get; set; }

    public string Description { get; set; }
  }

  public class Gender
  {
    public int Id { get; set; }

    public string Description { get; set; }
  }

  public class Place
  {
    public int Id { get; set; }

    public string Description { get; set; }

    public int IdCountry { get; set; }

    public string Country { get; set; }

    public int IdRegion { get; set; }

    public string Region { get; set; }
  }

  internal class ObjectMapperDataSourceFixture
  {
    public static List<Fact> GetFacts()
    {
      return new List<Fact> {
        new Fact() { Category = 1, Gender = 1, Place = 2, Date = new DateTime(2021, 1, 15), Expenses = 1000.12, Items = 30 },
        new Fact() { Category = 2, Gender = 2, Place = 2, Date = new DateTime(2021, 3, 5), Expenses = 200.50, Items = 5 },
        new Fact() { Category = 4, Gender = 2, Place = 5, Date = new DateTime(2021, 10, 17), Expenses = 11500.00, Items = 101 },
        new Fact() { Category = 3, Gender = 2, Place = 2, Date = new DateTime(2021, 8, 25), Expenses = 100.00, Items = 20 },
        new Fact() { Category = 2, Gender = 1, Place = 6, Date = new DateTime(2021, 2, 27), Expenses = 10.10, Items = 5 },
        new Fact() { Category = 1, Gender = 2, Place = 2, Date = new DateTime(2021, 8, 30), Expenses = 700.10, Items = 36 },
        new Fact() { Category = 5, Gender = 2, Place = 5, Date = new DateTime(2021, 12, 15), Expenses = 100.40, Items = 31 },
        new Fact() { Category = 1, Gender = 1, Place = 3, Date = new DateTime(2021, 9, 7), Expenses = 100.12, Items = 12 },
        new Fact() { Category = 3, Gender = 2, Place = 3, Date = new DateTime(2021, 6, 1), Expenses = 10.12, Items = 30 },
        new Fact() { Category = 2, Gender = 2, Place = 2, Date = new DateTime(2021, 6, 5), Expenses = 10000.12, Items = 30 },
        new Fact() { Category = 1, Gender = 2, Place = 1, Date = new DateTime(2021, 5, 4), Expenses = 100.12, Items = 1 },
        new Fact() { Category = 4, Gender = 2, Place = 2, Date = new DateTime(2021, 1, 3), Expenses = 10.12, Items = 6 },
        new Fact() { Category = 2, Gender = 2, Place = 3, Date = new DateTime(2021, 11, 9), Expenses = 100.12, Items = 44 },
        new Fact() { Category = 1, Gender = 2, Place = 3, Date = new DateTime(2021, 7, 1), Expenses = 10.12, Items = 8 },
        new Fact() { Category = 4, Gender = 1, Place = 1, Date = new DateTime(2021, 4, 24), Expenses = 100.12, Items = 5 },
        new Fact() { Category = 1, Gender = 1, Place = 6, Date = new DateTime(2021, 6, 2), Expenses = 10.12, Items = 7 },
        new Fact() { Category = 4, Gender = 3, Place = 6, Date = new DateTime(2021, 5, 18), Expenses = 100.12, Items = 30 },
        new Fact() { Category = 2, Gender = 1, Place = 2, Date = new DateTime(2021, 8, 21), Expenses = 60.99, Items = 8 },
        new Fact() { Category = 1, Gender = 2, Place = 2, Date = new DateTime(2021, 2, 16), Expenses = 6000.00, Items = 89 },
        new Fact() { Category = 4, Gender = 3, Place = 6, Date = new DateTime(2021, 3, 7), Expenses = 600.00, Items = 75 },
        new Fact() { Category = 1, Gender = 1, Place = 6, Date = new DateTime(2021, 1, 1), Expenses = 10.00, Items = 12 },
        new Fact() { Category = 4, Gender = 2, Place = 2, Date = new DateTime(2021, 7, 28), Expenses = 2000.00, Items = 30 },
        new Fact() { Category = 5, Gender = 2, Place = 6, Date = new DateTime(2021, 12, 20), Expenses = 50.10, Items = 11 },
        new Fact() { Category = 3, Gender = 1, Place = 3, Date = new DateTime(2021, 6, 8), Expenses = 130.50, Items = 2 }
      };
    }

    public static List<Category> GetCategories()
    {
      return new List<Category> {
        new Category() { Id = 1, Description = "toys" },
        new Category() { Id = 2, Description = "clothes" },
        new Category() { Id = 3, Description = "furniture" },
        new Category() { Id = 4, Description = "shoes" },
        new Category() { Id = 5, Description = "Video Games" }
      };
    }

    public static List<Gender> GetGender()
    {
      return new List<Gender> {
        new Gender() { Id = 1, Description = "male" },
        new Gender() { Id = 2, Description = "female" },
        new Gender() { Id = 3, Description = "No Data" }
      };
    }

    public static List<Place> GetPlaces()
    {
      return new List<Place> {
        new Place() { Id = 1, Description = "Lisbon", IdCountry = 1, Country = "Portugal", IdRegion = 1, Region = "Europe" },
        new Place() { Id = 2, Description = "New York", IdCountry = 2, Country = "USA", IdRegion = 2, Region = "North America" },
        new Place() { Id = 3, Description = "Paris", IdCountry = 3, Country = "France", IdRegion = 1, Region = "Europe" },
        new Place() { Id = 4, Description = "Munich", IdCountry = 4, Country = "Germany", IdRegion = 1, Region = "Europe" },
        new Place() { Id = 5, Description = "Berlin", IdCountry = 4, Country = "Germany", IdRegion = 1, Region = "Europe" },
        new Place() { Id = 6, Description = "London", IdCountry = 5, Country = "Uk", IdRegion = 1, Region = "Europe" }
      };
    }
  }
}