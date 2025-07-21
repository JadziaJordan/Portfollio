namespace AbstractFactory.products
{
    public class VRHeadset : IGadget
    {
        public string GetDetails() => "Samsung Gear VR";  // Details of Samsung VR Headset
    }

    public class AppleVRHeadset : IGadget
    {
        public string GetDetails() => "Apple Vision Pro";  // Details of Apple VR Headset
    }
}
