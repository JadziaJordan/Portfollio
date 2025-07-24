using System;

public class Spaceship
{
    public string Name { get; set; }
    public string Model { get; set; }
    public int CrewCapacity { get; set; }
    public double MaxSpeed { get; set; }
    public string Status { get; set; }
    public DateTime LaunchDate { get; set; }
    public string MissionType { get; set; }

    
    //  Constructor to initialize all properties of the spaceship
        public Spaceship(string name, string model, int crewCapacity, double maxSpeed,
             string status, DateTime launchDate, string missionType)
    {
        Name = name;
        Model = model;
        CrewCapacity = crewCapacity;
        MaxSpeed = maxSpeed;
        Status = status;
        LaunchDate = launchDate;
        MissionType = missionType;
    }

    // ✅ Display formatting
    public override string ToString()
    {
        return $"{Name} ({Model}) | Crew: {CrewCapacity} | Speed: {MaxSpeed} km/h | " +
               $"Status: {Status} | Launch: {LaunchDate.ToShortDateString()} | Mission: {MissionType}";
    }
}
