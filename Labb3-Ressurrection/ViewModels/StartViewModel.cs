using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_Ressurrection.Managers;
using Labb3_Ressurrection.Models;

namespace Labb3_Ressurrection.ViewModels;

public class StartViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly QuizModel _quizModel;

    public IRelayCommand PlayQuizCommand { get; }
    public IRelayCommand CreateQuizCommand { get; }
    public IRelayCommand EditQuizCommand { get; }
    public IRelayCommand CloseCommand { get; }


    public StartViewModel(NavigationManager navigationManager, QuizModel quizModel)
    {
        _navigationManager = navigationManager;
        _quizModel = quizModel;

        PlayQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new SelectQuizViewModel(_navigationManager, _quizModel));
        CreateQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new CreateQuizViewModel(_navigationManager, _quizModel));
        EditQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new EditQuizViewModel(_navigationManager, _quizModel));
        CloseCommand = new RelayCommand(() => { Application.Current.Shutdown(); });
    }
}