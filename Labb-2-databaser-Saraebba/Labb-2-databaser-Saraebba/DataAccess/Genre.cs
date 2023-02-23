using System;
using System.Collections.Generic;

namespace Labb_2_databaser_Saraebba.DataAccess;

public partial class Genre
{
    public int GenreId { get; set; }

    public string GenreNamn { get; set; } = null!;

    public virtual ICollection<Böcker> Isbns { get; } = new List<Böcker>();
}
