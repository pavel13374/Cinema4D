using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Кинотеатр
{
    internal class Seanc
    {
        public static Dictionary<(string, string), List<Tuple<TimeSpan, string>>> dict = new Dictionary<(string, string), List<Tuple<TimeSpan, string>>>();

        public static Dictionary<int, (int, int)> seats = new Dictionary<int, (int, int)>();


        public static void getNewSeances(DateTime day)
        {
            if (dict.Count != 0)
                dict.Clear();
            Sql sl = new Sql();
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                // Создаем объект команды для выполнения запроса
                string query = "SELECT m.Name_movie,m.Description_movie, t.show_date, s.Start_time, h.hall_number " +
                               "FROM Timetable t " +
                               "JOIN Movies m ON m.ID_movie = t.film_id " +
                               "JOIN Seance s ON s.ID_seance = t.seance_id " +
                               "JOIN Hall h ON h.ID_hall = t.hall_id " +
                               "WHERE t.show_date = @day " +
                               "ORDER BY m.Name_movie, t.show_date, s.Start_time";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@day", day.ToString("yyyy-MM-dd"));

                    // Открываем соединение
                    connection.Open();

                    // Выполняем запрос и получаем результат
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Извлекаем значения столбцов из текущей строки результата
                            string movieName = reader.GetString(0);
                            string movieDescription = reader.GetString(1);
                            TimeSpan startTime = reader.GetTimeSpan(3);
                            string hallNumber = reader.GetString(4);

                            var movieKey = (movieName, movieDescription);
                            if (!dict.ContainsKey(movieKey))
                            {
                                dict.Add(movieKey, new List<Tuple<TimeSpan, string>>());
                            }

                            dict[movieKey].Add(Tuple.Create(startTime, hallNumber));
                        }
                    }
                }
            }
        }

        public static void Get_id_seance(string hallName, string startTime, string showDate)
        {
            Sql sl = new Sql();
            using (SqlConnection connection = new SqlConnection(sl.Connect()))
            {
                string sqlQuery = @"SELECT Seats.ID_seat, Seats.nomer_row, Seats.nomer_seat
                    FROM Timetable
                    JOIN hall ON Timetable.hall_id = hall.ID_hall
                    JOIN Seance ON Timetable.seance_id = Seance.ID_seance
                    JOIN Movies ON Timetable.film_id = Movies.ID_movie
                    JOIN Seats ON Seats.hall_id = hall.ID_hall
                    WHERE hall.hall_number = @hallName
                      AND Seance.Start_time = @startTime
                      AND CONVERT(DATE, Timetable.show_date) = @showDate ;";


                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@hallName", hallName.ToString());
                    command.Parameters.AddWithValue("@startTime", startTime.ToString());
                    command.Parameters.AddWithValue("@showDate", showDate.ToString());

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Извлекаем данные о месте из текущей строки
                            int seatId = reader.GetInt32(0);
                            int seatRow = reader.GetInt32(1);
                            int seatNumber = reader.GetInt32(2);

                            // Добавляем данные в словарь
                            seats.Add(seatId, (seatRow, seatNumber));
                        }
                    }
                }
            }
        }
    }
}
