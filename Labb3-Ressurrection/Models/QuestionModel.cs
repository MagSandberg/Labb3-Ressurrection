namespace Labb3_Ressurrection.Models;

public class QuestionModel
{
    public string Statement { get; }
    public string[] Answers { get; }
    public int CorrectAnswer { get; }

    public QuestionModel(string statement, string[] answers, int correctAnswer)
    {
        Statement = statement;
        Answers = answers;
        CorrectAnswer = correctAnswer;
    }
}