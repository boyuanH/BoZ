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
using System.Windows.Threading;
using System.Security.Permissions;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Threading;
using WpfPageTransitions;

namespace RCNdemo
{
    //just add comments for jenkins test,jiahuan
    public static class ShowTime
    {
//         public static int Advertisement = 6;
//         public static int Welcome = 3;
//         public static int Signaturewall = 8;
//         public static int PleaseSign = 3;
//         public static int DefaultTime = 0;
        public static int Advertisement = 6;
        public static int Welcome = 3;
        public static int Signaturewall = 8;
        public static int PleaseSign = 3;
        public static int DefaultTime = 0;
    }
    public static class GlobalVar
    {
        public static int people_num = 1;
        public static double opacity = 0.4;
        public static string path = "D:\\RCN_demo\\test";
        public static int AdsCount = 1;
    }
    public static class GlFun
    {
        public static byte[] TransImageToByte(string path)
        {
            FileInfo fileInfo = new FileInfo(path);            
            
            BinaryReader binReader = new BinaryReader(File.Open(path, FileMode.Open));

            byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
            binReader.Close();
            return bytes;
        }
    }

    public class FileChangeInfo
    {
        public string fileName { get; set; }
        public bool fileStatus { get; set; }
    }


    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        bool Isadvertis = false;
        bool Iswelcome = false;
        bool Isdel = false;
        int i = 0;

        private DispatcherTimer d_timer = new DispatcherTimer();
        private FileSystemWatcher watcher ;
        List<string> filepath = new List<string>();
		List<string> filepath_del = new List<string>();
        SignatureBitmap signatureWall;

        private Queue<FileChangeInfo> fileTarget = new Queue<FileChangeInfo>();
        private BackgroundWorker worker;
        public MainWindow()
        {
            InitializeComponent();

            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowStyle = WindowStyle.None;
            }

            watcher = new FileSystemWatcher();
            watcher.IncludeSubdirectories = true;
            watcher.Filter = "*.jpg";
            if (checkDir(GlobalVar.path))
            {
                watcher.Path = GlobalVar.path;
                watcher.EnableRaisingEvents = true;
            }
            else
            {
                watcher.EnableRaisingEvents = false;
                System.Windows.MessageBox.Show("请在设置页面配置监控文件夹");
            }
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size;
            watcher.Created += new FileSystemEventHandler(OnNewFileArrived);
            watcher.Deleted += new FileSystemEventHandler(OnFileDeleted);



            this.KeyDown += WindowStateControl;

            pageTransitionControl.TransitionType = WpfPageTransitions.PageTransitionType.SlideAndFade;
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(FileChanged);


