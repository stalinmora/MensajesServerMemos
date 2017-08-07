using MensajesSweet.AccesoDatos;
using MensajesSweet.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensajesSweet.LogicaNegocios
{
    public class MensajesBol
    {
        private MensajesDal _mensajesDal = new MensajesDal();

        public List<EMensaje> Todos()
        {
            return _mensajesDal.GetAll();
        }
    }
}
