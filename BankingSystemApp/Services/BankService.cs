using BankingSystemApp.Models;
using BankingSystemApp.Enums;
namespace BankingSystemApp.Services;

public class BankService {
    public List<Bank> Banks { get; set; }

    public BankService() {
        this.Banks = new List<Bank>();
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
            Console.WriteLine($"Bank {NewBank.Name} already exists in {NewBank.Country}!!!");
            return false;
        }
        
        Banks.Add(NewBank);
        Console.WriteLine($"Bank: {NewBank.Name} added succesfully in {NewBank.Country}");
        return true;
    }

    public void ApplyFee(Bank bank, decimal fee) {
        bank.Balance = bank.Balance + fee;
        Console.WriteLine($"Fee = {fee} added to {bank.Name}. New balance: {bank.Balance}");
    }
}