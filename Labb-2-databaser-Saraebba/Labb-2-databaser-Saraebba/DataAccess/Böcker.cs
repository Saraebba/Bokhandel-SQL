using System;
using System.Collections.Generic;

namespace Labb_2_databaser_Saraebba.DataAccess;

public partial class Böcker
{
    public string Isbn { get; set; } = null!;

    public string Titel { get; set; } = null!;

    public string Språk { get; set; } = null!;

    public int? Pris { get; set; }

    public int AntalSidor { get; set; }

    public DateTime? Utgivningsdatum { get; set; }

    public int FörlagId { get; set; }

    public virtual Förlag Förlag { get; set; } = null!;

    public virtual ICollection<LagerSaldo> LagerSaldos { get; } = new List<LagerSaldo>();

    public virtual ICollection<OrderDetaljer> OrderDetaljers { get; } = new List<OrderDetaljer>();

    public virtual ICollection<Författare> Författares { get; } = new List<Författare>();

    public virtual ICollection<Genre> Genres { get; } = new List<Genre>();
}
