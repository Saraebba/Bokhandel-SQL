using System;
using System.Collections.Generic;

namespace Labb_2_databaser_Saraebba.DataAccess;

public partial class VKundOrdrar
{
    public int OrderNummer { get; set; }

    public string Kund { get; set; } = null!;

    public DateTime OrderDatum { get; set; }

    public string OrderTotal { get; set; } = null!;
}
