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
    public partial class uzayav : Form
    {
        public uzayav()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string numberZ = textBox1.Text;
            uzav uzav = new uzav(numberZ);
            uzav.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int randomNumber = random.Next(200, 1001);
            textBox1.Text = randomNumber.ToString();
        }

        private string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=remont";
        private void button2_Click(object sender, EventArgs e)
        {
            string number_z = textBox1.Text;
            string date_z = textBox2.Text;
            string slom_oborud = textBox3.Text;
            string tip = textBox4.Text;
            string client = textBox5.Text;
            string status = comboBox1.SelectedItem.ToString();
            string master = comboBox2.SelectedItem.ToString();
            string opisanie = textBox6.Text;

            // Формируем SQL-запрос для вставки данных в таблицу
            string query = "INSERT INTO zayavka (number_z, date_z, slom_oborud, tip, client, status, master, opisanie) " +
                           $"VALUES (@number_z, @date_z, @slom_oborud, @tip, @client, @status, @master, @opisanie)";

            try
            {
                // Устанавливаем соединение с базой данных
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Создаем команду для выполнения SQL-запроса
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        // Добавляем параметры в запрос
                        command.Parameters.AddWithValue("@number_z", number_z);
                        command.Parameters.AddWithValue("@date_z", date_z);
                        command.Parameters.AddWithValue("@slom_oborud", slom_oborud);
                        command.Parameters.AddWithValue("@tip", tip);
                        command.Parameters.AddWithValue("@client", client);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@master", master);
                        command.Parameters.AddWithValue("@opisanie", opisanie);

                        // Выполняем SQL-запрос
                        int rowsAffected = command.ExecuteNonQuery();

                        // Проверяем, были ли успешно добавлены данные
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Данные успешно добавлены в базу данных.");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось добавить данные в базу данных.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
