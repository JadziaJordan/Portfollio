using System;

class ConfigurationManager
{
    private static ConfigurationManager _instance;

    public string StoreName { get; }
    public string Currency { get; }
    public double TaxRate { get; private set; }
    
    private ConfigurationManager()
    {
        StoreName = "Gadget Galaxy";
        Currency = "ZAR";
        TaxRate = 15.0; // Default tax rate
    }

    
    public static ConfigurationManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ConfigurationManager();
        }
        return _instance;
    }

     // Changed to double & added private setter

    public void UpdateTaxRate(double newRate)
    {
        TaxRate = newRate;
    }
}