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
using System.Windows.Threading;
using CocktailMachine.Class;
using CocktailMachine.Window;

namespace CocktailMachine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UserHistory
    {
        private AddUserAccount addUserAccount = new AddUserAccount();
        private ArduinoConnection arduinoConnection;
        private GetUserHistory getUserHistory = new GetUserHistory();
        private SetupArduino setupArduino = new SetupArduino();
        private User User;

        public UserHistory()
        {
            InitializeComponent();

            MessageBuilder messageBuilder = new MessageBuilder('#', '%');
            arduinoConnection = new ArduinoConnection("COM8", 9600, messageBuilder);

            User = new User(arduinoConnection);

            setupArduino.setPrivate(arduinoConnection);

            tbHistorySearch.TextChanged += tbSearch_TextChanged;
            btJSON.Click += btSaveJson_Clicked;
            arduinoConnection.ConnectArduino();

            addUserAccount.setPrivates(this);
            getUserHistory.setPrivates(dgUserHistory, tbHistorySearch);

            btAddUser.Click += OpenAddUserAccount;

            getUserHistory.FillUserHistory();
            setupArduino.SendAllCocktailNamesToArduino();
        }

        private void tbSearch_TextChanged(object sender, RoutedEventArgs e)
        {
            getUserHistory.SearchHistory();
        }

        private void btSaveJson_Clicked(object sender, RoutedEventArgs e)
        {
            getUserHistory.datagridToJson();
        }

        private void OpenAddUserAccount(object sender, RoutedEventArgs e)
        {
            User = new User(arduinoConnection);
            addUserAccount.Show();
            this.Hide();
        }

        public void UserFingerprint()
        {
            User.getFingerprint();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
