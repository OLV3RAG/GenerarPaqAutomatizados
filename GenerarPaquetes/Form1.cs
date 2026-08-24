using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GenerarPaquetes.Business;
using GenerarPaquetes.Entities.DTOs;

namespace GenerarPaquetes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_CrearPaquete_Click(object sender, EventArgs e)
        {
            List<string> modulosSeleccionados = new List<string>();

            if (check_BatchLauncher.Checked)
            {
                modulosSeleccionados.Add("BatchLauncher");
            }
            if (check_BServiceSIAP.Checked)
            {
                modulosSeleccionados.Add("BusinessServiceSIAP");
            }
            if (check_CatalogsWS.Checked)
            {
                modulosSeleccionados.Add("CatalogsWS");
            }
            if (check_CloseService.Checked)
            {
                modulosSeleccionados.Add("CloseService");
            }
            if (check_PortalAPIRest.Checked)
            {
                modulosSeleccionados.Add("PortalAPIRest");
            }
            if (check_IDCMAS.Checked)
            {
                modulosSeleccionados.Add("IDCMAS");
            }
            if (check_IDCSIAPApi.Checked)
            {
                modulosSeleccionados.Add("IDCSIAPApi");
            }
            if (check_IDCSIAP.Checked)
            {
                modulosSeleccionados.Add("IDCSIAP");
            }
            if (check_QuotationWeb.Checked)
            {
                modulosSeleccionados.Add("QuotationWeb");
            }
            if (check_ProcesarMov.Checked)
            {
                modulosSeleccionados.Add("ProcesarMov");
            }
            if (check_PDFiscales.Checked)
            {
                modulosSeleccionados.Add("PDFiscales");
            }
            if (check_CFDI.Checked)
            {
                modulosSeleccionados.Add("CFDI");
            }
            if (check_MASWeb.Checked)
            {
                modulosSeleccionados.Add("MASWeb");
            }
            if (check_ServIntegracion.Checked)
            {
                modulosSeleccionados.Add("ServicioIntegracion");
            }
            if (check_ServicioEmision.Checked)
            {
                modulosSeleccionados.Add("ServicioEmision");
            }
            if (check_ServDocu.Checked)
            {
                modulosSeleccionados.Add("ServicioDocumentacion");
            }
            if (check_SIRI.Checked)
            {
                modulosSeleccionados.Add("SIRI");
            }
            if (check_RobotCFD.Checked)
            {
                modulosSeleccionados.Add("RobotCFD");
            }
            if (check_ServIntPermisos.Checked)
            {
                modulosSeleccionados.Add("ServicioIntegracionPermisos");
            }
            if (check_ServMAS.Checked)
            {
                modulosSeleccionados.Add("ServicioMAS");
            }
            if (check_WSCLPortAgt.Checked)
            {
                modulosSeleccionados.Add("WSCLPortAgt");
            }
            if (check_DTS.Checked)
            {
                modulosSeleccionados.Add("DTS");
            }
            if (check_IAdi.Checked)
            {
                modulosSeleccionados.Add("IAdi");
            }
            if (check_BD_SIAP.Checked)
            {
                modulosSeleccionados.Add("BD");
            }
            if (check_BD_MAS.Checked)
            {
                modulosSeleccionados.Add("BD_MAS");
            }


            PaqueteUatRequestDto datosPaquete = new PaqueteUatRequestDto();
            datosPaquete.BuildNumber = txt_BuildTFS.Text.Trim();
            datosPaquete.DestinationPath = txt_pathDestino.Text;
            datosPaquete.SelectedApps = modulosSeleccionados;

            txt_TerminalOutput.Clear();
            btn_CrearPaquete.Enabled = false;

            try
            {
                PackageBusiness negocio = new PackageBusiness();
                negocio.EnviarLog = MostrarLog;
                negocio.ProcesarPaquete(datosPaquete);

                MessageBox.Show("Paquete UAT armado con éxito.", "Finalizado", MessageBoxButtons.OK, MessageBoxIcon.Information);


                ReiniciarForm(this);
                txt_BuildTFS.Focus();
            }
            catch (Exception ex)
            {
                MostrarLog("[ERROR] " + ex.Message);
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btn_CrearPaquete.Enabled = true;
            }
        }   

        private void ReiniciarForm(Control contenedor)
        {
            foreach (Control ctrl in contenedor.Controls)
            {
                if(ctrl is CheckBox chk)
                {
                    chk.Checked = false;
                }
                else if (ctrl is TextBox txt && txt != txt_TerminalOutput)
                {
                    txt.Clear();
                }
                if(ctrl.HasChildren)
                {
                    ReiniciarForm(ctrl);
                }
            }
        }

        private void MostrarLog(string mensaje)
        {
            if (txt_TerminalOutput != null)
            {
                string hora = DateTime.Now.ToString("HH:mm:ss");
                txt_TerminalOutput.AppendText("[" + hora + "] " + mensaje + Environment.NewLine);
                txt_TerminalOutput.SelectionStart = txt_TerminalOutput.Text.Length;
                txt_TerminalOutput.ScrollToCaret();
                Application.DoEvents();
            }
        }

        private void btn_SeleccionarDestino_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txt_pathDestino.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }
        private void ObtenerCheckboxesSeleccionados(Control contenedor, List<string> lista)
        {
            foreach (Control control in contenedor.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox chk = (CheckBox)control;
                    if (chk.Checked)
                    {
                        lista.Add(chk.Text.Trim());
                    }
                }

                if (control.HasChildren)
                {
                    ObtenerCheckboxesSeleccionados(control, lista);
                }
            }
        }
        private void LimpiarCheckboxes(Control contenedor)
        {
            foreach (Control ctrl in contenedor.Controls)
            {
                if (ctrl is CheckBox chk)
                {
                    chk.Checked = false;
                }

                // Si el control contiene otros controles (como GroupBox, Panel, TabControl), busca dentro
                if (ctrl.HasChildren)
                {
                    LimpiarCheckboxes(ctrl);
                }
            }
        }

        private void btn_TemplatePaquete_Click(object sender, EventArgs e)
        {
            string buildNumber = txt_BuildTFS.Text.Trim();

            if (string.IsNullOrWhiteSpace(buildNumber))
            {
                MessageBox.Show("Ingresa el número de Build de TFS para escanear los aplicativos.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_BuildTFS.Focus();
                return;
            }

            txt_TerminalOutput.Clear();
            MostrarLog($"[INFO] Escaneando compilados en TFS para el Build: {buildNumber}...");

            try
            {
                PackageBusiness negocio = new PackageBusiness();
                negocio.EnviarLog = MostrarLog;

                List<string> appsDetectadas = negocio.DetectarAppsEnTFS(buildNumber);

                if (appsDetectadas.Count == 0)
                {
                    MessageBox.Show("No se encontraron carpetas de aplicativos en la ruta de la Build indicada.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 1. Limpiar todos los CheckBoxes antes de marcar los nuevos
                LimpiarCheckboxes(this);

                // 2. Diccionario que relaciona la clave detectada con el control CheckBox de tu Form
                var mapaControles = new Dictionary<string, CheckBox>(StringComparer.OrdinalIgnoreCase)
        {
            { "IDCMAS", check_IDCMAS },
            { "MAS", check_IDCMAS },
            { "IDCSIAP", check_IDCSIAP },
            { "SIAP", check_IDCSIAP },
            { "CFDI", check_CFDI },
            { "PortalCFDI", check_CFDI },
            { "RobotCFD", check_RobotCFD },
            { "IDCSIAPApi", check_IDCSIAPApi },
            { "BatchLauncher", check_BatchLauncher },
            { "BusinessServiceSIAP", check_BServiceSIAP },
            { "CatalogsWS", check_CatalogsWS },
            { "CloseService", check_CloseService },
            { "PortalAPIRest", check_PortalAPIRest },
            { "QuotationWeb", check_QuotationWeb },
            { "ProcesarMov", check_ProcesarMov },
            { "PDFiscales", check_PDFiscales },
            { "MASWeb", check_MASWeb },
            { "ServicioIntegracion", check_ServIntegracion },
            { "ServicioEmision", check_ServicioEmision },
            { "ServicioDocumentacion", check_ServDocu },
            { "SIRI", check_SIRI },
            { "ServicioIntegracionPermisos", check_ServIntPermisos },
            { "ServicioMAS", check_ServMAS },
            { "WSCLPortAgt", check_WSCLPortAgt },
            { "DTS", check_DTS },
            { "IAdi", check_IAdi },
          
        };

                // 3. Marcar las casillas encontradas
                int contadorMarcados = 0;
                foreach (string app in appsDetectadas)
                {
                    if (mapaControles.TryGetValue(app, out CheckBox chk) && chk != null)
                    {
                        chk.Checked = true;
                        contadorMarcados++;
                    }
                }

                MostrarLog($"[OK] Detección finalizada. Se seleccionaron {contadorMarcados} aplicativos automáticamente.");
            }
            catch (Exception ex)
            {
                MostrarLog($"[ERROR] Error al consultar TFS: {ex.Message}");
                MessageBox.Show("Error durante la detección: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}