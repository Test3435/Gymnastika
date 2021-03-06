﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Services.Providers;
using Gymnastika.Services.Session;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.Prism.ViewModel;
using System.Timers;
using Gymnastika.Services.Models;
using Microsoft.Practices.Prism.Events;
using Gymnastika.Modules.Sports.Events;
using Gymnastika.Modules.Sports.Facilities;

namespace Gymnastika.Modules.Sports.Widget
{
    public class DailySportViewModel : NotificationObject
    {
        readonly ISportsPlanProvider _sportsPlanProvider;
        readonly ISessionManager _sessionManager;
        readonly ISportProvider _sportProvider;
        readonly IEventAggregator _eventAggregator;

        public DailySportViewModel(ISportsPlanProvider sportsPlanProvider,ISportProvider sportProvider,ISessionManager sessionManager,IEventAggregator eventAggregator)
        {
            _sportsPlanProvider = sportsPlanProvider;
            _sessionManager = sessionManager;
            _sportProvider = sportProvider;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SportsPlanCreatedOrModifiedEvent>().Subscribe(OnSportsPlanCreateOrUpdate);
        }

        public DateTime Today
        {
            get { return DateTime.Now; }
        }

        void Refresh()
        {
            Plan = LoadPlan(User, Today);
            RaisePropertyChanged(() => Plan.SportsPlanItems);
        }

        SportsPlan _plan;
        public SportsPlan Plan
        {
            get { return _plan; }
            private set
            {
                _plan = value;
                RaisePropertyChanged(() => Plan);
            }
        }

        public User User
        {
            get { return _sessionManager.GetCurrentSession().AssociatedUser; }
        }

        
        string _time;
        public string Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    RaisePropertyChanged(() => Time);
                }
            }
        }

        public void Run()
        {
            Time = DateFacility.GetDayName(DateTime.Now.DayOfWeek);
            Refresh();
            
        }

        SportsPlan LoadPlan(User user,DateTime time)
        {
            SportsPlan plan = null;
            //DateTime now = Time;
            using (_sportsPlanProvider.GetContextScope())
            {
                plan = _sportsPlanProvider.Fetch((t) => t.User.Id == user.Id && t.Month == time.Month && t.Year == time.Year && time.Day == t.Day).FirstOrDefault();
                //plan = plans.FirstOrDefault((t) => SameDay(time, t.Time));
                if (plan != null)
                {
                    plan.SportsPlanItems = plan.SportsPlanItems.ToList();

                    foreach (var item in plan.SportsPlanItems)
                        item.Sport = _sportProvider.Fetch(t => (t.Id == item.Sport.Id)).FirstOrDefault();
                }
            }
            //plan.User = User;
            return plan;
        }



        void OnSportsPlanCreateOrUpdate(SportsPlan plan)
        {
            if(MathFacility.TheSameDay(DateTime.Now,plan))
            {
                Refresh();
            }
        }
    }
}
