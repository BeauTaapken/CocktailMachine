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
        private ArduinoConnection arduinoConnection;
        private int fingerprint;
        private GetUserHistory getUserHistory;

        public void setPrivates(ArduinoConnection arduinoconnection, GetUserHistory getuserhistory)
        {
            arduinoConnection = arduinoconnection;
            getUserHistory = getuserhistory;
        }

        //Code for filling the datagrid on the userhistory screen
        public void ArduinoDrinksStringForCocktail(string message)
        {
            List<string> amountDrinks = db.GetAllDrinkNamesWhereCocktailID(1);
            if (arduinoConnection.SendMessage("AmountDrinks:" + string.Join(";", amountDrinks)))
            {
                db.InsertIntoUserHistory(Convert.ToInt32(message.Split(';')[0]), message.Split(';')[1]);
                getUserHistory.FillUserHistory();
            }
            else
            {
                MessageBox.Show("Error sending message to arduino");
            }
        }
    }
}
