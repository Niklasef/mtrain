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
        [HttpPost]
        public IActionResult Command([ModelBinder(typeof(CommandModelBinder))]Server.ICommand command)
        {
            command.Execute();
            return Ok();
        }

        [HttpPost]
        public IActionResult Query([ModelBinder(typeof(QueryModelBinder))]Server.IQuery query)
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
