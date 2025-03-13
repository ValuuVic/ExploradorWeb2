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
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ExploradorWeb2
{
    public partial class Form1 : Form
    {
        List<Url> URLS = new List<Url>();
        string archivo = @"../../historial.json";
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
        private void CargarHistorial()
        {
            try
            {
                if (File.Exists(archivo))
                {
                    string json = File.ReadAllText(archivo);
                    URLS = JsonConvert.DeserializeObject<List<Url>>(json) ?? new List<Url>();
                }
                URLS = URLS.OrderByDescending(u => u.FechaAcceso).Take(10).ToList();
                historialComboBox.Items.Clear();
                historialComboBox.Items.AddRange(URLS.Select(u => u.url).ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer el historial: " + ex.Message);
            }
        }
        private void GuardarHistorial()
        {
            try
            {
                URLS = URLS.OrderByDescending(u => u.FechaAcceso).Take(10).ToList();
                string json = JsonConvert.SerializeObject(URLS);
                File.WriteAllText(archivo, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el historial: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarHistorial();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            string url = addressBar.Text.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                url = "https://www.google.com";
            }
            else if (!url.StartsWith("https://"))
            {
                url = url.EndsWith(".com") ? "https://" + url : "https://www.google.com/search?q=" + url;
            }

            addressBar.Text = url;
            if (webView?.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(url);
            }

            Url existente = URLS.Find(u => u.url == url);
            if (existente == null)
            {
                URLS.Add(new Url { url = url, FechaAcceso = DateTime.Now, repeticiones = 1 });
            }
            else
            {
                existente.repeticiones++;
                existente.FechaAcceso = DateTime.Now;
            }

            GuardarHistorial();
            historialComboBox.Items.Clear();
            historialComboBox.Items.AddRange(URLS.Select(u => u.url).ToArray());

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
                URLS.RemoveAll(u => u.url == urlAEliminar);
                GuardarHistorial();
                historialComboBox.Items.Clear();
                historialComboBox.Items.AddRange(URLS.Select(u => u.url).ToArray());
            }
        }
    }
}
