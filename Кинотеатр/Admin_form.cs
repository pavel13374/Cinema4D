using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Кинотеатр
{
    public partial class Admin_form : Form
    {
        private System.Drawing.Image banner;
        Form1 form1 = new Form1();
        System.Windows.Forms.TextBox name_move = new System.Windows.Forms.TextBox();
        System.Windows.Forms.ComboBox genge = new System.Windows.Forms.ComboBox();
        System.Windows.Forms.ComboBox filmmaker = new System.Windows.Forms.ComboBox();
        System.Windows.Forms.ComboBox age_lim = new System.Windows.Forms.ComboBox();
        System.Windows.Forms.TextBox Deskript = new System.Windows.Forms.TextBox();
        System.Windows.Forms.TextBox leng = new System.Windows.Forms.TextBox();
        System.Windows.Forms.TextBox Cost = new System.Windows.Forms.TextBox();
        System.Windows.Forms.TextBox rent_per = new System.Windows.Forms.TextBox();
        DateTimePicker startdata = new DateTimePicker();
        DateTimePicker date = new DateTimePicker();
        PictureBox pictureBox = new PictureBox();
        System.Windows.Forms.ComboBox Film_name = new System.Windows.Forms.ComboBox();
        System.Windows.Forms.ComboBox seance = new System.Windows.Forms.ComboBox();
        System.Windows.Forms.ComboBox hall = new System.Windows.Forms.ComboBox();
        System.Windows.Forms.ComboBox date_seance = new System.Windows.Forms.ComboBox();
        System.Windows.Forms.ComboBox time_seance = new System.Windows.Forms.ComboBox();
        public Admin_form(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        private void Admin_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Закрыть приложение полностью?", "Предупреждение", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                System.Windows.Forms.Application.ExitThread();
            }
            else if (result == DialogResult.No)
            {
                form1.Show();
            }
        }

        private void добавитьФильмToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sql sl = new Sql();
            flowLayoutPanel1.Controls.Clear();
            genge.Font = new Font("Impact", 10);
            genge.DropDownStyle = ComboBoxStyle.DropDownList;
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();

                string query = "SELECT Name_genre FROM Genre";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string genreName = reader.GetString(0);
                        genge.Items.Add(genreName);
                    }
                }
            }
            genge.AutoSize = true;
            filmmaker.Font = new Font("Impact", 10);
            filmmaker.DropDownStyle = ComboBoxStyle.DropDownList;
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();

                string query = "SELECT * FROM Filmmaker";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string Name = reader.GetString(1);
                        string Surname = reader.GetString(2);
                        string lastname = reader.GetString(3);
                        filmmaker.Items.Add(Name + " " + Surname + " " + lastname);
                    }
                }
            }
            filmmaker.AutoSize = true;

            age_lim.Font = new Font("Impact", 10);
            age_lim.DropDownStyle = ComboBoxStyle.DropDownList;
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();

                string query = "SELECT * FROM Age_limit";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int Agelim = reader.GetInt32(1);
                        age_lim.Items.Add(Agelim);
                    }
                }
            }
            Label geng = new Label();
            geng.Font = new Font("Impact", 16);
            geng.Margin = new Padding(5);
            geng.ForeColor = Color.White;
            geng.AutoSize = true;
            geng.Text = "Выберите жанр";

            Label agelim = new Label();
            agelim.Font = new Font("Impact", 16);
            agelim.Margin = new Padding(5);
            agelim.ForeColor = Color.White;
            agelim.AutoSize = true;
            agelim.Text = "Выберите возрастное ограничение";

            Label filmmak = new Label();
            filmmak.Font = new Font("Impact", 16);
            filmmak.Margin = new Padding(5);
            filmmak.ForeColor = Color.White;
            filmmak.AutoSize = true;
            filmmak.Text = "Выберите режисера";

            Label name = new Label();
            name.Font = new Font("Impact", 16);
            name.Margin = new Padding(5);
            name.ForeColor = Color.White;
            name.AutoSize = true;
            name.Text = "Введите название фильма";

            Label leg = new Label();
            leg.Font = new Font("Impact", 16);
            leg.Margin = new Padding(5);
            leg.ForeColor = Color.White;
            leg.AutoSize = true;
            leg.Text = "Введите длинну фильма в мин.";

            leng.KeyPress += textBox_KeyPress;

            Label desk = new Label();
            desk.Font = new Font("Impact", 16);
            desk.Margin = new Padding(5);
            desk.ForeColor = Color.White;
            desk.AutoSize = true;
            desk.Text = "Введите описание";

            Label cos = new Label();
            cos.Font = new Font("Impact", 16);
            cos.Margin = new Padding(5);
            cos.ForeColor = Color.White;
            cos.AutoSize = true;
            cos.Text = "Введите бюджет";

            Cost.KeyPress += textBox_KeyPress;

            Label rent = new Label();
            rent.Font = new Font("Impact", 16);
            rent.Margin = new Padding(5);
            rent.ForeColor = Color.White;
            rent.AutoSize = true;
            rent.Text = "Введите период проката";

            Label start = new Label();
            start.Font = new Font("Impact", 16);
            start.Margin = new Padding(5);
            start.ForeColor = Color.White;
            start.AutoSize = true;
            start.Text = "Выберите дату начала проката";

            startdata.Format = DateTimePickerFormat.Short;
            startdata.Value = DateTime.Now;
            startdata.Font = new Font("Impact", 10);

            Label ban = new Label();
            ban.Font = new Font("Impact", 16);
            ban.Margin = new Padding(5);
            ban.ForeColor = Color.White;
            ban.AutoSize = true;
            ban.Text = "Выберите плакат";

            System.Windows.Forms.Button but = new System.Windows.Forms.Button();
            but.Text = "Добавить фильм";
            but.ForeColor = Color.White;
            but.Font = new Font("Impact", 14);
            but.AutoSize = true;
            but.FlatStyle = FlatStyle.Flat;
            but.Click += add_film;

            System.Windows.Forms.Button add_ban = new System.Windows.Forms.Button();
            add_ban.Text = "Выбрать банер";
            add_ban.ForeColor = Color.White;
            add_ban.Font = new Font("Impact", 14);
            add_ban.AutoSize = true;
            add_ban.FlatStyle = FlatStyle.Flat;
            add_ban.Click += add_baner;

            name_move.Font = new Font("Impact", 10);
            Deskript.Font = new Font("Impact", 10);
            leng.Font = new Font("Impact", 10);
            Cost.Font = new Font("Impact", 10);
            rent_per.Font = new Font("Impact", 10);

            name_move.AutoSize = true;
            Deskript.AutoSize = true;
            leng.AutoSize = true;
            Cost.AutoSize = true;
            rent_per.AutoSize = true;

            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;

            flowLayoutPanel1.Controls.Add(name);
            flowLayoutPanel1.Controls.Add(name_move);
            flowLayoutPanel1.Controls.Add(geng);
            flowLayoutPanel1.Controls.Add(genge);
            flowLayoutPanel1.Controls.Add(filmmak);
            flowLayoutPanel1.Controls.Add(filmmaker);
            flowLayoutPanel1.Controls.Add(agelim);
            flowLayoutPanel1.Controls.Add(age_lim);
            flowLayoutPanel1.Controls.Add(desk);
            flowLayoutPanel1.Controls.Add(Deskript);
            flowLayoutPanel1.Controls.Add(leg);
            flowLayoutPanel1.Controls.Add(leng);
            flowLayoutPanel1.Controls.Add(cos);
            flowLayoutPanel1.Controls.Add(Cost);
            flowLayoutPanel1.Controls.Add(rent);
            flowLayoutPanel1.Controls.Add(rent_per);
            flowLayoutPanel1.Controls.Add(start);
            flowLayoutPanel1.Controls.Add(startdata);
            flowLayoutPanel1.Controls.Add(ban);
            flowLayoutPanel1.Controls.Add(add_ban);
            flowLayoutPanel1.Controls.Add(pictureBox);
            flowLayoutPanel1.Controls.Add(but);

        }
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void add_film(object sender, EventArgs e)
        {
            if (name_move.Text != "" && leng.Text != "" && genge.SelectedIndex != -1 && Deskript.Text != "" && filmmaker.SelectedIndex != -1 && rent_per.Text != "")
            {
                Sql sl = new Sql();
                string insertQuery = @"INSERT INTO Movies(Name_movie, Length_movie, Genre_ID, Description_movie, Director_ID, Cost, Age_limit_ID, Rental_period, Start_data) 
                            VALUES (@Name, @Length, @GenreID, @Description, @DirectorID, @Cost, @AgeLimitID, @RentalPeriod, @StartDate)";
                try
                {

                    using (SqlConnection connection = new SqlConnection(sl.Connect()))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Name", name_move.Text);
                            command.Parameters.AddWithValue("@Length", Convert.ToInt32(leng.Text));
                            command.Parameters.AddWithValue("@GenreID", genge.SelectedIndex + 1);
                            command.Parameters.AddWithValue("@Description", Deskript.Text);
                            command.Parameters.AddWithValue("@DirectorID", filmmaker.SelectedIndex + 1);
                            command.Parameters.AddWithValue("@Cost", Convert.ToInt32(Cost.Text));
                            command.Parameters.AddWithValue("@AgeLimitID", age_lim.SelectedIndex + 1);
                            command.Parameters.AddWithValue("@RentalPeriod", Convert.ToInt32(rent_per.Text));
                            command.Parameters.AddWithValue("@StartDate", startdata.Value);

                            command.ExecuteNonQuery();
                        }
                        string movieName = name_move.Text;
                        int movieId = 0;

                        string selectMovieQuery = $"SELECT ID_movie FROM Movies WHERE Name_movie = @MovieName";

                        using (SqlCommand selectMovieCommand = new SqlCommand(selectMovieQuery, connection))
                        {
                            selectMovieCommand.Parameters.AddWithValue("@MovieName", movieName);
                            var result = selectMovieCommand.ExecuteScalar();

                            if (result != null)
                            {
                                movieId = Convert.ToInt32(result);
                            }
                        }
                        string insertBannerQuery = @"INSERT INTO Banners (ID_Movies, banner)
                            VALUES (@MovieId, @ImageData)";
                        byte[] imageData;
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            banner.Save(memoryStream, banner.RawFormat);
                            imageData = memoryStream.ToArray();
                        }
                        using (SqlCommand insertBannerCommand = new SqlCommand(insertBannerQuery, connection))
                        {
                            insertBannerCommand.Parameters.AddWithValue("@MovieId", movieId);
                            insertBannerCommand.Parameters.AddWithValue("@ImageData", imageData);
                            insertBannerCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else { MessageBox.Show("Вызаполнили не все поля"); }
        }
        private void add_baner(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                banner = System.Drawing.Image.FromFile(selectedFilePath);

                pictureBox.Image = System.Drawing.Image.FromFile(selectedFilePath);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void добавитьСеансыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            Sql sl = new Sql();
            Film_name.Items.Clear();
            string query = "SELECT Name_movie FROM Movies";
            Film_name.Font = new Font("Impact", 10);
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string movieName = reader["Name_movie"].ToString();
                    Film_name.Items.Add(movieName);
                }

                reader.Close();
            }
            Label fil_n = new Label();
            fil_n.Font = new Font("Impact", 16);
            fil_n.Margin = new Padding(5);
            fil_n.ForeColor = Color.White;
            fil_n.AutoSize = true;
            fil_n.Text = "Выберите название фильма";

            date.Format = DateTimePickerFormat.Short;
            date.Value = DateTime.Now;
            date.Font = new Font("Impact", 10);

            Label date_fil = new Label();
            date_fil.Font = new Font("Impact", 16);
            date_fil.Margin = new Padding(5);
            date_fil.ForeColor = Color.White;
            date_fil.AutoSize = true;
            date_fil.Text = "Выберите дату фильма";

            string query1 = "SELECT Start_time FROM Seance";
            seance.Font = new Font("Impact", 10);
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query1, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    TimeSpan startTime = (TimeSpan)reader["Start_time"];
                    seance.Items.Add(startTime);
                }

                reader.Close();
            }
            Label time = new Label();
            time.Font = new Font("Impact", 16);
            time.Margin = new Padding(5);
            time.ForeColor = Color.White;
            time.AutoSize = true;
            time.Text = "Выберите время сеанса";

            string query2 = "SELECT hall_number FROM hall";
            hall.Font = new Font("Impact", 10);
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query2, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string hallNumber = (string)reader["hall_number"];
                    hall.Items.Add(hallNumber);
                }

                reader.Close();
            }
            Label hallw = new Label();
            hallw.Font = new Font("Impact", 16);
            hallw.Margin = new Padding(5);
            hallw.ForeColor = Color.White;
            hallw.AutoSize = true;
            hallw.Text = "Выберите зал";

            System.Windows.Forms.Button but = new System.Windows.Forms.Button();
            but.Text = "Добавить сеанс";
            but.ForeColor = Color.White;
            but.Font = new Font("Impact", 14);
            but.AutoSize = true;
            but.FlatStyle = FlatStyle.Flat;
            but.Click += add_seance;

            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;

            flowLayoutPanel1.Controls.Add(fil_n);
            flowLayoutPanel1.Controls.Add(Film_name);
            flowLayoutPanel1.Controls.Add(date_fil);
            flowLayoutPanel1.Controls.Add(date);
            flowLayoutPanel1.Controls.Add(time);
            flowLayoutPanel1.Controls.Add(seance);
            flowLayoutPanel1.Controls.Add(hallw);
            flowLayoutPanel1.Controls.Add(hall);
            flowLayoutPanel1.Controls.Add(but);


        }
        private void add_seance(object sender, EventArgs e)
        {
            Sql sl = new Sql();
            string query = "INSERT INTO Timetable(show_date, film_id, seance_id, hall_id) VALUES (@showDate, @filmId, @seanceId, @hallId)";

            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@showDate", date.Value);
                command.Parameters.AddWithValue("@filmId", Film_name.SelectedIndex + 1);
                command.Parameters.AddWithValue("@seanceId", seance.SelectedIndex + 1);
                command.Parameters.AddWithValue("@hallId", hall.SelectedIndex + 1);

                int rowsAffected = command.ExecuteNonQuery();
            }
        }

        private void удалитьФильмToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Sql sl = new Sql();
            Film_name.Items.Clear();
            flowLayoutPanel1.Controls.Clear();
            string query = "SELECT Name_movie FROM Movies";
            Film_name.Font = new Font("Impact", 10);
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string movieName = reader["Name_movie"].ToString();
                    Film_name.Items.Add(movieName);
                }

                reader.Close();
            }
            Film_name.Size = new Size(200,20);
            Label fil_n = new Label();
            fil_n.Font = new Font("Impact", 16);
            fil_n.Margin = new Padding(5);
            fil_n.ForeColor = Color.White;
            fil_n.AutoSize = true;
            fil_n.Text = "Выберите название фильма";

            System.Windows.Forms.Button del_but = new System.Windows.Forms.Button();
            del_but.Text = "Удалить сеанс";
            del_but.ForeColor = Color.White;
            del_but.Font = new Font("Impact", 14);
            del_but.AutoSize = true;
            del_but.FlatStyle = FlatStyle.Flat;
            del_but.Click += del_film;

            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;

            flowLayoutPanel1.Controls.Add(fil_n);
            flowLayoutPanel1.Controls.Add(Film_name);
            flowLayoutPanel1.Controls.Add(del_but);
        }
        private void del_film(object sender, EventArgs e)
        {
            int movieId = Film_name.SelectedIndex+1;
            Sql sl = new Sql();
            try 
            { 
                using (SqlConnection connection = new SqlConnection(sl.Connect()))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Banners WHERE ID_Movies = @MovieId";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@MovieId", movieId);
                    deleteCommand.ExecuteNonQuery();

                    string query = "DELETE FROM Timetable WHERE film_id = @FilmId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FilmId", movieId);

                        int rowsAffected = command.ExecuteNonQuery();
                    }
                    
                    // Удаление фильма из таблицы Movies
                    string deleteMovieQuery = $"DELETE FROM Movies WHERE ID_movie = {movieId}";
                    SqlCommand deleteMovieCommand = new SqlCommand(deleteMovieQuery, connection);
                    deleteMovieCommand.ExecuteNonQuery();
                    string quer = $"DBCC CHECKIDENT('Movies', RESEED, {movieId - 1})";
                    SqlCommand deleteMovieCommand1 = new SqlCommand(quer, connection);
                    deleteMovieCommand1.ExecuteNonQuery();
                    MessageBox.Show("Фильм успешно удален");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void удалитьСеансToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sql sl = new Sql();
            Film_name.Items.Clear();
            flowLayoutPanel1.Controls.Clear();
            string query = "SELECT Name_movie FROM Movies";
            Film_name.Font = new Font("Impact", 10);
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string movieName = reader["Name_movie"].ToString();
                    Film_name.Items.Add(movieName);
                }

                reader.Close();
            }
            Film_name.Size = new Size(200, 20);
            Film_name.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            flowLayoutPanel1.Controls.Add( Film_name );
        }
        int movieId;
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedItem = Film_name.SelectedIndex;
            movieId = selectedItem+1;
            Sql sl = new Sql();
            date_seance.Font = new Font("Impact", 10);
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();

                string query = "SELECT DISTINCT show_date FROM Timetable WHERE film_id = @MovieId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MovieId", selectedItem);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime date = reader.GetDateTime(0);
                            date_seance.Items.Add(date.ToString("dd.MM.yyyy"));
                        }
                    }
                }
            }
            date_seance.SelectedIndexChanged += ComboBox2_SelectedIndexChanged;
            flowLayoutPanel1.Controls.Add(date_seance);
        }
        List<string> list = new List<string>();
        string dates;
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            list.Clear();
            Sql sl = new Sql();
            DateTime selectedDate = Convert.ToDateTime(date_seance.SelectedItem); // Замените на вашу выбранную дату
            string date = selectedDate.ToString("dd.MM.yyyy");
            dates = date;
            MessageBox.Show(date);
            
            time_seance.Font = new Font("Impact", 10);
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();

                string query = "SELECT seance_id FROM Timetable WHERE film_id = @MovieId AND show_date = @SelectedDate";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MovieId", movieId);
                    command.Parameters.AddWithValue("@SelectedDate", date);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int sean = reader.GetInt32(0);
                            poluch_seance(sean);
                        }
                    }
                }


            }
            foreach (var lis in list)
            {
                time_seance.Items.Add(lis);
            }
            time_seance.SelectedIndexChanged += ComboBox3_SelectedIndexChanged;
            flowLayoutPanel1.Controls.Add(time_seance);
        }
        private void poluch_seance(int id_sean)
        {
            Sql sl = new Sql();
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();
                string quer = $"SELECT Start_time FROM Seance WHERE ID_seance = {id_sean}";
                using (SqlCommand command1 = new SqlCommand(quer, connection))
                {
                    using (SqlDataReader reader1 = command1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            list.Add(reader1.GetTimeSpan(0).ToString());
                        }
                    }
                }
            }
        }
        int times;
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            times = time_seance.SelectedIndex+1;
            System.Windows.Forms.Button but = new System.Windows.Forms.Button();
            but.Text = "Удалить сеанс";
            but.ForeColor = Color.White;
            but.Font = new Font("Impact", 14);
            but.AutoSize = true;
            but.FlatStyle = FlatStyle.Flat;
            but.Click += del_seance;
            flowLayoutPanel1 .Controls.Add(but);
        }
        private void del_seance(object sender, EventArgs e)
        {
            MessageBox.Show(dates+movieId.ToString()+ times.ToString());
            Sql sl = new Sql();
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();

                string query = "DELETE FROM Timetable WHERE show_date = @ShowDate AND film_id = @FilmId AND seance_id = @SeanceId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ShowDate", dates);
                    command.Parameters.AddWithValue("@FilmId", movieId);
                    command.Parameters.AddWithValue("@SeanceId", times);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Запись успешно удалена.");
                    }
                    else
                    {
                        MessageBox.Show("Запись не найдена.");
                    }
                }
            }
        }
    }
}
