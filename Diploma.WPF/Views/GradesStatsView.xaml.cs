﻿using Diploma.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Diploma.WPF.Views
{
    /// <summary>
    /// Interaction logic for GradesStatsView.xaml
    /// </summary>
    public partial class GradesStatsView : UserControl
    {
        private GradesStatsViewModel _vm;
        public GradesStatsView()
        {
            _vm = new GradesStatsViewModel();
            InitializeComponent();
            if(_vm.CurrentUserAccount.Role == "Teacher")
            {
                teacherOnlyStackPanel.Visibility = Visibility.Visible;
            }
        }
    }
}
