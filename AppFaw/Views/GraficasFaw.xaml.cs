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

namespace AppFaw.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraficasFaw : ContentPage
    {
        List<Entry> entries;
        public GraficasFaw()
        {
            InitializeComponent();
            entries = new List<Entry>();
            LoadEntries();

            graficasChart.Chart = new LineChart()
            {
               Entries = entries
            };
        }

        private void LoadEntries()
        {
            Entry lineGrafica = new Entry(70)
            {
                Label = "Alineacion",
                ValueLabel = "70",
                Color = SKColor.Parse("#53A1EA")
            };
        }
    }
}