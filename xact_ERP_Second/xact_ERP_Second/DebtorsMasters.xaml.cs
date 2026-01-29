using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using xact_ERP_Second.Data;
using xact_ERP_Second.Models;

namespace xact_ERP_Second
{
    public partial class DebtorsMasters : Window
    {
        // Stores AccountCode of the debtor currently being edited.
        
        private string _editingAccountCode = null;

     
        private bool _isEditConfirmed = false;

        
        private const string SearchPlaceholder = "Search by debtor name...";

        public DebtorsMasters()
        {
            InitializeComponent();

            SetSearchPlaceholder();

            LoadDebtors();
        }

      
        // LOAD DEBTORS FROM DATABASE
       
        private void LoadDebtors(string searchName = "")
        {
            try
            {
                var list = new List<Debtor>();

                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    // Base SELECT query
                    string sql =
                        "SELECT AccountCode, Name, DeliveryAddress, InvoiceAddress, PostalAddress, " +
                        "AccountHolder, AcoountNumber, Branch, Balance, SalesYearToDate, CostYearTODate " +
                        "FROM Debtors_Masters";

                    // If user typed something dd WHERE clause
                    if (!string.IsNullOrWhiteSpace(searchName))
                        sql += " WHERE Name LIKE @search";

                    using var cmd = new SqlCommand(sql, conn);

                    // Add parameter only when searching
                    if (!string.IsNullOrWhiteSpace(searchName))
                        cmd.Parameters.AddWithValue("@search", $"%{searchName}%");

                    using var reader = cmd.ExecuteReader();

                    // Read rows into Debtor objects
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
                }

                // Bind list to DataGrid
                DebtorsTable.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading debtors: " + ex.Message);
            }
        }

    
        // ROW CLICK → CONFIRM → POPULATE FORM
 
