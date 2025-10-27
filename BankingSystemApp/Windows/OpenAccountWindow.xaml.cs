using System.Windows;
using System.Windows.Controls;
using BankingSystemApp.Models;
using BankingSystemApp.Enums;
using BankingSystemApp.Services;

namespace BankingSystemApp;

public partial class OpenAccountWindow : Window {
    private readonly BankService _service;

    public OpenAccountWindow(BankService service) {
        InitializeComponent();
        _service = service;
    }

    private void OpenAccount_Click(object sender, RoutedEventArgs e) {
        string holder = HolderBox.Text;
        string iban = IbanBox.Text;
        string bankName = BankBox.Text;
        string type = ((ComboBoxItem)TypeBox.SelectedItem)?.Content.ToString();
        string currency = ((ComboBoxItem)CurrencyBox.SelectedItem)?.Content.ToString();

        if (string.IsNullOrWhiteSpace(holder) || string.IsNullOrWhiteSpace(iban) || string.IsNullOrWhiteSpace(bankName) || type == null || currency == null) {
            MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Bank bank = _service.Banks.FirstOrDefault(b => b.Name == bankName);
        if (bank == null) {
            MessageBox.Show("Bank not found! Please add it first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var acc = new Account(
            holder,
            Enum.Parse<AccountType>(type),
            Enum.Parse<AccountCurrency>(currency),
            iban,
            bank.Location
        );

        _service.OpenAccount(bank, acc);
        MessageBox.Show($"Account for {holder} created successfully in {bank.Name}!", "Success");
        Close();
    }
}