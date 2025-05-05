using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Кинотеатр
{
    internal class Sql
    { 
        public string ConnectinString { get; set; }

        public string Connect()
        {
            ConnectinString = "Data Source=192.168.0.8,1433;Initial Catalog=Kino;User ID=ivan;Password=4552;";
            return ConnectinString;
        }
    
    }
}
