using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb_2_databaser_Saraebba.Managers;
using System.Collections.ObjectModel;
using System.Linq;
using Labb_2_databaser_Saraebba.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Labb_2_databaser_Saraebba.ViewModels;

public class BokhandelslagerViewModel : ObservableObject
{
    private readonly BokhandelManager _bokhandelManager;
    private readonly NavigationManager _navigationManager;

    private ObservableCollection<Böcker> _böckers;

    public ObservableCollection<Böcker> Böckers
    {
        get { return _böckers; }
        set { SetProperty(ref _böckers, value); }
    }

    private ObservableCollection<Butiker> _butikers;

    public ObservableCollection<Butiker> Butikers
    {
        get { return _butikers; }
        set { SetProperty(ref _butikers, value); }
    }

    private ObservableCollection<LagerSaldo> _lagerSaldos;

    public ObservableCollection<LagerSaldo> LagerSaldos
    {
        get { return _lagerSaldos; }
        set { SetProperty(ref _lagerSaldos, value); }
    }

    private int _antal;

    public int Antal
    {
        get { return _antal; }
        set
        {
            SetProperty(ref _antal, value);
        }
    }

    private int _butikId;

    public int ButikId
    {
        get { return _butikId; }
        set { SetProperty(ref _butikId, value); }
    }

    private string _isbn;

    public string Isbn
    {
        get { return _isbn; }
        set { SetProperty(ref _isbn, value); }
    }

    private string _butikNamn;

    public string ButikNamn
    {
        get { return _butikNamn; }
        set { SetProperty(ref _butikNamn, value); }
    }


    private string _titel;

    public string Titel
    {
        get { return _titel; }
        set { SetProperty(ref _titel, value); }
    }

    private string _författare;

    public string Författare
    {
        get { return _författare; }
        set { SetProperty(ref _författare, value); }
    }

    private Butiker _selectedButik;

    public Butiker SelectedButik
    {
        get { return _selectedButik; }
        set
        {
            SetProperty(ref _selectedButik, value);
            ValdButikSaldo();
        }
    }

    private Böcker? _selectedBok;

    public Böcker? SelectedBok
    {
        get { return _selectedBok; }
        set
        {
            if (SetProperty(_selectedBok, value, (v) => _selectedBok = v))
            {
                _selectedSaldo = null;
            }
            LäggTillBokCommand.NotifyCanExecuteChanged();
            LäggTillAntalCommand.NotifyCanExecuteChanged();
            TaBortAntalCommand.NotifyCanExecuteChanged();
            TaBortBokCommand.NotifyCanExecuteChanged();
        }
    }

    private LagerSaldo? _selectedSaldo;

    public LagerSaldo? SelectedSaldo
    {
        get { return _selectedSaldo; }
        set
        {
            if (SetProperty(_selectedSaldo, value, (v) => _selectedSaldo = v))
            {
                _selectedBok = null;
            }
            LäggTillAntalCommand.NotifyCanExecuteChanged();
            TaBortAntalCommand.NotifyCanExecuteChanged();
            TaBortBokCommand.NotifyCanExecuteChanged();
            LäggTillBokCommand.NotifyCanExecuteChanged();

        }
    }

    public IRelayCommand TaBortAntalCommand { get; }
    public IRelayCommand LäggTillAntalCommand { get; }
    public IRelayCommand MenyCommand { get; }
    public IRelayCommand LäggTillBokCommand { get; }
    public IRelayCommand TaBortBokCommand { get; }


