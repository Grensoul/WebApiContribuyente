using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiContribuyente.Entidades;

namespace WebApiContribuyente.Controllers
{

    [ApiController]
    [Route("api/contribuyentes")]
    public class ContribuyentesController : ControllerBase
    {

        private readonly ApplicationDbContext dbContext;

        public ContribuyentesController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Contribuyente>>> Get()
        {
            return await dbContext.Contribuyentes.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Contribuyente contribuyente)
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
