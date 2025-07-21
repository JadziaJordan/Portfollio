﻿﻿﻿using System;
using AbstractFactory.factories;
using AbstractFactory.products;

namespace AbstractFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Galaxy Gadget Store!");

            // Provide two options for the user to choose from
            Console.WriteLine("Choose a brand:");
            Console.WriteLine("1. Samsung");
            Console.WriteLine("2. Apple");

            // Capture user input
            Console.Write("Enter your choice (1 or 2): ");
            string choice = Console.ReadLine();

            // Initialize the factory based on user input
            factories.IGadgetFactory factory = null;
            if (choice == "1")
            {
                factory = new factories.SamsungFactory();
            }
            else if (choice == "2")
            {
                factory = new factories.AppleFactory();
            }
            else
            {
                Console.WriteLine("Invalid choice. Defaulting to Samsung.");
                factory = new factories.SamsungFactory();
            }

            // Check if a valid factory is selected
            if (factory == null)
            {
                Console.WriteLine("Invalid brand selection. Please choose either 1 for Samsung or 2 for Apple.");
                return;
            }

            // Create a GalaxyGadgetStore object using the selected factory
            GadetGalaxy store = new GadetGalaxy(factory);  // This is now valid

            // Display the gadgets of the selected brand
            store.DisplayGadgets();
        }
    }
}
