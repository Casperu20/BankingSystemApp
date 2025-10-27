using System.Text;
using System.Windows;
using BankingSystemApp.Services;

namespace BankingSystemApp;

public partial class ViewHistoryWindow : Window {
    private readonly BankService _service;

    public ViewHistoryWindow(BankService service) {
        InitializeComponent();
        _service = service;
    }

    private void Search_Click(object sender, RoutedEventArgs e) {
        string iban = IbanBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(iban)) {
            MessageBox.Show(" - Please enter an IBAN.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var account = _service.GetAccount(iban);
        if (account == null) {
            MessageBox.Show($"No account found with IBAN {iban}.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (account.TransactionHistory.Count == 0) {
            HistoryTextBox.Text = $"No transactions found for {iban}.";
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Transaction History for {iban} ({account.AccountHolder}):");
        sb.AppendLine(new string('-', 60));

        foreach (var t in account.TransactionHistory)
            sb.AppendLine("• " + t);

        HistoryTextBox.Text = sb.ToString();
    }
}