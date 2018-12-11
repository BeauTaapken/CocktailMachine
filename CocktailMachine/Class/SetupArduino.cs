using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows;

namespace CocktailMachine.Class
{
    class SetupArduino
    {
        //TODO make funcion to get all drinks from the database(make in database class) and send them to the arduino, Made by Jeremy
        private Database db = new Database();
        private ArduinoConnection arduinoConnection;

        // Sets ArduinoConnection as UserHistory.xaml.cs initializes
        public void setPrivate(ArduinoConnection arduinoconnection)
        {
            arduinoConnection = arduinoconnection;
        }

        // Convert DataTable to String, seperated with "." after every row
        public string DtToSeperatedString(DataTable dt)
        {
            int i = 0;
            string str = "";
            while (i < dt.Rows.Count)
            {
                if (i < (dt.Rows.Count - 1))
                {
                    str += dt.Rows[i][0].ToString() + ".";
                }
                else
                {
                    str += dt.Rows[i][0].ToString();
                }
                i++;
            }
            return str;
        }

        // Contains all drink names in a string with correct format for Arduino (can't SendMessage() because ArduinoConnection wants port and baudrate)
        public string AllDrinkNamesMessage()
        {
            DataTable allDrinks = db.GetAllDrinkNames();
            string message = DtToSeperatedString(allDrinks);
            return message;
        }
    }
}
