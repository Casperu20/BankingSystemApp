using System.IO;
using System.Text.Json;
using BankingSystemApp.Models;
namespace BankingSystemApp.Services;

public class JsonStorageService
{
    private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\banks.json");
    
    // it has to: 
    // save all data (banks + accounts + balances) to JSON file
    // load all data from JSON file when app starts              BUN?
    
    public static void SaveBanksInJSON(List<Bank> banks) {
        try {
            Directory.CreateDirectory("Data"); // i have to make sure if the folder exists

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(banks, options);

            File.WriteAllText(FilePath, json);
            Console.WriteLine("Data saved in banks.json!");
        }
        catch (Exception e) {
            Console.WriteLine($" - ERROR saving data in banks.json: {e.Message} !!!");
        }
    }

    public static List<Bank> LoadBanksInJSON() {
        try
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine(" - No data file found -> creating new one !!!");
                return new List<Bank>();
            }

            string json = File.ReadAllText(FilePath);
            var banks = JsonSerializer.Deserialize<List<Bank>>(json);

            Console.WriteLine("Data loaded succesfully");
            if (banks != null)
                return banks;
            else
                return new List<Bank>();
        }
        catch (Exception e) {
            Console.WriteLine($" - ERROR loading data : {e.Message} !!!");
            return new List<Bank>();
        }
    }
} 