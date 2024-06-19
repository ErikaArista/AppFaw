using AppFaw.ViewModels;
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
using Newtonsoft.Json.Linq;


namespace AppFaw.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        private async void checkVer_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if(checkVer.IsChecked)
            {
                txtContraseña.IsPassword = false;
                return;
            }
            else
            {
                txtContraseña.IsPassword=true;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCorreoElectronico.Text) && !string.IsNullOrWhiteSpace(txtContraseña.Text))
            {
                var correo = txtCorreoElectronico.Text;
                var password = txtContraseña.Text;

                var loginInformacion = new
                {
                    email = correo,
                    password = password,
                };
                //consumo de api
                Uri uri = new Uri("http://10.0.2.2:8000/api/Login");
                var json = JsonConvert.SerializeObject(loginInformacion);
                var datos = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(uri, datos);
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //verificacion de que el json cno venga vacio
                        if (result.Contains("eyJ"))
                        {
                            var jsonResponse = JObject.Parse(result);
                            var token = jsonResponse.Value<string>("Token");
                            indicadorCargar.IsVisible = true;
                            indicadorCargar.IsRunning = true;
                            await Navigation.PushAsync(new Busqueda(token));
                        }
                        else if (result.Contains("Credenciales invalidas"))
                        {
                            await DisplayAlert("Error", "Usuario o contraseña incorrecto", "OK");
                        }
                        else
                        {
                            await DisplayAlert("Error", "Respuesta inesperada del servidor", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Error en la conexión con el servidor", "OK");
                    }
                }
            }
            else
            {
                DisplayAlert("Error", "Verifica que los campos estén llenos", "OK");
            }
        }


    }
}