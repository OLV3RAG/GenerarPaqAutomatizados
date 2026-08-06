namespace GenerarPaquetes.Entities.Constants
{
    public static class EtiquetasConstantes
    {
        // Palabras clave dentro de InstruccionesLiberacion.txt
        public const string StepOneToken = "step1";
        public const string StepTwoToken = "step2";
        public const string DestinoToken = "destino";

        // Direcciones IP y descripciones operativas
        public const string IpServidorApi = "10.110.10.175";
        public const string DescStepOneApi = "Generar SnapShot del servidor 10.110.10.175";
        public const string DescStepTwoApi = "Plan de reversion, restaurar SnapShot del servidor 10.110.10.175 generado en el paso 1";
        public const string DescStepOneSiap = "Cada Runbook contiene su paso de respaldo";
        public const string DescStepTwoSiap = "Plan de reversion, restaurar el respaldo generado en el paso 1 de cada runbook";
    }
}