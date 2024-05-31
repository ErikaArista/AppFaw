using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppFaw.ViewModels;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;

namespace AppFaw.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Busqueda : ContentPage
    {
        private List<Camion1> camiones;
        private string authToken;

        public Busqueda(string authToken)
        {
            InitializeComponent();
            this.authToken = authToken;
            LoadCamionData();
        }

        private async void LoadCamionData()
        {
            Uri camionUri = new Uri("http://10.0.2.2:8000/api/ConsultaDatos");

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                    var response = await client.GetAsync(camionUri);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        camiones = JsonConvert.DeserializeObject<List<Camion1>>(jsonResponse);
                        PopulatePicker();
                    }
                    else
                    {
                        await DisplayAlert("Error", $"No se pudo obtener los datos de los camiones. Código de estado: {response.StatusCode}", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }


        private void PopulatePicker()
        {
            foreach (var camion in camiones)
            {
                pickerBuscar.Items.Add(camion.VIN);
            }

            pickerBuscar.SelectedIndexChanged += PickerBuscar_SelectedIndexChanged;
        }

        private void PickerBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerBuscar.SelectedIndex != -1)
            {
                var selectedVIN = pickerBuscar.Items[pickerBuscar.SelectedIndex];
                var selectedCamion = camiones.Find(c => c.VIN == selectedVIN);

                if (selectedCamion != null)
                {
                    Fecha.Text = selectedCamion.FechaRealizacion.ToString("dd/MM/yyyy");
                    Estacion.Text = selectedCamion.Estacion;
                    Mejoras.Text = selectedCamion.MejorasContinuas;
                    Tiempo.Text = selectedCamion.TiempoAuditores.ToString();
                }
            }
        }
    }

    public class Camion1
    {
        public int idAuditoresVerificacion { get; set; }
        public int idUser { get; set; }
        public int idInspeccion { get; set; }
        public string VIN { get; set; }
        public string MejorasContinuas { get; set; }
        public DateTime FechaRealizacion { get; set; }
        public TimeSpan TiempoAuditores { get; set; }
        public string Estacion { get; set; }
    }
}
