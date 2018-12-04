using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailMachine.Class
{
    class User
    {
        //TODO make function to add user to database and function to wait until the fingerprint scanner has been used and a value has been sent back. Made by Mark

        public bool CheckFingerprintScanner()
        {
            //Check if the fingerprint scanner has been used
            return true;
        }
        
        public void AddUserToDB (string username, DateTime age, string fingerprintCode)
        {
            if (CheckFingerprintScanner())
            {
                //add username, age and fingerprintCode to database
            }
        }
         
       
    }
}
