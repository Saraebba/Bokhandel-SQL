using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb_2_databaser_Saraebba.Managers;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using Labb_2_databaser_Saraebba.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Labb_2_databaser_Saraebba.ViewModels;

public class Författare_BöckerViewModel : ObservableObject
{

    private readonly BokhandelManager _bokhandelManager;
    private readonly NavigationManager _navigationManager;

    private ObservableCollection<Författare> _författares;

    public ObservableCollection<Författare> Författares
    {
        get { return _författares; }
        set { SetProperty(ref _författares, value); }
    }

    private ObservableCollection<Böcker> _böckers;

    public ObservableCollection<Böcker> Böckers
    {
        get { return _böckers; }
        set { SetProperty(ref _böckers, value); }
    }

    private ObservableCollection<Förlag> _förlags;

    public ObservableCollection<Förlag> Förlags
    {
        get { return _förlags; }
        set { _förlags = value; }
    }


    private string _förnamn;

    public string Förnamn
    {
        get { return _förnamn; }
        set
        {
            SetProperty(_förnamn, value, (f) => _förnamn = f);
            LäggTillFörfattareCommand.NotifyCanExecuteChanged();
        }
    }

    private string _efternamn;

    public string Efternamn
    {
        get { return _efternamn; }
        set
        {
            SetProperty(_efternamn, value, (f) => _efternamn = f);
            LäggTillFörfattareCommand.NotifyCanExecuteChanged();
        }
    }

    private DateTime? _födelsedatum;

    public DateTime? Födelsedatum
    {
        get { return _födelsedatum; }
        set
        {
            SetProperty(_födelsedatum, value, (f) => _födelsedatum = f);
        }
    }

    private string _titel;

    public string Titel
    {
        get { return _titel; }
        set { SetProperty(ref _titel, value); }
    }

    private string _isbn;

    public string Isbn
    {
        get { return _isbn; }
        set
        {
            SetProperty(ref _isbn, value);
            LäggTillNyBokCommand.NotifyCanExecuteChanged();
            TabortBokCommand.NotifyCanExecuteChanged();
            UppdateraBokCommand.NotifyCanExecuteChanged();
        }
    }

    private Författare? _selectedFörfattare;

    public Författare? SelectedFörfattare
    {
        get { return _selectedFörfattare; }
        set
        {
            if (value is null) return;
            if (SetProperty(_selectedFörfattare, value, (v) => _selectedFörfattare = v))
            {
                Förnamn = _selectedFörfattare.Förnamn;
                Efternamn = _selectedFörfattare.Efternamn;
                Födelsedatum = _selectedFörfattare.Födelsedatum;
            }
            ValdFörfattare(_selectedFörfattare.Id);
            TabortFörfattareCommand.NotifyCanExecuteChanged();
            UppdateraFörfattareCommand.NotifyCanExecuteChanged();
        }
    }

    private Böcker? _selectedbok;

    public Böcker? SelectedBok
    {
        get { return _selectedbok; }
        set
        {
            if (value is null) return;
            if (SetProperty(_selectedbok, value, (v) => _selectedbok = v))
            {
                Titel = _selectedbok.Titel;
                Isbn = _selectedbok.Isbn;
                Pris = (int)_selectedbok.Pris;
                AntalSidor = _selectedbok.AntalSidor;
                Språk = _selectedbok.Språk;
                Utgivningsdatum = (DateTime)_selectedbok.Utgivningsdatum;
            }
        }
    }

    private Förlag _selectedFörlag;

    public Förlag SelectedFörlag
    {
        get { return _selectedFörlag; }
        set
        {
            SetProperty(ref _selectedFörlag, value);
        }
    }



    private string _språk;

    public string Språk
    {
        get { return _språk; }
        set
        {
            SetProperty(ref _språk, value);
            LäggTillNyBokCommand.NotifyCanExecuteChanged();
            TabortBokCommand.NotifyCanExecuteChanged();
            UppdateraBokCommand.NotifyCanExecuteChanged();
        }
    }

    private int _pris;

