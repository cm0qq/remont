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
    public partial class auth : Form
    {
        public auth()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=remont";
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT COUNT(*) FROM users WHERE login = @login AND pass = @pass";

                    using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@pass", password);

                        // Выполнение запроса и получение результата (количество строк)
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // Проверка результата
                        if (count > 0)
                        {
                            // Если пользователь с указанным логином и паролем найден

                            // Определяем какая форма должна быть открыта
                            if (login == "manager")
                            {
                                // Открываем форму для пользователя user1
                                Form1 form = new Form1();
                                form.Show();
                            }
                            else if (login == "user")
                            {
                                // Открываем форму для пользователя user2
                                uzayav uzayav = new uzayav();
                                uzayav.Show();
                                this.Hide();
                            }
                            // Добавьте другие варианты для других пользователей, если необходимо
                        }
                        else
                        {
                            // Если пользователь не найден
                            MessageBox.Show("Неверный логин или пароль. Пожалуйста, попробуйте снова.");
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
