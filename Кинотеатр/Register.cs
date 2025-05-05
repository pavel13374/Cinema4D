using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Кинотеатр
{
    public partial class Register : Form
    {
        private Form1 form1;
        private bool flag = false;
        private int rnd;
        public Register(Form1 form)
        {
            InitializeComponent();
            textBox3.UseSystemPasswordChar = true;
            pictureBox2.Visible = false;
            this.form1 = form;
            button2.Enabled = false;
            textBox4.Visible = false;
            label3.Visible = false;
        }

        private void Register_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flag)
            {
                this.Close();
                form1.Show();
            }
            else
            {
                DialogResult result = MessageBox.Show("Закрыть приложение полностью?", "Предупреждение", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Application.ExitThread();
                }
                else if (result == DialogResult.No)
                {
                    form1.Show();
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox3.UseSystemPasswordChar = true;
            pictureBox2.Visible = false;
            pictureBox1.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox3.UseSystemPasswordChar = false;
            pictureBox2.Visible = true;
            pictureBox1.Visible = false;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sql sl = new Sql(); 
            string email = textBox2.Text;
            DataTable dataTable = new DataTable("dataBase");
            try
            {
                    using (SqlConnection sqlConnection = new SqlConnection(sl.Connect()))
                    {

                        sqlConnection.Open();
                        string selectSQL = "SELECT * FROM [dbo].[users] WHERE [user_mail] = @email";
                        using (SqlCommand sqlCommand = new SqlCommand(selectSQL, sqlConnection))
                        {
                            sqlCommand.Parameters.AddWithValue("@email", email);

                            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                            {
                                sqlDataAdapter.Fill(dataTable);
                            }
                        }
                    }
                if (dataTable.Rows.Count == 0)
                {
                    Random rn = new Random();
                    rnd = rn.Next(111111, 999999);
                    Mail mal = new Mail();
                    mal.SendMessage(textBox1.Text.ToString(), email, "Код подтверждения", "Код подтверждения: " + rnd.ToString());
                    MessageBox.Show("Проверте почту \nЕсли вы не увидите сообщения с кодом проверте папку 'Спам'", "У вас новое сообщение");
                    textBox4.Visible = true;
                    label3.Visible = true;
                    button1.Visible = false;
                    button2.Enabled = true;
                }
                else { MessageBox.Show("Пользователь с такой почтой уже есть"); }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sql sl = new Sql();
            Hash hash = new Hash();
            if (textBox4.Text == rnd.ToString())
            {
                if (textBox1.Text != "")
                {
                    if (textBox4.Text != "")
                    {
                        if (textBox2.Text != "")
                        {
                            string userLogin = textBox1.Text;
                            string userPassword = hash.SHA(textBox3.Text);
                            string userMail = textBox2.Text;
                            int roleId = 2;

                            string query = "INSERT INTO users (user_login, user_password, user_mail, id_role) " +
                                           "VALUES (@UserLogin, @UserPassword, @UserMail, @RoleId)";

                            using (SqlConnection connection = new SqlConnection(sl.Connect()))
                            {
                                SqlCommand command = new SqlCommand(query, connection);

                                command.Parameters.AddWithValue("@UserLogin", userLogin);
                                command.Parameters.AddWithValue("@UserPassword", userPassword);
                                command.Parameters.AddWithValue("@UserMail", userMail);
                                command.Parameters.AddWithValue("@RoleId", roleId);

                                try
                                {

                                    connection.Open();

                                    int rowsAffected = command.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        MessageBox.Show("Пользователь зарегестрирован");
                                        
                                        this.Close();
                                    }

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                                }
                            }
                        }
                        else { MessageBox.Show("Введите пароль"); }
                    }
                    else { MessageBox.Show("Введите код подтверждения"); }
                }
                else { MessageBox.Show("Заполните поле 'Логин'"); }
            }
            else
            {
                MessageBox.Show("Неверный код подтверждения");
            }
        }
    }
}
