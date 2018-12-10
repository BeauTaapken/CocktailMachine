using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace CocktailMachine.Class
{
    class Drinks
    {
        // TODO get drinks from database(make function in database class) and return them to the arduino based on string gotten from the Arduino(can be tested with a hardcoded string), Made by Rik

        private Database db = new Database();
        private DataTable dtDrinks = new DataTable();
        private string drinks = "";


        //Code for filling the datagrid on the userhistory screen
        public string ArduinoDrinksString()
        {
            dtDrinks.Clear();
            dtDrinks = db.GetAllDrinkNames();
            foreach(DataRow row in dtDrinks.Rows)
            {
                drinks = drinks + row["name"].ToString() + ",";
            }
            return drinks;
        }
    }
}
