using System;
using BankingSystemApp.Enums;
namespace BankingSystemApp.Models;

public class Account {
    public String AccountHolder { get; set; }
    public AccountType Type { get; set; }
    public AccountCurrency Currency { get; set; }
    public string IBAN { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime CloseDate { get; set; }
    public decimal Amount { get; set; }
    public BankLocation Location { get; set; }

    public Account(string AccountHolder, AccountType Type, AccountCurrency Currency, string IBAN, BankLocation Location) {
        this.AccountHolder = AccountHolder;
        this.Type = Type;
        this.Currency = Currency;
        this.IBAN = IBAN;
        this.OpenDate = DateTime.Now;
        this.CloseDate = new DateTime(2999, 1, 1);;
        this.Amount = 0;
        this.Location = Location;
    }
    
    // Deposit and Witdrahw money
    public void Deposit(decimal money_amount) {
        if (money_amount <= 0) {
            Console.WriteLine(" - Deposit amount must be positive !!! ( > 0 $$$ )");
            return;
        }
        
        Amount += money_amount;
        Console.WriteLine($"Deposited {money_amount} {Currency} to {AccountHolder}'s account. New balance: {Amount} {Currency}");
    }

    public void Withdraw(decimal money_amount) {
        if (money_amount <= 0) {
            Console.WriteLine(" - Withdraw amount must be positive !!! ( > 0 $$$ )");
            return;
        }
        
        if (Amount < money_amount) {
            Console.WriteLine($"Not enough money in {AccountHolder}'s account. Balance: {Amount} {Currency}");
            return;
        }
        
        Amount -= money_amount;
        Console.WriteLine($"Withdrawn {money_amount} {Currency} from {AccountHolder}'s account. New balance: {Amount} {Currency}");
    }
    
    // transfer to another account
    public void TransferTo(Account target_account, decimal money_amount, decimal fee, decimal BankFee) {
        if (target_account == null) {
            Console.WriteLine(" - Target account NOT found !!!");
            return;
        }

        if (Amount < money_amount + fee) {
            Console.WriteLine($"Not enough money to complete the transfer (including fee)! Balance: {Amount} {Currency}");
            return;
        }
        
        Amount -= (money_amount + fee);
        target_account.Amount += money_amount;
        
        Console.WriteLine($"Transfered {money_amount} {Currency} from {AccountHolder}'s account to {target_account.AccountHolder}'s account. Fee = {fee} {Currency}");
        Console.WriteLine($"Sender new balance: {Amount} {Currency} | Receiver new balance: {target_account.Amount} {Currency}");
    }

    public void ChangeCurrency(AccountCurrency new_currency, decimal feeRate = 0.02m) { // 2%
        if (Currency == new_currency) {
            Console.WriteLine(" - Account already in this currency !!!");
            return;
        }

        var rates = new Dictionary<AccountCurrency, decimal>
        {
            { AccountCurrency.EUR, 1.0m },
            { AccountCurrency.USD, 1.16m },
            { AccountCurrency.GBP, 0.87m },
            { AccountCurrency.RON, 5.1m }
        };
        
        if (!rates.ContainsKey(new_currency) || !rates.ContainsKey(Currency)) {
            Console.WriteLine(" - Can't support this currency conversion !!!");
            return;
        }
        
        // calculate fee& check balanc
        decimal fee = Amount * feeRate;
        if (Amount < fee) {
            Console.WriteLine($" - Not enough money to complete the conversion (including fee)! Balance: {Amount} {Currency}");
            return;
        }
        
        decimal valueInEuro = Amount / rates[Currency];
        Amount = (valueInEuro * rates[new_currency]) - fee;
        Currency = new_currency;
        
        Console.WriteLine($"Converted to {new_currency} with fee = {fee} {Currency} (rate = {feeRate}). New balance: {Amount} {Currency}");
        }

    public void ChangeLocation(BankLocation new_location, List<BankLocation> allowed_locations) {
        if (!allowed_locations.Contains(new_location)) {
            Console.WriteLine(" - Selected location not available for your bank !!!");
            return;
        }
        
        Console.WriteLine($"Changed location from {Location} to {new_location} of {AccountHolder}'s bank");
        Location = new_location;
    }
    
    public override string ToString()
    {
        return $" ~ Account Data: {AccountHolder} | {IBAN} | {Currency} | Balance: {Amount}";
    }
    
    
}