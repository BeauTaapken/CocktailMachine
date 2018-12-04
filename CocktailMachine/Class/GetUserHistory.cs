using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CocktailMachine.Class
{
    class GetUserHistory
    {
        //TODO Add function for returning the userhistory of today and adding items to the userhistory table(in database). Made by Beau
        private Database db = new Database();
        private DataTable dtHistory = new DataTable();

        //Code for filling the datagrid on the userhistory screen
        public void FillUserHistory(DataGrid dgHistory)
        {
            dtHistory.Clear();
            dtHistory = db.getUserHistory();
            dgHistory.ItemsSource = String.Empty;
            if (dtHistory.Rows.Count > 0)
            {
                dgHistory.ItemsSource = dtHistory.DefaultView;
            }
        }

        public void SearchHistory(DataGrid dgHistory, TextBox tbSearch)
        {
            string test = tbSearch.Text;
            if (test != String.Empty && dgHistory.ItemsSource is DataView)
            {
                foreach (DataRowView drRow in (DataView)dgHistory.ItemsSource)
                {
                    string testing = (string) drRow["Name"];
                    if (!(testing.ToLower().StartsWith(test)))
                    {
                        
                        MessageBox.Show(drRow["Name"].ToString());
                    }
                }
            }
        }
    }
}
