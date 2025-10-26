using System.Windows;
using BankingSystemApp.Services;

namespace BankingSystemApp;

public partial class DepositWindow : Window
{
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

        _service.Deposit(iban, amount);
        MessageBox.Show($"Deposited {amount} successfully into account {iban}.");
        this.Close();
    }
}