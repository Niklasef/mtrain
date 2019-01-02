using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.AspNetCore.Mvc;

namespace HttpApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        [HttpPost]
        public IActionResult ExecuteCommand([ModelBinder(typeof(CommandModelBinder))]Server.ICommand command)
        {
            command.Execute();
            return Ok();
        }
    }
}
