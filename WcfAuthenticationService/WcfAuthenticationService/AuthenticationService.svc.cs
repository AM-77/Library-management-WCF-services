using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MySql.Data.MySqlClient;

namespace WcfAuthenticationService
{
    public class Service1 : IAuthenticationService
    {
        // An instantce of the database manager class.
        public DatabaseManager db_manager = new DatabaseManager();

        // The creatation of an admin account.
        public bool subscribtion_admin(string first_name, string second_name, string email, string password)
        {

            MySqlConnection connection = db_manager.connect();
       
            string query = "INSERT INTO `admin` (`id`, `first_name`, `second_name`, `email`, `password`) VALUES (NULL, '"+ first_name + "', '"+ second_name + "', '"+ email + "', '"+ password + "');";
            var cmd = new MySqlCommand(query, connection);
            int i = cmd.ExecuteNonQuery();

            db_manager.close();

            if (i == 1)
                return true;
            else
                return false;
        }

        // The creatation of a student account
        public bool subscribtion_student(string id, string first_name, string second_name, string specaility, string level, string email, string password)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "INSERT INTO `student` (`id`, `first_name`, `second_name`, `specaility`, `level`, `email`, `password`, `didnt_show`) VALUES ('"+ id + "', '"+ first_name + "', '"+ second_name +"', '"+ specaility + "', '"+ level + "', '"+ email +"', '"+ password + "', '0');";
            var cmd = new MySqlCommand(query, connection);
            int i = cmd.ExecuteNonQuery();

            db_manager.close();

            if (i == 1)
                return true;
            else
                return false;
        }

        // The creatation of a teacher account
        public bool subscribtion_teacher(string id, string first_name, string second_name, string grade, string email, string password)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "INSERT INTO `teacher` (`id`, `first_name`, `second_name`, `grade`, `email`, `password`, `didnt_show`) VALUES('"+ id + "', '"+ first_name + "', '"+ second_name + "', '"+ grade + "', '"+ email + "', '"+ password + "', '0'); ";
            var cmd = new MySqlCommand(query, connection);
            int i = cmd.ExecuteNonQuery();

            db_manager.close();

            if (i == 1)
                return true;
            else
                return false;

            
        }
        
        // Login service for all users
        public string login(string email, string password)
        {
            
            if (is_admin(email) && get_admin(email) != null)
            {
                Admin admin = get_admin(email);

                if (admin.email.ToLower().Equals(email.ToLower()) && admin.password.Equals(password))
                {
                    return "logged";
                }
                else
                {
                    return "wrong password";
                }
            }
            else
            {
                if (is_student(email) && get_student(email) != null)
                {
                    Student student = get_student(email);

                    if (student.email.ToLower().Equals(email.ToLower()) && student.password.Equals(password))
                    {
                        return "logged";
                    }
                    else
                    {
                        return "wrong password";
                    }

                }
                else
                {
                    if (is_teacher(email) && get_teacher(email) != null)
                    {
                        Teacher teacher = get_teacher(email);

                        if (teacher.email.ToLower().Equals(email.ToLower()) && teacher.password.Equals(password))
                        {
                            return "logged";
                        }
                        else
                        {
                            return "wronng password";
                        }

                    }

                }
            }

            return "wrong email";
        }


        // Test if the student is blocked or not using its id
        public bool is_blocked_student(string id_student)
        {

            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM blocked WHERE id_client = '" + id_student + "' ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
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

        // Test if the teacher is blocked or not using its id
        public bool is_blocked_teacher(string id_teacher)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM blocked WHERE id_client = '" + id_teacher + "' ;";
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

        // Test if the email is a student email
        public bool is_student(string email)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM student ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                if (reader["email"].Equals(email))
                {
                    db_manager.close();
                    return true;
                }
            }

            db_manager.close();
            return false;
        }

        // Test if the email is a teacher email
        public bool is_teacher(string email)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM teacher ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                if (reader["email"].Equals(email))
                {
                    db_manager.close();
                    return true;
                }
            }

            db_manager.close();
            return false;
        }

        /************* by id **************/

        public bool is_student_id(string id_student)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM student WHERE id='"+ id_student + "' ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (reader.GetString(0).Equals(id_student))
                {
                    db_manager.close();
                    return true;
                }
            }

            db_manager.close();
            return false;
        }

        public bool is_teacher_id(string id_teacher)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM teacher WHERE id='"+ id_teacher + "' ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (reader.GetString(0).Equals(id_teacher))
                {
                    db_manager.close();
                    return true;
                }
            }

            db_manager.close();
            return false;
        }

        /**********************************/


        // Test if the email is an admin email
        public bool is_admin(string email)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM admin ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                if (reader["email"].Equals(email))
                {
                    db_manager.close();
                    return true;
                }
            }

            db_manager.close();
            return false;
        }

        // Return a teacher from the database using its id if exists, returns null if not. 
        public Teacher get_teacher(string email)
        {
            Teacher teacher = null;

            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `teacher` WHERE email ='" + email + "';";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                teacher = (new Teacher(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetInt16(6)));
            }

            db_manager.close();


            return teacher;
        }


        // Return a student from the database using its id if exists, returns null if not. 
        public Student get_student(string email)
        {
            Student student = null;

            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `student` WHERE email='" + email + "';";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                student = (new Student(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt16(7)));
            }

            db_manager.close();


            return student;
        }

        public Admin get_admin(string email)
        {
            Admin admin = null;

            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `admin` WHERE email='" + email + "';";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                admin = (new Admin(reader.GetInt16(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4)));
            }

            db_manager.close();


            return admin;
        }




        // Return a teacher from the database using its id if exists, returns null if not. 
        public Teacher get_teacher_by_id(string id)
        {
            Teacher teacher = null;

            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `teacher` WHERE id ='" + id + "';";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                teacher = (new Teacher(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetInt16(6)));
            }

            db_manager.close();


            return teacher;
        }


        // Return a student from the database using its id if exists, returns null if not. 
        public Student get_student_by_id(string id)
        {
            Student student = null;

            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `student` WHERE id='" + id + "';";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                student = (new Student(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt16(7)));
            }

            db_manager.close();


            return student;
        }


    }
}