            d_timer.Interval = new TimeSpan(0, 0, 1);
            d_timer.Tick += new EventHandler(dTimer_Tick);
        }

        private void WindowStateControl(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)//Esc键  
            {
                RestoreTitleBar();
                RestoreWidowState();
            }
            if (e.Key == Key.F11)//F11键  
            {
                RestoreTitleBar();
                RestoreWidowState();
            }
            if (e.Key == Key.F12)
            {
                setting win = new setting();
                win.ValueChangedEvent += new ValueChanged(OnValueChanged);
                win.ShowDialog();
            }
        }

        private void OnValueChanged()
        {
            if (checkDir(GlobalVar.path))
            {
                watcher.Path = GlobalVar.path;
                if (!watcher.EnableRaisingEvents)
                {
                    watcher.EnableRaisingEvents = true;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("请在设置页面配置监控文件夹");
            }
        }

        public bool checkDir(string url)
        {
            try
            {
                if (!Directory.Exists(url))
                    Directory.CreateDirectory(url);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void RestoreWidowState()
        {
            this.WindowState = WindowState.Normal;
        }

        private void RestoreTitleBar()
        {
            this.WindowStyle = WindowStyle.SingleBorderWindow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            signatureWall = new SignatureBitmap(Convert.ToInt32(pageTransitionControl.ActualWidth), Convert.ToInt32(pageTransitionControl.ActualHeight * 4 / 5));
            if (signatureWall.CurrentSignature != 0)
            {
                i = signatureWall.CurrentSignature;
            }
            worker.RunWorkerAsync();
            d_timer.Start();

        }
        

        private void FileChanged(object sender, DoWorkEventArgs e)
        {
            signatureWall.InitLocalFile();
            while (true)
            {
                bool taskResult = true;
                try
                {
                    if (fileTarget.Count > 0)
                    {
                        //Timer stop
                        //DoSomeThing
                        FileChangeInfo fileInfo = fileTarget.Peek();

                        string message = fileInfo.fileName + " Queue De ";
                        if (fileInfo.fileStatus == true)
                        {
                            message += "Insert";
                            d_timer.Stop();
                            string strtmp = fileInfo.fileName;
                            StringBuilder sb = new StringBuilder(strtmp);
                            ImagePosition theposition = signatureWall.AddSignature(sb.ToString());
                            if (theposition == null)
                            {
                                d_timer.Interval = new TimeSpan(0, 0, ShowTime.DefaultTime);
                                d_timer.Start();
                                continue;
                            }
                            this.Dispatcher.Invoke(new Action(() => { ShowWelcomePage(fileInfo.fileName); }));
                            fileTarget.Dequeue();
                            Thread.Sleep(ShowTime.Welcome * 1000 - 10);
                            if (fileTarget.Count == 0)
                            {
                                d_timer.Interval = new TimeSpan(0, 0, ShowTime.DefaultTime);
                                d_timer.Start();
                            }
                        }
                        else
                        {
                            message += "Delete";
                            ImagePosition imgPosition = signatureWall.RemoveSignature(fileInfo.fileName);
                            if (imgPosition != null)
                            {
                                i--;
                            }
                            fileTarget.Dequeue();
                        }
                        Debug.WriteLine(message + (fileTarget.Count + 1));
                        //Rest WelcomePageTime
                    }
                    else
                    {
                        if (d_timer.IsEnabled == false)
                        {
                            //d_timer.Start();
                        }
                        Thread.Sleep(100);
                    }
                }
                catch (OutOfMemoryException ex)
                {
                    taskResult = false;
                    Thread.Sleep(3000);
                    continue;
                }
//                 finally
//                 {
//                     if (taskResult == true && fileTarget.Count != 0)
//                     {
//                         fileTarget.Dequeue();
//                     }
//                 }
                                
            }
        }

        private int CurrentPage = -1;

        private void dTimer_Tick(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowStyle = WindowStyle.None;
            }
            //d_timer.Interval = GetTimeSpan();
            switch (CurrentPage)
            {
                case -1:
                    {
                        //ShowPlsSignYourNamePage();
                        CurrentPage = 2;
                        //this.Dispatcher.Invoke(new Action(() => { ShowPlsSignYourNamePage(); }));
                    }
                    break;
                case 0:
                    {
                        this.Dispatcher.Invoke(new Action(() => { ShowAdPage(); }));
                    }
                    break;
                case 1:
                    {
                        this.Dispatcher.Invoke(new Action(() => { ShowSignatureWallPage(); }));
                    }
                    break;
                case 2:
                    {
                        this.Dispatcher.Invoke(new Action(() => { ShowPlsSignYourNamePage(); }));
                    }
                    break;

                case 3:
                    {
                        this.Dispatcher.Invoke(new Action(() => { ShowSignatureWallPage(); }));
                    }
                    break;
                default:
                    {
                        this.Dispatcher.Invoke(new Action(() => { ShowPlsSignYourNamePage(); }));
                    }   
                    break;
            }
        }

        private TimeSpan GetTimeSpan()
        {
            TimeSpan time ;
            switch (CurrentPage)
            {
                case 0:
                    time = new TimeSpan(0, 0, ShowTime.PleaseSign);
                    break;
                case 1:
                    time = new TimeSpan(0, 0, ShowTime.Advertisement);
                    break;
                case 2:
                    time = new TimeSpan(0, 0, ShowTime.Signaturewall);
                    break;
                case 3:
                    time = new TimeSpan(0, 0, ShowTime.Welcome);
                    break;
                default:
                    time = new TimeSpan(0, 0, ShowTime.DefaultTime);
                    break;                    

            }
            return time;
        }
        
        private void OnNewFileArrived(object sender,FileSystemEventArgs e)
        {

            FileChangeInfo info = new FileChangeInfo();
            info.fileName = e.Name;
            info.fileStatus = true;
            fileTarget.Enqueue(info);
            Debug.WriteLine(info.fileName + " insert into Directory");
        }

        private void OnFileDeleted(object sender,FileSystemEventArgs e)
        {
            FileChangeInfo info = new FileChangeInfo();
            info.fileName = e.Name;
            info.fileStatus = false;
            fileTarget.Enqueue(info);
            Debug.WriteLine(info.fileName + " delete from Directory");
        }

        private PageTransitionType GetRandomType()
        {
            Random ran = new Random();
            int c = Enum.GetNames(typeof(PageTransitionType)).Length;
            int num = ran.Next(0, c);
            PageTransitionType type =  (PageTransitionType)Enum.ToObject(typeof(PageTransitionType), num);
            return type;
        }

        private void ShowSignatureWallPage()
        {
            CurrentPage = 2;
            if (signatureWall.CurrentSignature == 0)
            {
                d_timer.Interval = new TimeSpan(0,0,ShowTime.DefaultTime);
                return;
            }
            SignatureWallPage p3 = new SignatureWallPage(signatureWall.SignatureWallBitmapImg);
//             JpegBitmapEncoder encoder = new JpegBitmapEncoder();
//             encoder.Frames.Add(BitmapFrame.Create(signatureWall.SignatureWallBitmapImg));
//             FileStream fileStream = new FileStream(@"D:\RCN_demo\myresult\" + signatureWall.CurrentSignature + DateTime.Now.ToString("hhmmss") + @".jpg", FileMode.Create, FileAccess.ReadWrite);
//             encoder.Save(fileStream);
//             fileStream.Close();

            pageTransitionControl.TransitionType = GetRandomType();
            
            pageTransitionControl.ShowPage(p3);
            d_timer.Interval = GetTimeSpan();

        }

        private void ShowPlsSignYourNamePage()
        {
            PleaseSignPage p2 = new PleaseSignPage();
            pageTransitionControl.TransitionType = GetRandomType();
            CurrentPage = 0;
            pageTransitionControl.ShowPage(p2);
            d_timer.Interval = GetTimeSpan();

        }

        private void ShowAdPage()
        {
            AdPage page = new AdPage();
            pageTransitionControl.TransitionType = GetRandomType();
            CurrentPage = 1;
            pageTransitionControl.ShowPage(page);
            d_timer.Interval = GetTimeSpan();

        }

        private void ShowWelcomePage(string fileName)
        {
            string filePath = System.IO.Path.Combine(GlobalVar.path, fileName);
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                BitmapImage bitImgwelcome = new BitmapImage(new Uri(fileInfo.FullName));
                pageTransitionControl.TransitionType = GetRandomType();
                i++;
                WelcomePage p1 = new WelcomePage(signatureWall.CurrentSignature, bitImgwelcome);
                d_timer.Stop();
                CurrentPage = 3;
                pageTransitionControl.ShowPage(p1);
            }
        }

        private void Window_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {

        }


    }
}
