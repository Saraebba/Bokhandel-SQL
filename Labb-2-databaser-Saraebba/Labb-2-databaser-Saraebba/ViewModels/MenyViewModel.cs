using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb_2_databaser_Saraebba.Managers;

namespace Labb_2_databaser_Saraebba.ViewModels;

public class MenyViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly BokhandelManager _bokhandelManager;


    public IRelayCommand BokHandelsLagerCommand { get; }
    public IRelayCommand EditFörfattareCommand { get; }

    public MenyViewModel(NavigationManager navigationManager, BokhandelManager bokhandelManager)
    {
        _navigationManager = navigationManager;
        _bokhandelManager = bokhandelManager;

        BokHandelsLagerCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new BokhandelslagerViewModel(_navigationManager, _bokhandelManager));
        EditFörfattareCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new Författare_BöckerViewModel(_bokhandelManager, _navigationManager));
    }
}