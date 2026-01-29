using System.Windows;
using System.Windows.Controls;
using xact_ERP_Second.Data;
using System.Windows.Media;

using xact_ERP_Second.Models;
using Microsoft.Data.SqlClient;


namespace xact_ERP_Second
{
   
    public partial class StockMaster : Window
    {

        private string _editingStockCode = null;
        //confirm edit 
        private bool _isEditConfirmed = false;

        private const string SearchPlaceholder = "Search by debtor name...";


        public StockMaster()
        {
            InitializeComponent();
            SetSearchPlaceholder();
        }


        //calls load function and sets placeholder
        private void SetSearchPlaceholder()
        {
            SearchBox.Text = SearchPlaceholder;
            SearchBox.Foreground = Brushes.Gray;
            LoadStock();
        }


        //Load and Search Feature 
        private void LoadStock(string searchName = "")
        {
            try
            {
                var list = new List<Stock>();

                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql =
                        "Select StockCode, StockName, StockDescription, Brand, Category, " +
                        "Location, Cost, SellingPrice, TotalPurchasedExclVat, TotalSalesExclVat, " +
                        "QntyPurchased, QntySold, StockOnHand " +
                        "FROM Stock_Masters"
                        ;

                    //checks id where clause is neede
                    if (!string.IsNullOrEmpty(searchName))
                        sql += " WHERE StockName LIKE @search";

                    using var cmd = new SqlCommand(sql, conn);   // this is what pushes the command through 

                    if (!string.IsNullOrWhiteSpace(searchName))
                        cmd.Parameters.AddWithValue("@search", $"%{searchName}%");

                    using var reader = cmd.ExecuteReader();

                    //what the table recieves put inthe rows 
                    while (reader.Read())
                    {
                        list.Add(new Stock
                        {
                            StockCode = reader["StockCode"].ToString(),
                            StockName = reader["StockName"].ToString(),

                            StockDescription = reader["StockDescription"].ToString(), //not showing

                            Brand = reader["Brand"].ToString(),
                            Category = reader["Category"].ToString(),
                            Location = reader["Location"].ToString(),

                            Cost = reader["Cost"] != DBNull.Value
                                ? Convert.ToDecimal(reader["Cost"])
                                : 0m,  //assign it 0 if null

                            SellingPrice = reader["SellingPrice"] != DBNull.Value
                                ? Convert.ToDecimal(reader["SellingPrice"])
                                : 0m,

                            TotalPurchasedExclVat = reader["TotalPurchasedExclVat"] != DBNull.Value
                                ? Convert.ToDecimal(reader["TotalPurchasedExclVat"])
                                : 0m,

                            TotalSalesExclVat = reader["TotalSalesExclVat"] != DBNull.Value
                                ? Convert.ToDecimal(reader["TotalSalesExclVat"])
                                : 0m,

                            QntyPurchased = reader["QntyPurchased"] != DBNull.Value
                                ? Convert.ToInt32(reader["QntyPurchased"])
                                : 0,

                            QntySold = reader["QntySold"] != DBNull.Value
                                ? Convert.ToInt32(reader["QntySold"])
                                : 0,

                            StockOnHand = reader["StockOnHand"] != DBNull.Value
                                ? Convert.ToInt32(reader["StockOnHand"])
                                : 0,

                        });
                    }


                }

                //Bind List
                StockTable.ItemsSource = list;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading debtors: " + ex.Message);
            }

        }




        //this fills the input fields for editing  
        private void StockTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //assig the value
            if (StockTable.SelectedItems is not Stock s)
                return;

            var result = MessageBox.Show(
                 $"Are you sure you want to edit this Item? '{s.StockName}'?",
                "Confirm Edit",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
                );

            if (result != MessageBoxResult.Yes)
            {
                // User cancelled → deselect row
                StockTable.SelectedItem = null;
                return;
            }

            _editingStockCode = s.StockCode;
            _isEditConfirmed = true;

