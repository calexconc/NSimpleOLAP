using NSimpleOLAP.Configuration;
using NSimpleOLAP.Configuration.Extensions;
using NSimpleOLAP.Data.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace NSimpleOLAP.Data.Readers
{
  /// <summary>
  /// Description of DBReader.
  /// </summary>
  public class DBReader : AbsReader
  {
    private Row _row;
    private IDbCommand _command;
    private IDataReader _reader;
    private IDbConnection _connection;

    public DBReader(DataSourceConfig config)
    {
      this.Config = config;
      this.Init();
    }

    public override bool Next()
    {
      bool ret = false;

      if (_reader == null)
        _reader = _command.ExecuteReader();

      if (_reader.Read())
      {
        _row.SetData(this.GetValues());
        this.Current = _row;
        ret = true;
      }
      else
      {
        _row = null;
        this.Current = null;
      }

      return ret;
    }

    public override void Dispose()
    {
      _reader.Close();
      _connection.Close();
      _reader.Dispose();
      _command.Dispose();
      _connection.Dispose();
    }

    #region private methods

    private void Init()
    {
      _row = new DBReader.Row(this.Config.Fields.GetFieldIndexes());
      _connection = this.GetConnection();
      _connection.Open();
      _command = _connection.CreateCommand();
      _command.CommandText = this.Config.DBConfig.Query;
    }

    private object[] GetValues()
    {
      object[] values = new object[this.Config.Fields.Count];

      for (int i = 0; i < this.Config.Fields.Count; i++)
      {
        if (_reader[this.Config.Fields[i].Name] != DBNull.Value)
          values[i] = Convert.ChangeType(_reader[this.Config.Fields[i].Name], this.Config.Fields[i].FieldType);
        else
          values[i] = null;
      }

      return values;
    }

    private IDbConnection GetConnection()
    {
      try
      {
        ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[this.Config.DBConfig.Connection];
        IDbConnection connection = DBFactory.CreateConnection(settings.ProviderName);
        connection.ConnectionString = settings.ConnectionString;

        return connection;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        throw;

        // todo some extra error handling
      }
    }

    #endregion private methods

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