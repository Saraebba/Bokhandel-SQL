using CommunityToolkit.Mvvm.ComponentModel;
using Labb_2_databaser_Saraebba.Managers;

namespace Labb_2_databaser_Saraebba.ViewModels;

public class NavigationViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;

    public ObservableObject CurrentViewModel => _navigationManager.CurrentViewModel;

    public NavigationViewModel(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        _navigationManager.CurrentViewModelChanged += CurrentViewModelChanged;
    }

    private void CurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}