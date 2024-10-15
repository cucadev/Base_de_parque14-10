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

namespace Base_de_parque14_10
{
    public partial class Form1 : Form
    {
        public Form1()
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
                string incidente = txtIncidente.Text;
                string empleado = txtEmpleado.Text;
                string zona = txtZona.Text;
                string tipo = txtTipo.Text;
                string fecha = txtFecha.Text;
                string inicio = txtInicio.Text;
                string fin = txtFin.Text;

                // Crea una instancia de la clase Persona con los valores
                Monitoreo nuevoMonitoreo = new Monitoreo(incidente, empleado, zona, tipo, fecha, inicio, fin);
                // Agrega la nueva Persona al DataGridView dgvPersonal

                // Limpia los cuadros de texto después de agregar la Persona
                txtIncidente.Clear();
                txtEmpleado.Clear();
                txtZona.Clear();
                txtTipo.Clear();
                txtFecha.Clear();
                txtInicio.Clear();
                txtFin.Clear();

                // Cadena de conexión a la base de datos MySQL
                string connectionString = "Server=localhost;Database=base_de_parque;Uid=root;Pwd=";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO monitoreo (Incidente, Empleado, Zona, Tipo, Fecha, Inicio, Fin) VALUES (@Incidente, @Empleado, @Zona, @Tipo, @Fecha, @Inicio, @Fin)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Agregar parámetros
                        command.Parameters.AddWithValue("@Incidente", nuevoMonitoreo.Incidente);
                        command.Parameters.AddWithValue("@Empleado", nuevoMonitoreo.Empleado);
                        command.Parameters.AddWithValue("@Zona", nuevoMonitoreo.Zona);
                        command.Parameters.AddWithValue("@Tipo", nuevoMonitoreo.Tipo);
                        command.Parameters.AddWithValue("@Fecha", nuevoMonitoreo.Fecha);
                        command.Parameters.AddWithValue("@Inicio", nuevoMonitoreo.Inicio);
                        command.Parameters.AddWithValue("@Fin", nuevoMonitoreo.Fin);

                        // Ejecutar la consulta
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Datos guardados correctamente.");
                            dataGridView1.Rows.Add(nuevoMonitoreo.Incidente, nuevoMonitoreo.Empleado, nuevoMonitoreo.Zona, nuevoMonitoreo.Tipo, nuevoMonitoreo.Fecha, nuevoMonitoreo.Inicio, nuevoMonitoreo.Fin);

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
            MostrarDatosEnGrilla();

        }
        private void MostrarDatosEnGrilla()
        {
            try
            {
                // Cadena de conexión a la base de datos MySQL
                string connectionString = "Server=localhost;Database=base_de_parque;Uid=root;Pwd=";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Incidente, Empleado, Zona, Tipo, Fecha, Inicio, Fin FROM monitoreo";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Asignar el DataTable al DataGridView
                            //dataGridView1.DataSource = dataTable;
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void Eliminar()
        {
            try
            {
                // Verificar si se hizo clic en una celda y no en la cabecera
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    // Obtener el ID de la fila seleccionada (suponiendo que el nombre de la columna es "ID")
                    string IncidenteToString = dataGridView1.SelectedCells[0].OwningRow.Cells["Incidente"].Value.ToString();
                    int IncidenteToDelete = Convert.ToInt32(IncidenteToString);
                    string connectionString = "Server=localhost;Database=base_de_parque;Uid=root;Pwd=";

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // Consulta SQL para eliminar la fila con el ID seleccionado
                        string deleteQuery = $"DELETE FROM monitoreo WHERE Incidente = {IncidenteToDelete}";
                        using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                        {
                            int rowsAffected = deleteCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Fila eliminada correctamente.");
                                MostrarDatosEnGrilla();
                                //Actualizar el DataGridView después de eliminar
                                //Asegúrate de que tengas un adapter y una dataTable configurados correctamente
                                //adapter.Fill(dataTable);
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
                string incidente = txtIncidente.Text;
                string empleado = txtEmpleado.Text;
                string zona = txtZona.Text;
                string tipo = txtTipo.Text;
                string fecha = txtFecha.Text;
                string inicio = txtInicio.Text;
                string fin = txtFin.Text;

                // Verifica que el ID no esté vacío, ya que es necesario para realizar la actualización
                if (string.IsNullOrWhiteSpace(incidente))
                {
                    MessageBox.Show("El Incidente es necesario para actualizar los datos.");
                    return;
                }

                // Crea una instancia de la clase Persona con los valores
                Monitoreo monitoreoActualizado = new Monitoreo(incidente, empleado, zona, tipo, fecha, inicio, fin);

                // Cadena de conexión a la base de datos MySQL
                string connectionString = "Server=localhost;Database=base_de_parque;Uid=root;Pwd=";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta de actualización
                    string query = "UPDATE monitoreo SET Empleado = @Empleado, Zona = @Zona, Tipo = @Tipo, Fecha = @Fecha, Inicio = @Inicio, Fin = @Fin WHERE Incidente = @Incidente";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Agregar parámetros
                        command.Parameters.AddWithValue("@Empleado", monitoreoActualizado.Empleado);
                        command.Parameters.AddWithValue("@Zona", monitoreoActualizado.Zona);
                        command.Parameters.AddWithValue("@Tipo", monitoreoActualizado.Tipo);
                        command.Parameters.AddWithValue("@Fecha", monitoreoActualizado.Fecha);
                        command.Parameters.AddWithValue("@Inicio", monitoreoActualizado.Inicio);
                        command.Parameters.AddWithValue("@Fin", monitoreoActualizado.Fin);
                        command.Parameters.AddWithValue("@Incidente", monitoreoActualizado.Incidente);

                        // Ejecutar la consulta
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Datos actualizados correctamente.");

                            // Limpia los cuadros de texto después de actualizar los datos
                            txtIncidente.Clear();
                            txtEmpleado.Clear();
                            txtZona.Clear();
                            txtTipo.Clear();
                            txtFecha.Clear();
                            txtInicio.Clear();
                            txtFin.Clear();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo actualizar los datos. Asegúrate de que el ID sea correcto.");
                        }
                    }
                }

                // Mostrar los datos actualizados en el DataGridView
                //MostrarDatosEnGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

