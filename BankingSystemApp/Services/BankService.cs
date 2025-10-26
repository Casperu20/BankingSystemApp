using BankingSystemApp.Models;
using BankingSystemApp.Enums;

namespace BankingSystemApp.Services;

public class BankService {
    public List<Bank> Banks { get; set; }

    public BankService() {
        this.Banks = JsonStorageService.LoadBanksInJSON();
    }
    
    public void SaveInJSON() {
        JsonStorageService.SaveBanksInJSON(Banks);
    }

    public bool AddBank(Bank NewBank) {
        // name of the banks must be unique, a bank can have the same name only if there are in different countries.
        bool exists = false;

        foreach (var bank in Banks) {
            bool sameName = (bank.Name == NewBank.Name);
            bool sameCountry = (bank.Country == NewBank.Country);

            if (sameName && sameCountry) {
                exists = true;
                break;
            }
        }
        
        if (exists) {
            Console.WriteLine($" - Bank {NewBank.Name} already exists in {NewBank.Country}!!!");
            return false;
        }
        
        Banks.Add(NewBank);
        SaveInJSON();
        Console.WriteLine($"Bank: {NewBank.Name} added succesfully in {NewBank.Country}");
        return true;
    }
    
    
    // OPERATIONS BANK
    public Bank GetBank(string bank_name, BankCountry bank_country) {
        Bank bank = null;

        foreach (var b in Banks) {
            if (b.Name == bank_name && b.Country == bank_country) {
                bank = b;
                break;
            }
        }

        if (bank == null) {
            Console.WriteLine($"Bank {bank_name} not found in {bank_country} !!!");
        }

        return bank;
    }

    public Account GetAccount(String IBAN) {
        foreach (var bank in Banks) {
            foreach (var account in bank.Accounts) {
                if (account.IBAN == IBAN) {
                    Console.WriteLine($"Account found {IBAN} in bank: {bank.Name}");
                    return account; 
                }
            }
        }
        
        Console.WriteLine($"Account {IBAN} not found in any bank !!!");
        return null;
    }
    
    public void ApplyFee(Bank bank, decimal fee) {
        bank.Balance = bank.Balance + fee;
        Console.WriteLine($"Fee = {fee} added to {bank.Name}. New balance: {bank.Balance}");
        SaveInJSON();
    }

    public void OpenAccount(Bank bank, Account new_account) {
        if (bank == null) {
            Console.WriteLine(" - Can't open account. Bank not found !!!");
            return;
        }
        
        // what if does already exist?
        foreach (var account in bank.Accounts) {
            if (account.IBAN == new_account.IBAN) {
                Console.WriteLine($" - Account: {new_account.IBAN} already exists in {bank.Name} !!!");
                return;
            }
        }
        
        bank.Accounts.Add(new_account);
        SaveInJSON();
        Console.WriteLine($"New Account opened by {new_account.AccountHolder} in {bank.Name} {bank.Country}");
    }

    public void CloseAccount(Bank bank, string iban) {
        if (bank == null) {
            Console.WriteLine(" - Can't close account. Bank not found !!!");
            return;
        }

        Account account_to_remove = null;
        foreach (var account in bank.Accounts) {
            if (account.IBAN == iban) {
                account_to_remove = account;
                break;
            }
        }

        if (account_to_remove == null) {
            Console.WriteLine($" - Account: {iban} not found in {bank.Name} !!!");
            return;
        }
        
        bank.Accounts.Remove(account_to_remove);
        SaveInJSON();
        Console.WriteLine($"Account: {iban} CLOSED from {bank.Name}");
    }
}