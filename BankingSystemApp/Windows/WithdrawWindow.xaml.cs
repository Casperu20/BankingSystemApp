using System.Windows;
using BankingSystemApp.Services;

namespace BankingSystemApp;

public partial class WithdrawWindow : Window
{
    private readonly BankService _service;

    public WithdrawWindow(BankService service) {
        InitializeComponent();
        _service = service;
    }

    private void ConfirmWithdraw_Click(object sender, RoutedEventArgs e)
    {
        string iban = IbanBox.Text.Trim();

        if (!decimal.TryParse(AmountBox.Text.Trim(), out decimal amount))
        {
            MessageBox.Show(" - Please enter a valid amount.");
            return;
        }
        
        bool succes = _service.Withdraw(iban, amount);

        if(succes)
            MessageBox.Show($"Withdrawn {amount} successfully from account {iban}.");
        else
            MessageBox.Show(" - Withdraw FAILED. Check IBAN or the amount!");
        this.Close();
    }
}