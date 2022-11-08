using System;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_Ressurrection.Managers;
using Labb3_Ressurrection.Models;
using Labb3_Ressurrection.Views;

namespace Labb3_Ressurrection.ViewModels;

public class QuizViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly QuizModel _quizModel;

    public IRelayCommand GetNextRandomQuestion { get; }
    public IRelayCommand EditQuizCommand { get; }
    public IRelayCommand QuitQuizCommand { get; }


    public string QuizTitle { get; set; }

    private string _quizQuestion;
    public string QuizQuestion
    {
        get => _quizQuestion;
        set
        {
            SetProperty(ref _quizQuestion, value);
        }
    }

    private string _quizAnswerOne;
    public string QuizAnswerOne
    {
        get => _quizAnswerOne;
        set
        {
            SetProperty(ref _quizAnswerOne, value);
        }
    }

    private string _quizAnswerTwo;
    public string QuizAnswerTwo
    {
        get => _quizAnswerTwo;
        set
        {
            SetProperty(ref _quizAnswerTwo, value);
        }
    }

    private string _quizAnswerThree;
    public string QuizAnswerThree
    {
        get => _quizAnswerThree;
        set
        {
            SetProperty(ref _quizAnswerThree, value);
        }
    }

    private int _correctUserAnswers;
    public int CorrectUserAnswers
    {
        get => _correctUserAnswers;
        set
        {
            SetProperty(ref _correctUserAnswers, value);
        }
    }

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

    public static int TotalQuizQuestions { get; set; }

    private int _quizCorrectAnswer;

    public int QuizCorrectAnswer
    {
        get { return _quizCorrectAnswer; }
        set
        {
            SetProperty(ref _quizCorrectAnswer, value);
        }
    }

    private int _userScore;
    public int UserScore
    {
        get { return _userScore; }
        set
        {
            SetProperty(ref _userScore, value);
        }
    }

    private double _quizPercent;
    public double QuizPercent
    {
        get { return _quizPercent; }
        set
        {
            SetProperty(ref _quizPercent, value);
        }
    }

    private int _questionsAsked;
    public int QuestionsAsked
    {
        get { return _questionsAsked; }
        set
        {
            SetProperty(ref _questionsAsked, value);
        }
    }

    private int _questionsAnswered;
    public int QuestionsAnswered
    {
        get { return _questionsAnswered; }
        set
        {
            SetProperty(ref _questionsAnswered, value);
        }
    }

    public int[] QuestionIndexArray { get; set; } = new int[100];

    private int _questionIndex;
    public int QuestionIndex
    {
        get { return _questionIndex; }
        set
        {
            SetProperty(ref QuestionIndexArray[QuestionsAsked], value);
        }
    }

    public QuizViewModel(NavigationManager navigationManager, QuizModel quizModel)
    {
        _navigationManager = navigationManager;
        _quizModel = quizModel;

        EditQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new EditQuizViewModel(_navigationManager, _quizModel));
        QuitQuizCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _quizModel));

        //QuizTitle = _quizModel.QuizTitles.quizTitles.FirstOrDefault();

        var randomQuestion = _quizModel.GetRandomQuestion();
        QuestionIndex = _quizModel.CurrentRandomQuestionIndex;

        TotalQuizQuestions = _quizModel.QuizQuestionProperties.Result.Count;
        QuestionsAsked++;


        QuizQuestion = randomQuestion.Statement;
        QuizAnswerOne = $"1. {randomQuestion.Answers[0]}";
        QuizAnswerTwo = $"2. {randomQuestion.Answers[1]}";
        QuizAnswerThree = $"3. {randomQuestion.Answers[2]}";


        GetNextRandomQuestion = new RelayCommand(() =>
        {
            QuizCorrectAnswer = randomQuestion.CorrectAnswer;
            UserAnswer();
            CalculateScore();
            CalculatePercent();

            var questionAsked = true;

            if (QuestionsAsked == TotalQuizQuestions - 1)
            {
                QuizQuestion = "You finished this quiz!";

                CheckBoxOne = false;
                CheckBoxTwo = false;
                CheckBoxThree = false;

                QuizAnswerOne = string.Empty;
                QuizAnswerTwo = string.Empty;
                QuizAnswerThree = string.Empty;
            }
            else
            {
                while (questionAsked)
                {
                    randomQuestion = _quizModel.GetRandomQuestion();
                    var currentIndex = _quizModel.CurrentRandomQuestionIndex;

                    if (QuestionIndexArray.Contains(currentIndex))
                    {
                        questionAsked = true;
                    }
                    else
                    {
                        questionAsked = false;
                    }
                }
                QuestionIndex = _quizModel.CurrentRandomQuestionIndex;

                QuestionsAsked++;
                QuestionsAnswered++;

                CheckBoxOne = false;
                CheckBoxTwo = false;
                CheckBoxThree = false;

                QuizQuestion = randomQuestion.Statement;
                QuizAnswerOne = $"1. {randomQuestion.Answers[0]}";
                QuizAnswerTwo = $"2. {randomQuestion.Answers[1]}";
                QuizAnswerThree = $"3. {randomQuestion.Answers[2]}";
                QuizCorrectAnswer = randomQuestion.CorrectAnswer;
                UserAnswer();
            }
        }, () => true);
    }
    public int UserAnswer()
    {
        if (CheckBoxOne.Equals(true) && 0 == QuizCorrectAnswer)
        {
            return CorrectUserAnswers++;
        }

        if (CheckBoxTwo.Equals(true) && 1 == QuizCorrectAnswer)
        {
            return CorrectUserAnswers++;
        }

        if (CheckBoxThree.Equals(true) && 2 == QuizCorrectAnswer)
        {
            return CorrectUserAnswers++;
        }
        return CorrectUserAnswers;
    }
    public int CalculateScore()
    {
        return UserScore = CorrectUserAnswers * TotalQuizQuestions;
    }
    public double CalculatePercent()
    {
        var percent = (double)CorrectUserAnswers / (double)QuestionsAsked * 100;
        QuizPercent = Math.Round(percent);
        return QuizPercent;
    }
}