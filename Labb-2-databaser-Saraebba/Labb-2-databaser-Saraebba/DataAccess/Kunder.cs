using System;
using System.Collections.Generic;

namespace Labb_2_databaser_Saraebba.DataAccess;

public partial class Kunder
{
    public int KundId { get; set; }

    public string Förnamn { get; set; } = null!;

    public string Efternamn { get; set; } = null!;

    public string? Adress { get; set; }

    public int? Postnummer { get; set; }

    public string? Stad { get; set; }

    public long Telefonnummer { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<Ordrar> Ordrars { get; } = new List<Ordrar>();
}
