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
using Microsoft.Web.WebView2.Core;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ExploradorWeb2
{
    public partial class Form1 : Form
    {
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

            string archivo = @"../../historial.text";
            try
            {
                List<string> historial = File.Exists(archivo) ? File.ReadAllLines(archivo).ToList() : new List<string>();
                historial.Add(url);
                if (historial.Count > 10)
                {
                    historial = historial.Skip(historial.Count - 10).ToList();
                }
                File.WriteAllLines(archivo, historial);
                historialComboBox.Items.Clear();
                historialComboBox.Items.AddRange(historial.ToArray());
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
    }
}
