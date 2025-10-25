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

    public Account(string AccountHolder, AccountType Type, AccountCurrency Currency, string IBAN, DateTime OpenDate, DateTime CloseDate, decimal Amount) {
        this.AccountHolder = AccountHolder;
        this.Type = Type;
        this.Currency = Currency;
        this.IBAN = IBAN;
        this.OpenDate = OpenDate;
        this.CloseDate = CloseDate;
        this.Amount = Amount;
    }
}