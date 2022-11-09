using System.Windows;
using Labb3_Ressurrection.Managers;
using Labb3_Ressurrection.Models;
using Labb3_Ressurrection.ViewModels;

namespace Labb3_Ressurrection
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationManager _navigationManager;
        private readonly QuizModel _quizModel;

        public App()
        {
            _navigationManager = new NavigationManager();
            _quizModel = new QuizModel();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _quizModel);
            var rootWindow = new MainWindow() { DataContext = new MainViewModel(_navigationManager) };
            rootWindow.Show();
            base.OnStartup(e);
        }
    }
}