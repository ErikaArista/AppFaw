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

        public Auditores(List<Camion1> camiones)
        {
            InitializeComponent();
            camionesList = camiones;
            PopularPickerIdAuditores();
        }

        private void PopularPickerIdAuditores()
        {
            var idAuditorSet = new HashSet<int>();

            foreach (var camion in camionesList)
            {
                if (idAuditorSet.Add(camion.idAuditoresVerificacion))
                {
                    pickerIdAuditores.Items.Add(camion.idAuditoresVerificacion.ToString());
                }
            }
        }

        private void MostrarDatosAuditor(int idAuditorSeleccionado)
        {
            // Filtrar los camiones relacionados con el auditor seleccionado
            var camionesRelacionados = camionesList.Where(c => c.idAuditoresVerificacion == idAuditorSeleccionado);

            NombreAuditor.Text = string.Empty;
            VinAuditor.Text = string.Empty;
            Tiempo.Text = string.Empty;

            // Mostrar los detalles del primer camión relacionado 
            var primerCamion = camionesRelacionados.FirstOrDefault();
            if (primerCamion != null)
            {
                // Mostrar el nombre del auditor (asumiendo que está en el User asociado al camión)
                if (primerCamion.users != null)
                {
                    NombreAuditor.Text = primerCamion.users.name;
                }

                // Mostrar los VIN y tiempos de ensamblaje de los camiones relacionados
                foreach (var camion in camionesRelacionados)
                {
                    VinAuditor.Text += camion.VIN + Environment.NewLine;
                    Tiempo.Text += camion.TiempoAuditores.ToString() + Environment.NewLine;
                }
            }
            else
            {
                NombreAuditor.Text = "Auditor no encontrado";
            }
        }

        private void pickerIdAuditores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerIdAuditores.SelectedIndex != -1)
            {
                int idAuditorSeleccionado = Convert.ToInt32(pickerIdAuditores.SelectedItem);
                MostrarDatosAuditor(idAuditorSeleccionado);
            }
        }
    }
}