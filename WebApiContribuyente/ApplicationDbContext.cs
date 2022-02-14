using Microsoft.EntityFrameworkCore;
using WebApiContribuyente.Entidades;

namespace WebApiContribuyente
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Contribuyente> Contribuyentes { get; set; }
        public DbSet<Declaracion> Declaraciones { get; set; }
    }
}
