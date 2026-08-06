namespace GenerarPaquetes.Entities.Domain
{
    /// <summary>
    /// Representa un documento de manual de instalación / Runbook (.doc).
    /// </summary>
    public class RunbookEntity
    {
        public string NombreArchivo { get; set; } = string.Empty;
        public string RutaOrigen { get; set; } = string.Empty;
        public string ModuloAsociado { get; set; } = string.Empty;

        public RunbookEntity() { }

        public RunbookEntity(string nombreArchivo, string rutaOrigen, string moduloAsociado)
        {
            NombreArchivo = nombreArchivo;
            RutaOrigen = rutaOrigen;
            ModuloAsociado = moduloAsociado;
        }
    }
}