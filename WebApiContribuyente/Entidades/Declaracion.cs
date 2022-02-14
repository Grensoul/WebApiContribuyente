namespace WebApiContribuyente.Entidades
{
    public class Declaracion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Mes { get; set; }
        public string RFC { get; set; }
        public Contribuyente Contribuyente { get; set; }

    }
}
