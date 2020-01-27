using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace AppInsightsDemo.API.DataAccess
{
  public interface IDataAccess
  {
    DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType);
    DbParameter GetParameter(string parameter, object value);
    DbParameter GetParameterOut(string parameter, DbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput);
    Task<int> ExecuteNonQueryAsync(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure);    
    Task<object> ExecuteScalarAsync(string procedureName, List<DbParameter> parameters);
    Task<DbDataReader> GetDataReaderAsync(string commandText, List<DbParameter> parameters, CommandType commandType = CommandType.Text);
  }
}
