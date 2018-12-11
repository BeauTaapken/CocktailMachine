using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailMachine.Class
{
    class User
    {
        //Doesn't work
        //private ArduinoConnection arduinoConnection;

        //TODO make function to add user to database and function to wait until the fingerprint scanner has been used and a value has been sent back. Made by Mark
        private Database DB = new Database();


        //private void setPrivate(ArduinoConnection test)
        //{
        //    arduinoConnection = test;

        //}

        public string getFingerprint()
        {
            //arduinoConnection.SendMessage("test");
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
