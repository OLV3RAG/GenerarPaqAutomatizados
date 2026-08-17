using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerarPaquetes.DAO.Interfaces
{
    internal interface ITfsMockDao
    {
        bool ExisteBuild(string rutaTfsBase, int numeroBuild); //Verifica si existe el buildNumber en el TFS
        string ObtenerRutaBuild(string rutaTfsBase, int numeroBuild); //Obtiene la ruta del build donde se encuentran los archivos
        List<string> BuscarCompilados(string rutaBuild, string nombreModulo); //Enlista y busca los compilados existentes.
    }
}
