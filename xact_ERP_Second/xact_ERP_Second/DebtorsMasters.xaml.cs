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
    public partial class DebtorsMasters : Window
    {
        private string _editingAccountCode = null;
        private bool _isEditConfirmed = false;
        private const string SearchPlaceholder = "Search by debtor name...";

        public DebtorsMasters()
        {
            InitializeComponent();
            SetSearchPlaceholder();
            LoadDebtors();

            // Deactivate button disabled by default
            DeactivateButton.IsEnabled = false;
        }

        // ================= LOAD + SEARCH =================
        private void LoadDebtors(string searchName = "")
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
                    "WHERE Status = 'Active'";

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
                MessageBox.Show("Error loading debtors: " + ex.Message);
            }
        }

        // ================= ROW CLICK → CONFIRM → POPULATE FORM =================
        private void DebtorsTable_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DebtorsTable.SelectedItem is not Debtor d)
                return;

            var result = MessageBox.Show(
                $"Are you sure you want to edit debtor '{d.Name}'?",
                "Confirm Edit",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                DebtorsTable.SelectedItem = null;
                return;
            }

            _editingAccountCode = d.AccountCode;
            _isEditConfirmed = true;

            Name.Text = d.Name;
            DeliveryAddress.Text = d.DeliveryAddress;
            InvoiceAddress.Text = d.InvoiceAddress;
            PostalAddress.Text = d.PostalAddress;
            AccHolder.Text = d.AccountHolder;
            AccNumber.Text = d.AcoountNumber.ToString();
            BranchName.Text = d.Branch;

            // ENABLE DEACTIVATE BUTTON 
            DeactivateButton.IsEnabled = true;
        }

        // ================= CLEAR =================
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

            // Reset edit mode
            _editingAccountCode = null;
            _isEditConfirmed = false;

            // Disable deactivate button
            DeactivateButton.IsEnabled = false;

            // Clear search
            SetSearchPlaceholder();
            LoadDebtors();

            if (DebtorsTable != null)
                DebtorsTable.SelectedItem = null;
        }

        // ================= SAVE =================
        private void SaveTransaction_Click(object sender, RoutedEventArgs e)
        {
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

            if (!int.TryParse(AccNumber.Text.Trim(), out int accNo))
            {
                MessageBox.Show("Account Number must be numeric.");
                return;
            }

            try
            {
                using var conn = Database.GetConnection();
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
                        "AccountHolder, AcoountNumber, Branch, Balance, SalesYearToDate, CostYearTODate, Status) " +
                        "VALUES (@AccountCode, @Name, @DeliveryAddress, @InvoiceAddress, @PostalAddress, " +
                        "@AccountHolder, @AcoountNumber, @Branch, @Balance, @SalesYearToDate, @CostYearTODate, 'Active')";

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

                Cancel_Click(null, null);
                LoadDebtors();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving debtor: " + ex.Message);
            }
        }

        // ================= DEACTIVATE =================
        private void Deactivate_Click(object sender, RoutedEventArgs e)
        {
            if (_editingAccountCode == null)
                return;

            var confirm = MessageBox.Show(
                "Are you sure you want to deactivate this debtor?",
                "Confirm Deactivate",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                string sql = "UPDATE Debtors_Masters SET Status='Inactive' WHERE AccountCode=@AccountCode";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@AccountCode", _editingAccountCode);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Debtor deactivated successfully!");

                // Refresh table
                LoadDebtors();

                // Clear form and disable Deactivate button
                Cancel_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deactivating debtor: " + ex.Message);
            }
        }

        // ================= SEARCH =================
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DebtorsTable == null) return; // safety check

            if (IsSearchPlaceholderActive() || string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                LoadDebtors();
            }
            else
            {
                LoadDebtors(SearchBox.Text.Trim());
            }
        }

        private void SetSearchPlaceholder()
        {
            SearchBox.Text = SearchPlaceholder;
            SearchBox.Foreground = Brushes.Gray;
        }

        private bool IsSearchPlaceholderActive()
        {
            return SearchBox.Text == SearchPlaceholder;
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (IsSearchPlaceholderActive())
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

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            
            Prev_Debtor prevWindow = new Prev_Debtor();
            prevWindow.ShowDialog();
            //LoadDebtors();
        }

    }
}
