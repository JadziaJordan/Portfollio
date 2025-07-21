namespace AbstractFactory.products
{
    public class WirelessBuds : IGadget
    {
        public string GetDetails() => "Samsung Galaxy Buds";  // Details of Samsung Earbuds
    }

    public class AppleEarbuds : IGadget
    {
        public string GetDetails() => "Apple AirPods";  // Details of Apple Earbuds
    }
}
