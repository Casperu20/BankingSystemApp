using System.Windows;
using BankingSystemApp.Services;

namespace BankingSystemApp;

public partial class TransferWindow : Window {
    private readonly BankService _service;

    public TransferWindow(BankService service) {
        InitializeComponent();
        _service = service;
    }

    private async void ConfirmTransfer_Click(object sender, RoutedEventArgs e) {
        string senderIban = SenderBox.Text.Trim();
        string receiverIban = ReceiverBox.Text.Trim();
        decimal amount;

        if (string.IsNullOrWhiteSpace(senderIban) || string.IsNullOrWhiteSpace(receiverIban)) {
            MessageBox.Show("Please fill in both sender and receiver IBAN fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!decimal.TryParse(AmountBox.Text.Trim(), out amount) || amount <= 0) {
            MessageBox.Show("Please enter a valid positive amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        MessageBox.Show("Processing transfer... This might take a few seconds ⏳", "Processing", MessageBoxButton.OK, MessageBoxImage.Information);

        bool success = await _service.Transfer(senderIban, receiverIban, amount);

        if (success)
            MessageBox.Show($"✅ Transfer of {amount} completed successfully.\n(Check logs or transaction history.)", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        else
            MessageBox.Show("❌ Transfer failed. Check IBANs, balance, or transfer amount.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        this.Close();
    }
}