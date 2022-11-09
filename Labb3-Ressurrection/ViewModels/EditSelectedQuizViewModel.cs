using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_Ressurrection.Managers;
using Labb3_Ressurrection.Models;
using static Labb3_Ressurrection.Models.QuizModel;

namespace Labb3_Ressurrection.ViewModels;

public class EditSelectedQuizViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly QuizModel _quizModel;

    public IRelayCommand EditQuestionCommand { get; }
    public IRelayCommand SaveQuizCommand { get; }
    public IRelayCommand MainMenuCommand { get; }
    public IRelayCommand RemoveQuestionCommand { get; }


    private List<QuestionProperties> _questionList;

    public List<QuestionProperties> QuestionList
    {
        get { return _questionList; }
        set { SetProperty(ref _questionList, value); }
    }

    private string _selectedQuiz = null!;

    public string SelectedQuiz
    {
        get => _quizModel.QuizTitle;
        set { SetProperty(ref _selectedQuiz, value); }
    }

    private int _selectedQuestionIndex;

    public int SelectedQuestionIndex
    {
        get => _selectedQuestionIndex;
        set
        {
            SetProperty(ref _selectedQuestionIndex, value);
            PopulateProperties(_selectedQuestionIndex);
        }
    }

    private string _quizQuestion;

    public string QuizQuestion
    {
        get => _quizQuestion;
        set { SetProperty(ref _quizQuestion, value); }
    }

    private string _quizAnswerOne;

    public string QuizAnswerOne
    {
        get => _quizAnswerOne;
        set
        {
            SetProperty(ref _quizAnswerOne, value);
            SetProperty(ref Answers[0], value);
        }
    }

    private string _quizAnswerTwo;

    public string QuizAnswerTwo
    {
        get => _quizAnswerTwo;
        set
        {
            SetProperty(ref _quizAnswerTwo, value);
            SetProperty(ref Answers[1], value);
        }

    }

    private string _quizAnswerThree;

    public string QuizAnswerThree
    {
        get => _quizAnswerThree;
        set
        {
            SetProperty(ref _quizAnswerThree, value);
            SetProperty(ref Answers[2], value);
        }

    }

    public int CorrectAnswer { get; set; }

    private bool _checkBoxOne;
    public bool CheckBoxOne
    {
        get => _checkBoxOne;
        set
        {
            SetProperty(ref _checkBoxOne, value);
            if (CheckBoxOne.Equals(true))
            {
                CheckBoxTwo = false;
                CheckBoxThree = false;
            }
        }
    }

    private bool _checkBoxTwo;
    public bool CheckBoxTwo
    {
        get => _checkBoxTwo;
        set
        {
            SetProperty(ref _checkBoxTwo, value);
            if (CheckBoxTwo.Equals(true))
            {
                CheckBoxOne = false;
                CheckBoxThree = false;
            }
        }
    }

    private bool _checkBoxThree;
    public bool CheckBoxThree
    {
        get => _checkBoxThree;
        set
        {
            SetProperty(ref _checkBoxThree, value);
            if (CheckBoxThree.Equals(true))
            {
                CheckBoxOne = false;
                CheckBoxTwo = false;
            }
        }
    }

    private string[] _answers = new string[3];
    public string[] Answers => _answers;

    public EditSelectedQuizViewModel(NavigationManager navigationManager, QuizModel quizModel)
    {
        _navigationManager = navigationManager;
        _quizModel = quizModel;

        var questions = _quizModel.QuizQuestionProperties.Result;
        QuestionList = questions;

        QuizQuestion = QuestionList[0].Statement;

        QuizAnswerOne = QuestionList[0].Answers[0];
        QuizAnswerTwo = QuestionList[0].Answers[1];
        QuizAnswerThree = QuestionList[0].Answers[2];

        CorrectAnswer = QuestionList[0].CorrectAnswer;
        CheckCorrectAnswer();

        RemoveQuestionCommand = new RelayCommand(() =>
        {
            _quizModel.RemoveQuestion(SelectedQuestionIndex);
            UpdateQuizList();
        }, () => true);

        EditQuestionCommand = new RelayCommand(() =>
        {
            if (IsQuestionComplete())
            {
                QuestionList[SelectedQuestionIndex].Statement = QuizQuestion;

                QuestionList[SelectedQuestionIndex].Answers[0] = QuizAnswerOne;
                QuestionList[SelectedQuestionIndex].Answers[1] = QuizAnswerTwo;
                QuestionList[SelectedQuestionIndex].Answers[2] = QuizAnswerThree;

                var correctAnswer = 0;

                if (CheckBoxOne)
                {
                    correctAnswer = 0;
                }
                else if (CheckBoxTwo)
                {
                    correctAnswer = 1;
                }
                else
                {
                    correctAnswer = 2;
                }

                QuestionList[SelectedQuestionIndex].CorrectAnswer = correctAnswer;
                UpdateQuizList();
                _quizModel.EditQuestion(SelectedQuestionIndex, QuestionList[SelectedQuestionIndex]);
            }
        }, () => true);

        SaveQuizCommand = new RelayCommand(() =>
        {
            if (IsQuizComplete())
            {
                _quizModel.SaveQuizAsync(SelectedQuiz);
                UpdateQuizList();
            }

        }, () => true);

        MainMenuCommand = new RelayCommand(() =>
        {
            _quizModel.QuizQuestionProperties.Result.Clear();
            _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _quizModel);
        }, () => true);
    }

    public void UpdateQuizList()
    {
        QuestionList = _quizModel.QuizQuestionProperties.Result;
    }

    public bool IsQuestionComplete()
    {
        if (string.IsNullOrEmpty(QuizQuestion))
        {
            return false;
        }

        if (string.IsNullOrEmpty(QuizAnswerOne) || string.IsNullOrEmpty(QuizAnswerTwo) ||
            string.IsNullOrEmpty(QuizAnswerThree))
        {
            return false;
        }

        if (CheckBoxOne == false && CheckBoxTwo == false && CheckBoxThree == false)
        {
            return false;
        }
        return true;
    }

    public bool IsQuizComplete()
    {
        if (IsQuestionComplete())
        {
            return true;
        }

        return false;
    }

    public void PopulateProperties(int index)
    {
        QuizQuestion = QuestionList[index].Statement;

        QuizAnswerOne = QuestionList[index].Answers[0];
        QuizAnswerTwo = QuestionList[index].Answers[1];
        QuizAnswerThree = QuestionList[index].Answers[2];

        CorrectAnswer = QuestionList[index].CorrectAnswer;
        CheckCorrectAnswer();
    }

    public void CheckCorrectAnswer()
    {
        if (CorrectAnswer == 0)
        {
            CheckBoxOne = true;
            CheckBoxTwo = false;
            CheckBoxThree = false;
        }
        if (CorrectAnswer == 1)
        {
            CheckBoxOne = false;
            CheckBoxTwo = true;
            CheckBoxThree = false;
        }
        if (CorrectAnswer == 2)
        {
            CheckBoxOne = false;
            CheckBoxTwo = false;
            CheckBoxThree = true;
        }
    }
}