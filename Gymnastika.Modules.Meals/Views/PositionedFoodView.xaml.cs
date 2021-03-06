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
    /// Interaction logic for PositionedFoodView.xaml
    /// </summary>
    public partial class PositionedFoodView : IPositionedFoodView
    {
        public PositionedFoodView()
        {
            InitializeComponent();
        }

        #region IPositionedFoodView Members

        public IPositionedFoodViewModel Context
        {
            get
            {
                return this.DataContext as IPositionedFoodViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        #endregion
    }
}
