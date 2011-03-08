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

namespace Gymnastika.Modules.Meals.Views
{
    /// <summary>
    /// Interaction logic for BMIIntroductionView.xaml
    /// </summary>
    public partial class BMIIntroductionView : IBMIIntroductionView
    {
        public BMIIntroductionView()
        {
            InitializeComponent();
        }

        #region IBMIIntroductionView Members

        public void ShowView()
        {
            this.Show();
        }

        public void CloseView()
        {
            this.Close();
        }

        #endregion
    }
}