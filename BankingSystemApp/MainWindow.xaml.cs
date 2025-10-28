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

public partial class MainWindow : Window {
    private BankService service;
    public MainWindow()
    {
        InitializeComponent();
        
        Loaded += MainWindow_Loaded;
    }
    
    private async void MainWindow_Loaded(object sender, RoutedEventArgs e) {
        service = new BankService();
        
        /* TESTING BEFORE THE APP
        var bank1_bcr = new Bank("BCR", "BCRROTM", BankLocation.TM, BankCountry.RO);
        service.AddBank(bank1_bcr);
        
        var account1 = new Account("Nicu Bunea", AccountType.Personal, AccountCurrency.EUR, "RO49BUN1000000000000000", BankLocation.TM);
        var account2 = new Account("Melisa Daj", AccountType.Personal, AccountCurrency.EUR, "RO53DAJ2000000000000003", BankLocation.TM);
        var accountToClose = new Account("Nu exist", AccountType.Company, AccountCurrency.RON, "RO33EXI5550000000000333", BankLocation.B);
        
        service.OpenAccount(bank1_bcr, account1);
        service.OpenAccount(bank1_bcr, account2);
        service.OpenAccount(bank1_bcr, accountToClose);
        
        /* before Deposit & Withdraw + transfer from BANK services!
        account1.Deposit(2200);
        account1.TransferTo(account2, 500, fee: 10, BankFee: 10);
        account2.Withdraw(60); 
        account1.Deposit(50000);
        */
        
        /*
        service.Deposit("RO49BUN1000000000000000", 1200);
        service.Withdraw("RO49BUN1000000000000000", 300);
        await service.Transfer("RO49BUN1000000000000000", "RO53DAJ2000000000000003", 500); // ASYNC!
        // Close:
        service.CloseAccount(bank1_bcr, "RO33EXI5550000000000333");
        
        foreach (var bank in service.Banks) {
            Console.WriteLine($"\n {bank.Name} - ({bank.Country}) -> {bank.Accounts.Count} accounts");
            foreach (var account in bank.Accounts) {
                Console.WriteLine($"  {account}");
            }
        }
        
        service.ShowTransactionHistory("RO49BUN1000000000000000");
        service.ShowTransactionHistory("RO53DAJ2000000000000003");
        
        // SAVE back to JSON file
        service.SaveInJSON();
        */
        // var banks = JsonStorageService.LoadBanksInJSON();
        // JsonStorageService.SaveBanksInJSON(banks);
        
        /* FIRST TESTING OF ACCount functions
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

    private void AddBank_Click(object sender, RoutedEventArgs e) {
        var addBankWindow = new AddBankWindow(service);
        addBankWindow.ShowDialog(); 
    }

    private void OpenAccount_Click(object sender, RoutedEventArgs e) {
        var window = new OpenAccountWindow(service);
        window.ShowDialog();
    }
    private void Deposit_Click(object sender, RoutedEventArgs e) {
        var depositWindow = new DepositWindow(service);
        depositWindow.Owner = this;
        depositWindow.ShowDialog();
    }

    private void Withdraw_Click(object sender, RoutedEventArgs e) {
        var withdrawWindow = new WithdrawWindow(service);
        withdrawWindow.Owner = this;
        withdrawWindow.ShowDialog();
    }

    private void Transfer_Click(object sender, RoutedEventArgs e) {
        var transferWindow = new TransferWindow(service);
        transferWindow.Owner = this;
        transferWindow.ShowDialog();
    }

    private void History_Click(object sender, RoutedEventArgs e) {
        var window = new ViewHistoryWindow(service);
        window.ShowDialog();
    }

    private void Exit_Click(object sender, RoutedEventArgs e) {
        var result = MessageBox.Show(
            "Do you want to save all data and exit?",
            "Save & Exit",
            MessageBoxButton.YesNo, // keep Yes and No only
            MessageBoxImage.Question
        );

        if (result == MessageBoxResult.No)
            return; // user canceled exit

        try
        {
            service.SaveInJSON();
            MessageBox.Show("✅ Data successfully saved to JSON file.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"⚠️ Error while saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        Application.Current.Shutdown(); 
    }

}