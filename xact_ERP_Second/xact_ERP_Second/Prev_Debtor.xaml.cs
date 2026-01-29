using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using xact_ERP_Second.Data;
using xact_ERP_Second.Models;

namespace xact_ERP_Second
{
    public partial class Prev_Debtor : Window
    {
        private const string SearchPlaceholder = "Search by Debtor name...";

        public Prev_Debtor()
        {
            InitializeComponent();

            // Run after InitializeComponent to ensure DebtorsTable is ready
            Loaded += Prev_Debtor_Loaded;
        }

        private void Prev_Debtor_Loaded(object sender, RoutedEventArgs e)
        {
            SetSearchPlaceholder();
            LoadPrevDebtors();
        }

        // ================= LOAD + SEARCH =================
        private void LoadPrevDebtors(string searchName = "")
        {
            if (DebtorsTable == null) return; // safety check

            try
            {
                var list = new List<Debtor>();

                using var conn = Database.GetConnection();
                conn.Open();

                string sql =
                    "SELECT AccountCode, Name, DeliveryAddress, InvoiceAddress, PostalAddress, " +
                    "AccountHolder, AcoountNumber, Branch, Balance, SalesYearToDate, CostYearTODate " +
                    "FROM Debtors_Masters " +
                    "WHERE Status = 'Inactive'";

                if (!string.IsNullOrWhiteSpace(searchName))
                    sql += " AND Name LIKE @search";

                using var cmd = new SqlCommand(sql, conn);
                if (!string.IsNullOrWhiteSpace(searchName))
                    cmd.Parameters.AddWithValue("@search", $"%{searchName}%");

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Debtor
                    {
                        AccountCode = reader["AccountCode"].ToString(),
                        Name = reader["Name"].ToString(),
                        DeliveryAddress = reader["DeliveryAddress"].ToString(),
                        InvoiceAddress = reader["InvoiceAddress"].ToString(),
                        PostalAddress = reader["PostalAddress"].ToString(),
                        AccountHolder = reader["AccountHolder"].ToString(),
                        AcoountNumber = reader["AcoountNumber"] != DBNull.Value
                            ? Convert.ToInt32(reader["AcoountNumber"])
                            : 0,
                        Branch = reader["Branch"].ToString(),
                        Balance = reader["Balance"] != DBNull.Value
                            ? Convert.ToDecimal(reader["Balance"])
                            : 0m,
                        SalesYearToDate = reader["SalesYearToDate"] != DBNull.Value
                            ? Convert.ToDecimal(reader["SalesYearToDate"])
                            : 0m,
                        CostYearToDate = reader["CostYearTODate"] != DBNull.Value
                            ? Convert.ToDecimal(reader["CostYearTODate"])
                            : 0m
                    });
                }

                DebtorsTable.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading previous debtors: " + ex.Message);
            }
        }

        // ================= CURRENT DEBTORS BUTTON =================
        private void Current_Click(object sender, RoutedEventArgs e)
        {
            // Go back to DebtorsMasters window
            foreach (Window win in Application.Current.Windows)
            {
                if (win is DebtorsMasters dm)
                {
                    dm.Focus();
                    break;
                }
            }

            this.Close();
        }

        // ================= SEARCH BOX =================
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text) || SearchBox.Text == SearchPlaceholder)
                LoadPrevDebtors();
            else
                LoadPrevDebtors(SearchBox.Text.Trim());
        }

        private void SetSearchPlaceholder()
        {
            SearchBox.Text = SearchPlaceholder;
            SearchBox.Foreground = Brushes.Gray;
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == SearchPlaceholder)
            {
                SearchBox.Text = "";
                SearchBox.Foreground = Brushes.Black;
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SetSearchPlaceholder();
            }
        }

        private void DebtorsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optional: handle selection logic if needed
        }
    }
}
