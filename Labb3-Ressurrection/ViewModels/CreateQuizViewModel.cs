using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_Ressurrection.Managers;
using Labb3_Ressurrection.Models;

namespace Labb3_Ressurrection.ViewModels;

public class CreateQuizViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly QuizModel _quizModel;

    public IRelayCommand AddQuestionCommand { get; }
    public IRelayCommand SaveQuizCommand { get; }
    public IRelayCommand QuitQuizCommand { get; }

    private string _titleTextBox;
    public string TitleTextBox
    {
        get => _titleTextBox;
        set => SetProperty(ref _titleTextBox, value);
    }

    private string _questionTextBox;
    public string QuestionTextBox
    {
        get => _questionTextBox;
        set => SetProperty(ref _questionTextBox, value);
    }

    public string AnswerOneTextBox
    {
        get => Answers[0];
        set => SetProperty(ref Answers[0], value);
    }

    public string AnswerTwoTextBox
    {
        get => Answers[1];
        set => SetProperty(ref Answers[1], value);
    }

    public string AnswerThreeTextBox
    {
        get => Answers[2];
        set => SetProperty(ref Answers[2], value);
    }

    private string[] _answers = new string[3];
    public string[] Answers => _answers;

    private bool _radioButtonOne;
    public bool RadioButtonOne
    {
        get => _radioButtonOne;
        set
        {
            SetProperty(ref _radioButtonOne, value);
            _correctAnswerRadioButton = CorrectAnswerValue(out int myInt);
        }
    }

    private bool _radioButtonTwo;
    public bool RadioButtonTwo
    {
        get => _radioButtonTwo;
        set
        {
            SetProperty(ref _radioButtonTwo, value);
            _correctAnswerRadioButton = CorrectAnswerValue(out int myInt);
        }
    }

    private bool _radioButtonThree;
    public bool RadioButtonThree
    {
        get => _radioButtonThree;
        set
        {
            SetProperty(ref _radioButtonThree, value);
            _correctAnswerRadioButton = CorrectAnswerValue(out int myInt);
        }
    }

    private int _correctAnswerRadioButton;
    public int CorrectAnswerRadioButton => _correctAnswerRadioButton;

    public int CorrectAnswerValue(out int myInt)
    {
        myInt = 3;

        if (RadioButtonOne == true)
        {
            myInt = 0;
        }
        else if (RadioButtonTwo == true)
        {
            myInt = 1;
        }
        else if (RadioButtonThree == true)
        {
            myInt = 2;
        }
        return myInt;
    }

    private int _questionCounter;
    public int QuestionCounter
    {
        get => _questionCounter;
        set
        {
            SetProperty(ref _questionCounter, value);
        }
    }

    public CreateQuizViewModel(NavigationManager navigationManager, QuizModel quizModel)
    {
        _navigationManager = navigationManager;
        _quizModel = quizModel;

        AddQuestionCommand = new RelayCommand(() =>
        {
            if (IsQuestionComplete(true))
            {
                _quizModel.AddQuestion(QuestionTextBox, CorrectAnswerRadioButton, (string[])Answers.Clone());

                QuestionCounter++;

                //Reset props
                QuestionTextBox = string.Empty;

                RadioButtonOne = false;
                RadioButtonTwo = false;
                RadioButtonThree = false;

                AnswerOneTextBox = "";
                AnswerTwoTextBox = "";
                AnswerThreeTextBox = "";
            }
        }, () => true);

        SaveQuizCommand = new RelayCommand(() =>
        {
            if (IsQuizComplete(true))
            {
                _quizModel.SaveQuizAsync(TitleTextBox);

                //Reset Title
                TitleTextBox = string.Empty;
                QuestionCounter = 0;
            }
        }, () => true);

        QuitQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _quizModel));
    }

    public bool IsQuestionComplete(bool isQuestionComplete)
    {
        if (string.IsNullOrEmpty(QuestionTextBox))
        {
            return false;
        }
        if (string.IsNullOrEmpty(Answers[0]) || string.IsNullOrEmpty(Answers[1]) || string.IsNullOrEmpty(Answers[2]))
        {
            return false;
        }
        if (RadioButtonOne == false && RadioButtonTwo == false && RadioButtonThree == false)
        {
            return false;
        }
        return true;
    }

    public bool IsQuizComplete(bool isQuizComplete)
    {
        if (QuestionCounter > 0)
        {
            if (string.IsNullOrEmpty(TitleTextBox))
            {
                return false;
            }
            return true;
        }
        return false;
    }
}