using AppInsightsDemo.API.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace AppInsightsDemo.API.DataAccess
{
  public class SqlDataAccess : IDataAccess
  {
    protected string ConnectionString { get; set; }

    public SqlDataAccess(IOptions<ConnectionStringOptions> connStringOptions)
    {
      this.ConnectionString = connStringOptions.Value.AdventureWorks;
    }

    protected async Task<SqlConnection> GetConnectionAsync()
    {
      SqlConnection connection = new SqlConnection(this.ConnectionString);
      if (connection.State != ConnectionState.Open)
        await connection.OpenAsync();
      return connection;
    }

    public DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType)
    {
      SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);
      command.CommandType = commandType;
      return command;
    }

    public DbParameter GetParameter(string parameter, object value)
    {
      SqlParameter parameterObject = new SqlParameter(parameter, value != null ? value : DBNull.Value);
      parameterObject.Direction = ParameterDirection.Input;
      return parameterObject;
    }

    public DbParameter GetParameterOut(string parameter, DbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput)
    {
      SqlParameter parameterObject = new SqlParameter(parameter, type); ;

      parameterObject.Direction = parameterDirection;

      if (value != null)
      {
        parameterObject.Value = value;
      }
      else
      {
        parameterObject.Value = DBNull.Value;
      }

      return parameterObject;
    }

    public async Task<int> ExecuteNonQueryAsync(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
    {
      int returnValue = -1;

      try
      {
        using (SqlConnection connection = await this.GetConnectionAsync())
        {
          DbCommand cmd = this.GetCommand(connection, procedureName, commandType);

          if (parameters != null && parameters.Count > 0)
          {
            cmd.Parameters.AddRange(parameters.ToArray());
          }

          returnValue = await cmd.ExecuteNonQueryAsync();
        }
      }
      catch (Exception ex)
      {
        //LogException("Failed to ExecuteNonQuery for " + procedureName, ex, parameters);
        throw;
      }

      return returnValue;
    }

    public async Task<object> ExecuteScalarAsync(string procedureName, List<DbParameter> parameters)
    {
      object returnValue = null;

      try
      {
        using (DbConnection connection = await this.GetConnectionAsync())
        {
          DbCommand cmd = this.GetCommand(connection, procedureName, CommandType.StoredProcedure);

          if (parameters != null && parameters.Count > 0)
          {
            cmd.Parameters.AddRange(parameters.ToArray());
          }

          returnValue = await cmd.ExecuteScalarAsync();
        }
      }
      catch (Exception ex)
      {
        //LogException("Failed to ExecuteScalar for " + procedureName, ex, parameters);
        throw;
      }

      return returnValue;
    }

    public async Task<DbDataReader> GetDataReaderAsync(string commandText, List<DbParameter> parameters, CommandType commandType = CommandType.Text)
    {
      DbDataReader ds;

      try
      {
        DbConnection connection = await this.GetConnectionAsync();
        {
          DbCommand cmd = this.GetCommand(connection, commandText, commandType);
          if (parameters != null && parameters.Count > 0)
          {
            cmd.Parameters.AddRange(parameters.ToArray());
          }

          ds = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }
      }
      catch (Exception ex)
      {
        //LogException("Failed to GetDataReader for " + procedureName, ex, parameters);
        throw;
      }

      return ds;
    }
  }
}
