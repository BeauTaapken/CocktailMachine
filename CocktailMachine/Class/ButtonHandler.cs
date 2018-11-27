using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CocktailMachine.Window;

namespace CocktailMachine
{
    public class ButtonHandler
    {
        private AddUserAccount addUserAccount;
        private UserHistory userHistory;

        public void setPrivates(AddUserAccount addUserAccount, UserHistory userHistory)
        {
            this.addUserAccount = addUserAccount;
            this.userHistory = userHistory;
        }

        public void OpenUserHistory(object sender, RoutedEventArgs e)
        {
            userHistory.Show();
            addUserAccount.Hide();
        }

        public void OpenAddUserAccount(object sender, RoutedEventArgs e)
        {
            addUserAccount.Show();
            userHistory.Hide();
        }
    }
}
