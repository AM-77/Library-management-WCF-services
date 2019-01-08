using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace SystemService
{
    public class DatabaseManager
    {
        public string db_host = "localhost";
        public string db_name = "mini_projet_poc";
        public string db_user = "root";
        public string db_password = "";

        private MySqlConnection db_connection = null;

        public DatabaseManager() { }

        public MySqlConnection connect()
        {
            if (!is_connected())
            {
                this.db_connection = new MySqlConnection("SERVER=" + db_host + ";DATABASE=" + db_name + ";UID=" + db_user + ";PASSWORD=" + db_password + ";");
                this.db_connection.Open();
            }

            return this.db_connection;
        }

        public bool is_connected()
        {

            if (this.db_connection != null)
            {
                return true;
            }

            return false;

        }

        public void close()
        {
            if (this.db_connection != null)
            {
                this.db_connection.Close();
                this.db_connection = null;
            }
        }
    }
}