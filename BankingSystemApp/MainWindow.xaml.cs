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
using BankingSystemApp.Enums;
using BankingSystemApp.Models;
using BankingSystemApp.Services;

namespace BankingSystemApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow() {
        InitializeComponent();
        
        var service = new BankService();
        
        var bank1 = new Bank("BCR", "BCRROBU", BankLocation.TM, BankCountry.RO);
        var bank2 = new Bank("BCR", "BCRDEBU", BankLocation.B, BankCountry.DE);
        var bank3 = new Bank("BCR", "BCRROCT", BankLocation.CT, BankCountry.RO);
        
        service.AddBank(bank1); // Should work 
        service.AddBank(bank2); // Should work again
        service.AddBank(bank3); // Should ERROR -> same name & country
        
        service.ApplyFee(bank1, 25.50m);
        service.ApplyFee(bank2, 10.00m);
    }
}