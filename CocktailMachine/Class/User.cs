using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CocktailMachine.Class
{
    class User
    {
        private ArduinoConnection arduinoConnection;
        public User(ArduinoConnection arduinoConnection)
        {
            this.arduinoConnection = arduinoConnection;
        }
        private Database DB = new Database();
        //private void setPrivate(ArduinoConnection test)
        //{
        //    arduinoConnection = test;

        //}

        public string getFingerprint()
        {
            arduinoConnection.SendMessage("test");
            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.Elapsed < TimeSpan.FromSeconds(600))
            {
                
            }

            s.Stop();
            //Check for fingerprint scanner data
            return "";
        }
        
        public void AddUserToDB (string username, DateTime age)
        {
            while (getFingerprint() != "")
            {
                //add username, age and fingerprintCode to database
                DB.uploadUserInfo(username, age, getFingerprint());
            }
            DB.uploadUserInfo(username, age, getFingerprint());
        }


    }
}
