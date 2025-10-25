using System.Collections.Generic;
using BankingSystemApp.Enums;
namespace BankingSystemApp.Models;

public class Bank {
    public string Name { get; set; }
    public string SwiftAccount { get; set; }
    public BankLocation Location { get; set; }
    public BankCountry Country { get; set; }
    public decimal Balance { get; set; }
    public List<Account> Accounts { get; set; }

    public Bank(string Name, string SwiftAccount, BankLocation Location, BankCountry Country)
    {
        this.Name = Name;
        this.SwiftAccount = SwiftAccount;
        this.Location = Location;
        this.Country = Country;
        this.Balance = 0;
        this.Accounts = new List<Account>();
    }
}