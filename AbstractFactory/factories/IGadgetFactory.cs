using AbstractFactory.products;

namespace AbstractFactory.factories
{
    // Step 4: Define the Abstract Factory interface
    public interface IGadgetFactory
    {
        IGadget CreateSmartWatch();  // Create Smartwatch
        IGadget CreateVRHeadset();   // Create VR Headset
        IGadget CreateWirelessEarbuds();  // Create Wireless Earbuds
    }
}
