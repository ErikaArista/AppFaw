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
        public User users { get; set; }
    }

    
    
    public class User
    {
        public int id {  get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string email_verified_at { get; set; }
        public string two_factor_confirmed_at { get; set; }
        public string telefono { get; set; }
        public string current_team_id { get; set; }
        public string profile_photo_path { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string profile_photo_url { get; set; }

    }
}
