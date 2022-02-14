using Microsoft.AspNetCore.Mvc;
using WebApiContribuyente.Entidades;

namespace WebApiContribuyente.Controllers
{

    [ApiController]
    [Route("api/contribuyentes")]
    public class ContribuyentesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Contribuyente>> Get()
        {
            return new List<Contribuyente>()
            {
                new Contribuyente() { Id = 1, Nombre = "Gustavo"},
                new Contribuyente() { Id = 1, Nombre = "Mario"}
            };
        }
    }
}
