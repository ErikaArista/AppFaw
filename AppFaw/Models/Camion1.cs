using System;
using System.Collections.Generic;
using System.Text;

namespace AppFaw.Models
{
    internal class Camion1
    {
        public int idAuditoresVerificacion { get; set; }
        public int idUser { get; set; }
        public int idInspeccion { get; set; }
        public string VIN { get; set; }
        public string MejorasContinuas { get; set; }
        public DateTime FechaRealizacion { get; set; }
        public TimeSpan TiempoAuditores { get; set; }
        public string Estacion { get; set; }
    }
}
