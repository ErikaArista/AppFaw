using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFaw.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Auditores : ContentPage
    {
        List<Camion1> camionesList;
        Dictionary<string, List<TimeSpan>> tiemposPorVin;

        public Auditores(List<Camion1> camiones)
        {
            InitializeComponent();
            camionesList = camiones;
            tiemposPorVin = new Dictionary<string, List<TimeSpan>>();
            PopularPickerIdAuditores();
        }

        private void PopularPickerIdAuditores()
        {
            var nombreAuditorSet = new HashSet<string>();

            foreach (var camion in camionesList)
            {
                if (nombreAuditorSet.Add(camion.users.name))
                {
                    pickerNombreAuditores.Items.Add(camion.users.name.ToString());
                }
            }
        }

        private async void MostrarDatosAuditor(string AuditorSeleccionado)
        {
            // Limpiar datos 
            VinAuditor.Text = string.Empty;
            Tiempo.Text = string.Empty;
            tiemposPorVin.Clear();

            // Busca los camiones relacionados con el auditor seleccionado
            var camionesRelacionados = camionesList.Where(c => c.users.name == AuditorSeleccionado);

            // Agregar los tiempos
            foreach (var camion in camionesRelacionados)
            {
                if (!tiemposPorVin.ContainsKey(camion.VIN))
                {
                    tiemposPorVin.Add(camion.VIN, new List<TimeSpan>());
                }

                tiemposPorVin[camion.VIN].Add(camion.TiempoAuditores);
            }

            //Muentra los VIN sin repeticion
            VinAuditor.Text = string.Join(Environment.NewLine, tiemposPorVin.Keys);

            //Muestra los tiempo por cada VIN
            foreach (var vin in tiemposPorVin.Keys)
            {
                Tiempo.Text += $"VIN: {vin}" + Environment.NewLine;
                foreach (var tiempo in tiemposPorVin[vin])
                {

                    Tiempo.Text += $"{tiempo} " + Environment.NewLine;
                }
                Tiempo.Text += Environment.NewLine;
            }

            if (tiemposPorVin.Keys.Count == 0)
            {
                await DisplayAlert("Error", "Auditor no encontrado", "Ok");
            }
        }

        private void pickerAuditores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerNombreAuditores.SelectedIndex != -1)
            {
                string AuditorSeleccionado = Convert.ToString(pickerNombreAuditores.SelectedItem);
                MostrarDatosAuditor(AuditorSeleccionado);
            }
        }
    }
}