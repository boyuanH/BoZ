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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace RCNdemo
{
    /// <summary>
    /// Interaction logic for setting.xaml
    /// </summary>
    public delegate void ValueChanged();

    public partial class setting : Window, INotifyPropertyChanged
    {
        public event ValueChanged ValueChangedEvent;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private string _DBSTR = GlobalVar.path;
        public string DBSTR
        {
            get { return _DBSTR; }
            set { _DBSTR = value; OnPropertyChanged("DBSTR"); }
        }
        private double _opacity = GlobalVar.opacity;
        public double opacity
        {
            get { return _opacity; }
            set { _opacity = value; OnPropertyChanged("opacity"); }
        }
        private int _pleasesign_time = ShowTime.PleaseSign;
        public int pleasesign_time
        {
            get { return _pleasesign_time; }
            set { _pleasesign_time = value; OnPropertyChanged("pleasesign_time"); }
        }

        private int _advertise_time = ShowTime.Advertisement;
        public int advertise_time
        {
            get { return _advertise_time; }
            set { _advertise_time = value; OnPropertyChanged("advertise_time"); }
        }

        private int _welcome_time = ShowTime.Welcome;
        public int welcome_time
        {
            get { return _welcome_time; }
            set { _welcome_time = value; OnPropertyChanged("welcome_time"); }
        }

        private int _signedwall_time = ShowTime.Signaturewall;
        public int signedwall_time
        {
            get { return _signedwall_time; }
            set { _signedwall_time = value; OnPropertyChanged("signedwall_time"); }
        }

        public setting()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        private bool checkDir(string url)
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
        private bool SaveSettings()
        {
            if (_DBSTR == "")
            {
                System.Windows.MessageBox.Show("监控路径不能为空");
                return false;
            }
            else
            {
                if (checkDir(_DBSTR))
                {
                    GlobalVar.path = _DBSTR;
                }
                else
                {
                    System.Windows.MessageBox.Show("监控路径设置不正确");
                    return false;
                }
                
            }
            if ((_opacity < 0) || (_opacity > 1))
            {
                System.Windows.MessageBox.Show("透明度请输入0~1之间的值");
                return false;
            }
            else
            {
                GlobalVar.opacity = _opacity;
            }
            if ((_pleasesign_time >0)&& (_pleasesign_time <61))
            {
             
                ShowTime.PleaseSign = _pleasesign_time;
            }
            else
            {
                System.Windows.MessageBox.Show("请赐签名页的显示时间请输入0~60s之间的值");
                return false;
                
            }

            if ((_advertise_time >0) && (_advertise_time <61))
            {
         
                ShowTime.Advertisement = _advertise_time;
            }
            else
            {
                System.Windows.MessageBox.Show("广告页的显示时间请输入0~60s之间的值");
                return false;              
            }

            if ((_welcome_time >0) && (_welcome_time < 61))
            {
            
                ShowTime.Welcome = _welcome_time;
            }
            else
            {
                System.Windows.MessageBox.Show("欢迎页的显示时间请输入0~60s之间的值");
                return false;              
            }

            if ((_signedwall_time > 0) && (_signedwall_time <61))
            {
            
                ShowTime.Signaturewall = _signedwall_time;
            }
            else
            {
                System.Windows.MessageBox.Show("签名墙的显示时间请输入0~60s之间的值");
                return false;               
            }

            if (ValueChangedEvent != null)
            {
                ValueChangedEvent();
            }

            return true;
        }

        private void BtnSelectPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "选择文件夹";
            fbd.SelectedPath = this.TextBox_DBPath.Text.ToString();
            DialogResult result = fbd.ShowDialog();

            if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                this.TextBox_DBPath.Text = fbd.SelectedPath;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(300);
            this.Close();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (SaveSettings())
            {
                this.Close();
            }
            else
            {
                return;
            }

        }
    }
}
