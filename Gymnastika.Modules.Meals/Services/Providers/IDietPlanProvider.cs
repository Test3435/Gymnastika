﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Meals.Models;
using Gymnastika.Services.Models;

namespace Gymnastika.Modules.Meals.Services.Providers
{
    public interface IDietPlanProvider
    {
        void Create(DietPlan dietPlan);
        void Update(DietPlan dietPlan);
        void Delete(DietPlan dietPlan);
        IEnumerable<DietPlan> GetDietPlans(int userId);
        IEnumerable<DietPlan> GetDietPlans(int userId, int count);
        IEnumerable<DietPlan> GetRecommendedDietPlans();
        DietPlan Get(User user, int skip);
        DietPlan Get(PlanType planType, int skip);
        DietPlan Get(User user, DateTime createdDate);
        int count(User user);
        int count(PlanType planType);
    }
}
