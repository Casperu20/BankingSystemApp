using System.Windows;
using BankingSystemApp.Services;

namespace BankingSystemApp;

public partial class DepositWindow : Window {
    private readonly BankService _service;

    public DepositWindow(BankService service)
    {
        InitializeComponent();
        _service = service;
    }

    private void ConfirmDeposit_Click(object sender, RoutedEventArgs e)
    {
        string iban = IbanBox.Text.Trim();
        if (!decimal.TryParse(AmountBox.Text.Trim(), out decimal amount))
        {
            MessageBox.Show("Please enter a valid amount.");
            return;
        }

        bool succes = _service.Deposit(iban, amount);
        
        if(succes)
            MessageBox.Show($"Deposited {amount} successfully into account {iban}.");
        else
            MessageBox.Show(" - Deposit FAILED. IBAN not found or invalid amount!");
        this.Close();
    }
}