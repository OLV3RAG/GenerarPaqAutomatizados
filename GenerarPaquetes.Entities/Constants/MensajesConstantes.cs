namespace GenerarPaquetes.Entities.Constants
{
    public static class MensajesConstantes
    {
        public const string ErrBuildRequerido = "El número de Build TFS es obligatorio.";
        public const string ErrRutaRequerida = "La ruta de destino es obligatoria.";
        public const string ErrModulosRequeridos = "Debe seleccionar al menos una aplicación o módulo.";

        public const string TituloExito = "Éxito";
        public const string TituloAdvertencia = "Atención";
        public const string TituloError = "Error en el Proceso";

        public const string MsgExitoGeneral = "Paquete UAT generado exitosamente.";
    }
}