    public BokhandelslagerViewModel(NavigationManager navigationManager, BokhandelManager bokhandelManager)
    {
        _bokhandelManager = bokhandelManager;
        _navigationManager = navigationManager;

        ListaBöcker();
        Butikers = new ObservableCollection<Butiker>(_bokhandelManager._butikers);

        LäggTillAntalCommand = new RelayCommand(LäggTillAntal, ButtonCommand_CanExecuteSaldo);
        TaBortAntalCommand = new RelayCommand(TabortAntal, ButtonCommand_CanExecuteSaldo);
        TaBortBokCommand = new RelayCommand(TabortBok, ButtonCommand_CanExecuteSaldo);
        LäggTillBokCommand = new RelayCommand(LäggTillBok, ButtonCommand_CanExecuteBok);
        MenyCommand = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new MenyViewModel(_navigationManager, _bokhandelManager));
    }

    private bool ButtonCommand_CanExecuteBok()
    {
        if (_selectedBok is null)
        {
            return false;
        }
        return true;
    }

    private bool ButtonCommand_CanExecuteSaldo()
    {
        if (_selectedSaldo is null)
        {
            return false;
        }
        return true;
    }

    public void LäggTillAntal()
    {
        using (var context = new BokhandelContext())
        {
            var nyttAntal = new LagerSaldo()
            {
                ButikId = _selectedButik.ButikId,
                Isbn = _selectedSaldo.Isbn,
                Antal = _selectedSaldo.Antal + 1
            };
            context.Update(nyttAntal);
            LagerSaldos.Clear();
            context.SaveChanges();
            var saldos = context.LagerSaldos.Include(t => t.IsbnNavigation).Where(b => b.ButikId == _selectedButik.ButikId);
            foreach (var saldo in saldos)
            {
                LagerSaldos.Add(saldo);
            }
        }
    }

    public void TabortAntal()
    {

        using (var context = new BokhandelContext())
        {
            var nyttAntal = new LagerSaldo()
            {
                ButikId = _selectedButik.ButikId,
                Isbn = _selectedSaldo.Isbn,
                Antal = _selectedSaldo.Antal - 1
            };
            context.Update(nyttAntal);
            LagerSaldos.Clear();
            context.SaveChanges();
            var saldos = context.LagerSaldos.Include(t => t.IsbnNavigation).Where(b => b.ButikId == _selectedButik.ButikId);
            foreach (var saldo in saldos)
            {
                LagerSaldos.Add(saldo);
            }
        }
    }

    public ObservableCollection<LagerSaldo> ValdButikSaldo()
    {
        LagerSaldos =
            new ObservableCollection<LagerSaldo>(
                _bokhandelManager._lagerSaldo.Where(l => l.ButikId.Equals(_selectedButik.ButikId)));
        return LagerSaldos;
    }


    public void TabortBok()
    {

        using (var context = new BokhandelContext())
        {
            foreach(var bok in context.LagerSaldos.Where(b => b.IsbnNavigation == _selectedSaldo.IsbnNavigation && b.ButikId.Equals(_selectedButik.ButikId)))
            {
                context.LagerSaldos.Remove(bok);
            }
            LagerSaldos.Clear();
            context.SaveChanges();
            var saldos = context.LagerSaldos.Include(t => t.IsbnNavigation).Where(b => b.ButikId == _selectedButik.ButikId);
            foreach (var saldo in saldos)
            {
                LagerSaldos.Add(saldo);
            }
        }
    }

    public void LäggTillBok()
    {
        using (var context = new BokhandelContext())
        {
            if (_selectedButik == null)
                return;
            if (context.LagerSaldos.Where(b => b.ButikId.Equals(_selectedButik.ButikId)).FirstOrDefault(b => b.IsbnNavigation.Isbn.Equals(_selectedBok.Isbn)) != null) return;
            var bok = new LagerSaldo()
            {
                ButikId = _selectedButik.ButikId,
                Isbn = _selectedBok.Isbn,
                Antal = 0
            };
            context.LagerSaldos.Add(bok);
            LagerSaldos.Clear();
            context.SaveChanges();
            var saldos = context.LagerSaldos.Include(t => t.IsbnNavigation).Where(b => b.ButikId == _selectedButik.ButikId);
            foreach (var saldo in saldos)
            {
                LagerSaldos.Add(saldo);
            }
        }
    }

    public void ListaBöcker()
    {
        using (var context = new BokhandelContext())
        {
            Böckers = new ObservableCollection<Böcker>(context.Böckers);
        }
    }
}