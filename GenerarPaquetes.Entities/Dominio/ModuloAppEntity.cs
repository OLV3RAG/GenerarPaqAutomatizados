using GenerarPaquetes.Entities.Enums;

namespace GenerarPaquetes.Entities.Domain
{
    /// <summary>
    /// Representa un módulo o aplicación del sistema a empaquetar.
    /// </summary>
    public class ModuloAppEntity
    {
        public string Alias { get; set; } = string.Empty;
        public string NombreReal { get; set; } = string.Empty;
        public string NombreMostrar { get; set; } = string.Empty;
        public TipoModuloEnum Categoria { get; set; }
        public bool RequiereSnapshotServer { get; set; }

        public ModuloAppEntity() { }

        public ModuloAppEntity(string alias, string nombreReal, string nombreMostrar, TipoModuloEnum categoria, bool requiereSnapshot = false)
        {
            Alias = alias;
            NombreReal = nombreReal;
            NombreMostrar = nombreMostrar;
            Categoria = categoria;
            RequiereSnapshotServer = requiereSnapshot;
        }
    }
}