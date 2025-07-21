using System;
using System.Collections.Generic;

namespace FreeLancerJadzia.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public DateTime PaymentDate { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
