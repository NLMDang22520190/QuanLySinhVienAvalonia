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
            }
            else
            {
                RuleValue = string.Empty;
            }
        }
        #endregion
    }
}
