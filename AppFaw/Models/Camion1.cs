using System;
using System.Collections.Generic;
using System.Text;

namespace AppFaw.Models
{
    internal class Camion1
    {
        public string VIN { get; set; }
        public string MejoraContinua { get; set; }
        public DateTime FechaRealizacion { get; set; }
        public TimeSpan Tiempo { get; set; }
        public string Estacion { get; set; }
    }
}
