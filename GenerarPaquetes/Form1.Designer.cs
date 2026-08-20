namespace GenerarPaquetes
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_BuildTFS = new System.Windows.Forms.TextBox();
            this.txt_pathDestino = new System.Windows.Forms.TextBox();
            this.btn_TemplatePaquete = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.check_BatchLauncher = new System.Windows.Forms.CheckBox();
            this.check_BServiceSIAP = new System.Windows.Forms.CheckBox();
            this.check_CatalogsWS = new System.Windows.Forms.CheckBox();
            this.check_CloseService = new System.Windows.Forms.CheckBox();
            this.check_DTS = new System.Windows.Forms.CheckBox();
            this.check_IDCSIAP = new System.Windows.Forms.CheckBox();
            this.check_IDCSIAPApi = new System.Windows.Forms.CheckBox();
            this.check_IDCMAS = new System.Windows.Forms.CheckBox();
            this.check_PortalAPIRest = new System.Windows.Forms.CheckBox();
            this.check_IAdi = new System.Windows.Forms.CheckBox();
            this.check_MASWeb = new System.Windows.Forms.CheckBox();
            this.check_CFDI = new System.Windows.Forms.CheckBox();
            this.check_PDFiscales = new System.Windows.Forms.CheckBox();
            this.check_ProcesarMov = new System.Windows.Forms.CheckBox();
            this.check_QuotationWeb = new System.Windows.Forms.CheckBox();
            this.check_ServicioEmision = new System.Windows.Forms.CheckBox();
            this.check_ServIntegracion = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.check_ServIntPermisos = new System.Windows.Forms.CheckBox();
            this.check_ServMAS = new System.Windows.Forms.CheckBox();
            this.check_WSCLPortAgt = new System.Windows.Forms.CheckBox();
            this.check_ServDocu = new System.Windows.Forms.CheckBox();
            this.check_SIRI = new System.Windows.Forms.CheckBox();
            this.check_RobotCFD = new System.Windows.Forms.CheckBox();
            this.btn_CrearPaquete = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_TerminalOutput = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(621, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Generador de Paquetes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(122, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Build TFS:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1030, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Ruta Destino:";
            // 
            // txt_BuildTFS
            // 
            this.txt_BuildTFS.Location = new System.Drawing.Point(122, 96);
            this.txt_BuildTFS.Name = "txt_BuildTFS";
            this.txt_BuildTFS.Size = new System.Drawing.Size(241, 22);
            this.txt_BuildTFS.TabIndex = 3;
            // 
            // txt_pathDestino
            // 
            this.txt_pathDestino.Location = new System.Drawing.Point(1033, 96);
            this.txt_pathDestino.Name = "txt_pathDestino";
            this.txt_pathDestino.Size = new System.Drawing.Size(249, 22);
            this.txt_pathDestino.TabIndex = 4;
            // 
            // btn_TemplatePaquete
            // 
            this.btn_TemplatePaquete.Location = new System.Drawing.Point(1187, 144);
            this.btn_TemplatePaquete.Name = "btn_TemplatePaquete";
            this.btn_TemplatePaquete.Size = new System.Drawing.Size(145, 41);
            this.btn_TemplatePaquete.TabIndex = 5;
            this.btn_TemplatePaquete.Text = "Paquete Template";
            this.btn_TemplatePaquete.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 168);
            this.label4.MaximumSize = new System.Drawing.Size(100, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 32);
            this.label4.TabIndex = 6;
            this.label4.Text = "Componentes / Aplicaciones";
            // 
            // check_BatchLauncher
            // 
            this.check_BatchLauncher.AutoSize = true;
            this.check_BatchLauncher.Location = new System.Drawing.Point(53, 31);
            this.check_BatchLauncher.Name = "check_BatchLauncher";
            this.check_BatchLauncher.Size = new System.Drawing.Size(118, 20);
            this.check_BatchLauncher.TabIndex = 0;
            this.check_BatchLauncher.Text = "BatchLauncher";
            this.check_BatchLauncher.UseVisualStyleBackColor = true;
            // 
            // check_BServiceSIAP
            // 
            this.check_BServiceSIAP.AutoSize = true;
            this.check_BServiceSIAP.Location = new System.Drawing.Point(53, 94);
            this.check_BServiceSIAP.Name = "check_BServiceSIAP";
            this.check_BServiceSIAP.Size = new System.Drawing.Size(160, 20);
            this.check_BServiceSIAP.TabIndex = 1;
            this.check_BServiceSIAP.Text = "BusinessServiceSIAP";
            this.check_BServiceSIAP.UseVisualStyleBackColor = true;
            // 
            // check_CatalogsWS
            // 
            this.check_CatalogsWS.AutoSize = true;
            this.check_CatalogsWS.Location = new System.Drawing.Point(53, 159);
            this.check_CatalogsWS.Name = "check_CatalogsWS";
            this.check_CatalogsWS.Size = new System.Drawing.Size(105, 20);
            this.check_CatalogsWS.TabIndex = 2;
            this.check_CatalogsWS.Text = "CatalogsWS";
            this.check_CatalogsWS.UseVisualStyleBackColor = true;
            // 
            // check_CloseService
            // 
            this.check_CloseService.AutoSize = true;
            this.check_CloseService.Location = new System.Drawing.Point(53, 233);
            this.check_CloseService.Name = "check_CloseService";
            this.check_CloseService.Size = new System.Drawing.Size(110, 20);
            this.check_CloseService.TabIndex = 3;
            this.check_CloseService.Text = "CloseService";
            this.check_CloseService.UseVisualStyleBackColor = true;
            // 
            // check_DTS
            // 
            this.check_DTS.AutoSize = true;
            this.check_DTS.Location = new System.Drawing.Point(53, 309);
            this.check_DTS.Name = "check_DTS";
            this.check_DTS.Size = new System.Drawing.Size(57, 20);
            this.check_DTS.TabIndex = 4;
            this.check_DTS.Text = "DTS";
            this.check_DTS.UseVisualStyleBackColor = true;
            // 
            // check_IDCSIAP
            // 
            this.check_IDCSIAP.AutoSize = true;
            this.check_IDCSIAP.Location = new System.Drawing.Point(338, 31);
            this.check_IDCSIAP.Name = "check_IDCSIAP";
            this.check_IDCSIAP.Size = new System.Drawing.Size(88, 20);
            this.check_IDCSIAP.TabIndex = 5;
            this.check_IDCSIAP.Text = "IDC_SIAP";
            this.check_IDCSIAP.UseVisualStyleBackColor = true;
            // 
            // check_IDCSIAPApi
            // 
            this.check_IDCSIAPApi.AutoSize = true;
            this.check_IDCSIAPApi.Location = new System.Drawing.Point(338, 94);
            this.check_IDCSIAPApi.Name = "check_IDCSIAPApi";
            this.check_IDCSIAPApi.Size = new System.Drawing.Size(108, 20);
            this.check_IDCSIAPApi.TabIndex = 6;
            this.check_IDCSIAPApi.Text = "IDC_SIAPApi";
            this.check_IDCSIAPApi.UseVisualStyleBackColor = true;
            // 
            // check_IDCMAS
            // 
            this.check_IDCMAS.AutoSize = true;
            this.check_IDCMAS.Location = new System.Drawing.Point(338, 159);
            this.check_IDCMAS.Name = "check_IDCMAS";
            this.check_IDCMAS.Size = new System.Drawing.Size(80, 20);
            this.check_IDCMAS.TabIndex = 7;
            this.check_IDCMAS.Text = "IDCMAS";
            this.check_IDCMAS.UseVisualStyleBackColor = true;
            // 
            // check_PortalAPIRest
            // 
            this.check_PortalAPIRest.AutoSize = true;
            this.check_PortalAPIRest.Location = new System.Drawing.Point(338, 233);
            this.check_PortalAPIRest.Name = "check_PortalAPIRest";
            this.check_PortalAPIRest.Size = new System.Drawing.Size(156, 20);
            this.check_PortalAPIRest.TabIndex = 8;
            this.check_PortalAPIRest.Text = "InstalarPortalAPIRest";
            this.check_PortalAPIRest.UseVisualStyleBackColor = true;
            // 
            // check_IAdi
            // 
            this.check_IAdi.AutoSize = true;
            this.check_IAdi.Location = new System.Drawing.Point(338, 309);
            this.check_IAdi.Name = "check_IAdi";
            this.check_IAdi.Size = new System.Drawing.Size(91, 20);
            this.check_IAdi.TabIndex = 9;
            this.check_IAdi.Text = "InterfazAdi";
            this.check_IAdi.UseVisualStyleBackColor = true;
            // 
            // check_MASWeb
            // 
            this.check_MASWeb.AutoSize = true;
            this.check_MASWeb.Location = new System.Drawing.Point(648, 31);
            this.check_MASWeb.Name = "check_MASWeb";
            this.check_MASWeb.Size = new System.Drawing.Size(87, 20);
            this.check_MASWeb.TabIndex = 10;
            this.check_MASWeb.Text = "MASWeb";
            this.check_MASWeb.UseVisualStyleBackColor = true;
            // 
            // check_CFDI
            // 
            this.check_CFDI.AutoSize = true;
            this.check_CFDI.Location = new System.Drawing.Point(648, 94);
            this.check_CFDI.Name = "check_CFDI";
            this.check_CFDI.Size = new System.Drawing.Size(94, 20);
            this.check_CFDI.TabIndex = 11;
            this.check_CFDI.Text = "PortalCFDI";
            this.check_CFDI.UseVisualStyleBackColor = true;
            // 
            // check_PDFiscales
            // 
            this.check_PDFiscales.AutoSize = true;
            this.check_PDFiscales.Location = new System.Drawing.Point(648, 159);
            this.check_PDFiscales.Name = "check_PDFiscales";
            this.check_PDFiscales.Size = new System.Drawing.Size(151, 20);
            this.check_PDFiscales.TabIndex = 12;
            this.check_PDFiscales.Text = "PortalDatosFiscales";
            this.check_PDFiscales.UseVisualStyleBackColor = true;
            // 
            // check_ProcesarMov
            // 
            this.check_ProcesarMov.AutoSize = true;
            this.check_ProcesarMov.Location = new System.Drawing.Point(648, 233);
            this.check_ProcesarMov.Name = "check_ProcesarMov";
            this.check_ProcesarMov.Size = new System.Drawing.Size(153, 20);
            this.check_ProcesarMov.TabIndex = 13;
            this.check_ProcesarMov.Text = "ProcesarMovimiento";
            this.check_ProcesarMov.UseVisualStyleBackColor = true;
            // 
            // check_QuotationWeb
            // 
            this.check_QuotationWeb.AutoSize = true;
            this.check_QuotationWeb.Location = new System.Drawing.Point(648, 309);
            this.check_QuotationWeb.Name = "check_QuotationWeb";
            this.check_QuotationWeb.Size = new System.Drawing.Size(115, 20);
            this.check_QuotationWeb.TabIndex = 14;
            this.check_QuotationWeb.Text = "QuotationWeb";
            this.check_QuotationWeb.UseVisualStyleBackColor = true;
            // 
            // check_ServicioEmision
            // 
            this.check_ServicioEmision.AutoSize = true;
            this.check_ServicioEmision.Location = new System.Drawing.Point(904, 233);
            this.check_ServicioEmision.Name = "check_ServicioEmision";
            this.check_ServicioEmision.Size = new System.Drawing.Size(126, 20);
            this.check_ServicioEmision.TabIndex = 18;
            this.check_ServicioEmision.Text = "ServicioEmision";
            this.check_ServicioEmision.UseVisualStyleBackColor = true;
            // 
            // check_ServIntegracion
            // 
            this.check_ServIntegracion.AutoSize = true;
            this.check_ServIntegracion.Location = new System.Drawing.Point(904, 309);
            this.check_ServIntegracion.Name = "check_ServIntegracion";
            this.check_ServIntegracion.Size = new System.Drawing.Size(144, 20);
            this.check_ServIntegracion.TabIndex = 19;
            this.check_ServIntegracion.Text = "ServicioIntegracion";
            this.check_ServIntegracion.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.check_ServIntPermisos);
            this.panel1.Controls.Add(this.check_ServMAS);
            this.panel1.Controls.Add(this.check_WSCLPortAgt);
            this.panel1.Controls.Add(this.check_ServIntegracion);
            this.panel1.Controls.Add(this.check_ServicioEmision);
            this.panel1.Controls.Add(this.check_ServDocu);
            this.panel1.Controls.Add(this.check_SIRI);
            this.panel1.Controls.Add(this.check_RobotCFD);
            this.panel1.Controls.Add(this.check_QuotationWeb);
            this.panel1.Controls.Add(this.check_ProcesarMov);
            this.panel1.Controls.Add(this.check_PDFiscales);
            this.panel1.Controls.Add(this.check_CFDI);
            this.panel1.Controls.Add(this.check_MASWeb);
            this.panel1.Controls.Add(this.check_IAdi);
            this.panel1.Controls.Add(this.check_PortalAPIRest);
            this.panel1.Controls.Add(this.check_IDCMAS);
            this.panel1.Controls.Add(this.check_IDCSIAPApi);
            this.panel1.Controls.Add(this.check_IDCSIAP);
            this.panel1.Controls.Add(this.check_DTS);
            this.panel1.Controls.Add(this.check_CloseService);
            this.panel1.Controls.Add(this.check_CatalogsWS);
            this.panel1.Controls.Add(this.check_BServiceSIAP);
            this.panel1.Controls.Add(this.check_BatchLauncher);
            this.panel1.Location = new System.Drawing.Point(23, 225);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1336, 414);
            this.panel1.TabIndex = 7;
            // 
            // check_ServIntPermisos
            // 
            this.check_ServIntPermisos.AutoSize = true;
            this.check_ServIntPermisos.Location = new System.Drawing.Point(1108, 31);
            this.check_ServIntPermisos.Name = "check_ServIntPermisos";
            this.check_ServIntPermisos.Size = new System.Drawing.Size(201, 20);
            this.check_ServIntPermisos.TabIndex = 24;
            this.check_ServIntPermisos.Text = "ServicioIntegracionPermisos";
            this.check_ServIntPermisos.UseVisualStyleBackColor = true;
            // 
            // check_ServMAS
            // 
            this.check_ServMAS.AutoSize = true;
            this.check_ServMAS.Location = new System.Drawing.Point(1174, 159);
            this.check_ServMAS.Name = "check_ServMAS";
            this.check_ServMAS.Size = new System.Drawing.Size(114, 20);
            this.check_ServMAS.TabIndex = 23;
            this.check_ServMAS.Text = "ServiciosMAS";
            this.check_ServMAS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.check_ServMAS.UseVisualStyleBackColor = true;
            // 
            // check_WSCLPortAgt
            // 
            this.check_WSCLPortAgt.AutoSize = true;
            this.check_WSCLPortAgt.Location = new System.Drawing.Point(1132, 309);
            this.check_WSCLPortAgt.Name = "check_WSCLPortAgt";
            this.check_WSCLPortAgt.Size = new System.Drawing.Size(156, 20);
            this.check_WSCLPortAgt.TabIndex = 22;
            this.check_WSCLPortAgt.Text = "WS-CLPortalAgentes";
            this.check_WSCLPortAgt.UseVisualStyleBackColor = true;
            // 
            // check_ServDocu
            // 
            this.check_ServDocu.AutoSize = true;
            this.check_ServDocu.Location = new System.Drawing.Point(904, 159);
            this.check_ServDocu.Name = "check_ServDocu";
            this.check_ServDocu.Size = new System.Drawing.Size(172, 20);
            this.check_ServDocu.TabIndex = 17;
            this.check_ServDocu.Text = "ServicioDocumentacion";
            this.check_ServDocu.UseVisualStyleBackColor = true;
            // 
            // check_SIRI
            // 
            this.check_SIRI.AutoSize = true;
            this.check_SIRI.Location = new System.Drawing.Point(904, 94);
            this.check_SIRI.Name = "check_SIRI";
            this.check_SIRI.Size = new System.Drawing.Size(190, 20);
            this.check_SIRI.TabIndex = 16;
            this.check_SIRI.Text = "ServiceAutomaticLoadSIRI";
            this.check_SIRI.UseVisualStyleBackColor = true;
            // 
            // check_RobotCFD
            // 
            this.check_RobotCFD.AutoSize = true;
            this.check_RobotCFD.Location = new System.Drawing.Point(904, 31);
            this.check_RobotCFD.Name = "check_RobotCFD";
            this.check_RobotCFD.Size = new System.Drawing.Size(100, 20);
            this.check_RobotCFD.TabIndex = 15;
            this.check_RobotCFD.Text = "Robot_CFD";
            this.check_RobotCFD.UseVisualStyleBackColor = true;
            // 
            // btn_CrearPaquete
            // 
            this.btn_CrearPaquete.Location = new System.Drawing.Point(636, 645);
            this.btn_CrearPaquete.Name = "btn_CrearPaquete";
            this.btn_CrearPaquete.Size = new System.Drawing.Size(161, 54);
            this.btn_CrearPaquete.TabIndex = 8;
            this.btn_CrearPaquete.Text = "Crear Paquete";
            this.btn_CrearPaquete.UseVisualStyleBackColor = true;
            this.btn_CrearPaquete.Click += new System.EventHandler(this.btn_CrearPaquete_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 791);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Consola / Salida:";
            // 
            // txt_TerminalOutput
            // 
            this.txt_TerminalOutput.Location = new System.Drawing.Point(32, 828);
            this.txt_TerminalOutput.Name = "txt_TerminalOutput";
            this.txt_TerminalOutput.Size = new System.Drawing.Size(1300, 22);
            this.txt_TerminalOutput.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 994);
            this.Controls.Add(this.txt_TerminalOutput);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btn_CrearPaquete);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_TemplatePaquete);
            this.Controls.Add(this.txt_pathDestino);
            this.Controls.Add(this.txt_BuildTFS);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_BuildTFS;
        private System.Windows.Forms.TextBox txt_pathDestino;
        private System.Windows.Forms.Button btn_TemplatePaquete;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox check_BatchLauncher;
        private System.Windows.Forms.CheckBox check_BServiceSIAP;
        private System.Windows.Forms.CheckBox check_CatalogsWS;
        private System.Windows.Forms.CheckBox check_CloseService;
        private System.Windows.Forms.CheckBox check_DTS;
        private System.Windows.Forms.CheckBox check_IDCSIAP;
        private System.Windows.Forms.CheckBox check_IDCSIAPApi;
        private System.Windows.Forms.CheckBox check_IDCMAS;
        private System.Windows.Forms.CheckBox check_PortalAPIRest;
        private System.Windows.Forms.CheckBox check_IAdi;
        private System.Windows.Forms.CheckBox check_MASWeb;
        private System.Windows.Forms.CheckBox check_CFDI;
        private System.Windows.Forms.CheckBox check_PDFiscales;
        private System.Windows.Forms.CheckBox check_ProcesarMov;
        private System.Windows.Forms.CheckBox check_QuotationWeb;
        private System.Windows.Forms.CheckBox check_ServicioEmision;
        private System.Windows.Forms.CheckBox check_ServIntegracion;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox check_ServIntPermisos;
        private System.Windows.Forms.CheckBox check_ServMAS;
        private System.Windows.Forms.CheckBox check_WSCLPortAgt;
        private System.Windows.Forms.CheckBox check_ServDocu;
        private System.Windows.Forms.CheckBox check_SIRI;
        private System.Windows.Forms.CheckBox check_RobotCFD;
        private System.Windows.Forms.Button btn_CrearPaquete;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txt_TerminalOutput;
    }
}

