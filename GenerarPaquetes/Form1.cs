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

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> modulosSeleccionados = new List<string>();
            ObtenerCheckboxesSeleccionados(this, modulosSeleccionados);

            PaqueteUatRequestDto datosPaquete = new PaqueteUatRequestDto();
            datosPaquete.BuildNumber = textBox1.Text.Trim();
            datosPaquete.DestinationPath = textBox2.Text.Replace("\"", "").Trim();
            datosPaquete.SelectedApps = modulosSeleccionados;

            textBox3.Clear();
            button2.Enabled = false;

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
                button2.Enabled = true;
            }
        }

        private void MostrarLog(string mensaje)
        {
            if (textBox3 != null)
            {
                string hora = DateTime.Now.ToString("HH:mm:ss");
                textBox3.AppendText("[" + hora + "] " + mensaje + Environment.NewLine);
                textBox3.SelectionStart = textBox3.Text.Length;
                textBox3.ScrollToCaret();
                Application.DoEvents();
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

        private void Form1_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }

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
    }
}