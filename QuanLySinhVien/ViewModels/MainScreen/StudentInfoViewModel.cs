using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using QuanLySinhVien.Models;
using QuanLySinhVien.Views.MainScreen;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class StudentInfoViewModel : ViewModelBase
    {
        #region Time

        private string _currentTime;

        public string CurrentTime
        {
            get => _currentTime;
            set => this.RaiseAndSetIfChanged(ref _currentTime, value);
        }

        private async void UpdateCurrentTime()
        {
            while (true)
            {
                CurrentTime = DateTime.Now.ToString("HH:mm dd/MM/yy", CultureInfo.InvariantCulture);
                await Task.Delay(1000);
            }
        }

        #endregion Time

        private ObservableCollection<StudentInfoModel> listStudentInfoModels;

        public ObservableCollection<StudentInfoModel> ListStudentInfoModels
        {
            get => listStudentInfoModels;
            set => this.RaiseAndSetIfChanged(ref listStudentInfoModels, value);
        }

        private ReactiveCommand<Unit, Unit> addStudentCommand;

        public ReactiveCommand<Unit, Unit> AddStudentCommand
        {
            get => addStudentCommand;
            set => this.RaiseAndSetIfChanged(ref addStudentCommand, value);
        }

        public StudentInfoViewModel()
        {
            ListStudentInfoModels = new ObservableCollection<StudentInfoModel>();
            ListStudentInfoModels.Add(new StudentInfoModel
            {
                StudentName = "Nguyen Van A",
                StudentBirthDay = "01/01/1990",
                StudentGender = "Nam",
                StudentAddress = "Ha Noi",
                StudentEmail = "",
                StudentGPA1 = 3.5f,
                StudentGPA2 = 3.6f
            });

            // Add 4 more StudentInfoModel objects to the list
            ListStudentInfoModels.Add(new StudentInfoModel
            {
                StudentName = "Nguyen Van B",
                StudentBirthDay = "02/02/1991",
                StudentGender = "Nam",
                StudentAddress = "Ha Noi",
                StudentEmail = "",
                StudentGPA1 = 3.7f,
                StudentGPA2 = 3.8f
            });

            ListStudentInfoModels.Add(new StudentInfoModel
            {
                StudentName = "Nguyen Van C",
                StudentBirthDay = "03/03/1992",
                StudentGender = "Nam",
                StudentAddress = "Ha Noi",
                StudentEmail = "",
                StudentGPA1 = 3.9f,
                StudentGPA2 = 4.0f
            });

            ListStudentInfoModels.Add(new StudentInfoModel
            {
                StudentName = "Nguyen Van D",
                StudentBirthDay = "04/04/1993",
                StudentGender = "Nam",
                StudentAddress = "Ha Noi",
                StudentEmail = "",
                StudentGPA1 = 4.1f,
                StudentGPA2 = 4.2f
            });

            ListStudentInfoModels.Add(new StudentInfoModel
            {
                StudentName = "Nguyen Van E",
                StudentBirthDay = "05/05/1994",
                StudentGender = "Nam",
                StudentAddress = "Ha Noi",
                StudentEmail = "",
                StudentGPA1 = 4.3f,
                StudentGPA2 = 4.4f
            });

            UpdateCurrentTime();

        }

        public void OpenAddStudentWindow()
        {
            AddStudentView addStudentView = new AddStudentView();
            addStudentView.DataContext = new AddStudentViewModel(addStudentView);
            addStudentView.Show();


        }
    }
}
