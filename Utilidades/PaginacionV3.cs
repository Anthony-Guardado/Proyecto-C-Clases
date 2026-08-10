namespace InventaMeCF.Utilidades
{
    public class PaginacionV3 : Paginacion
    {
        public Dictionary<string, string> Parametros { get; set; } = new Dictionary<string, string>();
        public PaginacionV3() { }
        public PaginacionV3(int totalRegistros, int pagina, int registrosPagina = 10, string controlador = "Home", string accion = "Index") : base(totalRegistros, pagina, registrosPagina, controlador, accion)
        {

        }
    }
}
