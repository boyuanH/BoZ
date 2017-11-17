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
    /// PictureSettingPage.xaml 的交互逻辑
    /// </summary>
    /// 

    public delegate void PictureSettingPageAdvancedBtnClickDelegate(object sender, EventArgs e);
    public delegate void PictureSettingPageOkBtnClickDelegate(object sender, EventArgs e);
    public delegate void PictureSettingPageBackBtnClickDelegate(object sender, EventArgs e);
    public partial class PictureSettingPage : UserControl
    {
        public PictureSettingPage()
        {
            InitializeComponent();
        }

        private void PictureSettingPageAdvancedBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PictureSettingPageBackBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PictureSettingPageOkBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
