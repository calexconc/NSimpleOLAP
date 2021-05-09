using NSimpleOLAP.Configuration;
using NSimpleOLAP.Data.Transformers;
using System.Collections.Generic;

namespace NSimpleOLAP.Data.Readers
{
  public class TransformerReader : AbsReader
  {
    private Row _row;
    private int _currentIndex;

    public TransformerReader(DataSourceConfig config)
    {
      this.Config = config;
      _currentIndex = 0;
      Init();
    }

    public override bool Next()
    {
      if (this.Config.TransformerConfig.Transformers.Count > _currentIndex)
      {
        var transformer = this.Config.TransformerConfig.Transformers[_currentIndex];

        _row.SetData(new object[] { transformer.Name, transformer.ReturnValue, new MemberIntervalTransformer(transformer.Name, transformer) });
        this.Current = _row;
        _currentIndex++;
        return true;
      }

      this.Current = null;

      return false;
    }

    public override void Dispose()
    {
    }

    private void Init()
    {
      _row = new TransformerReader.Row(new Dictionary<string, int> { { "Name", 0 }, { "Value", 1 }, { "Transformer", 2 } });
    }

    #region private class

    private class Row : AbsRowData
    {
      public Row(Dictionary<string, int> fields)
      {
        this._indexes = fields;
      }

      public void SetData(object[] values)
      {
        _values = values;
      }
    }

    #endregion private class
  }
}