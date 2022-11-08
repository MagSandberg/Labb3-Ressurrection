using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Path = System.IO.Path;

namespace Labb3_Ressurrection.Models;

public class QuizModel
{
    private IEnumerable<QuestionModel> _questions;
    public IEnumerable<QuestionModel> Questions => _questions;

    private string _title = string.Empty;
    public string Title => _title;
    public string QuizTitle { get; set; }

    private List<string> _titleList;
    public List<string> TitleList => _titleList;
    public int CurrentRandomQuestionIndex { get; set; }
    public int CurrentQuestionIndex { get; set; }

    public ListOfTitles? QuizTitles { get; set; }
    public ValueTask<List<QuestionProperties>?> QuizQuestionProperties { get; set; }

    public QuizModel()
    {
        _questions = new List<QuestionModel>();
        QuizTitles = GetTitles();
    }

    public class ListOfTitles
    {
        public List<string> quizTitles { get; set; }
    }

    public class QuestionProperties
    {
        public string Statement { get; set; }
        public string[] Answers { get; set; } = new string[3];
        public int CorrectAnswer { get; set; }

        public override string ToString()
        {
            var output = string.Empty;
            output += $"Question: {Statement}\n";
            output += $"Answers: ";
            foreach (var answer in Answers)
            {
                output += $"{answer}, ";
            }
            output += $"\nCorrect answer: {Answers[CorrectAnswer]}\n";
            output += "\n";
            return output;
        }
    }

    public async ValueTask<List<QuestionProperties>?> GetQuestions(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            title = "movies";
        }
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{title}.json");

        using var sr = new StreamReader(path);
        return await JsonSerializer.DeserializeAsync<List<QuestionProperties>>(sr.BaseStream);
    }

    public ListOfTitles GetTitles()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"quizlist.json");

        using var sr = new StreamReader(path);
        return JsonSerializer.Deserialize<ListOfTitles>(sr.BaseStream);
    }

    public async Task SaveQuizAsync(string title)
    {
        await Task.Run(async () =>
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{title}.json");

            if (!File.Exists(path))
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                await using var sw = new StreamWriter(path);
                await JsonSerializer.SerializeAsync(sw.BaseStream, Questions, options);
                sw.Close();
            }

            if (File.Exists(path))
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };

                    await using var sw = new StreamWriter(path);
                    await JsonSerializer.SerializeAsync(sw.BaseStream, Questions, options);
                    sw.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            var titlePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"quizlist.json");

            if (File.Exists(titlePath))
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };

                    using var sr = new StreamReader(titlePath);
                    QuizTitles = await JsonSerializer.DeserializeAsync<ListOfTitles>(sr.BaseStream);

                    if (!QuizTitles!.quizTitles.Contains(title))
                    {
                        QuizTitles?.quizTitles.Add(title);
                    }

                    sr.Close();

                    await using var sw = new StreamWriter(titlePath);
                    await JsonSerializer.SerializeAsync(sw.BaseStream, QuizTitles, options);
                    sw.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        });
    }

    public QuestionModel GetRandomQuestion()
    {
        var rand = new Random();
        var totalCount = QuizQuestionProperties.Result!.Count;

        var randomNum = rand.Next(0, totalCount);
        var index = 0;

        foreach (var question in QuizQuestionProperties.Result)
        {
            if (randomNum == index)
            {
                CurrentRandomQuestionIndex = QuizQuestionProperties.Result.IndexOf(question);
                return new QuestionModel(question.Statement, question.Answers, question.CorrectAnswer);
            }
            index++;
        }
        return null!;
    }

    public void AddQuestion(string statement, int correctAnswer, params string[] answers)
    {
        var myQuestionModel = new QuestionModel(statement, answers, correctAnswer);
        _questions = _questions.Concat(new[] { myQuestionModel });
    }

    public void RemoveQuestion(int index)
    {
        QuizQuestionProperties.Result.RemoveAt(index);
    }
}