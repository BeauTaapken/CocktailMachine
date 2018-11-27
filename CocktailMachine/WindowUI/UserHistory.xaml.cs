using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CocktailMachine.Window;

namespace CocktailMachine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UserHistory
    {
        AddUserAccount addUserAccount = new AddUserAccount();
        public UserHistory()
        {
            InitializeComponent();
            addUserAccount.setPrivates(this);
            btAddUser.Click += OpenAddUserAccount;
        }

        public void OpenAddUserAccount(object sender, RoutedEventArgs e)
        {
            addUserAccount.Show();
            this.Hide();
        }
    }
}
