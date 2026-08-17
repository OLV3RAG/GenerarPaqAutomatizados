using System;
using System.Collections.Generic;
using System.IO;
using GenerarPaquetes.Business.Interfaces;
using GenerarPaquetes.Business.Mappers;
using GenerarPaquetes.Entities;

namespace GenerarPaquetes.Business
{
    public class PackageBusiness
    {
        private readonly IFileSystemBusiness _fileSystem;
        private readonly ITfsMockBusiness _tfsMock;
        private string rutaDocs = "C:\\Users\\DAY-V\\Desktop\\Estructura_Completa_PruebasUAT\\GenerarPaqUAT";
        private string rutaTfs = "C:\\Users\\DAY-V\\Desktop\\Estructura_Completa_PruebasUAT\\TFS_Mock";
        private Action<string> Enviarlog;

        public PackageBusiness()
        {
            _fileSystem new FileSystemBusiness();
            _tfsMock new TfsMockBusiness();
        }
    }
}
