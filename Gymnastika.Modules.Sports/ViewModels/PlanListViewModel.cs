﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Sports.Services.Providers;
using Gymnastika.Modules.Sports.Models;
using System.Collections.ObjectModel;
using Gymnastika.Services.Session;
using Gymnastika.Modules.Sports.Extensions;
using Microsoft.Practices.Prism.Commands;
using Gymnastika.Modules.Sports.Services.Factories;
using Gymnastika.Services.Models;
using System.Collections.Specialized;
using Gymnastika.Common.Extensions;
using Gymnastika.Modules.Sports.Facility;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface IPlanListViewModel
    {
        DelegateCommand CreatePlanCommand { get; }

        User User { get; }

        ObservableCollection<ISportsPlanViewModel> ViewModels { get; }

        DelegateCommand LastWeekCommand { get; }

        DelegateCommand NextWeekCommand { get; }

        string DayRange { get; }

        DateTime Now { get; }

        DateTime CurrentWeek { get; }
    }

    public class PlanListViewModel : NotificationObject, IPlanListViewModel
    {
        readonly ISportsPlanProvider _planProvider;
        readonly IPlanItemProvider _itemProvider;
        readonly ISessionManager _sessionManager;
        readonly ISportsPlanViewModelFactory _planFactory;
        readonly ISportProvider _sportProvider;
        public PlanListViewModel(ISportsPlanProvider planProvider,IPlanItemProvider itemProvider,ISportProvider sportProvider,ISessionManager sessionManager,ISportsPlanViewModelFactory planFactory)
        {
            _planProvider = planProvider;
            _itemProvider = itemProvider;
            _sessionManager = sessionManager;
            _planFactory = planFactory;
            _sportProvider = sportProvider;
            ViewModels.CollectionChanged += OnPlansChanged;
            CurrentWeek = DateTime.Now;
            GotoWeek(CurrentWeek);
        }
        string _dayRange;
        public string DayRange
        {
            get { return _dayRange; }
            set
            {
                if (_dayRange != value)
                {
                    _dayRange = value;
                    RaisePropertyChanged(() => DayRange);
                }
            }
        }

       DateTime _currentWeek;
       public DateTime CurrentWeek
        {
            get { return _currentWeek; }
            private set
            {
                if (_currentWeek != value)
                {
                    _currentWeek = value;
                    DateTime sunday = Facility.Facility.Sunday(value);
                    DateTime sat = sunday.AddDays(6);
                    DayRange = String.Format("{0}年 {1}月{2}日-{3}月{4}日",sunday.Year,sunday.Month, sunday.Day, sat.Month, sat.Day);
                    RaisePropertyChanged(() => CurrentWeek);
                }
            }
        }


        IList<SportsPlan> GetWeekPlans(DateTime timeInThisWeek)
        {
            int dayOfWeek = (int)timeInThisWeek.DayOfWeek;
            DateTime Sunday = timeInThisWeek.AddDays(-dayOfWeek);
            IList<SportsPlan> plansInThisWeek = PlansInMemory
                .Where(t=>Facility.Facility.TheSameWeek(timeInThisWeek,t))
                .OrderBy(t=>t.Year)
                .OrderBy(t=>t.Month)
                .OrderBy(t=>t.Day)
                .ToList();

            return plansInThisWeek;
        }

        void GotoWeek(DateTime timeInThisWeek)
        {
            ViewModels.ReplaceBy(CreateViewModels(GetWeekPlans(timeInThisWeek)));
            CurrentWeek = timeInThisWeek;
        }


        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        ObservableCollection<ISportsPlanViewModel> _viewModels = new ObservableCollection<ISportsPlanViewModel>();
        public ObservableCollection<ISportsPlanViewModel> ViewModels
        {
            get { return _viewModels; }
        }

        ObservableCollection<SportsPlan> _plansInMemory;
        public ObservableCollection<SportsPlan> PlansInMemory
        {
            get
            {
                if (_plansInMemory == null)
                    _plansInMemory = LoadPlans().ToObservableCollection();
                return _plansInMemory;
            }
        }

        IList<SportsPlan> LoadPlans()
        {
            using (_planProvider.GetContextScope())
            {
                IList<SportsPlan> plans   =_planProvider.Fetch(t => (t.User == User)).ToList();
                //Avoid Lazy Loading
                foreach (SportsPlan plan in plans)
                {
                    plan.SportsPlanItems = plan.SportsPlanItems.ToList();
                    foreach(var item in plan.SportsPlanItems)
                        item.Sport = _sportProvider.Get(item.Sport.Id);
                    plan.User = User;
                }
                return plans;
            }
        }


        public User User
        {
            get { return _sessionManager.GetCurrentSession().AssociatedUser; }
        }


        ObservableCollection<ISportsPlanViewModel> CreateViewModels(IEnumerable<SportsPlan> plans)
        {
            ObservableCollection<ISportsPlanViewModel> viewmodels = new ObservableCollection<ISportsPlanViewModel>();
            foreach (var plan in plans)
            {
                viewmodels.Add(CreateViewModel(plan));
            }
            return viewmodels;
        }

        ISportsPlanViewModel CreateViewModel(SportsPlan plan)
        {
            ISportsPlanViewModel viewmodel = _planFactory.Create(plan);
            
            return viewmodel;
        }

        void OnPlansChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<SportsPlan> plans = sender as ObservableCollection<SportsPlan>;
            var newModels = e.NewItems;
            var oldModels = e.OldItems;
            if(newModels!=null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach(ISportsPlanViewModel plan in oldModels)
                        {
                            OnReleasePlan(plan);
                        }
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        foreach (ISportsPlanViewModel plan in oldModels)
                        {
                            OnReleasePlan(plan);
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        void OnReleasePlan(ISportsPlanViewModel viewmodel)
        {
            PlansInMemory.Remove(viewmodel.SportsPlan);
        }

        void DeletePlanFromRepository(SportsPlan plan)
        {
            if (plan.Id != 0)
            {
                using (_planProvider.GetContextScope())
                {

                    foreach (SportsPlanItem item in plan.SportsPlanItems)
                    {
                        _itemProvider.Delete(item);
                        plan.SportsPlanItems.Remove(item);
                    }
                    _planProvider.Delete(plan);
                }
            }
            plan.Id = 0;
        }

        void CreateOrUpdatePlanToRepository(SportsPlan plan)
        {
            using (_planProvider.GetContextScope())
            {
                _planProvider.CreateOrUpdate(plan);
            }
        }

       DelegateCommand _createPlanCommand;
       public DelegateCommand CreatePlanCommand
       {
           get
           {
               if (_createPlanCommand == null)
                   _createPlanCommand = new DelegateCommand(CreatePlan);
               return _createPlanCommand;
           }
       }

       void CreatePlan()
       {
           SportsPlan newPlan = new SportsPlan() { User = this.User };
           CreateOrUpdatePlanToRepository(newPlan);
           ViewModels.Add(CreateViewModel(newPlan));
       }


       DelegateCommand _lastWeekCommnad;
       public DelegateCommand LastWeekCommand 
       {
           get
           {
               if(_lastWeekCommnad == null)
                   _lastWeekCommnad = new DelegateCommand(GotoLastWeek);
               return _lastWeekCommnad;
           } 
       }

       DelegateCommand _nextWeekCommand;
       public DelegateCommand NextWeekCommand 
       {
           get
           {
               if (_nextWeekCommand == null)
                   _nextWeekCommand = new DelegateCommand(GotoNextWeek);
               return _nextWeekCommand;
           }
       }
        
        

        void GotoLastWeek()
        {
            GotoWeek(CurrentWeek.AddDays(-7));
        }

        void GotoNextWeek()
        {
            GotoWeek(CurrentWeek.AddDays(7));
        }
    }
}
