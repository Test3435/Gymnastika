﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.ViewModels;

namespace Gymnastika.Modules.Meals.Views
{
    public interface IDietPlanNutritionChartView
    {
        IDietPlanNutritionChartViewModel Context { get; set; }
    }
}
