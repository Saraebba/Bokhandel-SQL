using System;
using System.Collections.Generic;

namespace Labb_2_databaser_Saraebba.DataAccess;

public partial class Ordrar
{
    public int OrderId { get; set; }

    public int KundId { get; set; }

    public string Leveransadress { get; set; } = null!;

    public DateTime OrderDatum { get; set; }

    public virtual Kunder Kund { get; set; } = null!;

    public virtual ICollection<OrderDetaljer> OrderDetaljers { get; } = new List<OrderDetaljer>();
}
