﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using System.Windows.Data;
using Gymnastika.Modules.Meals.Models;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Modules.Meals.Services;
using Gymnastika.Services.Session;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public class MealsManagementViewModel : NotificationObject, IMealsManagementViewModel
    {
        private readonly IFoodService _foodService;
        private string _searchString;
        private ICommand _searchCommand;
        private ICommand _showSavedDietPlanCommand;
        private ICommand _showRecommendedDietPlanCommand;
        private readonly XDataHelpers.XDataRepository _db;

        public MealsManagementViewModel(
            IMealsManagementView view,
            IFoodListViewModel foodListViewModel,
            ICreateDietPlanViewModel createDietPlanViewModel,
            ISelectDietPlanViewModel recommendedDietPlanViewModel,
            ISelectDietPlanViewModel savedDietPlanViewModel,
            IFoodService foodService)
        {
            FoodListViewModel = foodListViewModel;
            CreateDietPlanViewModel = createDietPlanViewModel;
            _foodService = foodService;
            InitializeSavedDietPlanViewModel();
            InitializeRecommendedDietPlanViewModel();
            _db = new XDataHelpers.XDataRepository();
            InMemoryFoods = _db.Foods;
            View = view;
            View.Context = this;
            View.SearchKeyDown += new KeyEventHandler(SearchKeyDown);
        }

        #region IMealsManagementViewModel Members

        public IMealsManagementView View { get; set; }

        public string SearchString
        {
            get
            {
                return _searchString;
            }
            set
            {
                if (_searchString != value)
                {
                    _searchString = value;
                    RaisePropertyChanged("SearchString");
                }
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                    _searchCommand = new DelegateCommand(Search);

                return _searchCommand;
            }
        }

        public ICommand ShowSavedDietPlanCommand
        {
            get
            {
                if (_showSavedDietPlanCommand == null)
                    _showSavedDietPlanCommand = new DelegateCommand(ShowSavedDietPlan);

                return _showSavedDietPlanCommand;
            }
        }

        public ICommand ShowRecommendedDietPlanCommand
        {
            get
            {
                if (_showRecommendedDietPlanCommand == null)
                    _showRecommendedDietPlanCommand = new DelegateCommand(ShowRecommendedDietPlan);

                return _showRecommendedDietPlanCommand;
            }
        }

        public IEnumerable<Food> InMemoryFoods { get; set; }

        public ICollection<Food> SearchResults { get; set; }

        public IFoodListViewModel FoodListViewModel { get; set; }

        public ICreateDietPlanViewModel CreateDietPlanViewModel { get; set; }

        public ISelectDietPlanViewModel SavedDietPlanViewModel { get; set; }

        public ISelectDietPlanViewModel RecommendedDietPlanViewModel { get; set; }

        #endregion

        private void InitializeSavedDietPlanViewModel()
        {
            SavedDietPlanViewModel = ServiceLocator.Current.GetInstance<ISelectDietPlanViewModel>();
            SavedDietPlanViewModel.PlanType = PlanType.CreatedDietPlan;
            SavedDietPlanViewModel.Apply += new EventHandler(ApplySavedDietPlan);
            SavedDietPlanViewModel.Initialize();
        }

        private void InitializeRecommendedDietPlanViewModel()
        {
            RecommendedDietPlanViewModel = ServiceLocator.Current.GetInstance<ISelectDietPlanViewModel>();
            RecommendedDietPlanViewModel.PlanType = PlanType.RecommendedDietPlan;
            RecommendedDietPlanViewModel.Apply += new EventHandler(ApplyRecommendedDietPlan);
            RecommendedDietPlanViewModel.Initialize();
        }

        private void ApplySavedDietPlan(object sender, EventArgs e)
        {
            CreateDietPlanViewModel.DietPlanListViewModel = SavedDietPlanViewModel.DietPlanListViewModel;
        }

        private void ApplyRecommendedDietPlan(object sender, EventArgs e)
        {
            //CreateDietPlanViewModel.DietPlanListViewModel = RecommendedDietPlanViewModel.DietPlanListViewModel;
            CreateDietPlanViewModel.DietPlanListViewModel = ServiceLocator.Current.GetInstance<IDietPlanListViewModel>();

            for (int i = 0; i < 6; i++)
            {
                foreach (var foodItem in RecommendedDietPlanViewModel.DietPlanListViewModel.DietPlanList[i].DietPlanSubList)
                {
                    CreateDietPlanViewModel.DietPlanListViewModel.DietPlanList[i].AddFoodToPlan(foodItem);
                }
            }
        }

        private void SearchKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Search();
        }

        private void Search()
        {
            BindingExpression binding = View.GetBindingSearchString();
            binding.UpdateSource();

            SearchResults = new Collection<Food>();
            foreach (var food in InMemoryFoods)
            {
                string filter = SearchString.ToUpper(CultureInfo.InvariantCulture);

                if (food.Name.ToUpper(CultureInfo.InvariantCulture).Contains(filter))
                    SearchResults.Add(food);
            }

            FoodListViewModel.CurrentFoods = SearchResults;
            FoodListViewModel.Initialize();
        }

        private void ShowSavedDietPlan()
        {
            InitializeSavedDietPlanViewModel();

            ShowSelectDietPlan(SavedDietPlanViewModel);
        }

        private void ShowRecommendedDietPlan()
        {
            InitializeRecommendedDietPlanViewModel();

            ShowSelectDietPlan(RecommendedDietPlanViewModel);
        }

        private void ShowSelectDietPlan(ISelectDietPlanViewModel selectDietPlanViewModel)
        {
            selectDietPlanViewModel.View.ShowView();
            selectDietPlanViewModel.DietPlanListViewModel.View.ExpandAll();
        }
    }
}
