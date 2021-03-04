using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using UrlService.Domain;

namespace UrlService.DataAccess
{
  public class SqlUrlDataService : IUrlDataService
  {
    private string ConnectionString { get; }

    public SqlUrlDataService(IConfiguration cfg)
    {
      this.ConnectionString = cfg["ConnectionString"];
    }

    public Result<UrlEntry> AddShortUrl(UrlEntry entry)
    {
      using (var cn = Open(ConnectionString))
      {
        using (var cmd = CreateProc(cn, "AddUrlKey"))
        {
          cmd.Parameters.AddWithValue("@urlKey", entry.Key.Value);
          cmd.Parameters.AddWithValue("@url", entry.Url);
          return CreateResult(cmd, dr => entry);
        }
      }
    }

    public Result<UrlEntry> GetUrl(UrlKey key)
    {
      using (var cn = Open(ConnectionString))
      {
        using (var cmd = CreateProc(cn, "GetUrl"))
        {
          cmd.Parameters.AddWithValue("@urlKey", key.Value);
          return CreateResult(cmd, dr => new UrlEntry(key, dr.GetString(1)));
        }
      }
    }

    private static SqlConnection Open(string connectionString)
    {
      SqlConnection cn = new SqlConnection(connectionString);
      cn.Open();
      return cn;
    }

    private static SqlCommand CreateProc(SqlConnection cn, string name)
    {
      SqlCommand cmd = cn.CreateCommand();
      cmd.CommandText = name;
      cmd.CommandType = CommandType.StoredProcedure;
      return cmd;
    }

    private static Result<T> CreateResult<T>(SqlCommand cmd, Func<SqlDataReader, T> factory)
    {
      using (var dr = cmd.ExecuteReader())
      {
        if (dr.Read())
        {
          // Stored procs contain an error field that should either contain error text or and empty string.
          var error = dr["Error"].ToString();

          var result = string.IsNullOrEmpty(error) ?
            Result<T>.Ok(factory(dr)) :
            Result<T>.Fail(error.ToString());
          return result;
        }
        else
          return Result<T>.Fail($"{cmd.CommandText} - result contains no rows!");
      }
    }
  }
}
