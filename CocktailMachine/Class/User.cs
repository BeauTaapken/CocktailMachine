using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

namespace CocktailMachine.Class
{
    public class User
    {
        private ArduinoConnection arduinoConnection;
        private Database db = new Database();
        private bool receivedPrint;

        public void setPrivates(ArduinoConnection arduinoconnection)
        {
            arduinoConnection = arduinoconnection;
        }

        public void getFingerprint()
        {
            arduinoConnection.MessageReceived = false;
            arduinoConnection.SendMessage("Fingerprint");
            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.Elapsed < TimeSpan.FromSeconds(30) && arduinoConnection.MessageReceived == false)
            {
                if (arduinoConnection.MessageReceived)
                {
                    receivedPrint = true;
                }
            }
            s.Stop();

            
        }

        public void addUserAccount(string name, DateTime age, int fingerprint)
        {
            db.addUserInfo(name, age, fingerprint);
        }
    }
}
