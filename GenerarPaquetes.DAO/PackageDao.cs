using System.Configuration;
using System.IO;

namespace GenerarPaquetes.DAO
{
    public class PackageDao
    {
        public string ObtenerRutaDocumentos() => ConfigurationManager.AppSettings["RutaDocumentos"] ?? string.Empty;
        public string ObtenerRutaTFS() => ConfigurationManager.AppSettings["RutaTFS"] ?? string.Empty;
        public string ObtenerRutaRunbooks() => ConfigurationManager.AppSettings["RutaRunbooks"] ?? string.Empty;
        public bool ExisteDirectorio(string ruta) => Directory.Exists(ruta);

        public void CrearDirectorio(string ruta)
        {
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
        }

        public bool ExisteArchivo(string ruta) => File.Exists(ruta);

        public void CopiarArchivo(string origen, string destino, bool sobrescribir = true)
        {
            File.Copy(origen, destino, sobrescribir);
        }

        public string LeerTexto(string ruta)
        {
            return File.Exists(ruta) ? File.ReadAllText(ruta) : string.Empty;
        }

        public void EscribirTexto(string ruta, string contenido)
        {
            File.WriteAllText(ruta, contenido);
        }

        public void CopiarDirectorioRecursivo(string origen, string destino)
        {
            CrearDirectorio(destino);

            foreach (string archivo in Directory.GetFiles(origen))
            {
                string nombre = Path.GetFileName(archivo);
                File.Copy(archivo, Path.Combine(destino, nombre), true);
            }

            foreach (string subDirectorio in Directory.GetDirectories(origen))
            {
                string nombreSub = Path.GetFileName(subDirectorio);
                CopiarDirectorioRecursivo(subDirectorio, Path.Combine(destino, nombreSub));
            }
        }
    }
}