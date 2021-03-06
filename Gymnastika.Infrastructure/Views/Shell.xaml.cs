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
using Gymnastika.ViewModels;
using Microsoft.Practices.Unity;
using Microsoft.Surface.Presentation.Controls;

namespace Gymnastika.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : SurfaceWindow
    {
        public Shell()
        {
            InitializeComponent();
        }

        [Dependency]
        public ShellViewModel Model
        {
            get { return DataContext as ShellViewModel; }
            set { DataContext = value; }
        }
    }
}
