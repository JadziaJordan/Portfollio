using System;
using System.Collections.Generic;

namespace FreeLancerJadzia.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int BookingId { get; set; }

    public int FreelancerId { get; set; }

    public int CustomerId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Freelancer Freelancer { get; set; } = null!;
}
