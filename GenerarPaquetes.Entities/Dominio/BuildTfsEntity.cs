namespace GenerarPaquetes.Entities.Domain
{
    /// <summary>
    /// Representa la información del Build extraído de TFS.
    /// </summary>
    public class BuildTfsEntity
    {
        public string NumeroBuild { get; set; } = string.Empty;
        public string RutaBuildTFS { get; set; } = string.Empty;
        public bool ExisteEnTFS { get; set; }

        public BuildTfsEntity() { }

        public BuildTfsEntity(string numeroBuild, string rutaBuildTFS, bool existeEnTFS)
        {
            NumeroBuild = numeroBuild;
            RutaBuildTFS = rutaBuildTFS;
            ExisteEnTFS = existeEnTFS;
        }
    }
}