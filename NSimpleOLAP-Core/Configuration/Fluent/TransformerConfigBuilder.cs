using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSimpleOLAP.Configuration.Fluent
{
  public class TransformerConfigBuilder
  {
    private TransformerConfigElement _element;
    private Type _currentType;

    public TransformerConfigBuilder()
    {
      _element = new TransformerConfigElement();
    }

    public TransformerConfigBuilder AddIntervalSegment(string name, int? lowerBound, int? upperBound)
    {
      var element = new TransformerItemConfig();

      if (_currentType == null)
        _currentType = typeof(int);

      Validate(name, lowerBound, upperBound);

      element.IsIntervalSetup = true;
      element.FieldType = typeof(int);
      element.Name = name;
      element.UpperValue = upperBound;
      element.LowerValue = lowerBound;

      _element.Transformers.Add(element);
      element.ReturnValue = _element.Transformers.Count;

      return this;
    }

    public TransformerConfigBuilder AddIntervalSegment(string name, double? lowerBound, double? upperBound)
    {
      var element = new TransformerItemConfig();

      if (_currentType == null)
        _currentType = typeof(double);

      Validate(name, lowerBound, upperBound);

      element.IsIntervalSetup = true;
      element.FieldType = typeof(double);
      element.Name = name;
      element.UpperValue = upperBound;
      element.LowerValue = lowerBound;


      _element.Transformers.Add(element);
      element.ReturnValue = _element.Transformers.Count;

      return this;
    }

    private void Validate(string name, int? lowerBound, int? upperBound)
    {
      if (string.IsNullOrEmpty(name)
        || string.IsNullOrEmpty(name.Trim()))
        throw new Exception($"Transformer has no name.");

      if (_currentType != typeof(int))
        throw new Exception($"Transformer {name} is of different type from the previous segments.");

      if (lowerBound == null && upperBound == null)
        throw new Exception($"Transformer {name} has no defined bounds.");

      if (lowerBound.HasValue && upperBound.HasValue
        && lowerBound.Value > upperBound.Value)
        throw new Exception($"Transformer {name} has the lower bound bigger than the upper bound.");
    }

    private void Validate(string name, double? lowerBound, double? upperBound)
    {
      if (string.IsNullOrEmpty(name)
        || string.IsNullOrEmpty(name.Trim()))
        throw new Exception($"Transformer has no name.");

      if (_currentType != typeof(int))
        throw new Exception($"Transformer {name} is of different type from the previous segments.");

      if (lowerBound == null && upperBound == null)
        throw new Exception($"Transformer {name} has no defined bounds.");

      if (lowerBound.HasValue && upperBound.HasValue
        && lowerBound.Value > upperBound.Value)
        throw new Exception($"Transformer {name} has the lower bound bigger than the upper bound.");
    }

    internal TransformerConfigElement Create()
    {
      return _element;
    }
  }
}
