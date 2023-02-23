using System;
using System.Collections.Generic;

namespace Labb_2_databaser_Saraebba.DataAccess;

public partial class Butiker
{
    public int ButikId { get; set; }

    public string Adress { get; set; } = null!;

    public int Postnummer { get; set; }

    public string Stad { get; set; } = null!;

    public long Telefon { get; set; }

    public string Email { get; set; } = null!;

    public string Butiksnamn { get; set; } = null!;

    public virtual ICollection<LagerSaldo> LagerSaldos { get; } = new List<LagerSaldo>();
}
