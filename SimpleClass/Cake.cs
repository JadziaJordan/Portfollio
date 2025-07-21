using System;

class Cake
{

 public string Flavour { get; set; }
 public int Age { get; set; }

 public Cake(string flavour1, int age1)
    {
        Flavour = flavour1;
        Age = age1;
    }

    public void Introduce()
    {
        Console.WriteLine($"Hello, my cake order flavour is {Flavour} and I want this number candle on cake {Age}.");
    }
}