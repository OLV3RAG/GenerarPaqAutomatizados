using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerarPaquetes
{
    public partial class Form1 : Form
    {
        // Configuración de rutas locales predeterminadas
        private readonly string pathDocts = @"C:\Users\DAY-V\Desktop\Estructura_Completa_PruebasUAT\GenerarPaqUAT";
        private readonly string sourceTFS = @"C:\Users\DAY-V\Desktop\Estructura_Completa_PruebasUAT\TFS_Mock";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }

        // Botón 1: Template del paquete (Sin acción por el momento)
        private void button1_Click(object sender, EventArgs e)
        {
        }

        // Manejadores de los CheckBoxes (Conservados para evitar desincronización con Form1.Designer.cs)
        private void checkBox1_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox2_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox3_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox4_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox7_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox8_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox9_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox10_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox11_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox12_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox13_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox14_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox15_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox16_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox17_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox18_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox19_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox20_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox21_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox22_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox23_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox24_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox25_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox26_CheckedChanged(object sender, EventArgs e) { }

        // ===============================================================
        // BOTÓN 2: GENERAR EL PAQUETE COMPLETO
        // ===============================================================
        private void button2_Click(object sender, EventArgs e)
        {
            string buildNumber = textBox1.Text.Trim();
            string destinationPath = textBox2.Text.Replace("\"", "").Trim();

            if (string.IsNullOrWhiteSpace(buildNumber) || string.IsNullOrWhiteSpace(destinationPath))
            {
                MessageBox.Show("Ingresa el Build TFS y la Ruta Destino antes de continuar.", "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Recopila los textos/alias de cualquier checkBox que esté marcado
            List<string> selectedApps = new List<string>();
            ObtenerCheckboxesSeleccionados(this, selectedApps);

            if (selectedApps.Count == 0)
            {
                MessageBox.Show("Debes seleccionar al menos una aplicación o módulo para continuar.", "Sin Selección", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            textBox3.Clear();
            button2.Enabled = false;

            try
            {
                Directory.CreateDirectory(destinationPath);
                string rutaOrigen = Path.Combine(pathDocts, "Runbooks");
                string rutaUAT = Path.Combine(pathDocts, "UAT");

                Log("===============================================================");
                Log("CREACIÓN DE PAQUETE UAT - INICIANDO PROCESO");
                Log($"Build TFS: {buildNumber}");
                Log($"Ruta Destino: {destinationPath}");
                Log("===============================================================");

                // 1. COPIA Y PROCESAMIENTO DE ARCHIVOS BASE
                string instructions = "InstruccionesLiberacion.txt";
                string qFile = "Q-MexFile.xlsx";
                string qCopy = "Declaracion de Indisponibilidad de URLs Q3_Q42025 - MEX.xlsx";

                string pathQFile = Path.Combine(pathDocts, qFile);
                string pathQCopy = Path.Combine(destinationPath, qCopy);

                if (File.Exists(pathQFile))
                {
                    File.Copy(pathQFile, pathQCopy, true);
                    Log($"Archivo Q-MexFile copiado exitosamente como '{qCopy}'.");
                }
                else
                {
                    Log($"[WARN] No se encontró el archivo base {qFile} en: {pathDocts}");
                }

                ProcesarInstrucciones(destinationPath, instructions, selectedApps);

                // 2. PROCESAR MÓDULOS Y APLICACIONES SELECCIONADAS
                List<string> tfsAppsToSearch = new List<string>();

                foreach (var rawApp in selectedApps)
                {
                    string realApp = ResolverAlias(rawApp);
                    tfsAppsToSearch.Add(realApp);

                    Log($"Procesando módulo: '{rawApp}' -> Aplicación resolved: '{realApp}'");

                    // Caso especial: Crear estructura de BD
                    if (realApp == "BD" || realApp == "BD_MAS")
                    {
                        Directory.CreateDirectory(Path.Combine(destinationPath, "DB", "Scripts"));
                        Directory.CreateDirectory(Path.Combine(destinationPath, "DB", "StoredProcedures"));
                        Log($"Estructura de Base de Datos ({realApp}) creada en carpeta destino.");
                    }

                    // Copiar Runbooks (.doc)
                    if (Directory.Exists(rutaOrigen))
                    {
                        foreach (var docFile in Directory.GetFiles(rutaOrigen, $"*{realApp}*.doc"))
                        {
                            string fileName = Path.GetFileName(docFile);
                            File.Copy(docFile, Path.Combine(destinationPath, fileName), true);
                            Log($"Runbook copiado: {fileName}");
                        }
                    }

                    // Copiar carpetas UAT asociadas
                    bool encontradoUAT = false;
                    if (Directory.Exists(rutaUAT))
                    {
                        foreach (var dir in Directory.GetDirectories(rutaUAT, $"*{realApp}*"))
                        {
                            encontradoUAT = true;
                            string dirName = Path.GetFileName(dir);
                            CopiarDirectorio(dir, Path.Combine(destinationPath, dirName));
                            Log($"Carpeta UAT copiada: {dirName}");
                        }
                    }

                    if (!encontradoUAT)
                    {
                        Log($"[WARN] No se encontró carpeta UAT que coincida con '{realApp}'");
                    }
                }

                // 3. COPIAR COMPILADOS DESDE TFS
                Log("--------------------------------------------------------------");
                Log($"Buscando compilados en TFS ({Path.Combine(sourceTFS, buildNumber)})...");

                string buildDir = Path.Combine(sourceTFS, buildNumber);
                if (Directory.Exists(buildDir))
                {
                    foreach (var app in tfsAppsToSearch)
                    {
                        foreach (var dir in Directory.GetDirectories(buildDir, $"*{app}*", SearchOption.AllDirectories))
                        {
                            string dirName = Path.GetFileName(dir);
                            CopiarDirectorio(dir, Path.Combine(destinationPath, dirName));
                            Log($"Copiando compilado TFS desde: {dirName}");
                        }
                    }
                }
                else
                {
                    Log($"[WARN] No se encontró la carpeta del Build TFS ({buildDir}). Se omitió la extracción.");
                }

                Log("===============================================================");
                Log("Proceso completado con éxito.");
                Log($"Paquete generado en: {destinationPath}");
                Log("===============================================================");

                MessageBox.Show("Paquete UAT armado con éxito.", "Finalizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log($"[ERROR] {ex.Message}");
                MessageBox.Show($"Error durante la generación: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button2.Enabled = true;
            }
        }


        // Escanea recursivamente el formulario o contenedores buscando CheckBoxes marcados
        private void ObtenerCheckboxesSeleccionados(Control parent, List<string> lista)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is CheckBox cb && cb.Checked)
                {
                    lista.Add(cb.Text.Trim());
                }

                if (c.HasChildren)
                {
                    ObtenerCheckboxesSeleccionados(c, lista);
                }
            }
        }

        // Reemplazo dinámico de etiquetas en InstruccionesLiberacion.txt
        private void ProcesarInstrucciones(string destino, string instructionsFile, List<string> selectedApps)
        {
            string sourceInst = Path.Combine(pathDocts, instructionsFile);
            string destInst = Path.Combine(destino, instructionsFile);

            if (!File.Exists(sourceInst))
            {
                Log($"[WARN] El archivo de instrucciones no existe en: {sourceInst}");
                return;
            }

            var lines = File.ReadAllLines(sourceInst);
            var updatedLines = new List<string>();

            bool isApiOrPdf = selectedApps.Any(a =>
                a.IndexOf("fiscal", StringComparison.OrdinalIgnoreCase) >= 0 ||
                a.IndexOf("apirest", StringComparison.OrdinalIgnoreCase) >= 0 ||
                a.IndexOf("datosfiscales", StringComparison.OrdinalIgnoreCase) >= 0);

            string descStepOneAPI = "Generar SnapShot del servidor 10.110.10.175";
            string descStepTwoAPI = "Plan de reversion, restaurar SnapShot del servidor 10.110.10.175 generado en el paso 1";
            string descSIAP = "Cada Runbook contiene su paso de respaldo";
            string descStepTwoSIAP = "Plan de reversion, restaurar el respaldo generado en el paso 1 de cada runbook";

            foreach (var line in lines)
            {
                string newLine = line;

                if (newLine.IndexOf("step1", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    newLine = isApiOrPdf ? descStepOneAPI : descSIAP;
                }

                if (newLine.IndexOf("destino", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    newLine = newLine.Replace("destino", destino);
                }

                if (newLine.IndexOf("step2", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    newLine = isApiOrPdf ? descStepTwoAPI : descStepTwoSIAP;
                }

                updatedLines.Add(newLine);
            }

            File.WriteAllLines(destInst, updatedLines);
            Log($"Etiquetas actualizadas dentro de {instructionsFile}.");
        }

        // Diccionario para homologar palabras clave y alias a los nombres reales
        private string ResolverAlias(string input)
        {
            string cleanInput = input.ToLower().Trim();

            switch (cleanInput)
            {
                case "servicios_grupo": case "servs_grupo": return "Grupo_Servicio";
                case "portales_grupo": case "ports_grupo": return "Grupo_Portal";
                case "ws_grupo": case "webservices_grupo": return "Grupo_WS";
                case "servmas": case "serviciosmas": return "ServiciosMAS";
                case "emision": case "servemision": return "ServicioEmision";
                case "cfd": case "servcfd": return "ServicioCFD";
                case "docu": case "sdoc": return "ServicioDocumentacion";
                case "permisos": case "perm": case "sintperm": return "ServicioIntegracionPermisos";
                case "sinteg": case "integracion": return "ServicioIntegracion";
                case "siri": return "AutomaticLoadSIRI";
                case "closeserv": case "closeservice": return "CloseService";
                case "dts": return "DTS";
                case "fiscal": case "datosf": return "PortalDatosFiscales";
                case "portcfdi": case "portalcfdi": return "PortalCFDI";
                case "procmov": case "movs": return "ProcesarMovimiento";
                case "apirest": case "rest": return "InstalarPortalAPIRest";
                case "agentes": case "clagentes": return "WS-CLPortalAgentes";
                case "calcprima": case "calculoprima": return "WSCalculoPrima";
                case "cargaqa": case "wscarga": return "WSCargaQA";
                case "catalogsws": case "catalogos": return "CatalogsWS";
                case "mas": case "sistema": return "MAS";
                case "siap": return "SIAP";
                case "bd": return "BD";
                case "bd_mas": return "BD_MAS";
                default: return input;
            }
        }

        // Copia recursiva de archivos y subcarpetas
        private void CopiarDirectorio(string origen, string destino)
        {
            Directory.CreateDirectory(destino);

            foreach (string file in Directory.GetFiles(origen))
            {
                File.Copy(file, Path.Combine(destino, Path.GetFileName(file)), true);
            }

            foreach (string subDir in Directory.GetDirectories(origen))
            {
                CopiarDirectorio(subDir, Path.Combine(destino, Path.GetFileName(subDir)));
            }
        }

        // Imprime en la consola visual (textBox3)
        private void Log(string msg)
        {
            if (textBox3 != null)
            {
                textBox3.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}{Environment.NewLine}");
                textBox3.SelectionStart = textBox3.Text.Length;
                textBox3.ScrollToCaret();
                Application.DoEvents();
            }
        }
    }
}