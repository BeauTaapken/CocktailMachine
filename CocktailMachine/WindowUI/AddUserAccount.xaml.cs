﻿using CocktailMachine.Class;
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
using System.Windows.Shapes;

namespace CocktailMachine.Window
{
    /// <summary>
    /// Interaction logic for AddUserAccount.xaml
    /// </summary>
    public partial class AddUserAccount
    {
        private UserHistory userHistory;
        User user = new User();
        public AddUserAccount()
        {
            InitializeComponent();
            btUserHistory.Click += OpenUserHistory;
            btAskForFingerprint.Click += test;
        }

        private void test(object sender, RoutedEventArgs e)
        {
            user.getFingerprint();
        }

        public void OpenUserHistory(object sender, RoutedEventArgs e)
        {
            userHistory.Show();
            this.Hide();
        }

        public void setPrivates(UserHistory userHistory)
        {
            this.userHistory = userHistory;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