    public int Pris
    {
        get { return _pris; }
        set { SetProperty(ref _pris, value); }
    }

    private int _antalSidor;

    public int AntalSidor
    {
        get { return _antalSidor; }
        set { SetProperty(ref _antalSidor, value); }
    }

    private DateTime _utgivningsdatum;

    public DateTime Utgivningsdatum
    {
        get { return _utgivningsdatum; }
        set
        {
            SetProperty(ref _utgivningsdatum, value);
        }
    }

    private int _förlagId;

    public int FörlagId
    {
        get { return _förlagId; }
        set
        {
            SetProperty(ref _förlagId, value);
            LäggTillNyBokCommand.NotifyCanExecuteChanged();
            TabortBokCommand.NotifyCanExecuteChanged();
            UppdateraBokCommand.NotifyCanExecuteChanged();
        }
    }



    public IRelayCommand MenyCommand { get; }
    public IRelayCommand LäggTillFörfattareCommand { get; }
    public IRelayCommand TabortFörfattareCommand { get; }
    public IRelayCommand UppdateraFörfattareCommand { get; }
    public IRelayCommand LäggTillNyBokCommand { get; }
    public IRelayCommand TabortBokCommand { get; }
    public IRelayCommand UppdateraBokCommand { get; }

    public Författare_BöckerViewModel(BokhandelManager bokhandelManager, NavigationManager navigationManager)
    {
        _bokhandelManager = bokhandelManager;
        _navigationManager = navigationManager;

        ListaFörfattare();
        Förlags = new ObservableCollection<Förlag>(_bokhandelManager._förlags);
        LäggTillFörfattareCommand = new RelayCommand(LäggTillFörfattare, ButtonCommand_CanExecuteLäggTillFörfattare);
        TabortFörfattareCommand = new RelayCommand(TabortFörFattare, ButtonCommand_CanExecuteFörfattare);
        UppdateraFörfattareCommand = new RelayCommand(UppdateraFörfattare, ButtonCommand_CanExecuteFörfattare);
        LäggTillNyBokCommand = new RelayCommand(LäggTillNyBok, ButtonCommand_CanExecuteBok);
        TabortBokCommand = new RelayCommand(TabortBok, ButtonCommand_CanExecuteBok);
        UppdateraBokCommand = new RelayCommand(UppdateraBok, ButtonCommand_CanExecuteBok);
        MenyCommand = new RelayCommand(() =>
            _navigationManager.CurrentViewModel = new MenyViewModel(_navigationManager, _bokhandelManager));
    }

    private bool ButtonCommand_CanExecuteLäggTillFörfattare()
    {
        if (string.IsNullOrEmpty(_förnamn) || string.IsNullOrEmpty(_efternamn))
        {
            return false;
        }

        return true;
    }
    private bool ButtonCommand_CanExecuteFörfattare()
    {
        if (_selectedFörfattare is null)
        {
            return false;
        }

        return true;
    }

    private bool ButtonCommand_CanExecuteBok()
    {
        if (_selectedFörlag is null || string.IsNullOrEmpty(_språk) || string.IsNullOrEmpty(_isbn) || _isbn.Length > 13 || _isbn.Length < 13)
        {
            return false;
        }
        return true;
    }

    public ObservableCollection<Böcker> ValdFörfattare(int författarId)
    {
        using (var context = new BokhandelContext())
        {
            var författare = context.Författares.Include(p => p.BokIsbns)
                .FirstOrDefault(p => p.Id == författarId);
            if (författare == null) return null;
            Böckers = new ObservableCollection<Böcker>(författare.BokIsbns);
        }

        return Böckers;
    }

    public void LäggTillFörfattare()
    {
        using (var context = new BokhandelContext())
        {
            var författare = new Författare()
            {
                Förnamn = _förnamn,
                Efternamn = _efternamn,
                Födelsedatum = _födelsedatum
            };
            context.Författares.Add(författare);
            context.SaveChanges();
            _selectedFörfattare = null;
            Författares.Clear();
            var författares = context.Författares.ToList();
            foreach (var f in författares)
            {
                Författares.Add(f);
            }
        }
    }


