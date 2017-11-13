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
using System.ComponentModel;
using System.IO;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;


namespace RCNdemo
{
    public class position
    {
        public int row { get; set; }
        public int col { get; set; }
    }

    public partial class signedwall : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private double _opacity = GlobalVar.opacity;
        public double opacity
        {
            get { return _opacity; }
            set { _opacity = value; OnPropertyChanged("opacity"); }
        }
        public int m_DecodeWidth = 0;
        public int m_DecodeHeight = 0;
        private double screen_width = 0;
        private double screen_height = 0;

        private Bitmap image_pre = null;
        private Bitmap m_image = null;

        int m_rows = 2;
        int m_cols = 2;
        int m_rows_pre = 0;
        int m_cols_pre = 0;
        int cnt = 0;
        int num_rad = 0;
        Random rd = new Random();
        byte[] posi;
        Dictionary<string, position> posiDictionary = new Dictionary<string, position>();
        List<position> emposi=new List<position>();

        public signedwall()
        {
            InitializeComponent();
            posiDictionary.Clear();
            screen_width = SystemParameters.WorkArea.Width;
            screen_height = SystemParameters.WorkArea.Height / 5 * 4;
            m_DecodeWidth = (int)(screen_width / m_cols + 0.5);
            m_DecodeHeight = (int)(screen_height / m_rows + 0.5);
            cnt = m_cols * m_rows;
            num_rad = cnt;
            m_image = new Bitmap(m_DecodeWidth * m_cols, m_DecodeHeight * m_rows, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using(Graphics g = Graphics.FromImage(m_image))
            {
                g.Clear(System.Drawing.Color.White);
            }
            
            posi = new byte[cnt];
            for (int i = 0; i < cnt; i++) posi[i] = 0;
            this.DataContext = this;
            string path = @"..\resource\bg1_2.jpg";
            Image_Loaded(path,logo);
            path = @"..\resource\bg1.jpg";
            Image_Loaded(path, background);

            try
            {
                ShowAllSignExist();
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine("Out of Memory: {0}", e.Message);
            }
        }


        public void DeletSigna(string path)
        {
            foreach (var item in posiDictionary)
            {
                if (item.Key == path)
                {
                    GlobalVar.people_num = GlobalVar.people_num - 1;

                    var bitmap_remove = new Bitmap(m_DecodeWidth, m_DecodeHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    using (Graphics g = Graphics.FromImage(bitmap_remove))
                    {
                        g.Clear(System.Drawing.Color.White);
                    }
                    
                    int row_num = item.Value.row;
                    int col_num = item.Value.col;
                    int x = col_num * m_DecodeWidth;
                    int y = row_num * m_DecodeHeight;
                    using (Graphics g1 = Graphics.FromImage(m_image))
                    {
                        g1.DrawImage(bitmap_remove, x, y);
                        var bitmapsource = BitmapToBitmapImage(m_image.Clone() as Bitmap);
                        g1.Dispose();
                    }               
                    emposi.Add(new position { row = row_num, col = col_num });
                    posiDictionary.Remove(item.Key);
                    break;
                }
            }

        }

        public void AddSigna(string path)
        {
            byte[] bytes = GlFun.TransImageToByte(path);
            if (bytes == null)
            {
                return;
            }
            GlobalVar.people_num += 1;
			
            Graphics g = Graphics.FromImage(m_image);
            int row_num = 0;
            int col_num = 0;

            if (emposi.Count()!=0)
            {
                row_num = emposi[0].row;
                col_num = emposi[0].col;
                emposi.RemoveAt(0);
               
            }
            else
            {
                if (num_rad == 0)
                {
                    m_rows_pre = m_rows;
                    m_cols_pre = m_cols;
                    m_rows = m_rows_pre + 1;
                    m_cols = m_cols_pre + 1;
                    cnt = m_rows * m_cols - m_rows_pre * m_cols_pre;
                    num_rad = cnt;
                    m_DecodeHeight = (int)(screen_height/m_rows+0.5);
                    m_DecodeWidth = (int)(screen_width/m_cols+0.5);
                    image_pre = ResizeImage(m_image, m_DecodeWidth * m_cols_pre, m_DecodeHeight * m_rows_pre);
                    g.Clear(System.Drawing.Color.White);
                    g.DrawImage(image_pre, 0, 0);
                    posi = new byte[cnt];
                    for (int i = 0; i < cnt; i++) posi[i] = 0;
                }
                int num = rd.Next(1, num_rad);
                num_rad = num_rad - 1;
                int posi_real = 0;
                int num_ra = 0;
                for (int i = 0; i < cnt; i++)
                {
                    if (posi[i] == 0) num_ra = num_ra + 1;
                    if (num == num_ra)
                    {
                        posi[i] = 1;
                        posi_real = (i + 1);
                        break;
                    }
                }


                int dvalue = posi_real - m_rows_pre;
                if (dvalue > 0)
                {
                    row_num = (dvalue - 1) / m_cols + m_rows_pre;
                    col_num = (dvalue + m_cols - 1) % m_cols;
                }
                else
                {
                    row_num = (posi_real - 1) / (m_cols - m_cols_pre);
                    col_num = (posi_real + (m_cols - m_cols_pre) - 1) % (m_cols - m_cols_pre) + m_cols_pre;
                }
            }
            

                
            BitmapImage bmImage = new BitmapImage();
            bmImage.BeginInit();
            bmImage.StreamSource = new MemoryStream(bytes);
            bmImage.DecodePixelWidth = m_DecodeWidth;
            bmImage.DecodePixelHeight = m_DecodeHeight;
            bmImage.EndInit();
            bmImage.Freeze();

            var bitmap = BitmapImage2Bitmap(bmImage);
            
            int x = col_num * m_DecodeWidth;
            int y = row_num * m_DecodeHeight;
            g.DrawImage(bitmap, x, y);
            posiDictionary.Add(path, new position { row = row_num, col = col_num });   
            g.Dispose();                         
            return;
        }


        private void ShowAllSignExist()
        {
            DirectoryInfo file_dir = new DirectoryInfo(GlobalVar.path);
            if (!file_dir.Exists)
            {
                return;
            }
            FileInfo[] file_list = file_dir.GetFiles();
            int file_num = file_list.Length;
            foreach (FileInfo file in file_list)
            {
                AddSigna(file.FullName);
            }
            return;
        }

        public void ShowImage()
        {
            if (m_image != null)
            {            
                var bitmapsource = BitmapToBitmapImage(m_image.Clone() as Bitmap);
                signboard.Source = bitmapsource;
            }
        }

        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                return new Bitmap(bitmap);
            }
        }

        public BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Jpeg);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private void Image_Loaded(string path, System.Windows.Controls.Image imagecontrol)
        {
            if (!File.Exists(path))
            {
                return;
            }
            using (BinaryReader loader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                FileInfo fd = new FileInfo(path);
                int Length = (int)fd.Length;
                byte[] buf = new byte[Length];
                buf = loader.ReadBytes((int)fd.Length);
                loader.Dispose();
                loader.Close();
                BitmapImage bim = new BitmapImage();
                bim.BeginInit();
                bim.StreamSource = new MemoryStream(buf);
                bim.EndInit();              
                GC.Collect();
                var bitmap = BitmapImage2Bitmap(bim);
                var rebitmap = ResizeImage(bitmap, (int)screen_width, (int)screen_height);
                var source = BitmapToBitmapImage(rebitmap);
                imagecontrol.Source = source;
            }
        }
    }

}
