using System;
using System.Collections.Generic;

namespace FreeLancerJadzia.Models;

public partial class BookingType
{
    public int BookingTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