    public void TabortFörFattare()
    {
        if (_selectedFörfattare is null)
        {
            return;
        }

        using (var context = new BokhandelContext())
        {
            context.Böckers.RemoveRange(ValdFörfattare(_selectedFörfattare.Id));
            context.SaveChanges();
            Böckers.Clear();
        }
        using (var context = new BokhandelContext())
        {
            context.Böckers.RemoveRange(_selectedFörfattare.BokIsbns);
            context.Författares.Remove(_selectedFörfattare);
            context.SaveChanges();
            _selectedFörfattare = null;
            Författares.Clear();
            var författares = context.Författares.ToList();
            foreach (var f in författares)
            {
                Författares.Add(f);
            }
            Böckers.Clear();
        }
    }

    public void UppdateraFörfattare()
    {
        using (var context = new BokhandelContext())
        {
            var författare = context.Författares.FirstOrDefault(f => f.Id == _selectedFörfattare.Id);
            if (författare != null)
            {
                författare.Förnamn = _förnamn;
                författare.Efternamn = _efternamn;
                författare.Födelsedatum = _födelsedatum;
            }
            context.Update(författare);
            context.SaveChanges();
            _selectedFörfattare = null;
            Författares.Clear();
            var författares = context.Författares.ToList();
            foreach (var f in författares)
            {
                Författares.Add(f);
            }
        }
    }

    public void LäggTillNyBok()
    {
        if (_selectedFörfattare is null)
        {
            return;
        }
        using (var context = new BokhandelContext())
        {
            var författare = context.Författares.Include(p => p.BokIsbns)
                .FirstOrDefault(p => p.Id == _selectedFörfattare.Id);
            Böcker? bok;
            if (författare.BokIsbns.Any(b => b.Isbn.Equals(_isbn)))
                return;
            if (context.Böckers.Any(b => b.Isbn == _isbn))
                bok = context.Böckers.FirstOrDefault(b => b.Isbn == _isbn);
            else
                bok = new Böcker()
                {
                    Titel = _titel,
                    Isbn = _isbn,
                    Språk = _språk,
                    AntalSidor = _antalSidor,
                    Pris = _pris,
                    Utgivningsdatum = _utgivningsdatum,
                    FörlagId = _selectedFörlag.FörlagId
                };
            författare.BokIsbns.Add(bok);
            context.SaveChanges();
            _selectedbok = null;
            Böckers.Clear();
            ValdFörfattare(_selectedFörfattare.Id);
        }
    }

    public void UppdateraBok()
    {
        using (var context = new BokhandelContext())
        {
            if (_selectedbok == null) return;
            var bok = context.Böckers.FirstOrDefault(b => b.Isbn == _selectedbok.Isbn);
            if (bok != null)
            {
                if (bok.Isbn != _isbn)
                    return;
                bok.Isbn = _isbn;
                bok.Titel = _titel;
                bok.AntalSidor = _antalSidor;
                bok.Pris = _pris;
                bok.Språk = _språk;
                bok.FörlagId = _selectedFörlag.FörlagId;
                bok.Utgivningsdatum = _utgivningsdatum;
            }
            context.Update(bok);
            context.SaveChanges();
            _selectedbok = null;
            Böckers.Clear();
            ValdFörfattare(_selectedFörfattare.Id);
        }
    }
    public void TabortBok()
    {
        if (_selectedFörfattare is null || _selectedbok is null)
        {
            return;
        }
        using (var context = new BokhandelContext())
        {
            foreach (var b in context.Böckers.Where(b => b.Isbn == _selectedbok.Isbn))
            {
                context.Böckers.Remove(b);
            }
            context.SaveChanges();
            _selectedbok = null;
            Böckers.Clear();
            ValdFörfattare(_selectedFörfattare.Id);
        }
    }

    public void ListaFörfattare()
    {
        using (var context = new BokhandelContext())
        {
            Författares = new ObservableCollection<Författare>(context.Författares);
        }
    }
}