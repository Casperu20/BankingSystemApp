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
    
    public bool Deposit(string IBAN, decimal money_amount) {
        if (money_amount <= 0) {
            Console.WriteLine(" - Can't deposit negative or zero amount !!!");
            return false;
        }
        
        Account account = GetAccount(IBAN);
        if (account == null) {
            Console.WriteLine($" - Deposit FAILED. Account {IBAN} not found !!!");
            return false;
        }
        
        account.Deposit(money_amount);
        SaveInJSON();
        Console.WriteLine($"Deposited {money_amount} {account.Currency} to {account.AccountHolder}'s account ({IBAN}). New balance: {account.Amount} {account.Currency}");
        return true;
    }

    public bool Withdraw(string IBAN, decimal money_amount) {
        if (money_amount <= 0) {
            Console.WriteLine(" - Can't withdraw negative or zero amount !!!");
            return false;
        }
        
        Account account = GetAccount(IBAN);
        if (account == null) {
            Console.WriteLine($" - Withdraw FAILED. Account {IBAN} not found !!!");
            return false;
        }

        if (account.Amount < money_amount) {
            Console.WriteLine($" - Insufficient money in {IBAN} account. Balance: {account.Amount} {account.Currency}");
            return false;
        }
        
        account.Withdraw(money_amount);
        SaveInJSON();
        Console.WriteLine($"Withdrawn {money_amount} {account.Currency} from {account.AccountHolder}'s account ({IBAN}). New balance: {account.Amount} {account.Currency}");
        return true;
    }

    public async Task<bool> Transfer(string IBAN_sender, string IBAN_receiver, decimal money_amount) {
        if (money_amount <= 0) {
            Console.WriteLine(" - Transfer amount must be greater than 0 !!!");
            return false;
        }
        
        Account sender_account = GetAccount(IBAN_sender);
        Account receiver_account = GetAccount(IBAN_receiver);

        if (sender_account == null || receiver_account == null) {
            Console.WriteLine($" - Transfer FAILED. Account {IBAN_sender} or {IBAN_receiver} not found !!!");
            return false;
        }
        
        // find bank for them aswell
        Bank sender_bank = null;
        Bank receiver_bank = null;
        
        foreach (var bank in Banks) {
            if (bank.Accounts.Contains(sender_account))
                sender_bank = bank;
            if (bank.Accounts.Contains(receiver_account))
                receiver_bank = bank;
        }

        if (sender_bank == null || receiver_bank == null) {
            Console.WriteLine($" - Transfer FAILED. Account {IBAN_sender} or {IBAN_receiver} not found !!!");
            return false;
        }
        
        // i would implement fixed fees
        decimal sameBankFeeRate = 0.01m;  // 1%
        decimal diffBankFeeRate = 0.02m;
        decimal fee = 0;
        if (sender_bank == receiver_bank) {
            fee = money_amount * sameBankFeeRate;
            Console.WriteLine($"Same bank transfer. Fee = {fee} ");
        }
        else {
            fee = money_amount * diffBankFeeRate;
            Console.WriteLine($"Different bank transfer. Fee = {fee} ");
        }
        
        decimal total_amount = money_amount + fee;
        if (sender_account.Amount < total_amount) {
            Console.WriteLine($" - Insufficient money for transfer + fee! Balance: {sender_account.Amount} {sender_account.Currency}");
            return false;
        }
        
        sender_account.Amount = sender_account.Amount - total_amount;

        if (sender_account.Currency != receiver_account.Currency) { // should do a conversion
            var rates = new Dictionary<AccountCurrency, decimal>
            {
                { AccountCurrency.EUR, 1.0m },
                { AccountCurrency.USD, 1.16m },
                { AccountCurrency.GBP, 0.87m },
                { AccountCurrency.RON, 5.1m }
            };
            
            decimal valueInEuro = money_amount / rates[sender_account.Currency];
            decimal converted_amount = valueInEuro * rates[receiver_account.Currency];
            receiver_account.Amount = receiver_account.Amount + converted_amount;

            Console.WriteLine($"Transfer converted from {sender_account.Currency} to {receiver_account.Currency} ({converted_amount} received)");
        }
        else {
            receiver_account.Amount = receiver_account.Amount + money_amount;
        }
        
        // LOG:
        sender_account.TransactionHistory.Add($"Sent {money_amount} {sender_account.Currency} to {receiver_account.AccountHolder} ({receiver_account.IBAN}) on {DateTime.Now}. Fee: {fee}");

        receiver_account.TransactionHistory.Add($"Received {money_amount} {sender_account.Currency} from {sender_account.AccountHolder} ({sender_account.IBAN}) on {DateTime.Now}");
        
        // random waiting time for transfers
        Random rand = new Random();
        int delaySeconds;
        if (sender_bank == receiver_bank)
        {
            delaySeconds = rand.Next(1, 11);
            Console.WriteLine($"Estimated delay : {delaySeconds} sec");
        }
        else {
            delaySeconds = rand.Next(11, 21);
            Console.WriteLine($"Estimated delay : {delaySeconds} sec");
        }
        Console.WriteLine($"Processing Transfer...");
        await Task.Delay(delaySeconds * 1000);

        
        ApplyFee(sender_bank, fee);
        
        SaveInJSON();
        Console.WriteLine($"Transfered {money_amount} {sender_account.Currency} from ({IBAN_sender}) ->  ({IBAN_receiver}). Fee = {fee}");
        return true;
    }

    public void ShowTransactionHistory(string IBAN) {
        Account account = GetAccount(IBAN);
        if (account == null) {
            Console.WriteLine($" - Account {IBAN} not found!!!");
            return;
        }
        
        Console.WriteLine($"\n Transaction history for {account.AccountHolder} ({account.IBAN}):");
        if (account.TransactionHistory.Count == 0) {
            Console.WriteLine(" - No transactions yet!");
            return;
        }
        
        foreach (var transaction in account.TransactionHistory) {
            Console.WriteLine($" // {transaction}");
        }
    }
}