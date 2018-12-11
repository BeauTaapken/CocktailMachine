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
        private DataTable allCocktailNames = new DataTable();
        private MySqlDataAdapter planningadapter;
        private MySqlDataAdapter drinkNameAdapter;
        private MySqlDataAdapter cocktailNameAdapter;

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
            }
            catch (MySqlException)
            {
                MessageBox.Show("Upload User Info Error");
            }
        }

        // All drink names in datatable
        public DataTable GetAllDrinkNames()
        {
            try
            {
                allDrinkNames.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT drink.Name FROM drink", conn);
                conn.Close();
                drinkNameAdapter = new MySqlDataAdapter(cmd);
                drinkNameAdapter.Fill(allDrinkNames);
                return allDrinkNames;
            }
            catch (MySqlException)
            {
                MessageBox.Show("Get All Drinks Error");
                return allDrinkNames;
            }
        }

        // All cocktail names in datatable
        public DataTable GetAllCocktailNames()
        {
            try
            {
                allCocktailNames.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT cocktail.Name FROM cocktail", conn);
                conn.Close();
                cocktailNameAdapter = new MySqlDataAdapter(cmd);
                cocktailNameAdapter.Fill(allCocktailNames);
                return allCocktailNames;
            }
            catch (MySqlException)
            {
                MessageBox.Show("Get All Cocktails Error");
                return allCocktailNames;
            }
        }
    }
}
