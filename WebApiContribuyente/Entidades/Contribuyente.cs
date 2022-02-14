namespace WebApiContribuyente.Entidades
{
    public class Contribuyente
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public List<Declaracion> Declaraciones { get; set;}
    }
}
