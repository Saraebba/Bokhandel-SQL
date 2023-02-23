using System;
using System.Collections.Generic;

namespace Labb_2_databaser_Saraebba.DataAccess;

public partial class OrderDetaljer
{
    public int OrderId { get; set; }

    public string Isbn { get; set; } = null!;

    public int Antal { get; set; }

    public virtual Böcker IsbnNavigation { get; set; } = null!;

    public virtual Ordrar Order { get; set; } = null!;
}
