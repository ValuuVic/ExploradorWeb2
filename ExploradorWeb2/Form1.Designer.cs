namespace ExploradorWeb2
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
            this.webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.addressBar = new System.Windows.Forms.TextBox();
            this.goButton = new System.Windows.Forms.Button();
            this.historialComboBox = new System.Windows.Forms.ComboBox();
            this.buttonOrdenarV = new System.Windows.Forms.Button();
            this.buttonOrdenarF = new System.Windows.Forms.Button();
            this.buttonEliminar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.webView)).BeginInit();
            this.SuspendLayout();
            // 
            // webView
            // 
            this.webView.AllowExternalDrop = true;
            this.webView.CreationProperties = null;
            this.webView.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView.Location = new System.Drawing.Point(-3, 75);
            this.webView.Margin = new System.Windows.Forms.Padding(4);
            this.webView.Name = "webView";
            this.webView.Size = new System.Drawing.Size(731, 576);
            this.webView.Source = new System.Uri("https://www.google.com", System.UriKind.Absolute);
            this.webView.TabIndex = 0;
            this.webView.ZoomFactor = 1D;
            this.webView.Click += new System.EventHandler(this.webView_Click);
            // 
            // addressBar
            // 
            this.addressBar.Location = new System.Drawing.Point(16, 7);
            this.addressBar.Margin = new System.Windows.Forms.Padding(4);
            this.addressBar.Name = "addressBar";
            this.addressBar.Size = new System.Drawing.Size(564, 22);
            this.addressBar.TabIndex = 1;
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(589, 2);
            this.goButton.Margin = new System.Windows.Forms.Padding(4);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(117, 32);
            this.goButton.TabIndex = 2;
            this.goButton.Text = "Go!";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // historialComboBox
            // 
            this.historialComboBox.FormattingEnabled = true;
            this.historialComboBox.Location = new System.Drawing.Point(16, 42);
            this.historialComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.historialComboBox.Name = "historialComboBox";
            this.historialComboBox.Size = new System.Drawing.Size(384, 24);
            this.historialComboBox.TabIndex = 3;
            this.historialComboBox.SelectedIndexChanged += new System.EventHandler(this.historialComboBox_SelectedIndexChanged);
            // 
            // buttonOrdenarV
            // 
            this.buttonOrdenarV.Location = new System.Drawing.Point(407, 45);
            this.buttonOrdenarV.Name = "buttonOrdenarV";
            this.buttonOrdenarV.Size = new System.Drawing.Size(75, 23);
            this.buttonOrdenarV.TabIndex = 4;
            this.buttonOrdenarV.Text = "Visitas";
            this.buttonOrdenarV.UseVisualStyleBackColor = true;
            this.buttonOrdenarV.Click += new System.EventHandler(this.buttonOrdenarV_Click);
            // 
            // buttonOrdenarF
            // 
            this.buttonOrdenarF.Location = new System.Drawing.Point(488, 45);
            this.buttonOrdenarF.Name = "buttonOrdenarF";
            this.buttonOrdenarF.Size = new System.Drawing.Size(75, 23);
            this.buttonOrdenarF.TabIndex = 5;
            this.buttonOrdenarF.Text = "Fechas";
            this.buttonOrdenarF.UseVisualStyleBackColor = true;
            this.buttonOrdenarF.Click += new System.EventHandler(this.buttonOrdenarF_Click);
            // 
            // buttonEliminar
            // 
            this.buttonEliminar.Location = new System.Drawing.Point(569, 43);
            this.buttonEliminar.Name = "buttonEliminar";
            this.buttonEliminar.Size = new System.Drawing.Size(137, 23);
            this.buttonEliminar.TabIndex = 6;
            this.buttonEliminar.Text = "Eliminar elemento";
            this.buttonEliminar.UseVisualStyleBackColor = true;
            this.buttonEliminar.Click += new System.EventHandler(this.buttonEliminar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 644);
            this.Controls.Add(this.buttonEliminar);
            this.Controls.Add(this.buttonOrdenarF);
            this.Controls.Add(this.buttonOrdenarV);
            this.Controls.Add(this.historialComboBox);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.addressBar);
            this.Controls.Add(this.webView);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.webView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private System.Windows.Forms.TextBox addressBar;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.ComboBox historialComboBox;
        private System.Windows.Forms.Button buttonOrdenarV;
        private System.Windows.Forms.Button buttonOrdenarF;
        private System.Windows.Forms.Button buttonEliminar;
    }
}

