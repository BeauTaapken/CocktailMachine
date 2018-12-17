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
        private DataGrid dgHistory;
        private TextBox tbSearch;
        private int totalPrice;

        public void setPrivates(DataGrid dghistory, TextBox tbsearch)
        {
            dgHistory = dghistory;
            tbSearch = tbsearch;
        }

        //Code for filling the datagrid on the userhistory screen
        public void FillUserHistory()
        {
            dtHistory.Clear();
            dtHistory = db.getUserHistory();
            dgHistory.ItemsSource = String.Empty;
            if (dtHistory.Rows.Count > 0)
            {
                dgHistory.ItemsSource = dtHistory.DefaultView;
            }
        }
        
        //Function for searching through the userhistory datagrid
        public void SearchHistory()
        {
            string searchValue = tbSearch.Text;
            totalPrice = 0;
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
                    DataRow dtRows = drvRow.Row;
                    string stName = (string) drvRow["Name"];
                    if (stName.ToLower().StartsWith(searchValue))
                    {
                        dtSearch.Rows.Add(dtRows.ItemArray);
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

        //Function for saving the datagrid information to a json file.
        public void datagridToJson()
        {
            string output = JsonConvert.SerializeObject(dtHistory);
            string path = AppDomain.CurrentDomain.BaseDirectory + "/User_History_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".json";
            File.WriteAllText(path, output);
            MessageBox.Show("Json file has been created.");
        }
    }
}
