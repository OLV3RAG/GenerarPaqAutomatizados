using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerarPaquetes.DAO.Interfaces
{
    internal interface ITfsMockDao
    {
        bool ExisteBuild(string rutaTfsBase, int numeroBuild);
        string ObtenerRutaBuild(string rutaTfsBase, int numeroBuild);
        List<string> BuscarCompilados(string rutaBuild, string nombreModulo);
    }
}
