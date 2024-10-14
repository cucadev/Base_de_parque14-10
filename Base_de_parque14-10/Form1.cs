using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base_de_parque14_10;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Mysqlx.Cursor;

namespace Restaurant
{

    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            MostrarDatosEnGrilla();
        }
        public class Monitoreo
        {
            public string Incidente;
            public string Empleado;
            public string Zona;
            public string Tipo;
            public string Fecha;
            public string Inicio;
            public string Fin;

            // Constructor de la clase
            public Monitoreo(string incidente, string empleado, string zona, string tipo, string fecha, string inicio, string fin)
            {
                Incidente = incidente;
                Empleado = empleado;
                Zona = zona;
                Tipo = tipo;
                Fecha = fecha;
                Inicio = inicio;
                Fin = fin;
            }
            public Monitoreo()
            {
                Incidente = "";
                Empleado = "";
                Zona = "";
                Tipo = "";
                Fecha = "";
                Inicio = "";
                Fin = "";
            }
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtén los valores de los cuadros de texto
                string nombre = txtNombre.Text;
                string apellido = txtApellido.Text;
                string id = txtID.Text;
                string sector = txtSector.Text;
                string contacto = txtContacto.Text;

                // Crea una instancia de la clase Persona con los valores
                Persona nuevaPersona = new Persona(nombre, apellido, id, sector, contacto);
                // Agrega la nueva Persona al DataGridView dgvPersonal

                // Limpia los cuadros de texto después de agregar la Persona
                txtNombre.Clear();
                txtApellido.Clear();
                txtID.Clear();
                txtSector.Clear();
                txtContacto.Clear();

                // Cadena de conexión a la base de datos MySQL
                string connectionString = "Server=localhost;Database=isft225;Uid=root;Pwd=";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO isft225 (Nombre, Apellido, ID, Sector, Contacto) VALUES (@Nombre, @Apellido, @ID, @Sector, @Contacto)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Agregar parámetros
                        command.Parameters.AddWithValue("@Nombre", nuevaPersona.Nombre);
                        command.Parameters.AddWithValue("@Apellido", nuevaPersona.Apellido);
                        command.Parameters.AddWithValue("@ID", nuevaPersona.ID);
                        command.Parameters.AddWithValue("@Sector", nuevaPersona.Sector);
                        command.Parameters.AddWithValue("@Contacto", nuevaPersona.Contacto);

                        // Ejecutar la consulta
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Datos guardados correctamente.");
                            dgvPersonal.Rows.Add(nuevaPersona.Nombre, nuevaPersona.Apellido, nuevaPersona.ID, nuevaPersona.Sector, nuevaPersona.Contacto);

                        }
                        else
                        {
                            MessageBox.Show("No se pudo guardar los datos.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            //MostrarDatosEnGrilla();

        }
        private void MostrarDatosEnGrilla()
        {
            try
            {
                // Cadena de conexión a la base de datos MySQL
                string connectionString = "Server=localhost;Database=isft225;Uid=root;Pwd=";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Nombre, Apellido, ID, Sector, Contacto FROM isft225";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Asignar el DataTable al DataGridView
                            dgvPersonal.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }
        private void btnEvento_Click(object sender, EventArgs e)
        {
            Form1 frmEvento = new Form1();
            frmEvento.Show();
        }

        private void dgvPersonal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void Eliminar()
        {
            try
            {
                // Verificar si se hizo clic en una celda y no en la cabecera
                if (dgvPersonal.SelectedCells.Count > 0)
                {
                    // Obtener el ID de la fila seleccionada (suponiendo que el nombre de la columna es "ID")
                    string idToString = dgvPersonal.SelectedCells[0].OwningRow.Cells["ID"].Value.ToString();
                    int idToDelete = Convert.ToInt32(idToString);
                    string connectionString = "Server=localhost;Database=isft225;Uid=root;Pwd=";

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // Consulta SQL para eliminar la fila con el ID seleccionado
                        string deleteQuery = $"DELETE FROM isft225 WHERE ID = {idToDelete}";
                        using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                        {
                            int rowsAffected = deleteCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Fila eliminada correctamente.");
                                MostrarDatosEnGrilla();
                                // Actualizar el DataGridView después de eliminar
                                // Asegúrate de que tengas un adapter y una dataTable configurados correctamente
                                // adapter.Fill(dataTable);
                            }
                            else
                            {
                                MessageBox.Show("No se pudo eliminar la fila.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione una fila para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar la fila: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Eliminar();
        }

        private void eventoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 frmEvento = new Form1();
            frmEvento.Show();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtén los valores de los cuadros de texto
                string nombre = txtNombre.Text;
                string apellido = txtApellido.Text;
                string id = txtID.Text;
                string sector = txtSector.Text;
                string contacto = txtContacto.Text;

                // Verifica que el ID no esté vacío, ya que es necesario para realizar la actualización
                if (string.IsNullOrWhiteSpace(id))
                {
                    MessageBox.Show("El ID es necesario para actualizar los datos.");
                    return;
                }

                // Crea una instancia de la clase Persona con los valores
                Persona personaActualizada = new Persona(nombre, apellido, id, sector, contacto);

                // Cadena de conexión a la base de datos MySQL
                string connectionString = "Server=localhost;Database=isft225;Uid=root;Pwd=";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta de actualización
                    string query = "UPDATE isft225 SET Nombre = @Nombre, Apellido = @Apellido, Sector = @Sector, Contacto = @Contacto WHERE ID = @ID";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Agregar parámetros
                        command.Parameters.AddWithValue("@Nombre", personaActualizada.Nombre);
                        command.Parameters.AddWithValue("@Apellido", personaActualizada.Apellido);
                        command.Parameters.AddWithValue("@Sector", personaActualizada.Sector);
                        command.Parameters.AddWithValue("@Contacto", personaActualizada.Contacto);
                        command.Parameters.AddWithValue("@ID", personaActualizada.ID);

                        // Ejecutar la consulta
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Datos actualizados correctamente.");

                            // Limpia los cuadros de texto después de actualizar los datos
                            txtNombre.Clear();
                            txtApellido.Clear();
                            txtID.Clear();
                            txtSector.Clear();
                            txtContacto.Clear();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo actualizar los datos. Asegúrate de que el ID sea correcto.");
                        }
                    }
                }

                // Mostrar los datos actualizados en el DataGridView
                MostrarDatosEnGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

