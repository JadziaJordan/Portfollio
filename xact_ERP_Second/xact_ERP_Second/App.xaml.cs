using System;
using System.Windows;
using xact_ERP_Second.Data; // <-- reference your Data.cs class here

namespace xact_ERP_Second
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Initialize the reusable database connection
                Database.Initialize(); 

                // Optional: Test the connection
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    MessageBox.Show("Connected to ExactErpDemo database!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to initialize database:\n" + ex.Message);
                Shutdown(); // Stop the app if DB fails
                return;
            }

            // Continue loading the main window
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
