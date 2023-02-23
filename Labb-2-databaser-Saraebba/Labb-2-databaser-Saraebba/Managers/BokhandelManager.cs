using System.Collections.Generic;
using System.Linq;
using Labb_2_databaser_Saraebba.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Labb_2_databaser_Saraebba.Managers;

public class BokhandelManager
{
    private BokhandelContext _bokhandelContext;
    public ICollection<LagerSaldo> _lagerSaldo { get; set; }
    public ICollection<Böcker> _böckers { get; set; }
    public ICollection<Butiker> _butikers { get; set; }
    public ICollection<Författare> _författares { get; set; }
    public ICollection<Förlag> _förlags { get; set; }


    public BokhandelManager()
    {
        _bokhandelContext = new BokhandelContext();
        _lagerSaldo = _bokhandelContext.LagerSaldos.Include(ls => ls.IsbnNavigation).Include(ls => ls.Butik)
            .ToList();
        _böckers = _bokhandelContext.Böckers.Include(f => f.Författares).Include(g => g.Genres).ToList();
        _butikers = _bokhandelContext.Butikers.ToList();
        _författares = _bokhandelContext.Författares.ToList();
        _förlags = _bokhandelContext.Förlags.ToList();
    }
}