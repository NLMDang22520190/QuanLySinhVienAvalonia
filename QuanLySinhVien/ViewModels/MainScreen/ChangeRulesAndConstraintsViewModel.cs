using Avalonia.Controls;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
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
        #region Properties
        private ObservableCollection<QuiDinh> rules;

        public ObservableCollection<QuiDinh> Rules
        {
            get => rules;
            set => this.RaiseAndSetIfChanged(ref rules, value);
        }

        private ObservableCollection<QuiDinh> allRules;

        public ObservableCollection<QuiDinh> AllRules
        {
            get => allRules;
            set => this.RaiseAndSetIfChanged(ref allRules, value);
        }

        private bool isUpdating = false;

        private int selectedQuiDinhIndex = -1;

        private bool isEnableEditing = false;

        public bool IsEnableEditing { get => isEnableEditing; set => this.RaiseAndSetIfChanged(ref isEnableEditing, value); }

        public int SelectedQuiDinhIndex
        {
            get => selectedQuiDinhIndex;
            set
            {
                if (selectedQuiDinhIndex != value)
                {
                    selectedQuiDinhIndex = value;
                    this.RaisePropertyChanged(nameof(SelectedQuiDinhIndex));
                    if (isUpdating)
                    {
                        UpdateQuiDinhSearch();
                    }
                }
            }
        }

        private int selectedRuleIndex = -1;
        public int SelectedRuleIndex
        {
            get => selectedRuleIndex;
            set
            {
                if (selectedRuleIndex != value)
                {
                    selectedRuleIndex = value;
                    this.RaiseAndSetIfChanged(ref selectedRuleIndex, value);
                    UpdateRuleValue();
                }
            }
        }

        private string ruleValue;
        public string RuleValue
        {
            get => ruleValue;
            set => this.RaiseAndSetIfChanged(ref ruleValue, value);
        }

        private string originalRuleValue; // Add this property to store the original value

        #endregion

        #region ComboBox and TextBox
        private ObservableCollection<string> rulesCb;

        public ObservableCollection<string> RulesCb
        {
            get => rulesCb;
            set => this.RaiseAndSetIfChanged(ref rulesCb, value);
        }

        private ObservableCollection<string> ruleValues;

        public ObservableCollection<string> RuleValues
        {
            get => ruleValues;
            set => this.RaiseAndSetIfChanged(ref ruleValues, value);
        }

        public ChangeRulesAndConstraintsViewModel()
        {
            LoadRules();
            LoadFilterRules();
            LoadListComboBox();
            CancelEditCommand = ReactiveCommand.Create(CancelEdit);
        }

        #endregion

        #region Filter

        private void UpdateQuiDinhSearch()
        {
            isUpdating = true;
            string tenQuiDinh = selectedQuiDinhIndex != -1 ? AllRules[selectedQuiDinhIndex].TenQuiDinh : "";
            string giaTri = selectedQuiDinhIndex != -1 ? AllRules[selectedQuiDinhIndex].GiaTri.ToString() : "";

            SearchAndUpdateRules(tenQuiDinh, giaTri);
            isUpdating = false;
        }

        private void SearchAndUpdateRules(string ruleName, string value)
        {
            var query = AllRules.AsQueryable();
            if (!string.IsNullOrEmpty(ruleName))
            {
                query = query.Where(rule => rule.TenQuiDinh == ruleName);
            }

            if (!string.IsNullOrEmpty(value))
            {
                query = query.Where(rule => rule.GiaTri.ToString() == value);
            }

            Rules = new ObservableCollection<QuiDinh>(query.ToList());
        }

        private void UpdateRulesComboBox(List<string> rulesList)
        {
            RulesCb.Clear();
            var filteredRules = AllRules
                .Where(r => rulesList.Contains(r.MaQuiDinh))
                .Select(r => r.TenQuiDinh)
                .ToList();

            Rules.Clear();
            foreach (var rule in filteredRules)
            {
                RulesCb.Add(rule);
            }
        }
        #endregion

        #region Commands
        public void LoadListComboBox()
        {
            RulesCb = new ObservableCollection<string>();
            foreach (var rule in AllRules)
            {
                RulesCb.Add(rule.TenQuiDinh);
            }
        }

        private void LoadFilterRules()
        {
            var result = DataProvider.Ins.DB.QuiDinhs.ToList();
            AllRules = new ObservableCollection<QuiDinh>(result);
        }

        private void LoadRules()
        {
            var result = DataProvider.Ins.DB.QuiDinhs.ToList();

            if (Rules == null)
            {
                Rules = new ObservableCollection<QuiDinh>(result);
            }
            else
            {
                Rules.Clear();
                foreach (var rule in result)
                {
                    Rules.Add(rule);
                }
            }
        }

        private void UpdateRuleValue()
        {
            if (SelectedRuleIndex >= 0 && SelectedRuleIndex < Rules.Count)
            {
                RuleValue = Rules[SelectedRuleIndex].GiaTri.ToString();
                originalRuleValue = RuleValue; // Store the original value when a rule is selected
            }
            else
            {
                RuleValue = string.Empty;
                originalRuleValue = string.Empty;
            }
        }
        #endregion

        #region Edit Rules

        public void ChangeIsEditingState()
        {
            IsEnableEditing = !IsEnableEditing;
        }

        #endregion

        #region Save Command
        public async Task SaveRuleAsync(Window window)
        {
            if (SelectedRuleIndex >= 0 && SelectedRuleIndex < Rules.Count)
            {
                var selectedRule = Rules[SelectedRuleIndex];
                if (int.TryParse(RuleValue, out int newValue))
                {
                    if (selectedRule.GiaTri != newValue)
                    {
                        // Show confirmation message box
                        var message = MessageBoxManager.GetMessageBoxStandard("Xác nhận", "Bạn có chắc chắn muốn thay đổi quy định không?", ButtonEnum.YesNo, Icon.Question);
                        var result = await message.ShowWindowDialogAsync(window);

                        if (result == ButtonResult.Yes)
                        {
                            try
                            {
                                selectedRule.GiaTri = newValue;
                                DataProvider.Ins.DB.SaveChanges(); // Save changes to the database
                                MessageBoxManager.GetMessageBoxStandard("Thông báo", "Lưu Quy định thành công !", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(window);
                                LoadRules(); // Reload rules to reflect the changes
                            }
                            catch (Exception ex)
                            {
                                // Handle the exception (e.g., show a message box)
                                MessageBoxManager.GetMessageBoxStandard("Thông báo", "Lưu Quy định không thành công !" + "\nXin thử lại" + "Lỗi: " + ex.Message, ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(window);
                            }
                        }
                    }
                }
                else
                {
                    // Handle the case where the rule value is not a valid integer
                    MessageBoxManager.GetMessageBoxStandard("Thông báo", "Giá trị quy định không hợp lệ !" + "\nXin thử lại", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(window);
                }
            }
        }
        #endregion

        #region Cancel Command
        public ReactiveCommand<Unit, Unit> CancelEditCommand { get; }

        private void CancelEdit()
        {
            if (RuleValue != originalRuleValue)
            {
                RuleValue = originalRuleValue; // Revert to the original value
            }
        }
        #endregion
    }
}
