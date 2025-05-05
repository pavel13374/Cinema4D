using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Кинотеатр
{
    public partial class New_zal : Form
    {
        public string Date { get; set; }
        public string Seance { get; set; }
        public string Hall { get; set; }
        public string Film { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        private Panel hallPanel = new Panel();  
        private int seatButtonSize = 40;

        public New_zal(string date, string seance, string hall,string film,string login,string email)
        {
            InitializeComponent();
            Date = date;
            Seance = seance;
            Hall = hall;
            Film = film;
            Login = login;
            Email = email;
            this.Text = hall;
            Seats();
            Controls.Add(hallPanel);
            this.AutoSize = true;
        }
        public void Seats()
        {
            string hallNumber = Hall; 
            Sql sl = new Sql();
            string query = "SELECT s.nomer_row, s.nomer_seat FROM Seats s JOIN hall h ON s.hall_id = h.ID_hall WHERE h.hall_number = @HallNumber";

            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@HallNumber", hallNumber);

                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    Dictionary<int, int> seats = new Dictionary<int, int>();
                    int row = 0;
                    int seat = 0;
                    int rowNumber;
                    int seatNumber;
                    int max_rows = Max_rows();
                    int max_seat = Max_seat();
                    while (reader.Read())
                    {
                        rowNumber = reader.GetInt32(0);
                        seatNumber = reader.GetInt32(1);

                        if (seatNumber == 1 && seat == 0)
                        {
                            row = rowNumber;
                            seat = seatNumber;
                        }
                        else if (seatNumber == 1 && seat != 0)
                        {
                            seats.Add(row, seat);
                            row = rowNumber;
                            seat = seatNumber;
                        }
                        else if (row == max_rows && seatNumber == max_seat)
                        { 
                            seats.Add(row, seatNumber);
                        }
                        else
                        {
                            seat = seatNumber;
                        }
                    }

                    reader.Close();
                    CreateSeatButtons(seats);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                }
            }

        }

        private void CreateSeatButtons(Dictionary<int, int> seat)
        {
            hallPanel.AutoSize = true;
            hallPanel.Location = new System.Drawing.Point(70, 60);
            int timetableid = GetTimeId();
            List<int> id_seats = In_buy_row_seat(timetableid);
            List<int> id_seats1 = In_booking_row_seat(timetableid);  
            int hall_id = GetHallIdByName(Hall);
            foreach (var set in seat.Keys)
            {

                for (int i = 0; i <= seat[set]; i++)
                {
                    Button button = new Button();
                    button.Tag = $"{set}";
                    button.Text = (i).ToString();
                    int id_seat = GetSeats(set, i);
                    bool cont = id_seats.Contains(id_seat);
                    bool cont1 = id_seats1.Contains(id_seat);
                    if (cont)
                    {
                        button.Enabled = false;
                        button.BackColor = Color.Gray;
                    }
                    else if (cont1)
                    {
                        button.Enabled = false;
                        button.BackColor = Color.Red;
                    }
                    else
                    {
                        button.BackColor = Color.White;
                    }
                    button.Size = new Size(30, 30);
                    button.Click += SeatButton_Click;
                    button.Location = new Point((i - 1) * seatButtonSize, (set - 1) * seatButtonSize);
                    hallPanel.Controls.Add(button);
                }

            }


        }
        private void SeatButton_Click(object sender, EventArgs e)
        {
            Button seatButton = (Button)sender;
            string row = seatButton.Tag.ToString();
            string seats = seatButton.Text;
            Sql sl = new Sql();
            Mail mail = new Mail();
            DialogResult result = MessageBox.Show($"Вы хотите купить билет на {Film}?", "Осведомление", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {

                try 
                {

                    MessageBox.Show("Отлично,билет будет отправлен вам на почту");
                    int timetableid = GetTimeId();
                    int idseats = GetSeats(Convert.ToInt32(row), Convert.ToInt32(seats));
                    string query = @"INSERT INTO Buy (timetable_id, seat_id, method_pay)
                 VALUES (@TimetableID, @SeatID, @MethodPay)";

                    using (SqlConnection connection = new SqlConnection(sl.Connect()))
                    {
                        SqlCommand command = new SqlCommand(query, connection);

                        // Замените значения параметров на фактические данные
                        command.Parameters.AddWithValue("@TimetableID", timetableid);
                        command.Parameters.AddWithValue("@SeatID", idseats);
                        command.Parameters.AddWithValue("@MethodPay", "Карта");

                        try
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                        }
                    }

                    seatButton.Enabled = false;
                    seatButton.BackColor = Color.Gray;
                    mail.SendMessage(Login, Email, "Билет на фильм", $"Вы купили билет на фильм {Film} \nСеанс будет {Date} в {Seance}\nВаше место {seats},ряд {row}");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

            }
            else
            {
                DialogResult result1 = MessageBox.Show($"Выхотите забранировать билет?", "Осведомление", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    int timetableid = GetTimeId();
                    int idseats = GetSeats(Convert.ToInt32(row), Convert.ToInt32(seats));
                    string query = @"INSERT INTO Booking (timetable_id, seat_id, order_time)
                 VALUES (@TimetableID, @SeatID, @MethodPay)";

                    using (SqlConnection connection = new SqlConnection(sl.Connect()))
                    {
                        SqlCommand command = new SqlCommand(query, connection);

                        // Замените значения параметров на фактические данные
                        command.Parameters.AddWithValue("@TimetableID", timetableid);
                        command.Parameters.AddWithValue("@SeatID", idseats);
                        command.Parameters.AddWithValue("@MethodPay", Seance);

                        try
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                        }
                    }

                    seatButton.Enabled = false;
                    seatButton.BackColor = Color.Red;
                }
                else
                {
                    MessageBox.Show("Жалко,но ладно");
                }
            }
        }
        private int Max_rows()
        {
            Sql sl = new Sql();
            string query = "SELECT kolvo_rows FROM hall WHERE hall_number = @HallNumber";

            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@HallNumber", Hall);

                try
                {
                    connection.Open();

                    object result = command.ExecuteScalar();

                    int kolvoRows = Convert.ToInt32(result);
                    return (kolvoRows);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                    return (0);
                }
            }
        }
        private int Max_seat()
        {
            Sql sl = new Sql();
            string hallNumber = Hall; 

            string query = @"SELECT COUNT(nomer_seat) AS TotalSeats
                FROM Seats
                WHERE hall_id = (SELECT ID_hall FROM hall WHERE hall_number = @HallNumber)
                      AND nomer_row = (SELECT MAX(nomer_row) FROM Seats WHERE hall_id = (SELECT ID_hall FROM hall WHERE hall_number = @HallNumber))";

            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@HallNumber", hallNumber);

                try
                {
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int totalSeats = Convert.ToInt32(result);
                        return totalSeats;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                    return 0;
                }
            }
        }
        private object ExecuteScalar(string query)
        {

            Sql sl = new Sql();
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                return command.ExecuteScalar();
            }

        }

        // Получение film_id по названию фильма
        private int GetFilmIdByName(string filmName)
        {
            string query = $"SELECT ID_movie FROM Movies WHERE Name_movie = '{filmName}'";
            return Convert.ToInt32(ExecuteScalar(query));
        }

        // Получение seance_id по времени сеанса
        private int GetSeanceIdByTime(string startTime)
        {
            string query = $"SELECT ID_seance FROM Seance WHERE Start_time = '{startTime}'";
            return Convert.ToInt32(ExecuteScalar(query));
        }

        // Получение hall_id по названию зала
        private int GetHallIdByName(string hallName)
        {
            string query = $"SELECT ID_hall FROM hall WHERE hall_number = '{hallName}'";
            return Convert.ToInt32(ExecuteScalar(query));
        }
       private int GetTimeId()
       {
            int filmId = GetFilmIdByName(Film);
            int seanceId = GetSeanceIdByTime(Seance);
            int hallId = GetHallIdByName(Hall);
            DateTime date = Convert.ToDateTime(Date);
            string query = "SELECT ID_timetable FROM Timetable WHERE show_date = @showDate AND film_id = @filmId AND seance_id = @seanceId AND hall_id = @hallId";
            Sql sl = new Sql();
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@showDate", date.Date);
                command.Parameters.AddWithValue("@filmId", filmId);
                command.Parameters.AddWithValue("@seanceId", seanceId);
                command.Parameters.AddWithValue("@hallId", hallId);
                return Convert.ToInt32(command.ExecuteScalar()); 

            }


        }
        private int GetSeats(int row,int seat)
        {
            int hallId = GetHallIdByName(Hall);
            string query = $"SELECT ID_seat FROM Seats WHERE hall_id = {hallId} AND nomer_row = {row} AND nomer_seat = {seat}";
            return Convert.ToInt32(ExecuteScalar(query));
        }
        private List<int> In_buy_row_seat(int timeId)
        {
            string quer = $"SELECT seat_id FROM Buy WHERE timetable_id = {timeId}";
            List<int>id_seats = new List<int>();
            Sql sl = new Sql();
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(quer, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) 
                {
                    int id_seat = reader.GetInt32(0);
                    id_seats.Add(id_seat);
                }
                reader.Close();
            }
            return id_seats;
        }
        private List<int> In_booking_row_seat(int timeId)
        {
            string quer = $"SELECT seat_id FROM Booking WHERE timetable_id = {timeId}";
            List<int> id_seats = new List<int>();
            Sql sl = new Sql();
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(quer, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id_seat = reader.GetInt32(0);
                    id_seats.Add(id_seat);
                }
                reader.Close();
            }
            return id_seats;
        }

    } 
}
