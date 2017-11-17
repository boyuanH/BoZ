﻿using System;
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

namespace BoZPreparation_Tool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool isBirdsViewCreation;
        private bool isObjectDetection;
        private bool isDetectionConfirmmation;
        private bool isHeapMapCreation;
        private double cameraLocationLongitude;
        private double cameraLocationlatitude;
        private double targetLocationLongitude;
        private double targetLocationlatitude;
        private string cameraSettingFilePath;
        private double cameraHeight;
        private double cameraAngle;
        private string pictureFolder;
        private DateTime startTime;
        private DateTime endTime;

        public MainWindow()
        {
            InitializeComponent();
            pageTransitionControl.TransitionType = WpfPageTransitions.PageTransitionType.SlideAndFade;
            CameraSettingPage camerasettingPage = new CameraSettingPage();
            pageTransitionControl.ShowPage(camerasettingPage);
        }
    }
}
