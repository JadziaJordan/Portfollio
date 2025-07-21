using AbstractFactory.products;

namespace AbstractFactory.factories
{
    public class AppleFactory : IGadgetFactory
    {
        public IGadget CreateSmartWatch() => new products.AppleSmartwatch();  // Apple Smartwatch creation
        public IGadget CreateVRHeadset() => new products.AppleVRHeadset();   // Apple VR Headset creation
        public IGadget CreateWirelessEarbuds() => new products.AppleEarbuds();  // Apple Earbuds creation
    }
}
