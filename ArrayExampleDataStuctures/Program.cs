using System;

class Program
{
    static void Main(string[] args)
    {
        CustomArray<Spaceship> fleet = new CustomArray<Spaceship>();

        // Add spaceship using constructor
        Spaceship s1 = new Spaceship("Apollo", "X-1", 5, 25000, "active",
                                     new DateTime(2023, 5, 1), "research");

        Spaceship s2 = new Spaceship("Titan", "T-500", 10, 30000, "maintenance",
                                     new DateTime(2024, 8, 10), "transport");

        fleet.Add(s1);
        fleet.Add(s2);

        // Display all
        Console.WriteLine("Fleet:");
        fleet.Display();

        //  Search by name
        var found = fleet.Search(s => s.Name == "Titan");
        Console.WriteLine("\nSearch Result:");
        Console.WriteLine(found);

        // Remove one
        fleet.Remove(found);
        Console.WriteLine("\nAfter Removal:");
        fleet.Display();

        //  Optional: resize manually
        fleet.Resize(10);
        Console.WriteLine($"\nResized to 10. Count: {fleet.Count()}");
    }
}
