using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Security.Claims;
using System.Text;
using api.Model;
using api.Repository;
using api.Extension;

namespace api
{
    public class ToDo
    {
        private readonly IToDoRepository _toDoRepo;

        public ToDo(IToDoRepository toDoRepo)
        {
            _toDoRepo = toDoRepo;
        }

        [FunctionName("Get")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id:int?}")] HttpRequest req,
            int? id)        
        {
            try
            {
                var items = await _toDoRepo.GetToDoItems();
                return new OkObjectResult(items);
            }
            catch (Exception ex)
            {
                //log error
                //log.LogError(1,ex,"Error getting todo items");
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("Post")]
        public async Task<IActionResult> Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")] HttpRequest req,
            ILogger log)
        {
            var principal = req.ParsePrincipal();
            string body = await new StreamReader(req.Body).ReadToEndAsync();
            var payload = JsonConvert.DeserializeObject<ToDoItem>(body);

            try
            {
                var item = await _toDoRepo.CreateToDoItems(payload.ToDo, principal);
                return new OkResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //log error
                //log.LogError(1,ex,"Error creating todo item");
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("Patch")]
        public async Task<IActionResult> Patch(
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "todo/{id}")] HttpRequest req,
            ILogger log,
            int id)
        {
            var principal = req.ParsePrincipal();
            string body = await new StreamReader(req.Body).ReadToEndAsync();
            var payload = JsonConvert.DeserializeObject<ToDoItem>(body);

            try
            {
                var item = await _toDoRepo.MarkCompleteStatusToDoItems(id, payload.Completed, principal);
                if (item == 0) return new NotFoundResult();
                return new OkResult();
            }
            catch (Exception ex)
            {
                //log error
                //log.LogError(1,ex,"Error creating todo item");
                Console.WriteLine(ex);
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("Delete")]
        public async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo/{id}")] HttpRequest req,
            ILogger log,
            int id)
        {
            try
            {
                var item = await _toDoRepo.DeleteToDoItems(id);
                if (item == 0) return new NotFoundResult();
                return new OkResult();
            }
            catch (Exception ex)
            {
                //log error
                //log.LogError(1,ex,"Error creating todo item");
                return new StatusCodeResult(500);
            }       
        }
    }
}