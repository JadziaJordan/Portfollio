using System;
using System.Collections.Generic;

namespace FreeLancerJadzia.Models;

public partial class UserP
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Roles { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual Freelancer? Freelancer { get; set; }
}
