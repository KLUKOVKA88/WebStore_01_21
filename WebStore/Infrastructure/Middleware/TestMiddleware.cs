using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebStore.Infrastructure.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _Next;
        public TestMiddleware(RequestDelegate Next) =>  _Next = Next;

        public async Task InvokeAsync(HttpContext context)
        {
            //Действие до след узла в конвейере
            //context.Response.

            var next = _Next(context);

            //Действие во время того, как оставшаяся часть конвейера что-то там делает с контекстом

            await next; //Точка синхронизации

            //Действие по завершении работы оставшейся части конвейера

        }

    }
}
