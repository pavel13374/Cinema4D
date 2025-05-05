using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Кинотеатр
{
    public partial class Zab_passw : Form
    {
        private Form1 form1;
        private int rnd;
        private string email;
        private bool flag = false;
        public Zab_passw(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
            label2.Visible = false;
            textBox2.Visible = false;
            button1.Visible = false;
            button3.Visible = false;
            pictureBox1 .Visible = false;
            pictureBox2 .Visible = false;

        }
        private void Zab_passw_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flag)
            {
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                email = textBox1.Text;
                Sql sl = new Sql();
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
                    if (dataTable.Rows.Count > 0)
                    {
                        Random rn = new Random();
                        rnd = rn.Next(111111, 999999);
                        Mail mal = new Mail();
                        mal.SendMessage(dataTable.Rows[0]["user_login"].ToString(), email, "Код подтверждения", "Код подтверждения: " + rnd.ToString());
                        MessageBox.Show("Проверте почту \nЕсли вы не увидите сообщения с кодом проверте папку 'Спам'", "У вас новое сообщение");
                        label2.Visible = true;
                        textBox2.Visible = true;
                        button3.Visible = true;
                        button2.Visible = false;
                    }
                    else MessageBox.Show("Пользователя с такой почтой не найденно,проверте корректность введенных данных");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else { MessageBox.Show("Введите почту"); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {

                if (textBox2.Text == rnd.ToString())
                {
                    MessageBox.Show("Почта подтверждена");
                    label1.Text = "Введите новый пароль";
                    label2.Text = "Подтвердите пароль";
                    button1.Visible = true;
                    button3.Visible = false;
                    textBox1.UseSystemPasswordChar = true;
                    textBox2.UseSystemPasswordChar = true;
                    pictureBox1.Visible = true;
                    textBox1.Clear();
                    textBox2.Clear();
                }
                else
                {
                    MessageBox.Show("Неверный код");
                }
            }
            else { MessageBox.Show("Введите код подтверждения"); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (textBox2.Text != "")
                {

                    if (textBox1.Text == textBox2.Text)
                    {
                        Sql sl = new Sql();
                        Hash hash = new Hash();
                        string userEmail = email;
                        string newPassword = hash.SHA(textBox1.Text);


                        string query = "UPDATE users SET user_password = @NewPassword WHERE user_mail = @UserEmail";


                        using (SqlConnection connection = new SqlConnection(sl.Connect()))
                        {
                            SqlCommand command = new SqlCommand(query, connection);

                            command.Parameters.AddWithValue("@NewPassword", newPassword);
                            command.Parameters.AddWithValue("@UserEmail", userEmail);

                            try
                            {

                                connection.Open();

                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Пароль пользователя успешно изменен.");
                                    flag = true;
                                    this.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                            }
                        }
                    }
                    else
                        MessageBox.Show("Пароли не совпадают");
                }
                else { MessageBox.Show("Введите подтверждение пароля"); }
            }
            else { MessageBox.Show("Введите пароль"); }
        }

        private void Zab_passw_Load(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = false;
            textBox1.UseSystemPasswordChar = false;
            pictureBox2.Visible = true;
            pictureBox1.Visible = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            textBox1.UseSystemPasswordChar = true;
            pictureBox2.Visible = false;
            pictureBox1.Visible = true;
        }
    }
}
