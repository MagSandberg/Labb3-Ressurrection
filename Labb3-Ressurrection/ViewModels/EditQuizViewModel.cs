using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_Ressurrection.Managers;
using Labb3_Ressurrection.Models;

namespace Labb3_Ressurrection.ViewModels;

public class EditQuizViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly QuizModel _quizModel;
    public IRelayCommand EditSelectedQuizCommand { get; }
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
            _quizModel.QuizTitle = SelectedQuiz;
        }
    }

    public EditQuizViewModel(NavigationManager navigationManager, QuizModel quizModel)
    {
        _navigationManager = navigationManager;
        _quizModel = quizModel;

        QuizTitle = _quizModel.QuizTitles.quizTitles;

        EditSelectedQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new EditSelectedQuizViewModel(_navigationManager, _quizModel));
        QuitQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _quizModel));
    }
}