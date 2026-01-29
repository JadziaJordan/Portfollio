using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using xact_ERP_Second.Data;
using xact_ERP_Second.Models;

namespace xact_ERP_Second
{
    public partial class StockMaster : Window
    {
        private string _editingStockCode = null;
        private bool _isEditConfirmed = false;
        private const string SearchPlaceholder = "Search by stock name...";

        public StockMaster()
        {
            InitializeComponent();
            SetSearchPlaceholder();
            LoadStock();
        }

        // ================= LOAD + SEARCH =================
        private void LoadStock(string searchName = "")
        {
            if (StockTable == null) return;

            try
            {
                var list = new List<Stock>();

                using var conn = Database.GetConnection();
                conn.Open();

                // Always load only active stock
                string sql =
                    "SELECT StockCode, StockName, StockDescription, Brand, Category, Location, " +
                    "Cost, SellingPrice, TotalPurchasedExclVat, TotalSalesExclVat, QntyPurchased, QntySold, StockOnHand, Status " +
                    "FROM Stock_Masters " +
                    "WHERE Status = 'Active'";

                if (!string.IsNullOrWhiteSpace(searchName) && searchName != SearchPlaceholder)
                    sql += " AND StockName LIKE @search";

                using var cmd = new SqlCommand(sql, conn);

                if (!string.IsNullOrWhiteSpace(searchName) && searchName != SearchPlaceholder)
                    cmd.Parameters.AddWithValue("@search", $"%{searchName}%");

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Stock
                    {
                        StockCode = reader["StockCode"].ToString(),
                        StockName = reader["StockName"].ToString(),
                        StockDescription = reader["StockDescription"].ToString(),
                        Brand = reader["Brand"].ToString(),
                        Category = reader["Category"].ToString(),
                        Location = reader["Location"].ToString(),
                        Cost = reader["Cost"] != DBNull.Value ? Convert.ToDecimal(reader["Cost"]) : 0m,
                        SellingPrice = reader["SellingPrice"] != DBNull.Value ? Convert.ToDecimal(reader["SellingPrice"]) : 0m,
                        TotalPurchasedExclVat = reader["TotalPurchasedExclVat"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPurchasedExclVat"]) : 0m,
                        TotalSalesExclVat = reader["TotalSalesExclVat"] != DBNull.Value ? Convert.ToDecimal(reader["TotalSalesExclVat"]) : 0m,
                        QntyPurchased = reader["QntyPurchased"] != DBNull.Value ? Convert.ToInt32(reader["QntyPurchased"]) : 0,
                        QntySold = reader["QntySold"] != DBNull.Value ? Convert.ToInt32(reader["QntySold"]) : 0,
                        StockOnHand = reader["StockOnHand"] != DBNull.Value ? Convert.ToInt32(reader["StockOnHand"]) : 0,
                        Status = reader["Status"].ToString()
                    });
                }

                StockTable.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading stock: " + ex.Message);
            }
        }

