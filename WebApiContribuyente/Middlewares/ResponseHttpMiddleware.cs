namespace WebApiContribuyente.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseHttpMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ResponseHttpMiddleware>();
        }
    }

    public class ResponseHttpMiddleware
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<ResponseHttpMiddleware> logger;

        public ResponseHttpMiddleware(RequestDelegate siguiente, ILogger<ResponseHttpMiddleware> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        // Para poder utilizar la clase como Middleware se ocupa el método Invoke o
        // InvokeAsync si es asíncrono
        // Debe retornar un Task
        public async Task InvokeAsync(HttpContext context)
        {
            using (var ms = new MemoryStream())
            {
                // Se asigna el body del response en una variable y se le da el valor de memorystream
                var bodyOriginal = context.Response.Body;
                context.Response.Body = ms;
                
                // Permite continuar con la línea
                await siguiente(context);

                // Guardamos lo que le respondemos en el string
                ms.Seek(0, SeekOrigin.Begin);
                string response = new StreamReader(ms).ReadToEnd();
                
                // Leemos el stream y lo colocamos como estaba
                await ms.CopyToAsync(bodyOriginal);
                context.Response.Body = bodyOriginal;

                logger.LogInformation(response);
            }
        }
    }
}
