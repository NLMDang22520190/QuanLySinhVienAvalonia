using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using DynamicData.Binding;
using QuanLySinhVien.Models;
using QuanLySinhVien.Services;
using ReactiveUI;

namespace QuanLySinhVien.ViewModels.MainScreen
{
    public class ViewResultViewModel : ViewMainScreenViewModelBase<ResultModel>
    {

        private readonly ViewResultService _service;

        public ViewResultViewModel() : base()
        {
            _service = new ViewResultService();
            ListModels = new ObservableCollection<ResultModel>(_service.GetResults);
            _originalListModels = new ObservableCollection<ResultModel>(ListModels);
            UpdateListModelsName();


        }

        #region OverWrite

        protected override void UpdateListModelsName()
        {
            ListModelsName.Clear();
            foreach (var result in ListModels)
            {
                ListModelsName.Add(result.ResultName);
            }
        }

        protected override void CopyModelProperties(ObservableCollection<ResultModel> temp, ResultModel result)
        {
            //Tạo bản sao của mỗi đối tượng ResultModel và thêm vào temp
            temp.Add(new ResultModel
            {
                ResultID = result.ResultID,
                // Copy các thuộc tính từ result
                ResultName = result.ResultName,
                // Copy các thuộc tính khác nếu có
            });

        }

        protected override ResultModel CreateNewModel()
        {
            return new ResultModel
            {
                ResultID = (ListModels.Count + 1).ToString(),
            };
        }

        protected override ObservableCollection<ResultModel> SearchList()
        {
            return new ObservableCollection<ResultModel>(_originalListModels.Where(x =>
                x.ResultName.ToLower().Contains(SearchText.ToLower())));
        }

        #endregion


    }
}