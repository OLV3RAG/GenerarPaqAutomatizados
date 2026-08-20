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
        


        // ObtenerCheckboxesSeleccionados(this, modulosSeleccionados);

        PaqueteUatRequestDto datosPaquete = new PaqueteUatRequestDto();
            datosPaquete.BuildNumber = txt_BuildTFS.Text.Trim();
            datosPaquete.DestinationPath = txt_pathDestino.Text.Replace("\"", "").Trim();
            datosPaquete.SelectedApps = modulosSeleccionados;

            txt_TerminalOutput.Clear();
            btn_CrearPaquete.Enabled = false;

            try
            {
                PackageBusiness negocio = new PackageBusiness();
                negocio.EnviarLog = MostrarLog;
                negocio.ProcesarPaquete(datosPaquete);

                MessageBox.Show("Paquete UAT armado con éxito.", "Finalizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        //private void checkboxesList_SelectedIndexChanged(object sender, EventArgs e )
        //{
        //    if (check_BatchLauncher.Checked)
        //    {
        //        modulosSeleccionados.Add("BatchLauncher");
        //    }
        //    if (check_BServiceSIAP.Checked)
        //    {
        //        modulosSeleccionados.Add("BusinessServiceSIAP");
        //    }
        //    if (check_CatalogsWS.Checked)
        //    {
        //        modulosSeleccionados.Add("CatalogsWS");
        //    }
        //    if (check_CloseService.Checked)
        //    {
        //        modulosSeleccionados.Add("CloseService");
        //    }
        //    if (check_PortalAPIRest.Checked)
        //    {
        //        modulosSeleccionados.Add("PortalAPIRest");
        //    }
        //    if (check_IDCMAS.Checked)
        //    {
        //        modulosSeleccionados.Add("IDCMAS");
        //    }
        //    if (check_IDCSIAPApi.Checked)
        //    {
        //        modulosSeleccionados.Add("IDCSIAPApi");
        //    }
        //    if (check_IDCSIAP.Checked)
        //    {
        //        modulosSeleccionados.Add("IDCSIAP");
        //    }
        //    if (check_QuotationWeb.Checked)
        //    {
        //        modulosSeleccionados.Add("QuotationWeb");
        //    }
        //    if (check_ProcesarMov.Checked)
        //    {
        //        modulosSeleccionados.Add("ProcesarMov");
        //    }
        //    if (check_PDFiscales.Checked)
        //    {
        //        modulosSeleccionados.Add("PDFiscales");
        //    }
        //    if (check_CFDI.Checked)
        //    {
        //        modulosSeleccionados.Add("CFDI");
        //    }
        //    if (check_MASWeb.Checked)
        //    {
        //        modulosSeleccionados.Add("MASWeb");
        //    }
        //    if (check_ServIntegracion.Checked)
        //    {
        //        modulosSeleccionados.Add("ServicioIntegracion");
        //    }
        //    if (check_ServicioEmision.Checked)
        //    {
        //        modulosSeleccionados.Add("ServicioEmision");
        //    }
        //    if (check_ServDocu.Checked)
        //    {
        //        modulosSeleccionados.Add("ServicioDocumentacion");
        //    }
        //    if (check_SIRI.Checked)
        //    {
        //        modulosSeleccionados.Add("SIRI");
        //    }
        //    if (check_RobotCFD.Checked)
        //    {
        //        modulosSeleccionados.Add("RobotCFD");
        //    }
        //    if (check_ServIntPermisos.Checked)
        //    {
        //        modulosSeleccionados.Add("ServicioIntegracionPermisos");
        //    }
        //    if (check_ServMAS.Checked)
        //    {
        //        modulosSeleccionados.Add("ServicioMAS");
        //    }
        //    if (check_WSCLPortAgt.Checked)
        //    {
        //        modulosSeleccionados.Add("WSCLPortAgt");
        //    }
        //    if (check_DTS.Checked)
        //    {
        //        modulosSeleccionados.Add("DTS");
        //    }
        //    if (check_IAdi.Checked)
        //    {
        //        modulosSeleccionados.Add("IAdi");
        //    }
        //}
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


       

        private void Form1_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void btn_TemplatePaquete_Click(object sender, EventArgs e) { }

        private void check_BatchLauncher_CheckedChanged(object sender, EventArgs e) { }
        private void check_BServiceSIAP_CheckedChanged(object sender, EventArgs e) { }
        private void check_CatalogsWS_CheckedChanged(object sender, EventArgs e) { }
        private void check_CloseService_CheckedChanged(object sender, EventArgs e) { }
        private void check_PortalAPIRest_CheckedChanged(object sender, EventArgs e) { }
        private void check_IDCMAS_CheckedChanged(object sender, EventArgs e) { }
        private void check_IDCSIAPApi_CheckedChanged(object sender, EventArgs e) { }
        private void check_IDCSIAP_CheckedChanged(object sender, EventArgs e) { }
        private void check_QuotationWeb_CheckedChanged(object sender, EventArgs e) { }
        private void check_ProcesarMov_CheckedChanged(object sender, EventArgs e) { }
        private void check_PDFiscales_CheckedChanged(object sender, EventArgs e) { }
        private void check_CFDI_CheckedChanged(object sender, EventArgs e) { }
        private void check_MASWeb_CheckedChanged(object sender, EventArgs e) { }
        private void check_ServIntegracion_CheckedChanged(object sender, EventArgs e) { }
        private void check_ServicioEmision_CheckedChanged(object sender, EventArgs e) { }
        private void check_ServDocu_CheckedChanged(object sender, EventArgs e) { }
        private void check_SIRI_CheckedChanged(object sender, EventArgs e) { }
        private void check_RobotCFD_CheckedChanged(object sender, EventArgs e) { }
        private void check_ServIntPermisos_CheckedChanged(object sender, EventArgs e) { }
        private void check_ServMAS_CheckedChanged(object sender, EventArgs e) { }
        private void check_WSCLPortAgt_CheckedChanged(object sender, EventArgs e) { }
        private void check_DTS_CheckedChanged(object sender, EventArgs e) { }
        private void check_IAdi_CheckedChanged(object sender, EventArgs e) {}
    }
}