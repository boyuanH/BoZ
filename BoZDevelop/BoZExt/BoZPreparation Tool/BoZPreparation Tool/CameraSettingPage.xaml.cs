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
    public delegate void CameraSettingPageShowMapBtnClickDelegate(object sender, RoutedEventArgs e);



    public partial class CameraSettingPage : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event CameraSettingPageNextBtnClickDelegate CameraSettingPageNextBtnClickEvent;
        public event CameraSettingPageShowMapBtnClickDelegate CameraSettingPageShowMapBtnClickEvent;


        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string cameraLocationLongitude = "0.0000";
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

        private string cameraLocationLatitude = "0.0000";
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

        private string targetLocationLongitude = "0.0000";
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

        private string targetLocationLatitude = "0.0000";
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
            CameraSettingPageNextBtnClickEvent?.Invoke(this, e);
        }

        private void CameraSettingPageBrowerBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CameraSettingPageSettingFileLoadBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CameraSettingPageSettingFileSaveBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CameraSettingPageCameraLocationShowmapBtn_Click(object sender, RoutedEventArgs e)
        {
            CameraSettingPageShowMapBtnClickEvent?.Invoke(this, e);
        }

        private void CameraSettingPageTargetLocationShowmapBtn_Click(object sender, RoutedEventArgs e)
        {
            CameraSettingPageShowMapBtnClickEvent?.Invoke(this, e);

        }
    }
}
