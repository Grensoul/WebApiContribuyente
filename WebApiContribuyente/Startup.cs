using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WebApiContribuyente.Filtros;
using WebApiContribuyente.Middlewares;
using WebApiContribuyente.Services;

namespace WebApiContribuyente
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opciones =>
            {
                opciones.Filters.Add(typeof(FiltroDeExcepcion));
            }).AddJsonOptions(x => 
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Se encarga de configurar ApplicationDbContext como un servicio
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            /* Transient da una nueva instancia de la clase declarada, sirve para funciones
             * que ejecutan una funcionalidad y listo, sin tener que mantener información
             * que será reutilizada en otros lugares */
            services.AddTransient<IService, ServiceA>();
            // services.AddTransient<ServiceA>();
            services.AddTransient<ServiceTransient>();
            /* Scoped el tiempo de vida de la clase declarada aumenta, sin embargo, Scoped da
             * diferentes instancias de acuerdo a cada quien mande la solicitud, es decir Lorenzo
             * tiene su instancia y Contribuyente otra */
            // service.AddScoped<IService, ServiceA>();
            services.AddScoped<ServiceScoped>();
            /*Singleton se tiene la misma instancia siempre para todos los usuarios en todos los días,
             * todos los usuariosque hagan una petición van a tener la misma info compartida entre todos */
            //service.AddSingleton<IService, ServiceA>();   
            services.AddSingleton<ServiceSingleton>();
            services.AddTransient<FiltroDeAccion>();
            services.AddHostedService<EscribirEnArchivo>();
            services.AddResponseCaching();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiContribuyente", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Use permite agregar mi propio proceso sin afectar a los demas como Run
            //app.Use(async (context, siguiente) =>
            //{
            //    using (var ms = new MemoryStream())
            //    {
            //        // Se asigna el body del response en una variable y se le da el valor de memorystream
            //        var bodyOriginal = context.Response.Body;
            //        context.Response.Body = ms;

            //        // Permite continuar con la línea
            //        await siguiente.Invoke();

            //        // Guardamos lo que le respondemos en el string
            //        ms.Seek(0, SeekOrigin.Begin);
            //        string response = new StreamReader(ms).ReadToEnd();

            //        // Leemos el stream y lo colocamos como estaba
            //        await ms.CopyToAsync(bodyOriginal);
            //        context.Response.Body = bodyOriginal;

            //        logger.LogInformation(response);
            //    }
            //});

            // Método para utilizar la clase middleware propia
            //app.UseMiddleware<ResponseHttpMiddleware>();

            //Metodo para utilizar la clase middleware sin exponer la clase. 
            //app.UseResponseHttpMiddleware();

            /* Para condicionar la ejecución del middleware según una ruta específica se utiliza Map
             * Al utilizar Map permite que en lugar de ejecutar linealmente podemos agregar rutas
             * específicas para nuestro middleware*/
            app.Map("/ruta1", app =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("Interceptando peticiones");
                });
            });


            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

       
    }
}


