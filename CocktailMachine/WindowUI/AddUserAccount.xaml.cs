using CocktailMachine.Class;
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
        private User user;

        public AddUserAccount()
        {
            InitializeComponent();
            btUserHistory.Click += OpenUserHistory;
            btAskForFingerprint.Click += btAskForFingerprint_Click;
            btAddUser.Click += btAddUserAccount_Click;
            dpAge.SelectedDateChanged += dpAge_SelectedDateChanged;
        }

        public void setPrivates(UserHistory userHistory, User user)
        {
            this.userHistory = userHistory;
            this.user = user;
        }

        private void btAskForFingerprint_Click(object sender, RoutedEventArgs e)
        {
            user.getFingerprint();
        }

        private void dpAge_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            dpAge.Text = dpAge.DisplayDate.Year.ToString() + "-" + dpAge.DisplayDate.Month.ToString() + "-" + dpAge.DisplayDate.Day.ToString();
        }

        private void btAddUserAccount_Click(object sender, RoutedEventArgs e)
        {
            if (tbName.Text != String.Empty && dpAge.Text != String.Empty && iudFingerprint.Text != String.Empty)
            {
                user.addUserAccount(tbName.Text, Convert.ToDateTime(dpAge.Text), Convert.ToInt32(iudFingerprint.Text));
            }
            else
            {
                MessageBox.Show("Not every value has been entered");
            }
        }

        private void OpenUserHistory(object sender, RoutedEventArgs e)
        {
            userHistory.Show();
            this.Hide();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
