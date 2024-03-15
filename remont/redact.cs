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
    public partial class redact : Form
    {
        public redact()
        {
            InitializeComponent();
        }

        private string number_z;
        private string date_z;
        private string slom_oborud;
        private string tip;
        private string client;
        private string status;
        private string master;
        private string opisanie;

        public redact(string number_z, string date_z, string slom_oborud, string tip, string client, string status, string master, string opisanie)
        {
            InitializeComponent();
            this.number_z = number_z;
            this.date_z = date_z;
            this.slom_oborud = slom_oborud;
            this.tip = tip;
            this.client = client;
            this.status = status;
            this.master = master;
            this.opisanie = opisanie;

            // Установите переданные данные в соответствующие элементы управления
            textBox1.Text = number_z;
            textBox2.Text = date_z;
            textBox3.Text = slom_oborud;
            textBox4.Text = tip;
            textBox5.Text = client;
            comboBox1.Text = status;
            comboBox2.Text = master;
            textBox6.Text = opisanie;
        }       

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=remont";
            try
            {
                // Получите измененные данные из элементов управления
                string updatedNumber_z = textBox1.Text;
                string updatedDate_z = textBox2.Text;
                string updatedSlom_oborud = textBox3.Text;
                string updatedTip = textBox4.Text;
                string updatedClient = textBox5.Text;
                string updatedStatus = comboBox1.Text;
                string updatedMaster = comboBox2.Text;
                string updatedOpisanie = textBox6.Text;

                // Подключитесь к базе данных
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Создайте SQL-запрос для обновления данных в таблице
                    string updateQuery = @"UPDATE zayavka SET 
                                    number_z = @Number_z,
                                    date_z = @Date_z,
                                    slom_oborud = @Slom_oborud,
                                    tip = @Tip,
                                    client = @Client,
                                    status = @Status,
                                    master = @Master,
                                    opisanie = @Opisanie
                                    WHERE number_z = @OldNumber_z"; 

                    // Создайте команду для выполнения SQL-запроса
                    using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
                    {
                        // Добавьте параметры к команде
                        command.Parameters.AddWithValue("@Number_z", updatedNumber_z);
                        command.Parameters.AddWithValue("@Date_z", updatedDate_z);
                        command.Parameters.AddWithValue("@Slom_oborud", updatedSlom_oborud);
                        command.Parameters.AddWithValue("@Tip", updatedTip);
                        command.Parameters.AddWithValue("@Client", updatedClient);
                        command.Parameters.AddWithValue("@Status", updatedStatus);
                        command.Parameters.AddWithValue("@Master", updatedMaster);
                        command.Parameters.AddWithValue("@Opisanie", updatedOpisanie);
                        command.Parameters.AddWithValue("@OldNumber_z", number_z); // Используйте старое значение номера заявки для обновления

                        // Выполните SQL-запрос
                        int rowsAffected = command.ExecuteNonQuery();

                        // Проверьте, были ли обновлены какие-либо строки
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Данные успешно обновлены.");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить данные. Проверьте введенные значения.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении данных: " + ex.Message);
            }
        }
    }
}
