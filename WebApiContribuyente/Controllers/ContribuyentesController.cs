using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiContribuyente.Entidades;

namespace WebApiContribuyente.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ContribuyentesController : ControllerBase
    {

        private readonly ApplicationDbContext dbContext;

        public ContribuyentesController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        /*[HttpGet] // api/contribuyentes
        [HttpGet("listado")] // api/contribuyentes/listado
        [HttpGet("/listado")] // /listado
        public async Task<ActionResult<List<Contribuyente>>> Get()
        {
            return await dbContext.Contribuyentes.Include(x => x.Declaraciones).ToListAsync();
        }*/

        [HttpGet("primero")] // api/contribuyentes/primero
        public async Task<ActionResult<Contribuyente>> PrimerContribuyente([FromHeader] int valor)
        {
            return await dbContext.Contribuyentes.FirstOrDefaultAsync();
        }

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

        /*[HttpGet("{nombre}")]
        public async Task<ActionResult<Contribuyente>> Get([FromRoute] string nombre)
        {
            var contribuyente = await dbContext.Contribuyentes.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if (contribuyente == null)
            {
                return NotFound();
            }

            return contribuyente;
        }*/

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

        [HttpGet]
        public List<Contribuyente> Get()
        {
            return dbContext.Contribuyentes.Include(x => x.Declaraciones).ToList();
        }

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
