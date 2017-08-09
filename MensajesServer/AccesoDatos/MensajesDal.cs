using MensajesSweet.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MensajesSweet.AccesoDatos
{
    public class MensajesDal
    {
        public List<EMensaje> GetAll()
        {
            List<EMensaje> mensajes = new List<EMensaje>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connMensajes"].ToString()))
            {
                conn.Open();
                string sql = "SELECT "+ 
                                "A.MessageID as Id, " +
                                "A.Descripcion AS DESCRIPCION, "+
                                "A.Persona AS PERSONA, "+
                                "B.Descripcion AS DEPARTAMENTO,"+
                                "A.Fecha AS FECHA, "+
                                "A.Vigencia AS VIGENCIA, "+
                                "A.Observaciones AS OBSERVACIONES ,"+
                                "A.DocumentID AS DOCUMENTO, dateadd(day,A.Vigencia-1,A.Fecha) AS FECHA_VIGENCIA, d.NOMBRE AS NOMBRE " +
                                " FROM dbo.Messages A "+
                                "INNER JOIN dbo.Departments B " +
                                "on(A.DepartmentID = B.ID) " +
                                "INNER JOIN dbo.Documents d ON(A.DocumentID = d.ID) " +
                                "WHERE A.ESTADO = 1 AND dateadd(day,A.Vigencia,A.Fecha) >= getdate()";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader data = cmd.ExecuteReader();
                    while (data.Read())
                    {
                        EMensaje mensaje = new EMensaje
                        {
                            ID = Convert.ToInt32(data["ID"]),
                            Descripcion = Convert.ToString(data["DESCRIPCION"]),
                            Persona = Convert.ToString(data["PERSONA"]),
                            Departamento = Convert.ToString(data["DEPARTAMENTO"]),
                            Fecha = Convert.ToDateTime(data["FECHA"]),
                            Vigencia = Convert.ToInt32(data["VIGENCIA"]),
                            Observaciones = Convert.ToString(data["OBSERVACIONES"]),
                            Documento = Convert.ToInt32(data["DOCUMENTO"]),
                            FechaVigencia = Convert.ToDateTime(data["FECHA_VIGENCIA"]),
                            NombreDocumento = Convert.ToString(data["NOMBRE"]),
                        };
                        mensajes.Add(mensaje);
                    }
                }
            }
            return mensajes;
        }
    }
}
