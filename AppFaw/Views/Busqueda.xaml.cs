using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFaw.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Busqueda : ContentPage
    {
        private List<Camion1> camiones;

        public Busqueda()
        {
            InitializeComponent();
            LoadCamionData();
        }

        private async void LoadCamionData()
        {
            Uri camionUri = new Uri("http://127.0.0.1:8000/api/ConsultaDatos");

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(camionUri);

                    System.Diagnostics.Debug.WriteLine($"Status Code: {response.StatusCode}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine($"Response JSON: {jsonResponse}");

                        camiones = JsonConvert.DeserializeObject<List<Camion1>>(jsonResponse);
                        PopulatePicker();
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine($"Error Content: {errorContent}");
                        await DisplayAlert("Error", $"No se pudo obtener los datos de los camiones. Código de estado: {response.StatusCode}", "OK");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                    await DisplayAlert("Error", "Ocurrió un error al intentar obtener los datos de los camiones. Verifique su conexión a internet y vuelva a intentarlo.", "OK");
                }
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