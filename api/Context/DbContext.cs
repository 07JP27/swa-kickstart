using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace api.Context
{
  public class DbContext
  {
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DbContext(IConfiguration configuration)
    {
      _configuration = configuration;
      _connectionString = _configuration.GetConnectionString("SqlConnection");
    }

    public IDbConnection CreateConnection()
      => new SqlConnection(Environment.GetEnvironmentVariable("AzureSQL"));
  }
}