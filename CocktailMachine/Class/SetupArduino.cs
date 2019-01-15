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
        //TODO make funcion to get all cocktails from the database(make in database class) and send them to the arduino, Made by Jeremy
        private Database db = new Database();
        private ArduinoConnection arduinoConnection;

        // Sets ArduinoConnection as UserHistory.xaml.cs initializes
        public void setPrivate(ArduinoConnection arduinoconnection)
        {
            arduinoConnection = arduinoconnection;
        }

        // Makes a string from a List<string>, seperated by ";"
        public string ListToSeperatedString(List<string> list)
        {
            return "AllCocktails:" + string.Join(";", list);
        }

        // Contains all drink names in a string with correct format for Arduino and sends it to Arduino, checks if arduino is connected and checks if message is sent
        public void SendAllCocktailNamesToArduino()
        {
            if(arduinoConnection.IsConnected())
            {
                List<string> allCocktails = db.GetAllCocktailNamesList();
                string message = ListToSeperatedString(allCocktails);
                if (arduinoConnection.SendMessage(message))
                {

                }
                else
                {
                    MessageBox.Show("Error sending message to arduino");
                }
            }
            else
            {
                MessageBox.Show("Initial setup couldn't reach Arduino");
            }
        }
    }
}