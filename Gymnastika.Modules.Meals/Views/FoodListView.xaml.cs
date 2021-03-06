﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gymnastika.Modules.Meals.ViewModels;

namespace Gymnastika.Modules.Meals.Views
{
    /// <summary>
    /// Interaction logic for FoodListView.xaml
    /// </summary>
    public partial class FoodListView : IFoodListView
    {
        public FoodListView()
        {
            InitializeComponent();
        }

        #region IFoodListView Members

        public IFoodListViewModel Context
        {
            get
            {
                return this.DataContext as IFoodListViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public event SelectionChangedEventHandler FoodItemSelectionChanged;

        #endregion

        private void FoodList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FoodItemSelectionChanged != null)
                FoodItemSelectionChanged(sender, e);
        }
    }
}
