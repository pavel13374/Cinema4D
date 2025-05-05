using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Кинотеатр
{
    public partial class User_form : Form
    {
        Form1 form1 = new Form1();
        private TableLayoutPanel tableLayoutPanel;
        public string Login { get; set; }
        public string Email { get; set; }
        private bool flag = false;
        public User_form(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;

        }

        private void User_form_FormClosing(object sender, FormClosingEventArgs e)
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

        private void label2_Click(object sender, EventArgs e)
        {
            flag = true;
            this.Close();
            form1.Show();
        }

        private void User_form_Load(object sender, EventArgs e)
        {
            List<DateTime> list = new List<DateTime>();
            for (int i = 0; i < 4; i++)
            {
                list.Add(DateTime.Now.AddDays(i));
            }

            // Создание TableLayoutPanel
            tableLayoutPanel = new TableLayoutPanel(); // изменить эту строку
            tableLayoutPanel.Size = new Size(this.Width, 120);
            tableLayoutPanel.Margin = new Padding(20);
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.ColumnCount = 4;
            //tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.BackColor = Color.Transparent;
            FlowLayoutPanel fl = new FlowLayoutPanel();
            fl.AutoSize = true;
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Font = new Font("Impact", 28);
            lbl.Text = "EarthCinema";
            lbl.ForeColor = Color.White;
            fl.Controls.Add(lbl);
            fl.Margin = new Padding(10);
            tableLayoutPanel.Controls.Add(fl,0,0);
            // Создание 4 FlowLayoutPanel и добавление их в TableLayoutPanel
            for (int i = 0; i < 4; i++)
            {
                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();

                flowLayoutPanel.Size = new System.Drawing.Size(250, 150);

                // Добавление Panel на FlowLayoutPanel
                Panel panel = new Panel();
                panel.BackColor = flowLayoutPanel.BackColor = Color.Transparent;
                panel.Size = new Size(200, 50);
                flowLayoutPanel.Controls.Add(panel);

                // Добавление кнопки на Panel
                System.Windows.Forms.Button button = new System.Windows.Forms.Button();
                button.Name = $"button_{i}";
                button.Click += new EventHandler(Movies); // Один метод-обработчик для всех кнопок
                button.Text = list[i].ToString("dd.MM");
                button.Tag = list[i].ToString("dd.MM.yyyy"); // Сохранение значения даты в свойстве Tag кнопки
                button.Size = new Size(200, 50);
                button.Location = new Point((panel.Width - button.Width) / 2, (panel.Height - button.Height) / 2);
                button.Font = new Font("Impact", 20);
                button.BackColor = Color.SlateBlue;
                button.ForeColor = Color.White;
                panel.Controls.Add(button);

                // Добавление FlowLayoutPanel в TableLayoutPanel
                tableLayoutPanel.Controls.Add(flowLayoutPanel, i, 1);

            }
            // Добавление TableLayoutPanel на форму
            this.Controls.Add(tableLayoutPanel);
        }
        private void Movies(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;
            button.BackColor = Color.Blue;
            DateTime date = Convert.ToDateTime(button.Tag);
            // Меняем цвета кнопки, если не она нажата 
            foreach (Control control in tableLayoutPanel.Controls)
                // Если это кнопка и она не является нажатой кнопкой
                if (control is FlowLayoutPanel)
                    foreach (Control control2 in control.Controls)
                        if (control2 is Panel)
                            foreach (Control control3 in control2.Controls)
                                if (control3 is System.Windows.Forms.Button && control3 != button)
                                    control3.BackColor = Color.SlateBlue;


            // Работаем с памятью, чистим всё что движется и не движется
            List<Control> controlsToDelete = new List<Control>();

            foreach (Control control in Controls)
                if (control != tableLayoutPanel)
                    controlsToDelete.Add(control);


            foreach (Control control in controlsToDelete)
                Controls.Remove(control);

            // Заполняем наши фильмецы, для нужного дня, передавая параметр date, как параметр для выборки
            Seanc.getNewSeances(date);


            // Для расстояния между FlowLayoutPanel, которые содержат описание и т.п. фильмов.
            int temp = 150;
            Sql sl = new Sql();
            foreach (KeyValuePair<(string, string), List<Tuple<TimeSpan, string>>> kvp in Seanc.dict)
            {
               
                string movieName = kvp.Key.Item1;
                string movieDescription = kvp.Key.Item2;
                Image path_to_jpeg;
                byte[] bannerBytes;
                List<TimeSpan> timeSpans = kvp.Value.Select(t => t.Item1).ToList();
                List<string> halls = kvp.Value.Select(t => t.Item2).ToList();


                // Подключение к базе данных и создание объекта подключения
                using (SqlConnection connection = new SqlConnection(sl.Connect()))
                {
                    connection.Open();

                    // Создание команды SQL
                    string query = @"SELECT banner
                     FROM Banners
                     JOIN Movies ON Banners.ID_Movies = Movies.ID_movie
                     WHERE Movies.Name_movie = @MovieName";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MovieName", movieName);

                    // Выполнение запроса и чтение результатов
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Получение байтового массива из поля banner
                            bannerBytes = (byte[])reader["banner"];
                            using (MemoryStream stream = new MemoryStream(bannerBytes))
                            {
                                path_to_jpeg = Image.FromStream(stream);



                                addFilms(date.ToString("yyyy-MM-dd"), path_to_jpeg, temp, movieName, movieDescription, timeSpans, halls);
                            }

                        }
                    }

                }





                temp += 220;
                timeSpans.Clear();
                halls.Clear();
            }
        }
        public void addFilms(string date, Image path_image, int temp, string movieName, string description, List<TimeSpan> seances, List<string> halls)
        {
            FlowLayoutPanel pan = new FlowLayoutPanel();
            pan.FlowDirection = FlowDirection.TopDown;
            pan.BackColor = Color.SlateBlue;
            pan.BorderStyle = BorderStyle.Fixed3D;
            pan.Location = new Point(0, temp);
            pan.Size = new Size(this.Width, 180);

            PictureBox pictureBox = new PictureBox();


            pictureBox.Image = path_image;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Size = new Size(321, 197);
            pan.Controls.Add(pictureBox);

            System.Windows.Forms.Label nazvanie_filma = new System.Windows.Forms.Label();
            nazvanie_filma.Text = movieName.ToString();
            nazvanie_filma.Location = new Point(325, 197);
            nazvanie_filma.Font = new Font("Impact", 18);
            nazvanie_filma.ForeColor = Color.White;
            nazvanie_filma.Size = new Size(500, 25);
            pan.Controls.Add(nazvanie_filma);

            System.Windows.Forms.Label label_probel = new System.Windows.Forms.Label();
            label_probel.Text = "\n";
            pan.Controls.Add((label_probel));

            System.Windows.Forms.Label film_description = new System.Windows.Forms.Label();
            film_description.Text = description.ToString();
            film_description.Font = new Font("Impact", 12);
            film_description.Size = new Size(640, 50);
            film_description.ForeColor = Color.White;
            pan.Controls.Add(film_description);



            FlowLayoutPanel panel_for_seances = new FlowLayoutPanel();
            panel_for_seances.Size = new Size(this.Width, 60);
            panel_for_seances.BackColor = Color.SlateBlue;
            panel_for_seances.FlowDirection = FlowDirection.LeftToRight;
            DateTime currentTime = DateTime.Now;
            TimeSpan currentTimeOfDay = currentTime.TimeOfDay;
            for (int i = 0; i < seances.Count; i++)
            {
                System.Windows.Forms.Button seance_but = new System.Windows.Forms.Button();
                seance_but.Text = seances[i].ToString();
                seance_but.Tag = Tuple.Create(date.ToString(), movieName.ToString(), halls[i].ToString());
                seance_but.Font = new Font("Impact", 16);
                seance_but.AutoSize = true;
                seance_but.ForeColor = Color.White;
                seance_but.Margin = new Padding(0, 20, 0, 0);
                if (seances[i] <= currentTimeOfDay && date == currentTime.ToString("yyyy-MM-dd"))
                {
                   seance_but.Enabled = false;
                }
                seance_but.Click += new EventHandler(CreateHall);
                panel_for_seances.Controls.Add(seance_but);
            }
            pan.Controls.Add(panel_for_seances);

            Controls.Add(pan);
        }
        private void CreateHall(object sender, EventArgs e)
        {
            System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;
            var tag = (Tuple<string, string, string>)button.Tag;
            string date = tag.Item1; 
            string film = tag.Item2; 
            string hall = tag.Item3;
            string seance = button.Text;

            New_zal zal = new New_zal(date, seance, hall,film,Login,Email);
            zal.Show();

        }
    }
}
