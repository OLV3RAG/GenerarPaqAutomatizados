using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerarPaquetes.DAO.Helpers
{
    public static class FileOperationHelper
    {
        public static void CopiarEstructuraDelDirectorio(string origen, string destino) //Metodo que copia toda la estructura que tiene el directorio que se necesita
        {
            Directory.CreateDirectory(destino);

            foreach (string archivo in Directory.GetFiles(origen))
            {
                string archivoObjetivo = Path.Combine(destino, Path.GetFileName(archivo));
                File.Copy(archivo, archivoObjetivo, true);
            }
            
            foreach(string subdirectorio in Directory.GetDirectories(origen))
            {
                string subdireObjetivo = Path.Combine(destino, Path.GetFileName(subdirectorio));
                CopiarEstructuraDelDirectorio(subdirectorio, subdireObjetivo);
            }    
        }
    }
}
