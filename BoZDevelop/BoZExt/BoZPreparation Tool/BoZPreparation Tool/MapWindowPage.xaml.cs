﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BoZPreparation_Tool
{
    /// <summary>
    /// MapWindowPage.xaml 的交互逻辑
    /// </summary>
    /// 

    public delegate void MapWindowPageCancelBtnClickDelegate(object sender, EventArgs e);
    public delegate void MapWindowPageOkBtnClickDelegate(object sender, EventArgs e);

    public partial class MapWindowPage : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string mapLongitude;
        public string MapLongitude
        {
            get
            {
                return mapLongitude;
            }
            set
            {
                mapLatitude = MapLongitude;
                OnPropertyChanged("MapLongitude");
            }
        }
        private string mapLatitude;
        public string MapLatitude
        {
            get
            {
                return mapLatitude;
            }
            set
            {
                mapLatitude = value;
                OnPropertyChanged("MapLatitude");
            }
        }


        public MapWindowPage()
        {
            InitializeComponent();
        }

        private void MapWindowPageCancelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MapWindowPageOKBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
