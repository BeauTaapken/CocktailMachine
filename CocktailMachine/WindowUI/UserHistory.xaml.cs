﻿using System;
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

        public UserHistory()
        {
            InitializeComponent();

            MessageBuilder messageBuilder = new MessageBuilder('#', '%');
            arduinoConnection = new ArduinoConnection("COM10", 9600, messageBuilder);

            tbHistorySearch.TextChanged += tbSearch_TextChanged;
            arduinoConnection.ConnectArduino();

            addUserAccount.setPrivates(this);
            btAddUser.Click += OpenAddUserAccount;

            getUserHistory.FillUserHistory(dgUserHistory);
        }

        private void tbSearch_TextChanged(object sender, RoutedEventArgs e)
        {
            getUserHistory.SearchHistory(dgUserHistory, tbHistorySearch);
        }

        private void OpenAddUserAccount(object sender, RoutedEventArgs e)
        {
            addUserAccount.Show();
            this.Hide();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
