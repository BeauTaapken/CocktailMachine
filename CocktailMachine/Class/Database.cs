using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace CocktailMachine.Class
{
    class Database
    {
        //TODO make all database related functions here. also connection string(Everyone)

        private DataTable dtHistory = new DataTable();
        private MySqlDataAdapter planningadapter;

        MySqlConnection conn = new MySqlConnection("server=localhost;Database=riktomcom;UID=riktomcom;pwd=H2Ha%qeK");

        public DataTable getUserHistory()
        {
            try
            {
                dtHistory.Clear();
                conn.Open();
                MySqlCommand command = new MySqlCommand(
                    "SELECT User.Name AS Name, User.Age AS Age, History_user.Price, History_user.Date_Time AS Date, Cocktail.Name AS Cocktailname, Cocktail.Description AS Description FROM History_user INNER JOIN Cocktail ON (Cocktail.ID = History_user.ID) INNER JOIN User ON (User.ID = History_user.User_ID)",
                    conn);
                conn.Close();
                planningadapter = new MySqlDataAdapter(command);
                planningadapter.Fill(dtHistory);
                return dtHistory;
            }
            catch (MySqlException)
            {
                MessageBox.Show("Something went wrong");
                return dtHistory;
            }
        }
        //upload user info & fingerprint
        public DataTable uploadUserInfo(string username, DateTime userBirthday, string fingerprintCode)
        {
            try
            {
                dtHistory.Clear();
                conn.Open();
                MySqlCommand command = new MySqlCommand(
                    "INSERT INTO `fingerprints`(`Code`) VALUES(@fingerprintCode)",
                    conn);
                command.Parameters.AddWithValue("@fingerprintCode", fingerprintCode);
                MySqlCommand selectFingerprint = new MySqlCommand(
                    "SELECT Code, fingerprints.ID FROM fingerprints" ,
                    conn);
                MySqlCommand insertUser = new MySqlCommand(
                    "INSERT INTO `user`(`Name`, `Age`) VALUES (@name, @birthday)",
                    conn);
                insertUser.Parameters.AddWithValue("@name", username);
                insertUser.Parameters.AddWithValue("@birthday", userBirthday);
                conn.Close();
                planningadapter = new MySqlDataAdapter(command);
                planningadapter.Fill(dtHistory);

                return dtHistory;
            }
            catch (MySqlException)
            {
                MessageBox.Show("Something went wrong");
                return dtHistory;
            }
        }
    }
}
