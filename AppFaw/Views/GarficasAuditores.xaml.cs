using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;
using Entry = Microcharts.ChartEntry;
using SkiaSharp;
using AppFaw.Models;

namespace AppFaw.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GarficasAuditores : ContentPage
    {
        private List<Camion1> camionesList;
        List<Entry> entryList;
        public GarficasAuditores(List<Camion1> camiones)
        {
            InitializeComponent();
            camionesList = camiones;
            entryList = new List<Entry>();
            LoadEntries(camiones);
            PopularPickerAuditoresNombre(camiones);

            GraficaLinea.Chart = new LineChart()
            {
                Entries = entryList,
                LineMode = LineMode.Straight,
                LineSize = 8,
                PointMode = PointMode.Circle,
                PointSize = 18,
                LabelTextSize = 42,
            };

            pickerAuditor.SelectedIndexChanged += PickerName_SelectedIndexChanged;
        }


        private void PickerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerAuditor.SelectedIndex == -1) return;

            string selectedName = pickerAuditor.Items[pickerAuditor.SelectedIndex];

            var filteredCamiones = camionesList
                .Where(c => c.users.name == selectedName)
                .ToList();

            entryList.Clear();
            LoadEntries(filteredCamiones);

            GraficaLinea.Chart = new LineChart()
            {
                Entries = entryList,
                LineMode = LineMode.Straight,
                LineSize = 8,
                PointMode = PointMode.Circle,
                PointSize = 18,
                LabelTextSize = 42,
            };
        }
        private void LoadEntries(List<Camion1> camiones)
        {
            Random rand = new Random();

            var groupedCamiones = camiones
                .GroupBy(c => c.Estacion)
                .Select(g => new
                {
                    Estacion = g.Key,
                    TiempoTotal = g.Sum(c => c.TiempoAuditores.TotalMinutes)
                })
                .ToList();

            foreach (var group in groupedCamiones)
            {
                Entry grafica = new Entry((float)group.TiempoTotal)
                {
                    Label = group.Estacion,
                    ValueLabel = group.TiempoTotal.ToString("F2"), // Formato con dos decimales
                    Color = GenerateRandomColor(rand),
                };
                entryList.Add(grafica);
            }
        }
        private SKColor GenerateRandomColor(Random rand)
        {
            return new SKColor(
                (byte)rand.Next(256),
                (byte)rand.Next(256),
                (byte)rand.Next(256)
            );
        }

        private void PopularPickerAuditoresNombre(List<Camion1> camiones)
        {
            var NombreSet = new HashSet<string>();

            foreach (var camion in camiones)
            {
                if (NombreSet.Add(camion.users.name))
                {
                    pickerAuditor.Items.Add(camion.users.name.ToString());
                }
            }
        }

    }
}