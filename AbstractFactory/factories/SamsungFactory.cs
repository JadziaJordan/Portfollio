using AbstractFactory.products;

namespace AbstractFactory.factories
{
    // Step 5: Implement Concrete Factories for Samsung and Apple
    public class SamsungFactory : IGadgetFactory
    {
        public IGadget CreateSmartWatch() => new products.Smartwatch();  // Samsung Smartwatch creation
        public IGadget CreateVRHeadset() => new products.VRHeadset();   // Samsung VR Headset creation
        public IGadget CreateWirelessEarbuds() => new products.WirelessBuds();  // Samsung Earbuds creation
    }
}
