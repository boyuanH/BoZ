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

namespace BoZPreparation_Tool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool isBirdsViewCreation;
        private bool isObjectDetection;
        private bool isDetectionConfirmation;
        private bool isHeapMapCreation;
        private string cameraLocationLongitude;
        private string cameraLocationLatitude;
        private string targetLocationLongitude;
        private string targetLocationLatitude;
        private string cameraSettingFilePath;
        private string cameraHeight;
        private string cameraAngle;
        private string pictureFolder;
        private string startHour = "0";
        private string startMinute = "0";
        private string startSecond = "0";
        private string endHour = "0";
        private string endMinute = "0";
        private string endSecond = "0";

        CameraSettingPage cameraSettingPage = new CameraSettingPage();
        PictureSettingPage pictureSettingPage = new PictureSettingPage();
        AdvancedSettingPage advancedSettingPage = new AdvancedSettingPage();

        public MainWindow()
        {
            InitializeComponent();
            pageTransitionControl.TransitionType = WpfPageTransitions.PageTransitionType.SlideAndFade;
            pageTransitionControl.ShowPage(cameraSettingPage);
            cameraSettingPage.CameraSettingPageNextBtnClickEvent += new CameraSettingPageNextBtnClickDelegate(ShowPictureSettingPage);
            pictureSettingPage.PictureSettingPageAdvancedBtnClickEvent += new PictureSettingPageAdvancedBtnClickDelegate(ShowAdvancedSettingPage);
            pictureSettingPage.PictureSettingPageBackBtnClickEvent += new PictureSettingPageBackBtnClickDelegate(ShowCameraSettingPage);
            pictureSettingPage.PictureSettingPageOkBtnClickEvent += new PictureSettingPageOkBtnClickDelegate(ShowProcessingPage);
            advancedSettingPage.AdvancedSettingPageBackBtnClickEvent += new AdvancedSettingPageBackBtnClickDelegate(ShowPictureSettingPage);
            advancedSettingPage.AdvancedSettingPageOkBtnClickEvent += new AdvancedSettingPageOkBtnClickDelegatte(ShowProcessingPage);
        }

        public void ShowCameraSettingPage(object sender, EventArgs e)
        {
            pageTransitionControl.ShowPage(cameraSettingPage);
        }

        public void ShowPictureSettingPage(object sender, EventArgs e)
        {
            pageTransitionControl.ShowPage(pictureSettingPage);
        }

        public void ShowAdvancedSettingPage(object sender, EventArgs e)
        {
            pageTransitionControl.ShowPage(advancedSettingPage);
        }

        public void ShowProcessingPage(object sender,EventArgs e)
        {
            Processing processingPage = new Processing();
            pageTransitionControl.ShowPage(processingPage);
        }


    }
}
