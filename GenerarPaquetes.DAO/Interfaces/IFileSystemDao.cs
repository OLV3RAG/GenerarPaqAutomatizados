using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerarPaquetes.DAO.Interfaces
{
    internal interface IFileSystemDao
    {
        void CrearDirectorio(string ruta);
        bool ExisteArchivo(string ruta);
        bool ExisteDirectorio(string ruta);
        void CopiarArchivo(string origen, string destino, bool sobreescribir = true);
        void CopiarDirectorioRecursivo(string origen, string destino);
        string[] LeerLineasDeArchivo(string ruta);
        void EscribirLineasDeArchivo(string ruta, string[] lineas);
        string[] ObtenerArchivosDelDirectorio(string ruta, string patronBusqueda);
        string[] ObtenerLosSubdirectorios(string ruta, string patronBusqueda, SearchOption op = SearchOption.TopDirectoryOnly);
    }
}
