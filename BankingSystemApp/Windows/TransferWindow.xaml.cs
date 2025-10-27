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

        if (!decimal.TryParse(AmountBox.Text.Trim(), out decimal amount)) {
            MessageBox.Show(" - Please enter a valid amount.");
            return;
        }

        MessageBox.Show("Processing transfer... This might take a few seconds ⏳");

        bool succes = await _service.Transfer(senderIban, receiverIban, amount);
        
        if(succes)
            MessageBox.Show($"✅ Transfer of {amount} completed (check logs or history).");
        else
            MessageBox.Show(" - Transfer FAILED. Check IBAN, balance or the amount!");
        this.Close();
    }
}