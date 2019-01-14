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
        private DataTable allDrinkNames = new DataTable();
        List<string> allCocktailNamesList = new List<string>();
        List<string> drinksForCocktail = new List<string>();
        private MySqlDataAdapter planningadapter;
        private DataTable allCocktailNames = new DataTable();
        private int userIDAddAccount;
        private int userIDUserHistory;
        private int cocktailID;
        private double price;
        private DateTime dtAge;
        private int cocktailAge;

        MySqlConnection conn = new MySqlConnection("server=localhost;Database=cocktail;UID=root;pwd=");

        //Gets the user history and returns it as a datatable
        public DataTable getUserHistory()
        {
            try
            {
                dtHistory.Clear();
                conn.Open();
                MySqlCommand command = new MySqlCommand("SELECT User.Name AS Name, User.Age AS Age, History_user.Price, History_user.Date_Time AS Date, Cocktail.Name AS Cocktailname, Cocktail.Description AS Description FROM History_user INNER JOIN Cocktail ON (Cocktail.ID = History_user.Cocktail_ID) INNER JOIN User ON (User.ID = History_user.User_ID) WHERE DATE(History_user.Date_Time) = @now", conn);
                command.Parameters.AddWithValue("@now", DateTime.Now.Date);
                conn.Close();
                planningadapter = new MySqlDataAdapter(command);
                planningadapter.Fill(dtHistory);
                return dtHistory;
            }
            catch (MySqlException e)
            {
                MessageBox.Show("Something went wront with getting the data for the userhistory");
                return dtHistory;
            }
        }

        //upload user info & fingerprint
        public void addUserInfo(string username, DateTime userBirthday, int fingerprintCode)
        {
            try
            {
                conn.Open();
                MySqlCommand cmdInsertUser = new MySqlCommand(
                    "INSERT INTO `user`(`Name`, `Age`) VALUES (@name, @birthday)",
                    conn);
                cmdInsertUser.Parameters.AddWithValue("@name", username);
                cmdInsertUser.Parameters.AddWithValue("@birthday", userBirthday);
                cmdInsertUser.ExecuteNonQuery();
                conn.Close();

                conn.Open();
                MySqlCommand cmdSelectUserID = new MySqlCommand("SELECT ID FROM user WHERE Name = @name", conn);
                cmdSelectUserID.Parameters.AddWithValue("@name", username);
                MySqlDataReader drSelectUserID = cmdSelectUserID.ExecuteReader();
                while (drSelectUserID.Read())
                {
                    userIDAddAccount = Convert.ToInt32(drSelectUserID[0]);
                }
                conn.Close();

                conn.Open();
                MySqlCommand cmdAddFingerprint = new MySqlCommand(
                    "INSERT INTO fingerprints(Code, User_ID) VALUES(@fingerprintCode, @userid)",
                    conn);
                cmdAddFingerprint.Parameters.AddWithValue("@fingerprintCode", fingerprintCode);
                cmdAddFingerprint.Parameters.AddWithValue("@userid", userIDAddAccount);
                cmdAddFingerprint.ExecuteNonQuery();
                conn.Close();
            }
            catch (MySqlException)
            {
                MessageBox.Show("Upload User Info Error");
            }
        }

        // Checks if person is old enough to drink the selected cocktail
        public bool oldEnoughForCocktail(int userid, int cocktailid)
        {
            try
            {
                conn.Open();
                MySqlCommand cmdGetUserAge =
                    new MySqlCommand(
                        "SELECT DATE(user.Age) FROM user INNER JOIN fingerprints ON (fingerprints.User_ID = user.ID) WHERE fingerprints.Code = @userid",
                        conn);
                cmdGetUserAge.Parameters.AddWithValue("@userid", userid);
                MySqlDataReader drUserAge = cmdGetUserAge.ExecuteReader();
                while (drUserAge.Read())
                {
                    dtAge = Convert.ToDateTime(drUserAge[0]).Date;
                }

                conn.Close();
                conn.Open();
                MySqlCommand cmdGetCocktailAge =
                    new MySqlCommand("SELECT Age FROM cocktail WHERE ID = @cocktailid", conn);
                cmdGetCocktailAge.Parameters.AddWithValue("@cocktailid", cocktailid);
                MySqlDataReader drCocktailAge = cmdGetCocktailAge.ExecuteReader();
                while (drCocktailAge.Read())
                {
                    cocktailAge = Convert.ToInt32(drCocktailAge[0]);
                }

                conn.Close();

                //Checks if user is old enough
                if (DateTime.Now.Year - dtAge.Year - 1 >= cocktailAge || DateTime.Now.Year - dtAge.Year == cocktailAge && DateTime.Now.Month >= dtAge.Month && DateTime.Now.Day >= dtAge.Day)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't check if person is old enought to drink the cocktail");
            }
            return false;
        }

        // All drink names in datatable
        public List<string> GetAllDrinkNamesWhereCocktailID(int ID)
        {
            try
            {
                drinksForCocktail.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT drink.Name, cocktail_drink.Drink_Dose FROM cocktail_drink INNER JOIN drink ON(drink.ID = cocktail_drink.Drink_ID) WHERE cocktail_drink.Cocktail_ID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", ID);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    drinksForCocktail.Add(dr[0].ToString() + ":" + dr[1].ToString());
                }
                conn.Close();
                return drinksForCocktail;
            }
            catch (MySqlException)
            {
                MessageBox.Show("Get All Drinks Error");
                return drinksForCocktail;
            }
        }

        // Gets cocktailid based on selected cocktail
        public int getCocktailID(string cocktail)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT ID FROM `cocktail` WHERE Name = @cocktailname", conn);
                cmd.Parameters.AddWithValue("@cocktailname", cocktail);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    cocktailID = Convert.ToInt32(dr[0]);
                }
                conn.Close();
                return cocktailID;
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't get id of selected cocktail");
                return 0;
            }
        }

        // Gets all cocktail names in a List<string>
        public List<string> GetAllCocktailNamesList()
        {
            try
            {
                allCocktailNamesList.Clear();
                conn.Open();
                MySqlCommand command = new MySqlCommand("SELECT cocktail.Name FROM cocktail", conn);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    allCocktailNamesList.Add(reader.GetString(0));
                }
                conn.Close();
                return allCocktailNamesList;
            }
            catch (MySqlException)
            {
                MessageBox.Show("Get All Cocktails Error");
                conn.Close();
                return null;
            }
        }

        //Insert the chosen drink, the price and the person who ordered the drink in the userhistory table
        public void InsertIntoUserHistory(int fingerprint, string cocktailName)
        {
            try
            {
                conn.Open();
                MySqlCommand cmdSelectUserID =
                    new MySqlCommand(
                        "SELECT user.ID FROM user INNER JOIN fingerprints ON(fingerprints.User_ID = user.ID) WHERE fingerprints.Code = @fingerprint",
                        conn);
                cmdSelectUserID.Parameters.AddWithValue("@fingerprint", fingerprint);
                MySqlDataReader drUserID = cmdSelectUserID.ExecuteReader();
                while (drUserID.Read())
                {
                    userIDUserHistory = Convert.ToInt32(drUserID[0]);
                }

                conn.Close();
                conn.Open();
                MySqlCommand cmdSelectCocktailInfo =
                    new MySqlCommand("SELECT Price FROM cocktail WHERE Name = @cocktailname", conn);
                cmdSelectCocktailInfo.Parameters.AddWithValue("@cocktailname", cocktailName);
                MySqlDataReader drCocktailInfo = cmdSelectCocktailInfo.ExecuteReader();
                while (drCocktailInfo.Read())
                {
                    price = Convert.ToDouble(drCocktailInfo[0]);
                }

                conn.Close();
                conn.Open();
                MySqlCommand cmdInsertHistoryInfo =
                    new MySqlCommand(
                        "INSERT INTO history_user (Price, Date_Time, Cocktail_ID, User_ID) VALUES (@price, @dateTime, @cocktail_id, @user_id)", conn);
                cmdInsertHistoryInfo.Parameters.AddWithValue("@price", price);
                cmdInsertHistoryInfo.Parameters.AddWithValue("@dateTime", DateTime.Now);
                cmdInsertHistoryInfo.Parameters.AddWithValue("@cocktail_id", cocktailID);
                cmdInsertHistoryInfo.Parameters.AddWithValue("@user_id", userIDUserHistory);
                cmdInsertHistoryInfo.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong while inserting the order into the database. The person fingerprint was: " + fingerprint + " and the person ordered: " + cocktailName);
            }
        }
    }
}