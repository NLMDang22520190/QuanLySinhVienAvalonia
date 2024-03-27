using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reactive;
using System.Threading.Tasks;
using DynamicData.Binding;
using QuanLySinhVien.Models;
using QuanLySinhVien.Services;
using ReactiveUI;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ViewResultViewModel : ViewModelBase
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
                CurrentTime = DateTime.Now.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture);
                await Task.Delay(1000);
            }
        }

        #endregion Time

        #region ListModels

        private ObservableCollection<ResultModel> _listModels;
        private Stack<ObservableCollection<ResultModel>> _undoStack;
        private Stack<ObservableCollection<ResultModel>> _redoStack;

        private int _undoStackCount;
        private int _redoStackCount;

        public ObservableCollection<ResultModel> ListModels
        {
            get => _listModels;
            set => this.RaiseAndSetIfChanged(ref _listModels, value);
        }

        public int UndoStackCount
        {
            get => _undoStackCount;
            set => this.RaiseAndSetIfChanged(ref _undoStackCount, value);
        }

        public int RedoStackCount
        {
            get => _redoStackCount;
            set => this.RaiseAndSetIfChanged(ref _redoStackCount, value);
        }

        private void UpdateBothStackCount()
        {
            UndoStackCount = _undoStack.Count;
            RedoStackCount = _redoStack.Count;
        }

        #endregion ListModels

        #region Commands
        public ReactiveCommand<Unit, Unit> UndoCommand { get; }
        public ReactiveCommand<Unit, Unit> RedoCommand { get; }

        #endregion


        private readonly ViewResultService _service;

        public ViewResultViewModel()
        {
            _service = new ViewResultService();
            ListModels = new ObservableCollection<ResultModel>(_service.GetResults);

            _undoStack = new Stack<ObservableCollection<ResultModel>>();
            _redoStack = new Stack<ObservableCollection<ResultModel>>();

            var canUndo = this.WhenAnyValue(
                x => x.UndoStackCount,
                x => x > 0);

            var canRedo = this.WhenAnyValue(
                x => x.RedoStackCount,
                x => x > 0);

            UndoCommand = ReactiveCommand.Create(Undo, canUndo);
            RedoCommand = ReactiveCommand.Create(Redo, canRedo);

            UpdateCurrentTime();
        }

        #region UndoRedo

        public void Undo()
        {
            Debug.WriteLine("undo");
            if (_undoStack.Count > 0)
            {
                _redoStack.Push(new ObservableCollection<ResultModel>(ListModels));

                ListModels = _undoStack.Pop();

                UpdateBothStackCount();
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                _undoStack.Push(new ObservableCollection<ResultModel>(ListModels));
                ListModels = _redoStack.Pop();
            }

            UpdateBothStackCount();
        }

        public void BackupData()
        {

            ObservableCollection<ResultModel> temp = new ObservableCollection<ResultModel>();
            foreach (var result in ListModels)
            {
                // Tạo bản sao của mỗi đối tượng ResultModel và thêm vào temp
                temp.Add(new ResultModel
                {
                    ResultID = result.ResultID,
                    // Copy các thuộc tính từ result
                    ResultName = result.ResultName,
                    // Copy các thuộc tính khác nếu có
                });
            }

            _undoStack.Push(temp);
            UpdateBothStackCount();


        }

        #endregion


    }
}