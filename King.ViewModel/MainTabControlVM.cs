﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.ViewModel
{
    public class MainTabControlVM : ViewModelBase
    {
        public RelayCommand OpenRulesCommand { get; set; }

        public MainTabControlVM()
        {

        }
    }
}