        // ================= ROW CLICK =================
        private void StockTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StockTable.SelectedItem is not Stock s) return;

            var result = MessageBox.Show(
                $"Are you sure you want to edit '{s.StockName}'?",
                "Confirm Edit",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                StockTable.SelectedItem = null;
                return;
            }

            _editingStockCode = s.StockCode;
            _isEditConfirmed = true;

            StockName.Text = s.StockName;
            StockBrand.Text = s.Brand;
            StockCategory.Text = s.Category;
            StockLoctaion.Text = s.Location;

            // Add permanent R: when loading
            StockCost.Text = $"R:{s.Cost:F2}";
            StockPrice.Text = $"R:{s.SellingPrice:F2}";

            StockDesc.Text = s.StockDescription;
        }

        // ================= CLEAR =================
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            StockName.Text = "";
            StockBrand.Text = "";
            StockCategory.Text = "";
            StockLoctaion.Text = "";
            StockCost.Text = "R:0.00";
            StockPrice.Text = "R:0.00";
            StockDesc.Text = "";

            _editingStockCode = null;
            _isEditConfirmed = false;

            StockTable.SelectedItem = null;

            SetSearchPlaceholder();
            LoadStock();
        }

        // ================= SAVE =================
        private void SaveStock_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StockName.Text) ||
                string.IsNullOrWhiteSpace(StockBrand.Text) ||
                string.IsNullOrWhiteSpace(StockCategory.Text) ||
                string.IsNullOrWhiteSpace(StockLoctaion.Text) ||
                string.IsNullOrWhiteSpace(StockCost.Text.Replace("R:", "").Trim()) ||
                string.IsNullOrWhiteSpace(StockPrice.Text.Replace("R:", "").Trim()))
            {
                MessageBox.Show("Please fill in all required fields!");
                return;
            }

            if (!decimal.TryParse(StockCost.Text.Replace("R:", "").Trim(), out decimal cost))
            {
                MessageBox.Show("Cost must be numeric.");
                return;
            }

            // Prevent 0 cost
            if (cost <= 0)
            {
                MessageBox.Show("Cost cannot be zero or negative.");
                return;
            }

            if (!decimal.TryParse(StockPrice.Text.Replace("R:", "").Trim(), out decimal sellingPrice))
            {
                MessageBox.Show("Selling Price must be numeric.");
                return;
            }

            // Prevent 0 selling price
            if (sellingPrice <= 0)
            {
                MessageBox.Show("Selling Price cannot be zero or negative.");
                return;
            }

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                // ===== UPDATE =====
                if (_editingStockCode != null && _isEditConfirmed)
                {
                    string updateSql =
                        "UPDATE Stock_Masters SET " +
                        "StockName=@StockName, StockDescription=@StockDescription, Brand=@Brand, " +
                        "Category=@Category, Location=@Location, Cost=@Cost, SellingPrice=@SellingPrice, Status='Active' " +
                        "WHERE StockCode=@StockCode";

                    using var cmd = new SqlCommand(updateSql, conn);
                    cmd.Parameters.AddWithValue("@StockCode", _editingStockCode);
                    cmd.Parameters.AddWithValue("@StockName", StockName.Text.Trim());
                    cmd.Parameters.AddWithValue("@StockDescription", StockDesc.Text.Trim());
                    cmd.Parameters.AddWithValue("@Brand", StockBrand.Text.Trim());
                    cmd.Parameters.AddWithValue("@Category", StockCategory.Text.Trim());
                    cmd.Parameters.AddWithValue("@Location", StockLoctaion.Text.Trim());
                    cmd.Parameters.AddWithValue("@Cost", cost);
                    cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Stock updated successfully!");
                }
                // ===== INSERT =====
                else
                {
                    string getMax = "SELECT TOP 1 StockCode FROM Stock_Masters ORDER BY StockCode DESC";
                    string nextCode = "STK01";

                    using var cmdMax = new SqlCommand(getMax, conn);
                    var result = cmdMax.ExecuteScalar();
                    if (result != null)
                    {
                        string lastCode = result.ToString();
                        int number = int.Parse(lastCode.Substring(3)) + 1;
                        nextCode = "STK" + number.ToString("D2");
                    }

                    string insertSql =
                        "INSERT INTO Stock_Masters " +
                        "(StockCode, StockName, StockDescription, Brand, Category, Location, Cost, SellingPrice, " +
                        "TotalPurchasedExclVat, TotalSalesExclVat, QntyPurchased, QntySold, StockOnHand, Status) " +
                        "VALUES (@StockCode, @StockName, @StockDescription, @Brand, @Category, @Location, @Cost, @SellingPrice, " +
                        "@TotalPurchasedExclVat, @TotalSalesExclVat, @QntyPurchased, @QntySold, @StockOnHand, 'Active')";

                    using var cmd = new SqlCommand(insertSql, conn);

                    cmd.Parameters.AddWithValue("@StockCode", nextCode);
                    cmd.Parameters.AddWithValue("@StockName", StockName.Text.Trim());
                    cmd.Parameters.AddWithValue("@StockDescription", StockDesc.Text.Trim());
                    cmd.Parameters.AddWithValue("@Brand", StockBrand.Text.Trim());
                    cmd.Parameters.AddWithValue("@Category", StockCategory.Text.Trim());
                    cmd.Parameters.AddWithValue("@Location", StockLoctaion.Text.Trim());
                    cmd.Parameters.AddWithValue("@Cost", cost);
                    cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);

                    // Defaults
                    cmd.Parameters.AddWithValue("@TotalPurchasedExclVat", 0m);
                    cmd.Parameters.AddWithValue("@TotalSalesExclVat", 0m);
                    cmd.Parameters.AddWithValue("@QntyPurchased", 0);
                    cmd.Parameters.AddWithValue("@QntySold", 0);
                    cmd.Parameters.AddWithValue("@StockOnHand", 0);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Stock added successfully!");
                }

                Cancel_Click(null, null);
                LoadStock();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving stock: " + ex.Message);
            }
        }

        // ================= DELETE/DEACTIVATE =================
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (_editingStockCode == null) return;

            var confirm = MessageBox.Show(
                "Are you sure you want to deactivate this stock?",
                "Confirm Deactivate",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                string sql = "UPDATE Stock_Masters SET Status='Inactive' WHERE StockCode=@StockCode";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@StockCode", _editingStockCode);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Stock deactivated successfully!");
                Cancel_Click(null, null);
                LoadStock();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deactivating stock: " + ex.Message);
            }
        }

        // ================= SEARCH =================
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (StockTable == null) return;

            if (SearchBox.Text == SearchPlaceholder || string.IsNullOrWhiteSpace(SearchBox.Text))
                LoadStock();
            else
                LoadStock(SearchBox.Text.Trim());
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
                SetSearchPlaceholder();
        }

        private void Deleted_Click(object sender, RoutedEventArgs e)
        {
            Prev_Stock prevWindow = new Prev_Stock();
            prevWindow.ShowDialog();
        }

        // ================= PRICE BOXES: PERMANENT R: =================
        private void Price_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Prevent deleting the "R:" part
            if ((tb.SelectionStart <= 2 && (e.Key == Key.Back || e.Key == Key.Delete)) ||
                (tb.SelectionStart < 2 && e.Key == Key.Left))
            {
                e.Handled = true;
            }
        }

        private void Price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Only allow numbers and dot after R:
            TextBox tb = sender as TextBox;
            if (!Regex.IsMatch(e.Text, @"[0-9.]"))
                e.Handled = true;
        }

        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            // Ensure "R:" is always at start
            if (!tb.Text.StartsWith("R:"))
            {
                int sel = tb.SelectionStart;
                tb.Text = "R:" + tb.Text.Replace("R:", "");
                tb.SelectionStart = sel + 2;
            }
        }
    }
}
