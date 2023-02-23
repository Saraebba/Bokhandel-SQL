using System;
using System.Collections.Generic;

namespace Labb_2_databaser_Saraebba.DataAccess;

public partial class Förlag
{
    public int FörlagId { get; set; }

    public string Namn { get; set; } = null!;

    public virtual ICollection<Böcker> Böckers { get; } = new List<Böcker>();
}
