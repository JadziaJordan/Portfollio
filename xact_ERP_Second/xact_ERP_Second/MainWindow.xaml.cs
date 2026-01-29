using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using xact_ERP_Second.ViewPages;


namespace xact_ERP_Second
{
 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Step1_Click(object sender, RoutedEventArgs e)
        {
            //ReportIssues reportWindow = new ReportIssues(issueQueue, this);
            //reportWindow.Owner = this;
            //reportWindow.Show();
            //this.Hide();

            DebtorsMasters debtorsMasters = new DebtorsMasters();
            debtorsMasters.Show();
            this.Close();
        }

        private void Step2_Click(object sender, RoutedEventArgs e)
        {
            StockMaster stckMaster = new StockMaster();
            stckMaster.Show();
            this.Close();
        }

        private void Step3_Click(object sender, RoutedEventArgs e)
        {
            DebtorsEnquiry debtorsEnquiry = new DebtorsEnquiry();   
            debtorsEnquiry.Show();
            this.Close();

        }

        private void Step4_Click(object sender, RoutedEventArgs e)
        {
            StockEnquiry stockEnquiry = new StockEnquiry(); 
            stockEnquiry.Show();
            this.Close();
        }

        private void Step5_Click(object sender, RoutedEventArgs e)
        {
            StockAdjustment stockAdjustment = new StockAdjustment();
            stockAdjustment.Show();
            this.Close();

        }

        private void Step6_Click(object sender, RoutedEventArgs e)
        {
            InvoiceEnquiry invoiceEnquiry   = new InvoiceEnquiry();
            invoiceEnquiry.Show();
            this.Close();
        }

        private void Step7_Click(object sender, RoutedEventArgs e)
        {
         NewInvoice newInvoice = new NewInvoice();
            newInvoice.Show();

            this.Close();

        }
    }

    }



//private void MaintainDebtors_Click(object sender, MouseButtonEventArgs e)
//    {
//        // Open the other window/page
//        MaintainDebtorsWindow win = new MaintainDebtorsWindow();
//        win.Show();

//        // Optional: close current window
//        // this.Close();
//    }