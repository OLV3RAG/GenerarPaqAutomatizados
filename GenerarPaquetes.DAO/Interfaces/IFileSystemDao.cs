using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerarPaquetes.DAO.Interfaces
{
    public interface IFileSystemDao
    {
        void CrearDirectorio(string ruta); //Crea los directorios existentes
        bool ExisteArchivo(string ruta); //Verficia si los archivos existen
        bool ExisteDirectorio(string ruta); //Verifica si el directorio existe
        void CopiarArchivo(string origen, string destino, bool sobreescribir = true); //Copia los archivos correspondientes
        void CopiarDirectorioRecursivo(string origen, string destino); //Copiar el directorio que se necesita
        string[] LeerLineasDeArchivo(string ruta); //Leer el contenido de cada archivo
        void EscribirLineasDeArchivo(string ruta, string[] lineas); //Escribe las lineas del archivo
        string[] ObtenerArchivosDelDirectorio(string ruta, string patronBusqueda); //Obtiene los archivos necesarios del directorio
        string[] ObtenerLosSubdirectorios(string ruta, string patronBusqueda, SearchOption op = SearchOption.TopDirectoryOnly); //Obtiene los subdirectorios necesarios
    }
}
