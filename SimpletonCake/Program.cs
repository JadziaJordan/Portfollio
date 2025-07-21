using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to Jadzia's Bakery Below is your cake order\n");

        Cake order1 = Cake.GetInstance();
        Cake order2 = Cake.GetInstance();

        Console.WriteLine("Instance One:");
        Console.WriteLine($"Flavour: {order1.Flavour}, Frosting: {order1.Frosting}, Occasion: {order1.Occasion}\n");
        
        Console.WriteLine("Instance Two:");
        Console.WriteLine($"Flavour: {order2.Flavour}, Frosting: {order2.Frosting}, Occasion: {order2.Occasion}\n");

        // Update the flavour of the second instance
        order2.UpdateFlavour("Strawberry");

        Console.WriteLine("Welcome to Jadzia's Bakery Below is your new cake order\n");
        Console.WriteLine("Instance One:");
        Console.WriteLine($"Flavour: {order1.Flavour}, Frosting: {order1.Frosting}, Occasion: {order1.Occasion}\n");

        Console.WriteLine("Instance Two:");
        Console.WriteLine($"Flavour: {order2.Flavour}, Frosting: {order2.Frosting}, Occasion: {order2.Occasion}\n");
    }
}