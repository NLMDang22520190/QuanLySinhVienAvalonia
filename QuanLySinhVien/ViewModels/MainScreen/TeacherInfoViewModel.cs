using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLySinhVien.Models;
using ReactiveUI;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class TeacherInfoViewModel : ViewModelBase
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


        private ObservableCollection<TeacherInfoModel> listTeacherInfoModels;

        public ObservableCollection<TeacherInfoModel> ListTeacherInfoModels
        {
            get => listTeacherInfoModels;
            set => this.RaiseAndSetIfChanged(ref listTeacherInfoModels, value);
        }

        public TeacherInfoViewModel()
        {
            ListTeacherInfoModels = new ObservableCollection<TeacherInfoModel>();
            ListTeacherInfoModels.Add(new TeacherInfoModel
            {
                TeacherName = "Nguyen Van A",
                TeacherBirthDay = "01/01/1990",
                TeacherGender = "Nam",
                TeacherAddress = "Ha Noi",
                TeacherEmail = ""
            });

            // Add 4 more TeacherInfoModel objects to the list
            ListTeacherInfoModels.Add(new TeacherInfoModel
            {
                TeacherName = "Nguyen Van B",
                TeacherBirthDay = "02/02/1991",
                TeacherGender = "Nam",
                TeacherAddress = "Ho Chi Minh",
                TeacherEmail = ""
            });

            ListTeacherInfoModels.Add(new TeacherInfoModel
            {
                TeacherName = "Nguyen Thi C",
                TeacherBirthDay = "03/03/1992",
                TeacherGender = "Nu",
                TeacherAddress = "Da Nang",
                TeacherEmail = ""
            });

            ListTeacherInfoModels.Add(new TeacherInfoModel
            {
                TeacherName = "Tran Van D",
                TeacherBirthDay = "04/04/1993",
                TeacherGender = "Nam",
                TeacherAddress = "Hue",
                TeacherEmail = ""
            });

            ListTeacherInfoModels.Add(new TeacherInfoModel
            {
                TeacherName = "Le Thi E",
                TeacherBirthDay = "05/05/1994",
                TeacherGender = "Nu",
                TeacherAddress = "Can Tho",
                TeacherEmail = ""
            });

            UpdateCurrentTime();

        }
    }
}
