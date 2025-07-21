using System;
using System.Collections.Generic;

namespace FreeLancerJadzia.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Payment? Payment { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual UserP User { get; set; } = null!;
}
