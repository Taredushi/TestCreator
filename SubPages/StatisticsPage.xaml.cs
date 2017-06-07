using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestCreator.Database;
using TestCreator.Enumerators;
using TestCreator.ViewModel;

namespace TestCreator.SubPages
{
    /// <summary>
    /// Interaction logic for UsersPage.xaml
    /// </summary>
    public partial class StatisticsPage
    {
        public string Title { get; set; }

        public string TitleOfChart { get; set; }
        public string TitleOfAdminsChart { get; set; }
        private List<KeyValuePair<string, int>> ScoresOfAllUsers { get; set; }
        private List<KeyValuePair<string, int>> Scores { get; set; }
        public IEnumerable<KeyValuePair<string, int>> LoadColumnChartData => ScoresOfAllUsers;
        public IEnumerable<KeyValuePair<string, int>> LoadPieChartData => Scores;

        public StatisticsPage(User user)
        {
            InitializeComponent();
            LoggedUser = user;
            InitializePage();
        }

        private void InitializePage()
        {
            DataContext = this;

            InitializeChartOfUser();

            InitializeChartOfAdmin();

            TopPanel.Logout += Logout;
            Title = "Statystyki";
        }

        private void InitializeChartOfAdmin()
        {
            TitleOfAdminsChart = "Dane Użytkowników procentowo";

            var allUsers = DatabaseHelpers.GetAllUsers().Where(x=>x.Role == (int)Role.User).ToList();
            ScoresOfAllUsers = new List<KeyValuePair<string, int>>();
            foreach (var user in allUsers)
            {
                var testsOfUser = DatabaseHelpers.GetTestsOfUser(user);
                int correctAnswers = 0;
                int maxScores = 0;
                foreach (var testResult in testsOfUser)
                {
                    var test = DatabaseHelpers.GetTestByID(testResult.TestID);
                    var maxScoreForTest = test.Questions.Take(test.QuestionsLimit).Sum(question => question.Answers.Count(answer => answer.IsCorrect));
                    maxScores += maxScoreForTest;
                    correctAnswers += testResult.Result;
                }
                
                var columnValue = (maxScores != 0) ? (correctAnswers * 100 / maxScores) : 0;
                ScoresOfAllUsers.Add(new KeyValuePair<string, int>
                    (user.Name, columnValue));
            }
        }

        private void InitializeChartOfUser()
        {
            TitleOfChart = "Dane użytkownika " + LoggedUser.Name + " ze wszystkich testów.";

            var testsOfUser = DatabaseHelpers.GetTestsOfUser(LoggedUser);
            Scores = new List<KeyValuePair<string, int>>();
            int correctAnswers = 0;
            int incorrectAnswers = 0;
            foreach (var testResult in testsOfUser)
            {
                var test = DatabaseHelpers.GetTestByID(testResult.TestID);
                var maxScoreForTest = test.Questions.Take(test.QuestionsLimit).Sum(question => question.Answers.Count(answer => answer.IsCorrect));
                incorrectAnswers += maxScoreForTest - testResult.Result;
                correctAnswers += testResult.Result;
            }
            Scores.Add(new KeyValuePair<string, int>
                ("Poprawne odpowiedzi", correctAnswers));
            Scores.Add(new KeyValuePair<string, int>
                ("Nie poprawne odpowiedzi", incorrectAnswers));
        }

        #region Dependency Properties

        public static readonly DependencyProperty UserPropert = DependencyProperty.Register(
            "LoggedUser", typeof(User), typeof(UsersPage), new PropertyMetadata(default(User)));

        public User LoggedUser
        {
            get { return (User)GetValue(UserPropert); }
            set
            {
                SetValue(UserPropert, value);
                LoggedUserName = LoggedUser.Name + " " + LoggedUser.Surname;
                LoggedUserRole = Enum.GetName(typeof(Role), (Role)LoggedUser.Role);
                UserAvatar = LoggedUser.AvatarBitmap;
            }
        }

        public static readonly DependencyProperty UserNamePropert = DependencyProperty.Register(
            "LoggedUserName", typeof(string), typeof(UsersPage), new PropertyMetadata(default(string)));

        public string LoggedUserName
        {
            get { return (string)GetValue(UserNamePropert); }
            set
            {
                SetValue(UserNamePropert, value);
            }
        }

        public static readonly DependencyProperty UserRolePropert = DependencyProperty.Register(
            "LoggedUserRole", typeof(string), typeof(UsersPage), new PropertyMetadata(default(string)));

        public string LoggedUserRole
        {
            get { return (string)GetValue(UserRolePropert); }
            set
            {
                SetValue(UserRolePropert, value);
            }
        }

        public static readonly DependencyProperty UserAvatarPropert = DependencyProperty.Register(
            "UserAvatar", typeof(BitmapImage), typeof(UsersPage), new PropertyMetadata(default(BitmapImage)));

        public BitmapImage UserAvatar
        {
            get { return (BitmapImage)GetValue(UserAvatarPropert); }
            set
            {
                SetValue(UserAvatarPropert, value);
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Logout(object sender, EventArgs eventArgs)
        {
            OnPropertyChanged("Logout");
        }
        #endregion
        
    }
}
