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
using Aspose.Pdf;
using Aspose.Pdf.Text;

namespace remont
{
    public partial class zayavki : Form
    {
        public zayavki()
        {
            InitializeComponent();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            // Получение значения для поиска
            string searchValue = textBox1.Text.Trim();

            // Проверка наличия данных в DataGridView
            if (dataGridView1.DataSource == null)
                return;

            // Фильтрация данных в режиме реального времени
            ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = $"number_z LIKE '%{searchValue}%'";
        }

        private void zayavki_Load(object sender, EventArgs e)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=remont";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT number_z, date_z, slom_oborud, tip, client, status, master, opisanie FROM zayavka";

                    using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(selectQuery, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            textBox1.TextChanged += TxtSearch_TextChanged;
            dataGridView1.Columns["number_z"].HeaderText = "Номер заказа";
            dataGridView1.Columns["date_z"].HeaderText = "Дата заказа";
            dataGridView1.Columns["slom_oborud"].HeaderText = "Сломанное оборудование";
            dataGridView1.Columns["tip"].HeaderText = "Тип";
            dataGridView1.Columns["client"].HeaderText = "Клиент";
            dataGridView1.Columns["status"].HeaderText = "Статус";
            dataGridView1.Columns["master"].HeaderText = "Мастер";
            dataGridView1.Columns["opisanie"].HeaderText = "Описание";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=remont";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT number_z, date_z, slom_oborud, tip, client, status, master, opisanie FROM zayavka";

                    using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(selectQuery, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            textBox1.TextChanged += TxtSearch_TextChanged;
            dataGridView1.Columns["number_z"].HeaderText = "Номер заказа";
            dataGridView1.Columns["date_z"].HeaderText = "Дата заказа";
            dataGridView1.Columns["slom_oborud"].HeaderText = "Сломанное оборудование";
            dataGridView1.Columns["tip"].HeaderText = "Тип";
            dataGridView1.Columns["client"].HeaderText = "Клиент";
            dataGridView1.Columns["status"].HeaderText = "Статус";
            dataGridView1.Columns["master"].HeaderText = "Мастер";
            dataGridView1.Columns["opisanie"].HeaderText = "Описание";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string number_z = selectedRow.Cells["number_z"].Value.ToString();
                string date_z = selectedRow.Cells["date_z"].Value.ToString();
                string slom_oborud = selectedRow.Cells["slom_oborud"].Value.ToString();
                string tip = selectedRow.Cells["tip"].Value.ToString();
                string client = selectedRow.Cells["client"].Value.ToString();
                string status = selectedRow.Cells["status"].Value.ToString();
                string master = selectedRow.Cells["master"].Value.ToString();
                string opisanie = selectedRow.Cells["opisanie"].Value.ToString();

                redact redact = new redact(number_z, date_z, slom_oborud, tip, client, status, master, opisanie);
                redact.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Document document = new Document();
            // Добавляем страницу в документ
            Page page = document.Pages.Add();

            // Добавляем текст заголовка на страницу
            TextFragment title = new TextFragment("Статистика заявок\n\n");
            title.TextState.FontSize = 16;
            title.TextState.FontStyle = FontStyles.Bold;
            page.Paragraphs.Add(title);

            // Получаем количество выполненных заявок
            int completedCount = GetCompletedCount();
            // Добавляем информацию о выполненных заявках на страницу
            TextFragment completedText = new TextFragment($"Количество выполненных заявок: {completedCount}\n");
            page.Paragraphs.Add(completedText);

            // Получаем статистику по типам неисправностей
            var faultTypesStatistics = GetFaultTypesStatistics();
            // Добавляем статистику по типам неисправностей на страницу
            TextFragment faultTypesText = new TextFragment("Статистика по типам неисправностей:\n");
            page.Paragraphs.Add(faultTypesText);
            foreach (var statistic in faultTypesStatistics)
            {
                TextFragment statisticText = new TextFragment($"{statistic.Item1}: {statistic.Item2}\n");
                page.Paragraphs.Add(statisticText);
            }

            // Сохраняем документ в файл
            string outputPath = "statistic.pdf";
            document.Save(outputPath);
        }

        // Метод для получения количества выполненных заявок из базы данных
        static int GetCompletedCount()
        {
            int completedCount = 0;
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=remont";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT COUNT(*) FROM zayavka WHERE status = 'Выполнено'";

                    using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                    {
                        // Выполняем запрос и получаем результат
                        completedCount = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при выполнении запроса к базе данных: " + ex.Message);
            }

            return completedCount;
        }

        // Метод для получения статистики по типам неисправностей из базы данных
        static System.Collections.Generic.List<System.Tuple<string, int>> GetFaultTypesStatistics()
        {
            var statistics = new System.Collections.Generic.List<System.Tuple<string, int>>();
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=remont";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT tip, COUNT(*) AS Count FROM zayavka GROUP BY tip";

                    using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            // Читаем результаты запроса
                            while (reader.Read())
                            {
                                // Получаем тип неисправности и количество таких неисправностей
                                string faultType = reader.GetString(0);
                                int count = reader.GetInt32(1);
                                // Добавляем кортеж в список статистики
                                statistics.Add(new System.Tuple<string, int>(faultType, count));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при выполнении запроса к базе данных: " + ex.Message);
            }

            return statistics;
        }

    }
}
