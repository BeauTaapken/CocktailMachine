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
    public class Drink
    {
        // TODO get drinks from database(make function in database class) and return them to the arduino based on string gotten from the Arduino(can be tested with a hardcoded string), Made by Rik

        private Database db = new Database();
        private ArduinoConnection arduinoConnection;
        private int fingerprint;
        private UserHistory getUserHistory;

        public void setPrivates(ArduinoConnection arduinoconnection, UserHistory getuserhistory)
        {
            arduinoConnection = arduinoconnection;
            getUserHistory = getuserhistory;
        }

        //Code for filling the datagrid on the userhistory screen
        public void ArduinoDrinksStringForCocktail(string message)
        {
            int fingerprint = Convert.ToInt32(message.Split(';')[0]);
            int cocktailID = getCocktailID(message.Split(';')[1]);
            if (db.oldEnoughForCocktail(fingerprint, cocktailID))
            {
                List<string> amountDrinks = db.GetAllDrinkNamesWhereCocktailID(cocktailID);
                if (amountDrinks.Count != 0)
                {
                    if (arduinoConnection.SendMessage("AmountDrinks:" + string.Join(";", amountDrinks)))
                    {
                        db.InsertIntoUserHistory(fingerprint, message.Split(';')[1]);
                        getUserHistory.FillUserHistory();
                    }
                    else
                    {
                        MessageBox.Show("Error sending message to arduino");
                    }
                }
                else
                {
                    MessageBox.Show("cocktail heeft geen dranken");
                }
            }
            else
            {
                arduinoConnection.SendMessage("TooYoung");
            }
        }

        private int getCocktailID(string cocktail)
        {
            return db.getCocktailID(cocktail);
        }
    }
}
