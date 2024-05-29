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
            
           
        }
    }
}