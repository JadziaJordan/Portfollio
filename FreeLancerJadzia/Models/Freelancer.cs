using System;
using System.Collections.Generic;

namespace FreeLancerJadzia.Models;

public partial class Freelancer
{
    public int FreelancerId { get; set; }

    public int UserId { get; set; }

    public string? Skills { get; set; }

    public string? PortfolioUrl { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual UserP User { get; set; } = null!;
}
