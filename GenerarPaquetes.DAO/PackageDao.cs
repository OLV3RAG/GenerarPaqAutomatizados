using NLog.Internal;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace GenerarPaquetes.DAO
{
    public class PackageDao
    {
        // Las rutas se configuran desde el Appconfig del proyecto GenerarPaquetesPage
        public string ObtenerRutaDocumentos()
        {
            return ConfigurationManager.AppSettings["RutaDocumentos"] ?? string.Empty;
        }

        public string ObtenerRutaTFS()
        {
            return ConfigurationManager.AppSettings["RutaTFS"] ?? string.Empty;
        }

        // Verificación y gestión de carpetas
        public bool ExisteDirectorio(string ruta) => Directory.Exists(ruta);

        public void CrearDirectorio(string ruta)
        {
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
        }

        public string[] ObtenerDirectorios(string ruta, string patron = "*", SearchOption opcion = SearchOption.TopDirectoryOnly)
        {
            return Directory.Exists(ruta)
                ? Directory.GetDirectories(ruta, patron, opcion)
                : new string[0];
        }

        // Verificación y gestión de archivos
        public bool ExisteArchivo(string ruta) => File.Exists(ruta);

        public void CopiarArchivo(string origen, string destino, bool sobrescribir = true)
        {
            File.Copy(origen, destino, sobrescribir);
        }

        public string[] ObtenerArchivos(string ruta, string patron = "*.*")
        {
            return Directory.Exists(ruta)
                ? Directory.GetFiles(ruta, patron)
                : new string[0];
        }

        public string[] LeerLineasArchivo(string ruta)
        {
            return File.Exists(ruta)
                ? File.ReadAllLines(ruta)
                : new string[0];
        }

        public void EscribirLineasArchivo(string ruta, IEnumerable<string> lineas)
        {
            File.WriteAllLines(ruta, lineas);
        }
    }
}