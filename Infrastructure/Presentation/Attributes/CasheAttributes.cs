using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    public class CasheAttribute(int TimeToLiveInSeconds = 90) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Create cashe key
            string casheKey = CreateCasheKey(context.HttpContext.Request);

            var casheService = context.HttpContext.RequestServices.GetRequiredService<ICasheService>();

            //search for value with cashe key
            var cashedValue = await casheService.GetAsync(casheKey);

            //return value if not null
            if (cashedValue is not null)
            {
                context.Result = new ContentResult
                {
                    Content = cashedValue,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                return;
            }

            //Invoke .next
            var executedContext = await next();

            //set value with cashe key if result is OK
            if (executedContext.Result is OkObjectResult okResult)
            {
                var timeToLive = TimeSpan.FromSeconds(TimeToLiveInSeconds);
                await casheService.SetAsync(casheKey, okResult.Value!, timeToLive);
            }
        }

        private string CreateCasheKey(HttpRequest request)
        {
            StringBuilder key = new();

            key.Append(request.Path + '?');
            foreach (var item in request.Query.OrderBy(i => i.Key))
            {
                key.Append($"{item.Key}={item.Value}&");
            }
            return key.ToString();
        }
    }
}
