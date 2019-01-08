using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MySql.Data.MySqlClient;
using System.Net.Mail;

namespace SystemService
{
    public class Service1 : ISystemService
    {
        public DatabaseManager db_manager = new DatabaseManager();

        // Executed every 1h
        public void didnt_show()
        {
            List<Reservation> resrervations = new List<Reservation>();

            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `reservation` ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                resrervations.Add(new Reservation(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4), reader.GetInt32(5)));
            }

            db_manager.close();

            foreach(Reservation resrvation in resrervations)
            {
               if(resrvation.confirmed == 0)
                {
                    if(DateTime.Now >= resrvation.borrowing_day.AddHours(24))
                    {

                        // Free the book
                        MySqlConnection connection1 = db_manager.connect();

                        string query1 = "UPDATE `work` SET `reserved` = '0' WHERE `work`.`id` = '"+ resrvation.id_work +"' ;";
                        var cmd1 = new MySqlCommand(query1, connection1);
                        var reader1 = cmd1.ExecuteNonQuery();

                        db_manager.close();

                        // Send email to the waitting list
                        if (!resrvation.waitting_list.Equals(""))
                        {
                            Work work = get_work(resrvation.id_work);
                            Char[] chars = { ' ' };
                            String[] waitting_list = resrvation.waitting_list.Split(chars);
                            List<String> waitting_array = waitting_list.ToList<String>();
                            String email = "";
                            foreach (String waitting_person in waitting_array)
                            {

                                if (is_student(waitting_person) && !is_blocked_student(waitting_person))
                                {
                                    email = get_student(waitting_person).email;
                                }
                                else
                                {
                                    if (is_teacher(waitting_person) && !is_blocked_teacher(waitting_person))
                                    {
                                        email = get_teacher(waitting_person).email;
                                    }
                                }

                                // sending an email
                                MailMessage mail = new MailMessage("the-email@univ-c2.com", email);
                                SmtpClient client = new SmtpClient();
                                client.Port = 25;
                                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                                client.UseDefaultCredentials = false;
                                client.Host = "smtp.gmail.com";
                                mail.Subject = "Library's book borrowring.";
                                mail.Body = "The book '" + work.title + "' that you wanted to borrow earlier is now available.";
                                client.Send(mail);

                            }
                        }

                        // Increament didnt_show
                        if (is_student(resrvation.id_client))
                        {
                            Student student = get_student(resrvation.id_client);

                            if (student.didnt_show < 2)
                            {
                                MySqlConnection connection2 = db_manager.connect();

                                string query2 = "UPDATE `student` SET `didnt_show` = '" + (student.didnt_show + 1) + "' WHERE `student`.`id` = '" + student.id + "';";
                                var cmd2 = new MySqlCommand(query2, connection2);
                                var reader2 = cmd2.ExecuteNonQuery();

                                db_manager.close();
                            }
                            else
                            {
                                MySqlConnection connection2 = db_manager.connect();

                                string query2 = "INSERT INTO `blocked` (`id`, `id_client`, `blocked_day`) VALUES(NULL, '" + student.id + "', CURRENT_TIMESTAMP);";
                                var cmd2 = new MySqlCommand(query2, connection2);
                                var reader2 = cmd2.ExecuteNonQuery();

                                db_manager.close();

                                // Notify the the student that his account has been blocked for 30 day.
                                MailMessage mail = new MailMessage("the-email@univ-c2.com", student.email);
                                SmtpClient client = new SmtpClient();
                                client.Port = 25;
                                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                                client.UseDefaultCredentials = false;
                                client.Host = "smtp.gmail.com";
                                mail.Subject = "Library's book borrowring.";
                                mail.Body = "Your account has been blocked for 30 day.";
                                client.Send(mail);

                            }
                        }
                        else
                        {
                            if (is_teacher(resrvation.id_client))
                            {
                                Teacher teacher = get_teacher(resrvation.id_client);

                                if (teacher.didnt_show < 2)
                                {
                                    MySqlConnection connection2 = db_manager.connect();

                                    string query2 = "UPDATE `teacher` SET `didnt_show` = '" + (teacher.didnt_show + 1) + "' WHERE `student`.`id` = '" + teacher.id + "';";
                                    var cmd2 = new MySqlCommand(query2, connection2);
                                    var reader2 = cmd2.ExecuteNonQuery();

                                    db_manager.close();
                                }
                                else
                                {
                                    MySqlConnection connection2 = db_manager.connect();

                                    string query2 = "INSERT INTO `blocked` (`id`, `id_client`, `blocked_day`) VALUES(NULL, '" + teacher.id + "', CURRENT_TIMESTAMP);";
                                    var cmd2 = new MySqlCommand(query2, connection2);
                                    var reader2 = cmd2.ExecuteNonQuery();

                                    db_manager.close();


                                    // Notify the the teacher that his account has been blocked for 30 day.
                                    MailMessage mail = new MailMessage("the-email@univ-c2.com", teacher.email);
                                    SmtpClient client = new SmtpClient();
                                    client.Port = 25;
                                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    client.UseDefaultCredentials = false;
                                    client.Host = "smtp.gmail.com";
                                    mail.Subject = "Library's book borrowring.";
                                    mail.Body = "Your account has been blocked for 30 day.";
                                    client.Send(mail);
                                }
                            }
                        }

                    }
                } 
            }
        }

        // Excute evryday
        public void auto_deblock()
        {
            List<Blocked> blocked_list = new List<Blocked>();
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `blocked` ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                blocked_list.Add(new Blocked(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2)));
            }

            db_manager.close();

            foreach (Blocked blocked in blocked_list)
            {
                if(blocked.blocked_day.AddDays(30).Day >= DateTime.Now.Day)
                {

                    MySqlConnection connection1 = db_manager.connect();

                    string query1 = "DELETE FROM `blocked` WHERE `blocked`.`id` = '"+ blocked.id +"' ;";
                    var cmd1 = new MySqlCommand(query1, connection1);
                    var reader1 = cmd1.ExecuteNonQuery();

                    db_manager.close();


                    if (is_student(blocked.id_client))
                    {
                        
                        // Notify the the student that his account has been blocked.
                        MailMessage mail = new MailMessage("the-email@univ-c2.com", get_student(blocked.id_client).email);
                        SmtpClient client = new SmtpClient();
                        client.Port = 25;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Host = "smtp.gmail.com";
                        mail.Subject = "Library's book borrowring.";
                        mail.Body = "Your account has been deblocked.";
                        client.Send(mail);
                    }
                    else
                    {
                        if (is_teacher(blocked.id_client))
                        {
                            // Notify the the teacher that his account has been deblocked.
                            MailMessage mail = new MailMessage("the-email@univ-c2.com", get_teacher(blocked.id_client).email);
                            SmtpClient client = new SmtpClient();
                            client.Port = 25;
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = false;
                            client.Host = "smtp.gmail.com";
                            mail.Subject = "Library's book borrowring.";
                            mail.Body = "Your account has been deblocked.";
                            client.Send(mail);
                        }
                    }

                }
            }
        }

        // Excutes evry day
        public void reminder()
        {
            List<Reservation> resrervations = new List<Reservation>();
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `reservation` ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                resrervations.Add(new Reservation(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4), reader.GetInt32(5)));
            }

            db_manager.close();

            foreach (Reservation resrvation in resrervations)
            {
                if(resrvation.borrowing_day.AddDays(14) == DateTime.Now )
                {
                    if (is_student(resrvation.id_client))
                    {

                        // Notify the student that he have to return the book before 15 days
                        MailMessage mail = new MailMessage("the-email@univ-c2.com", get_student(resrvation.id_client).email);
                        SmtpClient client = new SmtpClient();
                        client.Port = 25;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Host = "smtp.gmail.com";
                        mail.Subject = "Library's book borrowring.";
                        mail.Body = "Tomorrow your last day to return the book.";
                        client.Send(mail);
                    }
                    else
                    {
                        if (is_teacher(resrvation.id_client))
                        {
                            // Notify the teacher that he have to return the book before 15 days
                            MailMessage mail = new MailMessage("the-email@univ-c2.com", get_teacher(resrvation.id_client).email);
                            SmtpClient client = new SmtpClient();
                            client.Port = 25;
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = false;
                            client.Host = "smtp.gmail.com";
                            mail.Subject = "Library's book borrowring.";
                            mail.Body = "Tomorrow your last day to return the book.";
                            client.Send(mail);
                        }
                    }
                }
            }
        }

        // Test if the id is a student id
        public bool is_student(string id_student)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM student ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
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

        // Test if the id is a teacher id
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

        // Return a teacher from the database using its id if exists, returns null if not. 
        public Teacher get_teacher(string id)
        {
            Teacher teacher = null;

            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `teacher` WHERE id='" + id + "';";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                teacher = (new Teacher(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetInt32(6)));
            }

            db_manager.close();


            return teacher;
        }


        // Return a student from the database using its id if exists, returns null if not. 
        public Student get_student(string id)
        {
            Student student = null;

            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `student` WHERE id='" + id + "';";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                student = (new Student(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(7)));
            }

            db_manager.close();


            return student;
        }

        // Test if the student is blocked or not using its id
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

        public Work get_work(int id_work)
        {
            Work work = null;

            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `work` WHERE id='" + id_work + "' ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                work = (new Work(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(0)));
            }

            db_manager.close();


            return work;
        }
    }
}