        private void DebtorsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DebtorsTable.SelectedItem is not Debtor d)
                return;

            // Ask user if they really want to edit this debtor
            var result = MessageBox.Show(
                $"Are you sure you want to edit debtor '{d.Name}'?",
                "Confirm Edit",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                // User cancelled → deselect row
                DebtorsTable.SelectedItem = null;
                return;
            }

            // User confirmed edit mode
            _editingAccountCode = d.AccountCode;
            _isEditConfirmed = true;

            //cannot edit these fields, AccountCodes stre=ing Balance decimal , SalesYearToDatedecimal, CostYearTODatedecimal
            // Populate TOP input fields
            Name.Text = d.Name;
            DeliveryAddress.Text = d.DeliveryAddress;
            InvoiceAddress.Text = d.InvoiceAddress;
            PostalAddress.Text = d.PostalAddress;
            AccHolder.Text = d.AccountHolder;
            AccNumber.Text = d.AcoountNumber.ToString();
            BranchName.Text = d.Branch;
            //BranchCode.Text = ""; // not stored in DB
        }

       
       
        // CLEAR BUTTON

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Clear all input fields
            Name.Text = "";
            DeliveryAddress.Text = "";
            InvoiceAddress.Text = "";
            PostalAddress.Text = "";
            AccHolder.Text = "";
            AccNumber.Text = "";
            BranchName.Text = "";
            //BranchCode.Text = "";

            // Reset edit mode
            _editingAccountCode = null;
            _isEditConfirmed = false;

            // Deselect DataGrid row
            DebtorsTable.SelectedItem = null;

            // Reset search placeholder
            SetSearchPlaceholder();
        }

 
        // SAVE BUTTON → INSERT OR UPDATE

        private void SaveTransaction_Click(object sender, RoutedEventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(Name.Text) ||
                string.IsNullOrWhiteSpace(DeliveryAddress.Text) ||
                string.IsNullOrWhiteSpace(InvoiceAddress.Text) ||
                string.IsNullOrWhiteSpace(AccHolder.Text) ||
                string.IsNullOrWhiteSpace(AccNumber.Text) ||
                string.IsNullOrWhiteSpace(BranchName.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            // Ensure Account Number is numeric
            if (!int.TryParse(AccNumber.Text.Trim(), out int accNo))
            {
                MessageBox.Show("Account Number must be numeric.");
                return;
            }

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    // ========== UPDATE MODE ==========
                    if (_editingAccountCode != null && _isEditConfirmed)
                    {
                        var confirm = MessageBox.Show(
                            "Save changes to this debtor?",
                            "Confirm Update",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (confirm != MessageBoxResult.Yes)
                            return;

                        string updateSql =
                            "UPDATE Debtors_Masters SET " +
                            "Name=@Name, DeliveryAddress=@DeliveryAddress, InvoiceAddress=@InvoiceAddress, " +
                            "PostalAddress=@PostalAddress, AccountHolder=@AccountHolder, AcoountNumber=@AcoountNumber, " +
                            "Branch=@Branch " +
                            "WHERE AccountCode=@AccountCode";

                        using var cmd = new SqlCommand(updateSql, conn);

                        cmd.Parameters.AddWithValue("@AccountCode", _editingAccountCode);
                        cmd.Parameters.AddWithValue("@Name", Name.Text.Trim());
                        cmd.Parameters.AddWithValue("@DeliveryAddress", DeliveryAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@InvoiceAddress", InvoiceAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@PostalAddress",
                            string.IsNullOrWhiteSpace(PostalAddress.Text)
                                ? (object)DBNull.Value
                                : PostalAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@AccountHolder", AccHolder.Text.Trim());
                        cmd.Parameters.AddWithValue("@AcoountNumber", accNo);
                        cmd.Parameters.AddWithValue("@Branch", BranchName.Text.Trim());

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Debtor updated successfully!");
                    }

                    // ========== INSERT MODE ==========
                    else
                    {
                        string getMax =
                            "SELECT TOP 1 AccountCode FROM Debtors_Masters ORDER BY AccountCode DESC";

                        string nextCode = "AC01";

                        using (var cmdMax = new SqlCommand(getMax, conn))
                        {
                            var result = cmdMax.ExecuteScalar();
                            if (result != null)
                            {
                                string lastCode = result.ToString();
                                int number = int.Parse(lastCode.Substring(2)) + 1;
                                nextCode = "AC" + number.ToString("D2");
                            }
                        }

                        string insertSql =
                            "INSERT INTO Debtors_Masters " +
                            "(AccountCode, Name, DeliveryAddress, InvoiceAddress, PostalAddress, " +
                            "AccountHolder, AcoountNumber, Branch, Balance, SalesYearToDate, CostYearTODate) " +
                            "VALUES (@AccountCode, @Name, @DeliveryAddress, @InvoiceAddress, @PostalAddress, " +
                            "@AccountHolder, @AcoountNumber, @Branch, @Balance, @SalesYearToDate, @CostYearTODate)";

                        using var cmd = new SqlCommand(insertSql, conn);

                        cmd.Parameters.AddWithValue("@AccountCode", nextCode);
                        cmd.Parameters.AddWithValue("@Name", Name.Text.Trim());
                        cmd.Parameters.AddWithValue("@DeliveryAddress", DeliveryAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@InvoiceAddress", InvoiceAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@PostalAddress",
                            string.IsNullOrWhiteSpace(PostalAddress.Text)
                                ? (object)DBNull.Value
                                : PostalAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@AccountHolder", AccHolder.Text.Trim());
                        cmd.Parameters.AddWithValue("@AcoountNumber", accNo);
                        cmd.Parameters.AddWithValue("@Branch", BranchName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Balance", 0m);
                        cmd.Parameters.AddWithValue("@SalesYearToDate", 0m);
                        cmd.Parameters.AddWithValue("@CostYearTODate", 0m);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Debtor saved successfully!");
                    }
                }

                // Reset form + reload grid
                Cancel_Click(null, null);
                LoadDebtors();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving debtor: " + ex.Message);
            }
        }


        // SEARCH PLACEHOLDER HELPERS

        // SEARCH BUTTON
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            // If placeholder is showing → load all
            if (IsSearchPlaceholderActive())
            {
                LoadDebtors();
                return;
            }

            // Otherwise search by typed text
            LoadDebtors(SearchBox.Text.Trim());
        }



        // Set grey placeholder text
        private void SetSearchPlaceholder()
        {
            SearchBox.Text = SearchPlaceholder;
            SearchBox.Foreground = Brushes.Gray;
        }

        // Check if placeholder is currently showing
        private bool IsSearchPlaceholderActive()
        {
            return SearchBox.Text == SearchPlaceholder;
        }

        // User clicks inside → remove placeholder
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (IsSearchPlaceholderActive())
            {
                SearchBox.Text = "";
                SearchBox.Foreground = Brushes.Black;
            }
        }

        // User leaves empty → restore placeholder
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SetSearchPlaceholder();
            }
        }
    }
}
