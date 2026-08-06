using System.Collections.Generic;
using GenerarPaquetes.Entities.Enums;

namespace GenerarPaquetes.Entities.DTOs
{
    public class ResultadoProcesoDto
    {
        public EstadoProcesoEnum Estado { get; set; } = EstadoProcesoEnum.NoIniciado;
        public string RutaDestino { get; set; } = string.Empty;
        public int ModulosProcesados { get; set; }
        public int RunbooksCopied { get; set; }
        public int CompiladosCopiados { get; set; }
        public List<LogEntryDto> Logs { get; set; } = new List<LogEntryDto>();

        public bool EsExitoso => Estado == EstadoProcesoEnum.Completado || Estado == EstadoProcesoEnum.CompletadoConAdvertencias;
    }
}