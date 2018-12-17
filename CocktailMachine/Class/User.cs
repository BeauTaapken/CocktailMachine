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
            receivedPrint = false;
            arduinoConnection.SendMessage("fingerprint");
            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.Elapsed < TimeSpan.FromSeconds(10) && receivedPrint == false)
            {
                if (arduinoConnection.MessageReceived)
                {
                    receivedPrint = true;
                }
            }
            s.Stop();

            if (receivedPrint)
            {
                MessageBox.Show("Fingerprint has been scanned");
            }
            else
            {
                MessageBox.Show("Fingerprint not scanned. Try again");
            }
        }

        public void addUserAccount(string name, DateTime age, int fingerprint)
        {
            db.addUserInfo(name, age, fingerprint);
        }
    }
}
