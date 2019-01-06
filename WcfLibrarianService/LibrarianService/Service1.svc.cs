using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MySql.Data.MySqlClient;

namespace LibrarianService
{
    public class Service1 : ILibrarianService
    {
        public DatabaseManager db_manager = new DatabaseManager();
        public DatabaseManager db_manager1 = new DatabaseManager();

        public bool confirm_reservation(string id_client, int id_work)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `reservation` ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                System.Diagnostics.Debug.WriteLine(id_work + " " + reader.GetInt32(1));

                if (reader["id_client"].Equals(id_client) && reader.GetInt32(1) == id_work)
                {
                    MySqlConnection connection1 = db_manager1.connect();
                    string query1 = "UPDATE `reservation` SET `confirmed` = '1' WHERE `reservation`.`id` = '" + reader.GetInt32(0) + "' ;";
                    var cmd1 = new MySqlCommand(query1, connection1);
                    var reader1 = cmd1.ExecuteNonQuery();
                    db_manager1.close();
                    db_manager.close();
                    return true;
                }
            }

            return false;
        }
        
        public bool deblock(string id_client)
        {
            if (is_student(id_client))
            {
                if (is_blocked_student(id_client))
                {
                    MySqlConnection connection = db_manager.connect();
                    string query = "DELETE FROM `blocked` WHERE `blocked`.`id_client` = '"+ id_client +"';";
                    var cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    
                    string query1 = "UPDATE `student` SET `didnt_show` = '0' WHERE `student`.`id` = '"+ id_client + "';";
                    var cmd1 = new MySqlCommand(query1, connection);
                    cmd1.ExecuteNonQuery();
                    db_manager.close();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (is_teacher(id_client))
                {
                    if (is_blocked_teacher(id_client))
                    {
                        MySqlConnection connection = db_manager.connect();
                        string query = "DELETE FROM `blocked` WHERE `blocked`.`id_client` = '" + id_client + "';";
                        var cmd = new MySqlCommand(query, connection);
                        cmd.ExecuteNonQuery();

                        string query1 = "UPDATE `teacher` SET `didnt_show` = '0' WHERE `teacher`.`id` = '" + id_client + "';";
                        var cmd1 = new MySqlCommand(query1, connection);
                        cmd1.ExecuteNonQuery();

                        db_manager.close();
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        
        public bool is_blocked_student(string id_student)
        {

            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM blocked WHERE id_client = '" + id_student + "' ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {

                if (reader["id_client"].Equals(id_student))
                {
                    db_manager.close();
                    return true;
                }
            }

            db_manager.close();
            return false;
        }
        // Test if the teacher is blocked or not using its id
        public bool is_blocked_teacher(string id_teacher)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM blocked WHERE id_client = '" + id_teacher + "' ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                if (reader["id_client"].Equals(id_teacher))
                {
                    db_manager.close();
                    return true;
                }
            }

            db_manager.close();
            return false;
        }

        // Test if the email is a student email
        public bool is_student(string id_student)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM student ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                if (reader["id"].Equals(id_student))
                {
                    db_manager.close();
                    return true;
                }
            }

            db_manager.close();
            return false;
        }

        // Test if the email is a teacher email
        public bool is_teacher(string id_teacher)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM teacher ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                if (reader["id"].Equals(id_teacher))
                {
                    db_manager.close();
                    return true;
                }
            }

            db_manager.close();
            return false;
        }

    }
}
