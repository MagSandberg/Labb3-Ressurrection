using CommunityToolkit.Mvvm.ComponentModel;
using Labb3_Ressurrection.Managers;

namespace Labb3_Ressurrection.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;

    public ObservableObject CurrentViewModel => _navigationManager.CurrentViewModel;

    public MainViewModel(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;

        _navigationManager.CurrentViewModelChanged += CurrentViewModelChanged;
    }


    private void CurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}