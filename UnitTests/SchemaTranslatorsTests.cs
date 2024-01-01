using NSimpleOLAP;
using NSimpleOLAP.Query;
using NUnit.Framework;
using System.Collections.Generic;
using NSimpleOLAP.Common.Utils;

namespace UnitTests
{
  [TestFixture]
  public class SchemaTranslatorsTests
  {
    private Cube<int> cube;

    public SchemaTranslatorsTests()
    {
      Init();
    }

    public void Init()
    {
      cube = CubeSourcesFixture.GetBasicCubeThreeDimensionsTwoMeasures2();
      cube.Initialize();
      cube.Process();
    }

    [OneTimeTearDown]
    public void Dispose()
    {
      cube.Dispose();
    }

    [Test]
    public void Dimension_Translator_GetDimension_Test()
    {
      var translator = new DimensionReferenceTranslator<int>(cube.Schema);

      var result = translator.GetDimension("category");

      Assert.AreEqual(1, result);
    }

    [Test]
    public void Dimension_Translator_GetDimensionMember_Test()
    {
      var translator = new DimensionReferenceTranslator<int>(cube.Schema);
      var dimKey = translator.GetDimension("category");
      var result = translator.GetDimensionMember(dimKey, "shoes");

      Assert.AreEqual(4, result);
    }

    [Test]
    public void Dimension_Translator_Translate_Simple_Test()
    {
      var translator = new DimensionReferenceTranslator<int>(cube.Schema);

      var result = translator.Translate("category.shoes");

      Assert.IsTrue(result.Length > 0);
      Assert.AreEqual(new KeyValuePair<int, int>(1, 4), result[0]);
    }

    [Test]
    public void Dimension_Translator_Translate_Two_Dims_Test()
    {
      var translator = new DimensionReferenceTranslator<int>(cube.Schema);

      var result = translator.Translate("sex.male.category.shoes");

      Assert.IsTrue(result.Length > 1);
      Assert.AreEqual(new KeyValuePair<int, int>(2, 1), result[0]);
      Assert.AreEqual(new KeyValuePair<int, int>(1, 4), result[1]);
    }

    [Test]
    public void Dimension_Translator_Translate_Two_Dims_With_Selector_All_Test()
    {
      var translator = new DimensionReferenceTranslator<int>(cube.Schema);

      var result = translator.Translate("sex.male.category.All");

      Assert.IsTrue(result.Length > 2);
      Assert.AreEqual(new KeyValuePair<int, int>(2, 1), result[0]);
      Assert.AreEqual(new KeyValuePair<int, int>(1, 0), result[1]);
      Assert.AreEqual(new KeyValuePair<int, int>(0, 0), result[2]);
    }

    [Test]
    public void Dimension_Translator_Translate_Two_Dims_With_Selector_All_2_Test()
    {
      var translator = new DimensionReferenceTranslator<int>(cube.Schema);

      var result = translator.Translate("sex.All.category.shoes");

      Assert.IsTrue(result.Length > 2);
      Assert.AreEqual(new KeyValuePair<int, int>(2, 0), result[0]);
      Assert.AreEqual(new KeyValuePair<int, int>(0, 0), result[1]);
      Assert.AreEqual(new KeyValuePair<int, int>(1, 4), result[2]);
    }

    [Test]
    public void Dimension_Translator_Translate_Two_Dims_No_Segment_Test()
    {
      var translator = new DimensionReferenceTranslator<int>(cube.Schema);

      var result = translator.Translate("sex.category");

      Assert.IsTrue(result.Length > 1);
      Assert.AreEqual(new KeyValuePair<int, int>(2, 0), result[0]);
      Assert.AreEqual(new KeyValuePair<int, int>(1, 0), result[1]);
    }

    [Test]
    public void Dimension_Translator_Translate_One_Dim_No_Segment_Test()
    {
      var translator = new DimensionReferenceTranslator<int>(cube.Schema);

      var result = translator.Translate("sex");

      Assert.IsTrue(result.Length == 1);
      Assert.AreEqual(new KeyValuePair<int, int>(2, 0), result[0]);
    }

    [Test]
    public void Measure_Translator_Translate_Test()
    {
      var translator = new MeasureReferenceTranslator<int>(cube.Schema);

      var result = translator.Translate("quantity");

      Assert.AreEqual(5, result);
    }

    [Test]
    public void Measure_Translator_Translate_Type_Test()
    {
      var translator = new MeasureReferenceTranslator<int>(cube.Schema);

      var key = translator.Translate("quantity");
      var result = translator.MeasureType(key);

      Assert.AreEqual(typeof(int), result);
    }

    [Test]
    public void Measure_Translator_Translate_Type_2_Test()
    {
      var translator = new MeasureReferenceTranslator<int>(cube.Schema);

      var key = translator.Translate("spent");
      var result = translator.MeasureType(key);

      Assert.AreEqual(typeof(double), result);
    }
  }
}