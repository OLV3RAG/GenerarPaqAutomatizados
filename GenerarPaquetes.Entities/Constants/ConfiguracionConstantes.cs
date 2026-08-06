namespace GenerarPaquetes.Entities.Constants
{
    public static class ConfiguracionConstantes
    {
        // Rutas locales de origen predeterminadas
        public const string PathDoctsDefault = @"C:\Users\DAY-V\Desktop\Estructura_Completa_PruebasUAT\GenerarPaqUAT";
        public const string SourceTFSDefault = @"C:\Users\DAY-V\Desktop\Estructura_Completa_PruebasUAT\TFS_Mock";

        // Archivos base clave
        public const string ArchivoInstrucciones = "InstruccionesLiberacion.txt";
        public const string ArchivoQMexOriginal = "Q-MexFile.xlsx";
        public const string ArchivoQMexDestino = "Declaracion de Indisponibilidad de URLs Q3_Q42025 - MEX.xlsx";

        // Estructura de carpetas internas
        public const string CarpetaRunbooks = "Runbooks";
        public const string CarpetaUAT = "UAT";
        public const string CarpetaDB = "DB";
        public const string CarpetaDBScripts = @"DB\Scripts";
        public const string CarpetaDBProcedures = @"DB\StoredProcedures";
    }
}