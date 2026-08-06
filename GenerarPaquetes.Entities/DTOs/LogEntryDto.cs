using System;
using GenerarPaquetes.Entities.Enums;

namespace GenerarPaquetes.Entities.DTOs
{
    public class LogEntryDto
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Mensaje { get; set; } = string.Empty;
        public TipoLogEnum Tipo { get; set; } = TipoLogEnum.Info;

        public LogEntryDto() { }

        public LogEntryDto(string mensaje, TipoLogEnum tipo = TipoLogEnum.Info)
        {
            Timestamp = DateTime.Now;
            Mensaje = mensaje;
            Tipo = tipo;
        }

        public override string ToString()
        {
            string prefijo = Tipo switch
            {
                TipoLogEnum.Warning => "[WARN] ",
                TipoLogEnum.Error => "[ERROR] ",
                TipoLogEnum.Success => "[OK] ",
                _ => ""
            };

            return $"[{Timestamp:HH:mm:ss}] {prefijo}{Mensaje}";
        }
    }
}