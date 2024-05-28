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

        private void checkVer_CheckedChanged(object sender, CheckedChangedEventArgs e)
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
            if(!string.IsNullOrWhiteSpace(txtCorreoElectronico.Text) && !string.IsNullOrWhiteSpace(txtContraseña.Text))
            {

                var correo = txtCorreoElectronico.Text;
                var password = txtContraseña.Text;

                var loginInformacion = new
                {
                    email = correo,
                    password = password,
                };

                Uri uri = new Uri("http://10.0.2.2:8000/api/Login");
                var json = JsonConvert.SerializeObject(loginInformacion);
                var datos = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(uri,datos);
                    var result = await response.Content.ReadAsStringAsync();
                    
                    if (response.StatusCode == HttpStatusCode.OK && !result.ToString().Contains("Credenciales invalidas"))
                    {
                        indicadorCargar.IsVisible = true;
                        indicadorCargar.IsRunning = true;
                        await Navigation.PushAsync(new Busqueda());
                    }
                    else
                    {
                        await DisplayAlert("Error", "Usuario o contraseña incorrecto", "OK");
                    }
                }
            }
            else
            {
                DisplayAlert("Error", "Verifica que los campos esten llenos", "OK");
            }
        }
    }
}