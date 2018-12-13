﻿using System;
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
        private MySqlDataAdapter drinkNameAdapter;
        private DataTable allCocktailNames = new DataTable();
        private MySqlDataAdapter cocktailNameAdapter;
        private int userID;
        private int cocktailID;
        private double price;

        MySqlConnection conn = new MySqlConnection("server=localhost;Database=cocktail;UID=root;pwd=");

        //Gets the user history and returns it as a datatable
        public DataTable getUserHistory()
        {
            try
            {
                dtHistory.Clear();
                conn.Open();
                MySqlCommand command = new MySqlCommand("SELECT User.Name AS Name, User.Age AS Age, History_user.Price, History_user.Date_Time AS Date, Cocktail.Name AS Cocktailname, Cocktail.Description AS Description FROM History_user INNER JOIN Cocktail ON (Cocktail.ID = History_user.Cocktail_ID) INNER JOIN User ON (User.ID = History_user.User_ID) WHERE History_user.Date_Time = @now", conn);
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
        public void uploadUserInfo(string username, DateTime userBirthday, string fingerprintCode)
        {
            try
            {
                conn.Open();
                MySqlCommand insertUser = new MySqlCommand(
                    "INSERT INTO `user`(`Name`, `Age`) VALUES (@name, @birthday)",
                    conn);
                MySqlCommand selectUserID = new MySqlCommand(
                    "SELECT ID FROM user", 
                    conn);
                MySqlCommand command = new MySqlCommand(
                    "INSERT INTO `fingerprints`(`Code`) VALUES(@fingerprintCode)",
                    conn);
                command.Parameters.AddWithValue("@fingerprintCode", fingerprintCode);
                MySqlCommand selectFingerprint = new MySqlCommand(
                    "SELECT Code, fingerprints.ID FROM fingerprints" ,
                    conn);

                insertUser.Parameters.AddWithValue("@name", username);
                insertUser.Parameters.AddWithValue("@birthday", userBirthday);
                conn.Close();
            }
            catch (MySqlException)
            {
                MessageBox.Show("Upload User Info Error");
            }
        }

        // All drink names in datatable
        public List<string> GetAllDrinkNamesWhereCocktailID(int ID)
        {
            try
            {
                drinksForCocktail.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT cocktail_drink.Drink_Dose, drink.Name FROM cocktail_drink INNER JOIN drink ON(drink.ID = cocktail_drink.Drink_ID) AND cocktail_drink.Cocktail_ID = @ID", conn);
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
                    userID = Convert.ToInt32(drUserID[0]);
                }

                conn.Close();
                conn.Open();
                MySqlCommand cmdSelectCocktailInfo =
                    new MySqlCommand("SELECT ID, Price FROM cocktail WHERE Name = @cocktailname", conn);
                cmdSelectCocktailInfo.Parameters.AddWithValue("@cocktailname", cocktailName);
                MySqlDataReader drCocktailInfo = cmdSelectCocktailInfo.ExecuteReader();
                while (drCocktailInfo.Read())
                {
                    cocktailID = Convert.ToInt32(drCocktailInfo[0]);
                    price = Convert.ToDouble(drCocktailInfo[1]);
                }

                conn.Close();
                conn.Open();
                MySqlCommand cmdInsertHistoryInfo =
                    new MySqlCommand(
                        "INSERT INTO history_user (Price, Date_Time, Cocktail_ID, User_ID) VALUES (@price, @dateTime, @cocktail_id, @user_id)", conn);
                cmdInsertHistoryInfo.Parameters.AddWithValue("@price", price);
                cmdInsertHistoryInfo.Parameters.AddWithValue("@dateTime", DateTime.Now);
                cmdInsertHistoryInfo.Parameters.AddWithValue("@cocktail_id", cocktailID);
                cmdInsertHistoryInfo.Parameters.AddWithValue("@user_id", userID);
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