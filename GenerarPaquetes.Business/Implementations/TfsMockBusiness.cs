using GenerarPaquetes.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerarPaquetes.Business.Implementations
{
    public class TfsMockBusiness : ITfsMockBusiness
    {
        public bool ExisteBuild(string rutaTfsBase, string numeroBuild)
        {
            string rutaBuild = Path.Combine(rutaTfsBase, numeroBuild);
            return Directory.Exists(rutaBuild);
        }

        public string ObtenerRutaBuild(string rutaTfsBase, string numeroBuild)
        {
            return Path.Combine(rutaTfsBase, numeroBuild);
        }

        public List<string> BuscarCompiladosPorModulo(string rutaBuild, string nombreModulo)
        {
            List<string> resultados = new List<string>();

            if (Directory.Exists(rutaBuild))
            {
                string[] subdirectorios = Directory.GetDirectories(rutaBuild, $"*{nombreModulo}*", SearchOption.AllDirectories);
                resultados.AddRange(subdirectorios);
            }

            return resultados;
        }

        public List<string> BuscarCompilados(string rutaBuild, string nombreModulo)
        {
            throw new NotImplementedException();
        }
    }
}

