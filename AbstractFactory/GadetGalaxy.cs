using AbstractFactory.factories;
using AbstractFactory.products;
using System;
using System.Collections.Generic;

public class GadetGalaxy
{
    private readonly IGadgetFactory _gadgetFactory;

    // Constructor accepts a gadget factory to be used
    public GadetGalaxy(IGadgetFactory gadgetFactory)
    {
        _gadgetFactory = gadgetFactory;
    }

    // Display gadgets method will call the factory to create and display gadgets
    public void DisplayGadgets()
    {
        // List of gadgets created by the factory
        List<IGadget> gadgets = new List<IGadget>
        {
            _gadgetFactory.CreateSmartWatch(),
            _gadgetFactory.CreateVRHeadset(),
            _gadgetFactory.CreateWirelessEarbuds()
        };

        // Displaying all the gadgets' details
        Console.WriteLine("Available Gadgets:");
        foreach (var gadget in gadgets)
        {
            Console.WriteLine($"- {gadget.GetDetails()}");  // Output the details of each gadget
        }
    }
}
