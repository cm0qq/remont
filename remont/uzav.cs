using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace remont
{
    public partial class uzav : Form
    {
        private string numberZ;
        public uzav(string numberZ)
        {
            InitializeComponent();
            this.numberZ = numberZ;
        }

        private void uzav_Load(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
        }

        private void LoadDataFromDatabase()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=remont";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM zayavka WHERE number_z = @numberZ";

                using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@numberZ", numberZ);

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
        }
    }
}
