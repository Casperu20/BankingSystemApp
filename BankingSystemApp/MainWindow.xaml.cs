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
        /*
        var service = new BankService();
        
        var bank1 = new Bank("BCR", "BCRROBU", BankLocation.TM, BankCountry.RO);
        var bank2 = new Bank("BCR", "BCRDEBU", BankLocation.B, BankCountry.DE);
        var bank3 = new Bank("BCR", "BCRROCT", BankLocation.CT, BankCountry.RO);
        
        service.AddBank(bank1); // Should work 
        service.AddBank(bank2); // Should work again
        service.AddBank(bank3); // Should ERROR -> same name & country
        
        service.ApplyFee(bank1, 25.50m);
        service.ApplyFee(bank2, 10.00m);
        */

        var banks = JsonStorageService.LoadBanksInJSON();

        if (banks.Count == 0) {
            var bank1_bcr = new Bank("BCR", "BCRROTM", BankLocation.TM, BankCountry.RO);
            var account1 = new Account("Nicu Bunea", AccountType.Personal, AccountCurrency.EUR, "RO49BUN1000000000000000", BankLocation.TM);
            var account2 = new Account("Melisa Daj", AccountType.Personal, AccountCurrency.EUR, "RO53DAJ2000000000000003", BankLocation.TM);
            
            account1.Deposit(1000);
            account1.TransferTo(account2, 500, fee: 10, BankFee: 10);
            account2.Withdraw(60); 
            
            bank1_bcr.Accounts.Add(account1);
            bank1_bcr.Accounts.Add(account2);
            banks.Add(bank1_bcr);
        }

        foreach (var bank in banks) {
            Console.WriteLine($"\n {bank.Name} - ({bank.Country}) -> {bank.Accounts.Count} accounts");
            foreach (var account in bank.Accounts) {
                Console.WriteLine($"  {account}");
            }
        }
        
        // SAVE back to JSON file
        JsonStorageService.SaveBanksInJSON(banks);
        
        
        
        /*  Antecedent testing for Account operations
        account1.Withdraw(500); // FAIL
        Console.WriteLine(account1);
        
        Console.WriteLine("\n");
        account1.ChangeCurrency(AccountCurrency.USD);
        Console.WriteLine(account1);
        account1.ChangeCurrency(AccountCurrency.RON);
        Console.WriteLine(account1);
        
        var available = new List<BankLocation> { BankLocation.TM, BankLocation.B, BankLocation.CT, BankLocation.IS, BankLocation.AR };
        account1.ChangeLocation(BankLocation.CT, available); }*/
    }
}