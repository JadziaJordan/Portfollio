namespace AbstractFactory.products
{
    // Step 2: Implement concrete gadgets for Samsung, createing the concrete Products
    public class Smartwatch : IGadget
    {
        public string GetDetails() => "Samsung Galaxy Watch";  // Details of Samsung Smartwatch
    }

    public class AppleSmartwatch : IGadget
    {
        public string GetDetails() => "Apple Watch Ultra";  // Details of Apple Smartwatch
    }
}
