class Cake
{
    private static Cake _instance;

    // Private constructor
    private Cake()
    {
        Flavour = "Vanilla";
        Frosting = "Chocolate";
        Occasion = "Birthday"; 
    }

    // Singleton method
    public static Cake GetInstance()
    {
         if (_instance == null)
         {
            _instance = new Cake();
         }

         return _instance;
    }

    // Properties
    public string Flavour { get; set;}
    public string Frosting { get; }
    public string Occasion { get; }

    public void UpdateFlavour(string newFlavour)
    {
      
        Flavour = newFlavour;
    }
}