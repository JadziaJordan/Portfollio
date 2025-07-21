using System;
using System.Collections.Generic;

namespace FreeLancerJadzia.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public int FreelancerId { get; set; }

    public int BookingTypeId { get; set; }

    public int? PaymentId { get; set; }

    public DateTime BookingDate { get; set; }

    public string BookingStatus { get; set; } = null!;

    public virtual BookingType BookingType { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Freelancer Freelancer { get; set; } = null!;

    public virtual Payment? Payment { get; set; }

    public virtual Review? Review { get; set; }
}
