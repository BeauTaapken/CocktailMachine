using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace CocktailMachine.Class
{
    class GetUserHistory
    {
        //TODO Add function for returning the userhistory of today and adding items to the userhistory table(in database). Made by Beau
        private Database db = new Database();
        private DataTable dtHistory = new DataTable();
        private DataTable dtSearch = new DataTable();

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
            string searchValue = tbSearch.Text;
            if (searchValue != String.Empty && dgHistory.ItemsSource is DataView)
            {
                dtSearch.Rows.Clear();
                dtSearch.Columns.Clear();
                dtSearch.Columns.Add("Name", typeof(String));
                dtSearch.Columns.Add("Age", typeof(DateTime));
                dtSearch.Columns.Add("Price", typeof(int));
                dtSearch.Columns.Add("Date", typeof(DateTime));
                dtSearch.Columns.Add("Cocktail", typeof(String));
                dtSearch.Columns.Add("Description", typeof(String));
                foreach (DataRowView drvRow in (DataView)dtHistory.DefaultView)
                {
                    DataRow test = drvRow.Row;
                    string testing = (string) drvRow["Name"];
                    if (testing.ToLower().StartsWith(searchValue))
                    {
                        dtSearch.Rows.Add(test.ItemArray);
                    }
                }

                dgHistory.ItemsSource = String.Empty;
                dgHistory.ItemsSource = dtSearch.DefaultView;
            }
            else if (searchValue == String.Empty)
            {
                dgHistory.ItemsSource = String.Empty;
                dgHistory.ItemsSource = dtHistory.DefaultView;
            }
        }

        public void datagridToJson()
        {
            string output = JsonConvert.SerializeObject(dtHistory);
            File.WriteAllText("User_History.json", output);
        }
    }
}
