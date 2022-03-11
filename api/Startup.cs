using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using api.Context;
using api.Repository;

[assembly: FunctionsStartup(typeof(api.Startup))]

namespace api
{
  public class Startup : FunctionsStartup
  {
    public override void Configure(IFunctionsHostBuilder builder)
    {
      builder.Services.AddSingleton<DbContext>();
      builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
    }
  }
}