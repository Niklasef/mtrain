using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace HttpApi
{
    public class CommandModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string body;
            using (var reader = new StreamReader(bindingContext.HttpContext.Request.Body))
            {
                body = reader.ReadToEnd();
            }

            var model = JsonConvert.DeserializeObject<Server.ICommand>(
                body,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
