using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensajesSweet.Entidades
{
    public class EMensaje
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public string Persona { get; set; }
        public string Departamento { get; set; }
        public DateTime Fecha { get; set; }
        public int Vigencia { get; set; }
        public string Observaciones { get; set; }
        public int Documento { get; set; }
    }
}
