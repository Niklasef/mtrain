using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HttpApi
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private static object padLock = new object();
        [HttpPost]
        public IActionResult Command([ModelBinder(typeof(CommandModelBinder))]Server.ICommand command)
        {
            lock (padLock)
            {
                command.Execute();
                return Ok();
            }
        }

        [HttpPost]
        public IActionResult Query([ModelBinder(typeof(QueryModelBinder))]Server.IQuery query)
        {
            lock (padLock)
            {
                var result = query.Execute();
                var json = JsonConvert.SerializeObject(
                    result,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                return new ContentResult()
                {
                    Content = json,
                    ContentType = "application/json",
                    StatusCode = 200
                };
            }
        }
    }
}
