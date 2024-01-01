using NSimpleOLAP.Configuration;
using NSimpleOLAP.Data.Interfaces;
using System;

namespace NSimpleOLAP.Data.Transformers
{
  public class MemberIntervalTransformer : IDataTransformer
  {
    private ValueType _lowerBound;
    private ValueType _upperBound;
    private Type _type;

    public MemberIntervalTransformer(string name, TransformerItemConfig config)
    {
      Name = name;
      Config = config;
      _type = config.FieldType;

      Init();
    }

    private void Init()
    {
      if (!Config.IsIntervalSetup)
        throw new Exception($"Transformer {Name} does not setup a value interval.");

      if (Config.UpperValue == null && Config.LowerValue == null)
        throw new Exception($"Transformer {Name} does not setup upper and lower bounds.");

      if (Config.ReturnValue == null)
        throw new Exception($"Transformer {Name} does not setup a return value.");

      if (Config.UpperValue != null)
        _upperBound = (ValueType)Config.UpperValue;

      if (Config.LowerValue != null)
        _lowerBound = (ValueType)Config.LowerValue;
    }

    public string Name { get; private set; }
    public TransformerItemConfig Config { get; private set; }

    public object Transform(object value)
    {
      if (value != null && CheckValue(value))
        return Config.ReturnValue;

      return null;
    }

    private bool CheckValue(object value)
    {
      if (_type == typeof(int))
        return CheckCondition((int?)_lowerBound, (int?)_upperBound, (int)value);
      else if (_type == typeof(double))
        return CheckCondition((double?)_lowerBound, (double?)_upperBound, (double)value);
      else
        return false;
    }

    private bool CheckCondition(int? lower, int? upper, int value)
    {
      if (lower.HasValue && upper.HasValue)
        return value >= lower.Value && value <= upper.Value;

      if (lower.HasValue)
        return value >= lower.Value;

      if (upper.HasValue)
        return value <= upper.Value;

      return false;
    }

    private bool CheckCondition(double? lower, double? upper, double value)
    {
      if (lower.HasValue && upper.HasValue)
        return value >= lower.Value && value <= upper.Value;

      if (lower.HasValue)
        return value >= lower.Value;

      if (upper.HasValue)
        return value <= upper.Value;

      return false;
    }
  }
}