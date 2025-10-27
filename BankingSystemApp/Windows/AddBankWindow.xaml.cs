using System.Windows;
using BankingSystemApp.Enums;
using BankingSystemApp.Models;
using BankingSystemApp.Services;

namespace BankingSystemApp;

public partial class AddBankWindow : Window {
    private BankService _service;

    public AddBankWindow(BankService service)
    {
        InitializeComponent();
        _service = service;

        // Populate combo boxes
        CountryBox.ItemsSource = Enum.GetValues(typeof(BankCountry)).Cast<BankCountry>();
        LocationBox.ItemsSource = Enum.GetValues(typeof(BankLocation)).Cast<BankLocation>();
    }

    private void AddBank_Click(object sender, RoutedEventArgs e) {
        string name = NameBox.Text.Trim();
        string swift = SwiftBox.Text.Trim();
        BankCountry? country = (BankCountry?)CountryBox.SelectedItem;
        BankLocation? location = (BankLocation?)LocationBox.SelectedItem;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(swift) || country == null || location == null) {
            MessageBox.Show("⚠️ Please fill in all fields!", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (swift.Length < 8 || swift.Length > 11) {
            MessageBox.Show(" - ⚠️ SWIFT code should be between 8 and 11 characters long.", "Invalid SWIFT Code", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        
        var newBank = new Bank(name, swift, (BankLocation)location, (BankCountry)country);
        bool added = _service.AddBank(newBank);

        if (added) {
            MessageBox.Show($"✅ Bank {name} successfully added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
        else {
            MessageBox.Show($"❌ Bank {name} already exists in {country}!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
