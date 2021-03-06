﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public interface ISubDietPlanProvider
    {
        void Create(SubDietPlan subDietPlan);
        void Update(SubDietPlan subDietPlan);
        IEnumerable<SubDietPlan> GetSubDietPlans(DietPlan dietPlan);
        SubDietPlan Get(int id);
    }
}
