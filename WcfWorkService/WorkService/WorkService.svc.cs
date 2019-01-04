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

    }   
}
