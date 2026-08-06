using GenerarPaquetes.Entities; // Importa directamente PaqueteEntity

// Creación de la entidad principal
PaqueteEntity paquete = new PaqueteEntity
{
    BuildNumber = textBox1.Text.Trim(),
    DestinationPath = textBox2.Text.Trim(),
    SelectedApps = ObtenerCheckboxesSeleccionados(this)
};
namespace GenerarPaquetes.Entities
{
    /// <summary>
    /// Entidad Principal del Dominio que representa el Paquete UAT a construir.
    /// </summary>
    public class PaqueteEntity
    {
        public string BuildNumber { get; set; } = string.Empty;
        public string DestinationPath { get; set; } = string.Empty;
        public List<string> SelectedApps { get; set; } = new List<string>();

        public PaqueteEntity() { }

        public PaqueteEntity(string buildNumber, string destinationPath, List<string> selectedApps)
        {
            BuildNumber = buildNumber;
            DestinationPath = destinationPath;
            SelectedApps = selectedApps ?? new List<string>();
        }

        /// <summary>
        /// Valida las reglas básicas de la entidad antes de enviarla a la capa de negocio.
        /// </summary>
        public bool EsValido(out string mensajeError)
        {
            if (string.IsNullOrWhiteSpace(BuildNumber))
            {
                mensajeError = "El número de Build TFS es obligatorio.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(DestinationPath))
            {
                mensajeError = "La ruta de destino es obligatoria.";
                return false;
            }

            if (SelectedApps == null || SelectedApps.Count == 0)
            {
                mensajeError = "Debe seleccionar al menos una aplicación o módulo.";
                return false;
            }

            mensajeError = string.Empty;
            return true;
        }
    }
}