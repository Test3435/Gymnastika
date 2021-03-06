﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Views;
using System.Windows.Input;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Services.Models;

namespace Gymnastika.Modules.Meals.ViewModels
{
    public interface ISelectDietPlanViewModel
    {
        ISelectDietPlanView View { get; set; }
        IDietPlanListViewModel DietPlanListViewModel { get; set; }
        DietPlan CurrentDietPlan { get; set; }
        IList<DietPlan> InMemoryDietPlans { get; set; }
        User CurrentUser { get; set; }
        PlanType PlanType { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        ICommand ShowPreviousPageCommand { get; }
        ICommand ShowNextPageCommand { get; }
        ICommand ApplyCommand { get; }
        event EventHandler Apply;
        void Initialize();
    }
}
