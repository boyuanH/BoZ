using System;
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
    /// CameraSettingPage.xaml 的交互逻辑
    /// </summary>
    /// 
    public delegate void CameraSettingPageNextBtnClickDelegate(object sender, EventArgs e);
    public delegate void CameraSettingPageSettingFileLoadBtnClickDelegate(object sender, EventArgs e);
    public delegate void CameraSettingPageSaveBtnClickDelegate(object sender, EventArgs e);


    public partial class CameraSettingPage : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event CameraSettingPageNextBtnClickDelegate CameraSettingPageNextBtnClickEvent;
        public event CameraSettingPageSettingFileLoadBtnClickDelegate CameraSettingPageSettingFileLoadBtnClickEvent;
        public event CameraSettingPageSaveBtnClickDelegate CameraSettingPageSaveBtnClickEvent;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string cameraLocationLongitude;
        public string CameraLocationLongitude
        {
            get
            {
                return cameraLocationLongitude;
            }
            set
            {
                cameraLocationLongitude = value;
                OnPropertyChanged("CameraLocationLongitude");
            }
        }

        private string cameraLocationLatitude;
        public string CameraLocationLatitude
        {
            get
            {
                return cameraLocationLatitude;
            }
            set
            {
                cameraLocationLatitude = value;
                OnPropertyChanged("CameraLocationLatitude");
            }
        }

        private string targetLocationLongitude;
        public string TargetLocationLongitude
        {
            get
            {
                return targetLocationLongitude;
            }
            set
            {
                targetLocationLongitude = value;
                OnPropertyChanged("TargetLocationLongitude");
            }
        }

        private string targetLocationLatitude;
        public string TargetLocationLatitude
        {
            get
            {
                return targetLocationLatitude;
            }
            set
            {
                targetLocationLatitude = value;
                OnPropertyChanged("TargetLocationLatitude");
            }
        }

        private string cameraSettingFilePath;
        public string CameraSettingFilePath
        {
            get
            {
                return cameraSettingFilePath;
            }
            set
            {
                cameraSettingFilePath = value;
                OnPropertyChanged("CameraSettingFilePath");
            }
        }

        private string cameraHeight;
        public string CameraHeight
        {
            get
            {
                return cameraHeight;
            }
            set
            {
                cameraHeight = value;
                OnPropertyChanged("CameraHeight");
            }
        }

        private string cameraAngle;
        public string CameraAngle
        {
            get
            {
                return cameraAngle;
            }
            set
            {
                cameraAngle = value;
                OnPropertyChanged("CameraAngle");
            }
        }

        public CameraSettingPage()
        {
            InitializeComponent();
        }

        private void CameraSettingPageNextBtn_Click(object sender, RoutedEventArgs e)
        {
            if(CameraSettingPageNextBtnClickEvent != null)
            {
                CameraSettingPageNextBtnClickEvent(this, e);
            }
        }

        private void CameraSettingPageBrowerBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CameraSettingPageSettingFileLoadBtn_Click(object sender, RoutedEventArgs e)
        {
            if(CameraSettingPageSettingFileLoadBtnClickEvent != null)
            {
                CameraSettingPageSettingFileLoadBtnClickEvent(this, e);
            }
        }

        private void CameraSettingPageSettingFileSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(CameraSettingPageSaveBtnClickEvent != null)
            {
                CameraSettingPageSaveBtnClickEvent(this, e);
            }
        }

        private void CameraSettingPageCameraLocationShowmapBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CameraSettingPageTargetLocationShowmapBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
