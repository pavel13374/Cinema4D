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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Кинотеатр
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
            pictureBox2.Visible = false;
            textBox1.Clear();
            textBox2.Clear();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = false;
            pictureBox2.Visible = true;
            pictureBox1.Visible = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            pictureBox2.Visible = false;
            pictureBox1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sql sl = new Sql();
            Hash h = new Hash();
            string email = textBox1.Text;
            string pas = h.SHA(textBox2.Text).ToLower();
            if (textBox1.Text.Length > 0)
            {
                if (textBox2.Text.Length > 0)
                {
                    try
                    {
                        DataTable dataTable = new DataTable("dataBase");
                        using (SqlConnection sqlConnection = new SqlConnection(sl.Connect()))
                        {

                            sqlConnection.Open();
                            string selectSQL = "SELECT * FROM [dbo].[users] WHERE [user_mail] = @email AND [user_password] = @password";
                            using (SqlCommand sqlCommand = new SqlCommand(selectSQL, sqlConnection))
                            {
                                sqlCommand.Parameters.AddWithValue("@email", email);
                                sqlCommand.Parameters.AddWithValue("@password", pas);

                                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                                {
                                    sqlDataAdapter.Fill(dataTable);
                                }
                            }


                        }
                        if (dataTable.Rows.Count > 0)
                        {
                            using (SqlConnection connection = new SqlConnection(sl.Connect()))
                            {
                                connection.Open();

                                string query = @"SELECT user_login,id_role
                                 FROM users
                                 WHERE user_mail = @email";

                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@email", email);

                                    using (SqlDataReader reader = command.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            string login = reader.GetString(0);
                                            int role = reader.GetInt32(1);
                                            if (role == 2)
                                            {
                                                User_form us = new User_form(this);
                                                us.Login = login;
                                                us.Email = email;
                                                us.Show();
                                                this.Hide();
                                            }
                                            else if (role == 1)
                                            {
                                                Admin_form admin = new Admin_form(this);
                                                admin.Show();
                                                this.Hide();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else MessageBox.Show("Неверный логин или пароль"); textBox2.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else MessageBox.Show("Введите пароль");
            }
            else MessageBox.Show("Введите логин");
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Zab_passw zp = new Zab_passw(this);
            zp.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Register reg = new Register(this);
            reg.Show();
            this.Hide();
        }
    }
}
