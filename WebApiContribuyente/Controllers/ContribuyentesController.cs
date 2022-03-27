using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiContribuyente.Entidades;
using WebApiContribuyente.Filtros;
using WebApiContribuyente.Services;

namespace WebApiContribuyente.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ContribuyentesController : ControllerBase
    {

        private readonly ApplicationDbContext dbContext;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger<ContribuyentesController> logger;


        public ContribuyentesController(ApplicationDbContext context, IService service,
            ServiceTransient serviceTransient, ServiceScoped serviceScoped,
            ServiceSingleton serviceSingleton, ILogger<ContribuyentesController> logger)
        {
            this.dbContext = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
        }

        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult ObtenerGuid()
        {
            logger.LogInformation("Durante la ejecución de la acción");
            return Ok(new
            {
                ContribuyentesControllerTransient = serviceTransient.guid,
                ServiceA_Transient = service.GetTransient(),
                ContribuyentessControllerScoped = serviceScoped.guid,
                ServiceA_Scoped = service.GetScoped(),
                ContribuyentesControllerSingleton = serviceSingleton.guid,
                ServiceA_Singleton = service.GetSingleton()
            });
        }

        [HttpGet] // api/contribuyentes
        [HttpGet("listado")] // api/contribuyentes/listado
        [HttpGet("/listado")] // /listado
        //[Authorize]
        public async Task<ActionResult<List<Contribuyente>>> Get()
        {
            // * Niveles de logs
            // Critical
            // Error
            // Warning
            // Information - Configuration actual
            // Debug
            // Trace
            throw new NotImplementedException();
            logger.LogInformation("Se obtiene el listado de Contribuyentes");
            logger.LogWarning("¡Se obtiene el listado de Contribuyentes!");
            service.EjecutarJob();
            return await dbContext.Contribuyentes.Include(x => x.Declaraciones).ToListAsync();
        }

        /*[HttpGet("primero")] // api/contribuyentes/primero
        public async Task<ActionResult<Contribuyente>> PrimerContribuyente([FromHeader] int valor)
        {
            return await dbContext.Contribuyentes.FirstOrDefaultAsync();
        }*/

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Contribuyente>> Get(int id)
        {
            var contribuyente = await dbContext.Contribuyentes.FirstOrDefaultAsync(x => x.Id == id);

            if(contribuyente == null)
            {
                return NotFound();
            }

            return contribuyente;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Contribuyente>> GetContribuyenteNombre([FromRoute] string nombre)
        {
            var contribuyente = await dbContext.Contribuyentes.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if (contribuyente == null)
            {
                logger.LogError("No se encontró el contribuyente");
                return NotFound();
            }

            return contribuyente;
        }

        /*[HttpGet("{id:int}/{nombre?}")]
        public async Task<ActionResult<Contribuyente>> Get(int id, string nombre)
        {
            var contribuyente = await dbContext.Contribuyentes.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if (contribuyente == null)
            {
                return NotFound("No se encontró ningún recurso");
            }

            return contribuyente;
        }*/

        /*[HttpGet("{id:int}/{nombre=Lorenzo}")]
        public async Task<ActionResult<Contribuyente>> Get(int id, string nombre)
        {
            var contribuyente = await dbContext.Contribuyentes.FirstOrDefaultAsync(x => x.Id == id);

            if (contribuyente == null)
            {
                return NotFound("No se encontró ningún recurso");
            }

            return contribuyente;
        }*/

        /*[HttpGet]
        public List<Contribuyente> Get()
        {
            return dbContext.Contribuyentes.Include(x => x.Declaraciones).ToList();
        }*/

        [HttpGet("{id:int}/{nombre=Lorenzo}")]
        public ActionResult<Contribuyente> Get(int id, string nombre)
        {
            var contribuyente = dbContext.Contribuyentes.FirstOrDefault(x => x.Id == id);
            if (contribuyente == null)
            {
                return NotFound("No se encontró ningún recurso");
            }
            return contribuyente;
        }

        [HttpGet("query")]
        public async Task<ActionResult<Contribuyente>> Get([FromQuery] string nombre)
        {
            var contribuyente = await dbContext.Contribuyentes.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            if (contribuyente == null)
            {
                return NotFound();
            }
            return contribuyente;
        }

        [HttpGet("query2")]
        public async Task<ActionResult<Contribuyente>> Get([FromQuery] string nombre, [FromQuery] int id)
        {
            var contribuyente = await dbContext.Contribuyentes.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));
            if (contribuyente == null)
            {
                return NotFound();
            }
            return contribuyente;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Contribuyente contribuyente)
        {
            // Ejemplo para validar desde el controlador con la BD con ayuda del dbContext

            var existeContribuyenteMismoNombre = await dbContext.Contribuyentes.AnyAsync(x => x.Nombre == contribuyente.Nombre);

            if (existeContribuyenteMismoNombre)
            {
                return BadRequest("Ya existe un contribuyente con el mismo nombre");
            }

            dbContext.Add(contribuyente);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")] // api/contribuyentes/1
        public async Task<ActionResult> Put(Contribuyente contribuyente, int id)
        {
            if(contribuyente.Id != id)
            {
                return BadRequest("El id del contribuyente no coincide con el establecido en la url.");
            }

            dbContext.Update(contribuyente);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Contribuyentes.AnyAsync(x => x.Id == id);
            if(!exist)
            {
                return NotFound();
            }

            dbContext.Remove(new Contribuyente()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }


    }
}
