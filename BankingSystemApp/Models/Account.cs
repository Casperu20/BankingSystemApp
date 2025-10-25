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

    public Account(string AccountHolder, AccountType Type, AccountCurrency Currency, string IBAN) {
        this.AccountHolder = AccountHolder;
        this.Type = Type;
        this.Currency = Currency;
        this.IBAN = IBAN;
        this.OpenDate = DateTime.Now;
        this.CloseDate = new DateTime(2999, 1, 1);;
        this.Amount = 0;
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

    public override string ToString()
    {
        return $" ~ Account Data: {AccountHolder} | {IBAN} | {Currency} | Balance: {Amount}";
    }
    
    
}