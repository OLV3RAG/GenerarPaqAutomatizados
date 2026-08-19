using System.Collections.Generic;

namespace GenerarPaquetes.Entities
{
    public class PaqueteEntity
    {
        public string BuildNumber { get; set; } = string.Empty;
        public string DestinationPath { get; set; } = string.Empty;
        public List<string> SelectedApps { get; set; } = new List<string>();
    }
}