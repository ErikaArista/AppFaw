using AppFaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace AppFaw.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Busqueda : ContentPage
    {
        public Busqueda()
        {
            InitializeComponent();
        }

        private async void ObtenerDatosButton_Clicked(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("http://10.0.2.2:8000/api/auditoresverificacion");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var camiones = JsonConvert.DeserializeObject<List<Camion1>>(result);
                    listView.ItemsSource = camiones;
                }
                else
                {
                    await DisplayAlert("Error", "No se pudieron obtener los datos del camión.", "Ok");
                }
            }
        }
    }
}