using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
//         private bool isBirdsViewCreation;
//         private bool isObjectDetection;
//         private bool isDetectionConfirmation;
//         private bool isHeapMapCreation;
//         private string cameraLocationLongitude;
//         private string cameraLocationLatitude;
//         private string targetLocationLongitude;
//         private string targetLocationLatitude;
//         private string cameraSettingFilePath;
//         private string cameraHeight;
//         private string cameraAngle;
//         private string pictureFolder;
//         private string startHour = "0";
//         private string startMinute = "0";
//         private string startSecond = "0";
//         private string endHour = "0";
//         private string endMinute = "0";
//         private string endSecond = "0";

        CameraSettingPage cameraSettingPage = new CameraSettingPage();
        PictureSettingPage pictureSettingPage = new PictureSettingPage();
        AdvancedSettingPage advancedSettingPage = new AdvancedSettingPage();
        MapWindowPage mapWindowPage = new MapWindowPage();
        BackgroundWorker worker = null;

        private string strTempDir;

        public MainWindow()
        {
            InitializeComponent();
            pageTransitionControl.TransitionType = WpfPageTransitions.PageTransitionType.SlideAndFade;
            pageTransitionControl.ShowPage(cameraSettingPage);
            cameraSettingPage.CameraSettingPageNextBtnClickEvent += new CameraSettingPageNextBtnClickDelegate(ShowPictureSettingPage);
            cameraSettingPage.CameraSettingPageShowMapBtnClickEvent += new CameraSettingPageShowMapBtnClickDelegate(ShowMap);
            pictureSettingPage.PictureSettingPageAdvancedBtnClickEvent += new PictureSettingPageAdvancedBtnClickDelegate(ShowAdvancedSettingPage);
            pictureSettingPage.PictureSettingPageBackBtnClickEvent += new PictureSettingPageBackBtnClickDelegate(ShowCameraSettingPage);
            pictureSettingPage.PictureSettingPageOkBtnClickEvent += new PictureSettingPageOkBtnClickDelegate(ShowProcessingPage);
            advancedSettingPage.AdvancedSettingPageBackBtnClickEvent += new AdvancedSettingPageBackBtnClickDelegate(ShowPictureSettingPage);
            advancedSettingPage.AdvancedSettingPageOkBtnClickEvent += new AdvancedSettingPageOkBtnClickDelegatte(ShowProcessingPage);
            mapWindowPage.MapWindowPageCancelBtnClickEvent += new MapWindowPageCancelBtnClickDelegate(ShowCameraSettingPage);
            mapWindowPage.MapWindowPageOkBtnClickEvent += new MapWindowPageOkBtnClickDelegate(ShowCameraSettingPage);
            worker = new BackgroundWorker();
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
            DirectoryInfo directoryInfo = new DirectoryInfo(pictureSettingPage.PictureFolder);
            strTempDir = System.IO.Path.Combine(directoryInfo.Parent.FullName, directoryInfo.Name + "_Results");
            Processing processingPage = new Processing();
            pageTransitionControl.ShowPage(processingPage);
            CallBirdsView();
        }

        public void ShowMap(object sender, RoutedEventArgs e)
        {
            //mapWindowPage.SetCoordinate();
            pageTransitionControl.ShowPage(mapWindowPage);
        }

        private void CallIniWriterToWriteParameters(string fileName, Dictionary<string, string> paras)
        {
            string parastring = " ";
            foreach(var item in paras)
            {
                parastring += " -" + item.Key.ToString() + " "+item.Value.ToString();
            }
            ProcessStartInfo processInfo = new ProcessStartInfo("IniWriter.exe", fileName + parastring);
            processInfo.CreateNoWindow = false;
            processInfo.UseShellExecute = false;
            processInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processInfo.RedirectStandardOutput = true;

            Process iniWriteProcess = Process.Start(processInfo);
            iniWriteProcess.WaitForExit();
            //返回值为 0
            StreamReader reader = iniWriteProcess.StandardOutput;
            string outputstring = reader.ReadToEnd();

        }

        private void CallBirdsView()
        {
            Dictionary<string, string> birdsPara = new Dictionary<string, string>();
            birdsPara.Add(BoZConstant.BirdViewConfigPara_CameraHeight, cameraSettingPage.CameraHeight);
            birdsPara.Add(BoZConstant.BirdViewConfigPara_CameraPitch, cameraSettingPage.CameraAngle);
            birdsPara.Add(BoZConstant.BirdViewConfigPara_StartFrameNo,"1");
            birdsPara.Add(BoZConstant.BirdViewConfigPara_ProcessingFrameNum, "1200");
            birdsPara.Add(BoZConstant.BirdViewConfigPara_ObjInfo,"1");
            birdsPara.Add(BoZConstant.BirdViewConfigPara_SaveDataPath, strTempDir);
            birdsPara.Add(BoZConstant.BirdViewConfigPara_FrameSkipNum, "1");
            CallIniWriterToWriteParameters(BoZConstant.BirdViewConfigFile, birdsPara);
            //Set BirdView.ini
        }

        private void CallDCD()
        {

        }

        private void CallLRU3D()
        {

        }

        private void CallHeatmap()
        {

        }
    }
}
