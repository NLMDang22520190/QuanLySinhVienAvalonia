﻿using QuanLySinhVien.Models;
using QuanLySinhVien.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ViewMainScreenViewModelBase<T> : ViewModelBase
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

        protected ObservableCollection<T> _listModels;
        protected Stack<ObservableCollection<T>> _undoStack;
        protected Stack<ObservableCollection<T>> _redoStack;

        private ObservableCollection<string> _listModelsName;

        private int _undoStackCount;
        private int _redoStackCount;

        public ObservableCollection<string> ListModelsName
        {
            get => _listModelsName;
            set => this.RaiseAndSetIfChanged(ref _listModelsName, value);
        }

        public ObservableCollection<T> ListModels
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
            UpdateListModelsName();
        }



        #endregion ListModels

        #region Commands

        public ReactiveCommand<Unit, Unit> AddNewRowCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> UndoCommand { get; }
        public ReactiveCommand<Unit, Unit> RedoCommand { get; }

        #endregion

        #region Properties

        private int _selectedItemRow;

        public int SelectedItemRow
        {
            get => _selectedItemRow;
            set => this.RaiseAndSetIfChanged(ref _selectedItemRow, value);
        }

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                this.RaiseAndSetIfChanged(ref _searchText, value);
                UpdateListModels();
            }
        }

        protected ObservableCollection<T> _originalListModels;

        #endregion

        public ViewMainScreenViewModelBase()
        {

            _undoStack = new Stack<ObservableCollection<T>>();
            _redoStack = new Stack<ObservableCollection<T>>();

            ListModelsName = new ObservableCollection<string>();


            var canUndo = this.WhenAnyValue(
                x => x.UndoStackCount,
                x => x > 0);

            var canRedo = this.WhenAnyValue(
                x => x.RedoStackCount,
                x => x > 0);

            AddNewRowCommand = ReactiveCommand.Create(AddNewRow);
            DeleteCommand = ReactiveCommand.Create(Delete);
            SaveCommand = ReactiveCommand.Create(Save);
            UndoCommand = ReactiveCommand.Create(Undo, canUndo);
            RedoCommand = ReactiveCommand.Create(Redo, canRedo);


            UpdateCurrentTime();
        }

        #region UndoRedo

        public void Undo()
        {

            if (_undoStack.Count > 0)
            {
                _redoStack.Push(new ObservableCollection<T>(ListModels));

                ListModels = _undoStack.Pop();

                UpdateBothStackCount();
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                _undoStack.Push(new ObservableCollection<T>(ListModels));
                ListModels = _redoStack.Pop();
            }

            UpdateBothStackCount();
        }

        public void BackupData()
        {

            ObservableCollection<T> temp = new ObservableCollection<T>();
            foreach (var result in ListModels)
            {
                // Tạo bản sao của mỗi đối tượng ResultModel và thêm vào temp
                CopyModelProperties(temp, result);
            }

            _undoStack.Push(temp);
            UpdateBothStackCount();


        }

        #endregion

        public void AddNewRow()
        {
            BackupData();

            var newResult = CreateNewModel();

            ListModels.Add(newResult);

        }

        public void Delete()
        {
            if (SelectedItemRow >= 0)
            {
                BackupData();

                ListModels.RemoveAt(SelectedItemRow);
            }
        }

        public void Save()
        {

            _undoStack.Clear();
            _redoStack.Clear();
            UpdateBothStackCount();
        }

        private void UpdateListModels()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                ListModels = new ObservableCollection<T>(_originalListModels);
            }
            else
            {
                ListModels = SearchList();
            }

        }

        #region Needs overwrite

        protected virtual void UpdateListModelsName()
        {
            ListModelsName.Clear();
            //foreach (var result in ListModels)
            //{
            //    ListModelsName.Add(result.ResultName);
            //}
        }

        protected virtual void CopyModelProperties(ObservableCollection<T> temp, T results)
        {
            // Tạo bản sao của mỗi đối tượng ResultModel và thêm vào temp
            //temp.Add(new ResultModel
            //{
            //    ResultID = result.ResultID,
            //    // Copy các thuộc tính từ result
            //    ResultName = result.ResultName,
            //    // Copy các thuộc tính khác nếu có
            //});

        }

        protected virtual T CreateNewModel()
        {
            return Activator.CreateInstance<T>();
        }

        protected virtual ObservableCollection<T> SearchList()
        {
            return new ObservableCollection<T>();
        }

        #endregion
    }

}
