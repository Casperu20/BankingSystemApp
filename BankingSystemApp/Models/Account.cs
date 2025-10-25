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

    public override string ToString()
    {
        return $" ~ Account Data: {AccountHolder} | {IBAN} | {Currency} | {Amount}";
    }
}