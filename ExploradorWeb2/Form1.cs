using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ExploradorWeb2
{
    public partial class Form1 : Form
    {
        List<Url> URLS = new List<Url>();
        public Form1()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
            webView.NavigationStarting += EnsureHttps;
            InitializeAsync();
            //addressBar.Text = "https://www.";
        }

        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.WebMessageReceived += UpdateAddressBar;

            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");
        }

        void UpdateAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            String uri = args.TryGetWebMessageAsString();
            addressBar.Text = uri;
            webView.CoreWebView2.PostWebMessageAsString(uri);
        }

        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
            String uri = args.Uri;
            if (!uri.StartsWith("https://"))
            {
                webView.CoreWebView2.ExecuteScriptAsync($"alert('{uri} is not safe, try an https link')");
                args.Cancel = true;
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            webView.Size = this.ClientSize - new System.Drawing.Size(webView.Location);
            goButton.Left = this.ClientSize.Width - goButton.Width;
            addressBar.Width = goButton.Left - addressBar.Left;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string archivo = @"../../historial.text";
            try
            {
                string[] lineas = File.Exists(archivo) ? File.ReadAllLines(archivo) : new string[0];

                // Crear una lista para almacenar las últimas 10 URLs
                List<string> ultimas10Urls = new List<string>();

 
                int totalLineas = lineas.Length;
                int inicio = totalLineas > 10 ? totalLineas - 10 : 0;

                for (int i = inicio; i < totalLineas; i++)
                {
                    string[] partes = lineas[i].Split('|');
                    if (partes.Length > 0 && !string.IsNullOrWhiteSpace(partes[0]))
                    {
                        ultimas10Urls.Add(partes[0]); 
                    }
                }
                historialComboBox.Items.Clear();
                historialComboBox.Items.AddRange(ultimas10Urls.ToArray());
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error al guardar o leer el historial: " + ex.Message);
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {

            string url = addressBar.Text.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                addressBar.Text = "https://www.google.com";
                url = addressBar.Text;
            }
            else if (!url.StartsWith("https://"))
            {
                if (url.EndsWith(".com"))
                {
                    addressBar.Text = "https://" + url;
                }
                else
                {
                    addressBar.Text = "https://www.google.com/search?q=" + url;
                }
                url = addressBar.Text;
            }
            if (webView?.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(url);
            }
            string direccion = addressBar.Text.Trim();
            Url existente = URLS.Find(u => u.url == direccion);
            if (existente == null)
            {
                Url nuevaUrl = new Url();
                nuevaUrl.url = addressBar.Text;
                nuevaUrl.FechaAcceso = DateTime.Now;
                nuevaUrl.repeticiones = 1;
                URLS.Add(nuevaUrl);
            }
            else
            {
                existente.repeticiones++;
                existente.FechaAcceso = DateTime.Now;
            }

            string archivo = @"../../historial.text";
            try
            {
                List<string> historialNuevo = URLS.Select(u => $"{u.url}|{u.repeticiones}|{u.FechaAcceso}").ToList();
                if (historialNuevo.Count > 10)
                {
                    historialNuevo = historialNuevo.Skip(historialNuevo.Count - 10).ToList();
                }
                File.WriteAllLines(archivo, historialNuevo);
                List<string> ultimas10Urls = historialNuevo.Select(linea => linea.Split('|')[0]).ToList();
                historialComboBox.Items.Clear();
                historialComboBox.Items.AddRange(ultimas10Urls.ToArray());
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error al guardar o leer el historial: " + ex.Message);
            }

        }

        private void webView_Click(object sender, EventArgs e)
        {

        }

        private void historialComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (historialComboBox.SelectedItem != null)
            {
                addressBar.Text = historialComboBox.SelectedItem.ToString();
            }
        }

        private void buttonOrdenarV_Click(object sender, EventArgs e)
        {
            URLS = URLS.OrderByDescending(a => a.repeticiones).ToList(); 
            historialComboBox.DataSource = null;
            historialComboBox.DataSource = URLS.Select(u => u.url).ToList();
        }

        private void buttonOrdenarF_Click(object sender, EventArgs e)
        {
            URLS = URLS.OrderByDescending(a => a.FechaAcceso).ToList();
            historialComboBox.DataSource = null;
            historialComboBox.DataSource = URLS.Select(u => u.url).ToList();
        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            if (historialComboBox.SelectedItem != null)
            {
                string urlAEliminar = historialComboBox.SelectedItem.ToString();
                Url urlEncontrada = URLS.FirstOrDefault(u => u.url == urlAEliminar);
                if (urlEncontrada != null)
                {
                    URLS.Remove(urlEncontrada);
                }
                string archivo = @"../../historial.text";
                List<string> historialGuardado = URLS.Select(u => $"{u.url}|{u.repeticiones}|{u.FechaAcceso}").ToList();
                File.WriteAllLines(archivo, historialGuardado);
                historialComboBox.DataSource = null;
                historialComboBox.Items.Clear();
                historialComboBox.Text = ""; 
                historialComboBox.SelectedIndex = -1; 
                historialComboBox.Items.AddRange(URLS.Select(u => u.url).ToArray());
            }
        }
    }
}
