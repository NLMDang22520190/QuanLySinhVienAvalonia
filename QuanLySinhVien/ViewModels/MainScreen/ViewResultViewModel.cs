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

        #endregion

        private ObservableCollection<ResultModel> _listModels;
        private Stack<ObservableCollection<ResultModel>> _undoStack;
        private Stack<ObservableCollection<ResultModel>> _redoStack;

        public ObservableCollection<ResultModel> ListModels
        {
            get => _listModels;
            set => this.RaiseAndSetIfChanged(ref _listModels, value);
        }

        public ReactiveCommand<Unit, Unit> UndoCommand { get; }
        public ReactiveCommand<Unit, Unit> RedoCommand { get; }

        private readonly ViewResultService _service;

        public ViewResultViewModel()
        {
            Debug.WriteLine("Vm created");
            _service = new ViewResultService();
            ListModels = new ObservableCollection<ResultModel>(_service.GetResults);

            _undoStack = new Stack<ObservableCollection<ResultModel>>();
            _redoStack = new Stack<ObservableCollection<ResultModel>>();

            var canUndo = this.WhenAnyValue(
                x => x._undoStack,
                x => x.Count > 0);

            var canRedo = this.WhenAnyValue(x => x._redoStack, 
                x => x.Count > 0);

            UndoCommand = ReactiveCommand.Create(Undo, canUndo);
            RedoCommand = ReactiveCommand.Create(Redo, canRedo);

            //ListModels.CollectionChanged += (sender, e) =>
            //{
            //    BackupData();
            //};

            //ListModels
            //    .ToObservableChangeSet()
            //    .Subscribe(_ => UpdateListCommand.Execute().Subscribe());

            UpdateCurrentTime();
        }

        public async Task UpdateListAsync(ObservableCollection<ResultModel> modifiedList)
        {
            BackupData();
            ListModels.Clear();
            foreach (var result in modifiedList)
            {
                ListModels.Add(result);
            }
            Debug.WriteLine("Data updated");
        }


        public void Undo()
        {
            Debug.WriteLine("undo");
            if (_undoStack.Count > 0)
            {
                _redoStack.Push(new ObservableCollection<ResultModel>(ListModels));
                ListModels = _undoStack.Pop();
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                _undoStack.Push(new ObservableCollection<ResultModel>(ListModels));
                ListModels = _redoStack.Pop();
            }
        }

        public void BackupData()
        {
            Debug.WriteLine("Data has been backed up");
            _undoStack.Push(new ObservableCollection<ResultModel>(ListModels));
            Debug.WriteLine("number of undo: " + _undoStack.Count);

            // Đăng ký một người nghe cho canUndo
            canUndo().Subscribe(result =>
            {
                Debug.WriteLine("Can Undo: " + result);
            });
        }

        private IObservable<bool> canUndo()
        {
            return this.WhenAnyValue(
                x => x._undoStack,
                x => x.Count > 0);
        }
    }
}
