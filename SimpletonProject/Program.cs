using System;

class Program
{
    static void Main()
    {
        ConfigurationManager config1 = ConfigurationManager.GetInstance();

        
        Console.WriteLine("Config1:\n");
        Console.WriteLine($"Store: {config1.StoreName}, Currency: {config1.Currency}, Tax Rate: {config1.TaxRate}%\n");

        // Retrieve another instance
        ConfigurationManager config2 = ConfigurationManager.GetInstance();

        Console.WriteLine("Config2:\n");
        Console.WriteLine($"Store: {config2.StoreName}, Currency: {config2.Currency}, Tax Rate: {config2.TaxRate}%\n");

      
        config2.UpdateTaxRate(18.0);

        // Display updated settings
        Console.WriteLine($"Updated Tax Rate config1: {config1.TaxRate}%\n");
        Console.WriteLine($"Updated Tax Rate config2: {config2.TaxRate}%\n");

        // Verify that both instances are the same
        Console.WriteLine($"Are both instances the same? {config1 == config2}");

        Console.ReadLine();
    }
}