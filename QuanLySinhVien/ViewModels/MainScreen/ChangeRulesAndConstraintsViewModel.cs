using Avalonia.Controls;
using QuanLySinhVien.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;


namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ChangeRulesAndConstraintsViewModel : ViewModelBase
    {
        #region Model's properties
        private string _maQuiDinh = string.Empty;
        public string MaQuiDinh
        {
            get => _maQuiDinh;
            set => this.RaiseAndSetIfChanged(ref _maQuiDinh, value);
        }

        private string _tenQuiDinh = string.Empty;

        public string TenQuiDinh
        {
            get => _tenQuiDinh;
            set => this.RaiseAndSetIfChanged(ref _tenQuiDinh, value);
        }

        private int? _giaTri = 0;

        public int? GiaTri
        {
            get => _giaTri;
            set
            {
                this.RaiseAndSetIfChanged(ref _giaTri, value);
            }
        }

        #endregion


        #region Command
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        #endregion

        public ChangeRulesAndConstraintsViewModel()
        {
            SaveCommand = ReactiveCommand.Create(Save);
            CancelCommand = ReactiveCommand.Create(Cancel);
        }

        private void Save()
        {
            // Save changes to database
        }

        private void Cancel()
        {  
            // Reset to previous state
        }
    }
}
