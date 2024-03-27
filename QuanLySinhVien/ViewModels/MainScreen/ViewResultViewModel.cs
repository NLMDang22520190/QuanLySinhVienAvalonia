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

        #endregion

        private ObservableCollection<ResultModel> _listModels;
        private Stack<ObservableCollection<ResultModel>> _undoStack;
        private Stack<ObservableCollection<ResultModel>> _redoStack;

        public ObservableCollection<ResultModel> ListModels
        {
            get => _listModels;
            set => this.RaiseAndSetIfChanged(ref _listModels, value);
        }

        private bool _canUndo;
        public bool CanUndo
        {
            get => _canUndo;
            private set => this.RaiseAndSetIfChanged(ref _canUndo, value);
        }

        private bool _canRedo;
        public bool CanRedo
        {
            get => _canRedo;
            private set => this.RaiseAndSetIfChanged(ref _canRedo, value);
        }

        public ReactiveCommand<Unit, Unit> UndoCommand { get; }
        public ReactiveCommand<Unit, Unit> RedoCommand { get; }

        private readonly ViewResultService _service;
        public ReactiveCommand<Unit, Unit> UpdateListCommand { get; }

        public ViewResultViewModel()
        {
            Debug.WriteLine("Vm created");
            _service = new ViewResultService();
            ListModels = new ObservableCollection<ResultModel>(_service.GetResults);

            _undoStack = new Stack<ObservableCollection<ResultModel>>();
            _redoStack = new Stack<ObservableCollection<ResultModel>>();

            UndoCommand = ReactiveCommand.Create(Undo, this.WhenAnyValue(x => x.CanUndo));
            RedoCommand = ReactiveCommand.Create(Redo, this.WhenAnyValue(x => x.CanRedo));

            UpdateListCommand = ReactiveCommand.CreateFromTask(UpdateListAsync);


            ListModels.CollectionChanged += (sender, e) =>
            {
                BackupData();
            };

            //ListModels
            //    .ToObservableChangeSet()
            //    .Subscribe(_ => UpdateListCommand.Execute().Subscribe());

            UpdateCurrentTime();
        }

        private async Task UpdateListAsync()
        {
            Debug.WriteLine("bruh");
            ListModels.Clear();
            var results = _service.GetResults;
            foreach (var result in results)
            {
                ListModels.Add(result);
            }
        }

        private async void UpdateCurrentTime()
        {
            while (true)
            {
                CurrentTime = DateTime.Now.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture);
                await Task.Delay(1000);
            }
        }

        public void Undo()
        {
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
            _undoStack.Push(new ObservableCollection<ResultModel>(ListModels));
        }
    }
}
