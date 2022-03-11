using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using api.Model;
namespace api.Extension
{
    public static class ParsePrincipalExtension
    {
      public static ClientPrincipal ParsePrincipal(this HttpRequest req)
      {
        var principal = new ClientPrincipal();

        if (req.Headers.TryGetValue("x-ms-client-principal", out var header))
        {
          var data = header[0];
          var decoded = Convert.FromBase64String(data);
          var json = Encoding.UTF8.GetString(decoded);
          principal = JsonConvert.DeserializeObject<ClientPrincipal>(json);
        }

        principal.UserRoles = principal.UserRoles?.Except(new string[] { "anonymous" }, StringComparer.CurrentCultureIgnoreCase);

        return principal;
      }
    }
}