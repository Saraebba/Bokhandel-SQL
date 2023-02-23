using System;
using System.Collections.Generic;

namespace Labb_2_databaser_Saraebba.DataAccess;

public partial class Författare
{
    public int Id { get; set; }

    public string Förnamn { get; set; } = null!;

    public string Efternamn { get; set; } = null!;

    public DateTime? Födelsedatum { get; set; }

    public virtual ICollection<Böcker> BokIsbns { get; } = new List<Böcker>();
}
