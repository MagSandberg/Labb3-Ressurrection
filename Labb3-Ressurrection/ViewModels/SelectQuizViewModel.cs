using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_Ressurrection.Managers;
using Labb3_Ressurrection.Models;

namespace Labb3_Ressurrection.ViewModels;

public class SelectQuizViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly QuizModel _quizModel;

    public IRelayCommand QuitQuizCommand { get; }

    public List<string> QuizTitle { get; set; }

    private string _selectedQuiz = null!;
    public string SelectedQuiz
    {
        get => _selectedQuiz;
        set
        {
            SetProperty(ref _selectedQuiz, value);
            _quizModel.QuizQuestionProperties = _quizModel.GetQuestions(SelectedQuiz);
        }
    }

    public IRelayCommand PlaySelectedQuizCommand { get; }

    public SelectQuizViewModel(NavigationManager navigationManager, QuizModel quizModel)
    {
        _navigationManager = navigationManager;
        _quizModel = quizModel;


        QuizTitle = _quizModel.QuizTitles?.quizTitles;

        //_quizModel.GetQuestions(SelectedQuiz);

        PlaySelectedQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new QuizViewModel(_navigationManager, _quizModel));
        QuitQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _quizModel));
    }
}