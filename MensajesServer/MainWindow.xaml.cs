using MensajesSweet.Entidades;
using MensajesSweet.LogicaNegocios;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MensajesServer
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ActualizaGridPrincipal();
        }

        public void ActualizaGridPrincipal()
        {
            MensajesBol mensajes = new MensajesBol();
            dtgMensajes.ItemsSource = null;
            dtgMensajes.ItemsSource = mensajes.Todos();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            EMensaje mensaje = ((Button)sender).Tag as EMensaje;
            string cadena = "Esta seguro(a)\nDe eliminar el registro:\n"+mensaje.Descripcion+" "+mensaje.Departamento+
                " "+mensaje.Fecha.ToString("dd/MM/yyyy");
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(cadena, "Confirmación de Eliminación", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connMensajes"].ToString()))
                {
                    conn.Open();
                    string sql = "UPDATE dbo.Messages SET Estado = 0 WHERE MessageID = @ID";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = mensaje.ID;
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
            }
            ActualizaGridPrincipal();
        }

        private void btnCrearMensaje_Click(object sender, RoutedEventArgs e)
        {
            AddMensaje nuevo = new AddMensaje();
            nuevo.Owner = this;
            nuevo.ShowDialog();
            /*
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.InitialDirectory = Directory.GetCurrentDirectory();
            Nullable<bool> result = openFileDlg.ShowDialog();
            DateTime Hoy = DateTime.Today;
            if (result == true)
            {
                FileInfo fi = new FileInfo(openFileDlg.FileName);
                FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
                BinaryReader rdr = new BinaryReader(fs);
                byte[] fileData = rdr.ReadBytes((int)fs.Length);
                rdr.Close();
                fs.Close();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connMensajes"].ToString()))
                {
                    conn.Open();
                    //string sql = "INSERT INTO Documents VALUES (@DATA, @NOMBRE, default)";
                    SqlCommand cmd = new SqlCommand("dbo.CreaRegistro", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = "PRUEBAS CARLITOS";
                    cmd.Parameters.Add("@Persona", SqlDbType.NVarChar).Value = "CARLOS FABRE";
                    cmd.Parameters.Add("@Departamento", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = Hoy;
                    cmd.Parameters.Add("@Vigencia", SqlDbType.Int).Value = 5;
                    cmd.Parameters.Add("@Observaciones", SqlDbType.NVarChar).Value = "";
                    cmd.Parameters.Add("@Data", SqlDbType.Image, fileData.Length).Value = fileData;
                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = fi.Name;
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }

                MessageBox.Show(fi.FullName, "Archivo Insertado.!");
            }
            */
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            ActualizaGridPrincipal();
        }
    }
}
