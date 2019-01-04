using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MySql.Data.MySqlClient;
using System.Net.Mail;

namespace WorkService
{
    public class Service1 : IWorkService
    {
        // La creation d'une instance de la classe du base de donnees
        public DatabaseManager db_manager = new DatabaseManager();

        // L insertion d une ouvrage dans la base de donnees.
        public bool create_work(string title, string author, string theme, string keywords, string barecode, string type, int reserved)
        {

            MySqlConnection connection = db_manager.connect();

            string query = "INSERT INTO `work` (`id`, `title`, `author`, `theme`, `keywords`, `barecode`, `type`, `reserved`) VALUES (NULL, '" + title + "', '" + author + "', '" + theme + "', '" + keywords + "', '" + barecode + "', '" + type + "', '0') ;";
            var cmd = new MySqlCommand(query, connection);
            int res = cmd.ExecuteNonQuery();
            db_manager.close();

            if (res == -1)
                return false;
            else
                return true;

        }
        
        // La suppression d une ouvrage de la base de donnees
        public bool delete_work(int id_work)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "DELETE FROM `work` WHERE `id` = '" + id_work + "' ;";
            var cmd = new MySqlCommand(query, connection);
            int res = cmd.ExecuteNonQuery();

            db_manager.close();

            if (res == -1)
                return false;
            else
                return true;
        }
        
        // La rende d une ouvrage 
        public bool free_work(int id_work)
        {
            MySqlConnection connection = db_manager.connect();

            string query = "UPDATE `work` SET `reserved` = '0' WHERE `work`.`id` = " + id_work + ";";
            var cmd = new MySqlCommand(query, connection);
            int res = cmd.ExecuteNonQuery();

            db_manager.close();

            Work work = get_work(id_work);

            Reservation resrvation = get_reservation(id_work);

            if(!resrvation.waitting_list.Equals(""))
            {

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
                        if(is_teacher(waitting_person) && !is_blocked_teacher(waitting_person))
                        {
                            email = get_teacher(waitting_person).email;
                        }
                    }

          /*          // sending an email
                    MailMessage mail = new MailMessage("library@univ-constantine2.dz", email);
                    SmtpClient client = new SmtpClient();
                    client.Port = 25;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Host = "smtp.gmail.com";
                    mail.Subject = "Library's book borowring.";
                    mail.Body = "The book '" + work.title + "' that you wanted to borrow earlier is now available.";
                    client.Send(mail);
                    */
                }
            }

            if (res == -1)
                return false;
            else
                return true;
        }
        
        public List<Work> get_all_works()
        {
            List<Work> all_works = new List<Work>();
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM work ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                all_works.Add(new Work(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(0)));
            }

            db_manager.close();


            return all_works;
        }

        public List<Work> get_works(string title, string author, string theme, string keywords, string barecode, string type)
        {
            List<Work> works = new List<Work>();
            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `work` WHERE title LIKE '%"+ title +"%' AND author LIKE '%"+ author + "%' AND theme LIKE '%"+ theme + "%' AND keywords LIKE '%"+ keywords + "%' AND barecode LIKE '%"+ barecode + "%' AND type LIKE '%"+ type + "%'  ;";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                works.Add(new Work(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(0)));
            }

            db_manager.close();


            return works;
        }

        public Work get_work_by_barecode(string barecode)
        {
            Work work = null;

            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `work` WHERE barecode='" + barecode + "';";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                work = (new Work(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(7)));
            }

            db_manager.close();


            return work;
        }

        public Work get_work(int id_work)
        {
            Work work = null;

            MySqlConnection connection = db_manager.connect();

            string query = "SELECT * FROM `work` WHERE id=" + id_work + ";";
            var cmd = new MySqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read() && reader != null)
            {
                work = (new Work(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(7)));
            }

            db_manager.close();


            return work;
        }

    }   
}
