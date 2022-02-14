using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiContribuyente.Entidades;

namespace WebApiContribuyente.Controllers
{
    [ApiController]
    [Route("api/declaraciones")]
    public class DeclaracionesController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public DeclaracionesController (ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Declaracion>>> GetAll()
        {
            return await dbContext.Declaraciones.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Declaracion>> Get(int id)
        {
            return await dbContext.Declaraciones.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Declaracion declaracion)
        {
            var existeContribuyente = await dbContext.Contribuyentes.AnyAsync(x => x.Id == declaracion.ContribuyenteId);
            
            if(!existeContribuyente)
            {
                return BadRequest($"No existe el contribuyente con el id: {declaracion.ContribuyenteId}");
            }

            dbContext.Add(declaracion);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Declaracion declaracion, int id)
        {
            var exist = await dbContext.Declaraciones.AnyAsync(x => x.Id == id);

            if(!exist)
            {
                return NotFound("La declaración especificada no existe");
            }

            if(declaracion.Id != id)
            {
                return BadRequest("El id de la declaración no coincide con el establecido en la url.");
            }

            dbContext.Update(declaracion);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Declaraciones.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound("El recurso especificado no existe");
            }

            dbContext.Remove(new Declaracion { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }


}
