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
using AppFaw.Models;

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
            Uri camionUri = new Uri("http://10.0.2.2:8000/api/Consulta-Tiempos");

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
                        PopularPickerEstaciones();
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
            pickerBuscar.Items.Clear();

            //HashSet evita que dupliquemos los Vin
            var vinSet = new HashSet<string>();

            foreach (var camion in camiones)
            {
                if (vinSet.Add(camion.VIN))
                {
                    pickerBuscar.Items.Add(camion.VIN);
                }
            }

            // Aqui hacemos que solo se ejecute un evento a la vez
            pickerBuscar.SelectedIndexChanged -= PickerBuscar_SelectedIndexChanged;
            pickerBuscar.SelectedIndexChanged += PickerBuscar_SelectedIndexChanged;
        }

        private void PopularPickerEstaciones()
        {
            pickerEstaciones.Items.Clear();
            var estaciones = new HashSet<string>();

            foreach (var camion in camiones)
            {
                estaciones.Add(camion.Estacion);
            }

            foreach (var estacion in estaciones)
            {
                pickerEstaciones.Items.Add(estacion);
            }

            pickerEstaciones.SelectedIndexChanged -= PickerEstaciones_SelectedIndexChanged;
            pickerEstaciones.SelectedIndexChanged += PickerEstaciones_SelectedIndexChanged;
        }

        private void PickerEstaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerEstaciones.SelectedIndex != -1)
            {
                var selectedEstacion = pickerEstaciones.Items[pickerEstaciones.SelectedIndex];
                var relatedVINs = camiones.Where(c => c.Estacion == selectedEstacion).Select(c => c.VIN).Distinct().ToList();

                pickerBuscar.Items.Clear();
                if (relatedVINs.Count > 0)
                {
                    foreach (var vin in relatedVINs)
                    {
                        pickerBuscar.Items.Add(vin);
                    }
                }
                else
                {
                    DisplayAlert("Alerta", "No se encontraron VINs para esta estación.", "OK");
                }
            }
        }

        private void PickerBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerBuscar.SelectedIndex != -1)
            {
                var selectedVIN = pickerBuscar.Items[pickerBuscar.SelectedIndex];
                var selectedEstacion = pickerEstaciones.SelectedIndex != -1 ? pickerEstaciones.Items[pickerEstaciones.SelectedIndex] : null;

                // Aqui hacemos que se verifique si el Vin tiene una estacion 
                var camion = camiones.FirstOrDefault(c => c.VIN == selectedVIN && c.Estacion == selectedEstacion);

                if (camion != null)
                {
                    Fecha.Text = camion.FechaRealizacion.ToString("dd/MM/yyyy");
                    Mejoras.Text = camion.MejorasContinuas;
                    Tiempo.Text = camion.TiempoAuditores.ToString();
                }
                else
                {
                    ShowAlertAsync();
                }
            }
        }

        private async void ShowAlertAsync()
        {
            await DisplayAlert("Alerta", "El VIN seleccionado no tiene una estación asociada.", "OK");
        }

        private async void ButtonGraficas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Graficas(camiones));
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
        public User users { get; set; }
    }
}