             StockName.Text = s.StockName;
             StockBrand.Text = s.Brand;
             StockCategory.Text = s.Category;
             StockLoctaion.Text = s.Location;
             StockCost.Text = s.Cost.ToString();
             StockPrice.Text = s.SellingPrice.ToString();
             StockDesc.Text = s.StockDescription;


        }


        //Clear button clears searxh bar aswell
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            StockName.Text = "";
            StockBrand.Text = "";
            StockCategory.Text = "";
            StockLoctaion.Text = "";
            StockCost.Text = ("0.00");
            StockPrice.Text = ("0.00");
            StockDesc.Text = "";

            _editingStockCode = null;
            _isEditConfirmed = false;
                        
            StockTable.SelectedItem = null;

            // Reset search placeholder
            SetSearchPlaceholder();
        }


        private void SaveStock_Click(object sender, RoutedEventArgs e)
        {
            //check if text is filed 

            if (
                string.IsNullOrWhiteSpace(StockName.Text) ||
                string.IsNullOrWhiteSpace(StockBrand.Text) ||
                string.IsNullOrWhiteSpace(StockCategory.Text) ||
                string.IsNullOrWhiteSpace(StockLoctaion.Text) ||
                string.IsNullOrWhiteSpace(StockCost.Text = ("0.00")) ||
                string.IsNullOrWhiteSpace(StockPrice.Text = ("0.00")) ||
                string.IsNullOrWhiteSpace(StockDesc.Text)

                )
            {
                MessageBox.Show("Please fill in all required fields!");
                return;
            }

        
            // Validate Cost (decimal)
            if (!decimal.TryParse(StockCost.Text.Trim(), out decimal cost))
            {
                MessageBox.Show("Cost must be a valid decimal number.");
                return;
            }

            // Validate Selling Price (decimal)
            if (!decimal.TryParse(StockPrice.Text.Trim(), out decimal sellingPrice))
            {
                MessageBox.Show("Selling Price must be a valid decimal number.");
                return;
            }

            //if passes all validation 
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();


                    //update
                    if (_editingStockCode != null && _isEditConfirmed)
                    {
                        var confirm = MessageBox.Show(
                            "Save changes to this debtor?",
                            "Confirm Update",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (confirm != MessageBoxResult.Yes)
                            return;




                        string updateSql =
                            "UPDATE Stock_Masters SET " +
                            "StockName=@StockName, StockDescription=@StockDescription, Brand=@Brand, " +
                            "Category=@Category, Location=@Location, Cost=@Cost, SellingPrice=@SellingPrice, " +
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

                        cmd.ExecuteNonQuery(); //

                        MessageBox.Show("Stock has been updated sussecfully");

                    }
                    else //this is the insert mode 
                    {
                        string getMax =
                           "SELECT TOP 1 StockCode FROM Stock_Masters ORDER BY StockCode DESC";

                        string nextCode = "STK01";

                        using (var cmdMax = new SqlCommand(getMax, conn))
                        {
                            var result = cmdMax.ExecuteScalar();
                            if (result != null)
                            {
                                string lastCode = result.ToString();
                                int number = int.Parse(lastCode.Substring(3)) + 1;
                                nextCode = "AC" + number.ToString("D2");
                            }
                        }//closed connection automatical

                        //  string sql =
                        //"Select StockCode, StockName, StockDescription, Brand, Category, " +
                        //"Location, Cost, SellingPrice, TotalPurchasedExclVat, TotalSalesExclVat, " +
                        //"QntyPurchased, QntySold, StockOnHand " +
                        //"FROM Stock_Masters"
                        //;

                        string insertSql =
                            "INSERT INTO StockMasters" +
                            "StockCode, StockName, StockDescription, Brand, Category, " +
                            "Location, Cost, SellingPrice, TotalPurchasedExclVat, TotalSalesExclVat, " +
                            "QntyPurchased, QntySold, StockOnHand " +
                            "VALUES (@StockCode, @StockName, @StockDescription, @Brand, @Category, " +
                            "@Location, @Cost, @SellingPrice, @TotalPurchasedExclVat, @TotalSalesExclVat" +
                            "@QntyPurchased, @QntySold, @StockOnHand";


                        using var cmd = new SqlCommand(insertSql, conn);

                        cmd.Parameters.AddWithValue("StockCode", nextCode);
                        cmd.Parameters.AddWithValue("@StockName", StockName.Text.Trim());
                        cmd.Parameters.AddWithValue("@StockDescription", StockDesc.Text.Trim());
                        cmd.Parameters.AddWithValue("@Brand", StockBrand.Text.Trim());
                        cmd.Parameters.AddWithValue("@Category", StockCategory.Text.Trim());
                        cmd.Parameters.AddWithValue("@Location", StockLoctaion.Text.Trim());
                        cmd.Parameters.AddWithValue("@Cost", cost);
                        cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);
                        cmd.Parameters.AddWithValue("@TotalPurchasedExclVat", 0m);
                        cmd.Parameters.AddWithValue("@TotalSalesExclVat", 0m);
                        cmd.Parameters.AddWithValue("@Location", StockLoctaion.Text.Trim());
                        cmd.Parameters.AddWithValue("@Cost", cost);
                        cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);

                    }


                }

            }
            catch
            {
               

            }




        }  


        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

                 

        private void SearchBoxStock_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        // User clicks inside → remove placeholder
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
          
        }

        // User leaves empty → restore placeholder
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
          
        }

      

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

       
    }
}
