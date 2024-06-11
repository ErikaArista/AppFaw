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
    public partial class Graficas : ContentPage
    {
        List<Entry> entryList;
        public Graficas(List<Camion1> camiones)
        {
            InitializeComponent();
            entryList = new List<Entry>();
            LoadEntries(camiones);
            PopularPickerFechas(camiones);

            GraficaLinea.Chart = new LineChart()
            {
                Entries = entryList,
                LineMode = LineMode.Straight,
                LineSize = 8,
                PointMode= PointMode.Circle,
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

        private void PopularPickerFechas(List<Camion1> camiones)
        {
            var fechaSet = new HashSet<DateTime>();

            foreach (var camion in camiones)
            {
                if (fechaSet.Add(camion.FechaRealizacion.Date))
                {
                    // Añadir la fecha única al DatePicker
                    pickerFecha.Items.Add(camion.FechaRealizacion.Date.ToString("d"));
                }
            }
        }
    }
}
