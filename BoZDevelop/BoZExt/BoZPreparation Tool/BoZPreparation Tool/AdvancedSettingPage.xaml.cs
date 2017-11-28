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
    /// AdvancedSettingPage.xaml 的交互逻辑
    /// </summary>
    /// 

    public delegate void AdvancedSettingPageBackBtnClickDelegate(object sender, EventArgs e);
    public delegate void AdvancedSettingPageOkBtnClickDelegatte(object sender, EventArgs e);

    public partial class AdvancedSettingPage : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event AdvancedSettingPageBackBtnClickDelegate AdvancedSettingPageBackBtnClickEvent;
        public event AdvancedSettingPageOkBtnClickDelegatte AdvancedSettingPageOkBtnClickEvent;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isBirdsViewCreation = false;
        public bool IsBirdsViewCreation
        {
            get
            {
                return isBirdsViewCreation;
            }
            set
            {
               isBirdsViewCreation = value;
               OnPropertyChanged("IsBirdsViewCreation");
            }
        }
        private bool isObjectDetection = false;
        public bool IsObjectDetection
        {
            get
            {
                return isObjectDetection;
            }
            set
            {
                isObjectDetection = value;
                OnPropertyChanged("IsObjectDetection");
            }
        }
        private bool isDetectionConfirmation = false;
        public bool IsDetectionConfirmation
        {
            get
            {
                return isDetectionConfirmation;
            }
            set
            {
                isDetectionConfirmation = value;
                OnPropertyChanged("IsDetectionConfirmation");
            }
        }
        private bool isHeapMapCreation = false;
        public bool IsHeapMapCreation
        {
            get
            {
                return isHeapMapCreation;
            }
            set
            {
                isHeapMapCreation = value;
                OnPropertyChanged("IsHeapMapCreation");
            }
        }

        public AdvancedSettingPage()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void AdvancedSettingPageBackBtn_Click(object sender, RoutedEventArgs e)
        {
            AdvancedSettingPageBackBtnClickEvent?.Invoke(this, e);
        }

        private void AdvancedSettingPageOKBtn_Click(object sender, RoutedEventArgs e)
        {
            AdvancedSettingPageOkBtnClickEvent?.Invoke(this, e);
        }
    }
}
