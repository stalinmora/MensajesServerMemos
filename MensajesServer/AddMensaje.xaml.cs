using MensajesSweet.Entidades;
using MensajesSweet.LogicaNegocios;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MensajesServer
{
    /// <summary>
    /// Lógica de interacción para AddMensaje.xaml
    /// </summary>
    public partial class AddMensaje : Window
    {
        string archivo = "";
        string archivo2 = "";
        int codigoMensaje = 0;
        int codigoDocumento = 0;
        bool isActualizar = false;
        public AddMensaje()
        {
            InitializeComponent();
            this.isActualizar = false;
            
        }

        public AddMensaje(int codigo)
        {
            InitializeComponent();
            this.isActualizar = true;
            btnGuardar.Content = "Actualizar";
            MensajesBol bol = new MensajesBol();
            EMensaje mensaje = bol.PorId(codigo);
            codigoMensaje = codigo;
            txtDescripcion.Text = mensaje.Descripcion;
            txtPersona.Text = mensaje.Persona;
            //cmbDepartamento.SetValue = mensaje.Departamento;
            dpVigencia.SelectedDate = mensaje.Fecha;
            txtObservaciones.Text = mensaje.Observaciones;
            txtVigencia.Text = Convert.ToString(mensaje.Vigencia);
            lblArchivo.Content = mensaje.NombreDocumento;
            archivo2 = mensaje.NombreDocumento;
            this.codigoDocumento = mensaje.Documento;
        }

        private void btnArchivo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Nullable<bool> result = openFileDlg.ShowDialog();
            this.archivo = openFileDlg.FileName;
            lblArchivo.Content = openFileDlg.FileName.ToString();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connMensajes"].ToString()))
            {
                conn.Open();
                if (!this.isActualizar)
                {
                    FileInfo fi = new FileInfo(archivo);
                    FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
                    BinaryReader rdr = new BinaryReader(fs);
                    byte[] fileData = rdr.ReadBytes((int)fs.Length);
                    rdr.Close();
                    fs.Close();
                    //string sql = "INSERT INTO Documents VALUES (@DATA, @NOMBRE, default)";
                    SqlCommand cmd = new SqlCommand("dbo.CreaRegistro", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = txtDescripcion.Text;
                    cmd.Parameters.Add("@Persona", SqlDbType.NVarChar).Value = txtPersona.Text;
                    cmd.Parameters.Add("@Departamento", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = dpVigencia.SelectedDate;
                    cmd.Parameters.Add("@Vigencia", SqlDbType.Int).Value = Convert.ToInt32(txtVigencia.Text);
                    cmd.Parameters.Add("@Observaciones", SqlDbType.NVarChar).Value = txtObservaciones.Text;
                    cmd.Parameters.Add("@Data", SqlDbType.Image, fileData.Length).Value = fileData;
                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = fi.Name;
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    
                    if (lblArchivo.Content.ToString() != this.archivo2.ToString())
                    {
                        MessageBox.Show("Esta cambiando el archivo");
                        FileInfo fi = new FileInfo(archivo);
                        FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
                        BinaryReader rdr = new BinaryReader(fs);
                        byte[] fileData = rdr.ReadBytes((int)fs.Length);
                        rdr.Close();
                        fs.Close();
                        string sql = "UPDATE Documents SET DATA = @Data, NOMBRE = @Nombre where ID = " + codigoDocumento;
                        MessageBox.Show(sql);
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.Add("@Data", SqlDbType.Image, fileData.Length).Value = fileData;
                        cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = fi.Name;
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    else
                    {
                        string sql = "UPDATE Messages SET DESCRIPCION = @Descripcion,PERSONA = @Persona, "+ 
                            "DepartmentID = @Departamento, FECHA = @Fecha, VIGENCIA = @Vigencia, OBSERVACIONES = @Observaciones " +
                            "where MessageID = "+codigoMensaje;
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = txtDescripcion.Text;
                        cmd.Parameters.Add("@Persona", SqlDbType.NVarChar).Value = txtPersona.Text;
                        cmd.Parameters.Add("@Departamento", SqlDbType.Int).Value = 1;
                        cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = dpVigencia.SelectedDate;
                        cmd.Parameters.Add("@Vigencia", SqlDbType.Int).Value = Convert.ToInt32(txtVigencia.Text);
                        cmd.Parameters.Add("@Observaciones", SqlDbType.NVarChar).Value = txtObservaciones.Text;
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        
                    }       
                }
                conn.Close();
            }
            this.frmAddMensaje.Close();
        }
    }
}
