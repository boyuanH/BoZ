using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RCNdemo
{
    public class ImagePosition
    {
        public int row { get; set; }
        public int col { get; set; }
        public int scal { get; set; }
    }
    public class SignatureBitmap
    {
        private Dictionary<string, ImagePosition> PositionDictionary ;
        private int[,] Occupancy;
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        private int RowWidth { get { return (int)((double)stageWidth / (double)Rows); } }
        private int ColHeight { get { return (int)((double)stageHeight / (double)Cols); } }

        public int CurrentSignature {
            get
            {
                if (isInited == false)
                {
                    return 0;
                }
                return PositionDictionary.Count; 
            } 
        }

        public BitmapImage SignatureWallBitmapImg
        {
            get
            {
                return BitmapToBitmapImage(ShowSignatureImg);
            }
        }
        private Bitmap SignatureImg { get; set; }
        private Bitmap ShowSignatureImg { get; set; }
        private int stageWidth;
        private int stageHeight;
        private bool isInited = false;

        public SignatureBitmap(int stageWidth,int stageHeight)
        {
            PositionDictionary = new Dictionary<string, ImagePosition>();
            this.stageWidth = stageWidth;
            this.stageHeight = stageHeight;
            SignatureImg = new Bitmap(stageWidth, stageHeight);
            Graphics g = Graphics.FromImage(SignatureImg);
            g.Clear(Color.White);
            g.Dispose();
            Rows = 4;
            Cols = 3;
            Occupancy = new int[Rows, Cols];

            
        }
        
        public int InitLocalFile()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GlobalVar.path);
            Queue<string> fileNames = new Queue<string>();
            int i = 0;
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                if (fileInfo.Extension.ToLower() == ".jpg" )
                {
                    //AddSignature(fileInfo.Name);
                    fileNames.Enqueue(fileInfo.Name);
                    
                }
            }

            while (true)
            {

                if (fileNames.Count == 0)
                {
                    isInited = true;
                    break;
                }
                string filename=fileNames.Peek();
                try
                {
                    var tmpPosition = AddSignature(filename);
                    if (tmpPosition != null)
                    {
                        fileNames.Dequeue();
                        i++;
                    }
                }
                catch (OutOfMemoryException ex)
                {
                    Thread.Sleep(300);
                    if (PositionDictionary.ContainsKey(filename))
                    {
                        PositionDictionary.Remove(filename);
                    }
                    continue;
                }
            }

            return i;
        }

        public ImagePosition AddSignature(string fileName)
        {
            object lockThis = new object();
            lock (lockThis)
            {
                ResizeScene();
                ImagePosition tmpPosition = GetRandomPosition();
                FileInfo info = new FileInfo(Path.Combine(GlobalVar.path, fileName));
                if (!info.Exists)
                {
                    return null;
                }

                if (PositionDictionary.ContainsKey(fileName))
                {
                    return null;
                }

                PositionDictionary.Add(fileName, tmpPosition);
                string strpath = info.FullName;
                //                 Bitmap localbmp = new Bitmap(strpath);
                //                 Bitmap bmp = localbmp.Clone() as Bitmap;
                FileInfo finfo = new FileInfo(strpath);
                if (!finfo.Exists){
                    return null ;
                }
                try
                {
                    Bitmap bmp = new Bitmap(strpath);
                    Bitmap targetBmp = DrawImageAtSpecificRectangle(bmp, new Rectangle(0, 0, RowWidth, ColHeight), new Size(RowWidth, ColHeight), true);
                    Bitmap signBmp = DrawImageAtSpecificRectangle(targetBmp, new Rectangle(tmpPosition.row * RowWidth, tmpPosition.col * ColHeight, RowWidth, ColHeight), new Size(SignatureImg.Width, SignatureImg.Height), false);

                    Bitmap temp1 = SignatureImg;
                    Bitmap temp2 = ShowSignatureImg;

                    SignatureImg = signBmp.Clone() as Bitmap;
                    ShowSignatureImg = SignatureImg.Clone() as Bitmap;
					
                    Occupancy[tmpPosition.row, tmpPosition.col] = 1;

                    bmp.Dispose();
                    targetBmp.Dispose();
                    signBmp.Dispose();
                    temp1.Dispose();
                    if (temp2 != null)
                    {
                        temp2.Dispose();
                    }

                    Debug.Print("current signature number is " + CurrentSignature);
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine("ArgumentException" + fileName);
                    if (PositionDictionary.ContainsKey(fileName)) {
                        PositionDictionary.Remove(fileName);
                    }
                    return null;
                }

                return tmpPosition;
            }
        }

        public ImagePosition RemoveSignature(string fileName)
        {
            object lockThis = new object();
            lock (lockThis)
            {
                if (!PositionDictionary.ContainsKey(fileName))
                {
                    return null;
                }
                ImagePosition removePosition = PositionDictionary[fileName];
                PositionDictionary.Remove(fileName);
                Occupancy[removePosition.row, removePosition.col] = 0;

                Bitmap empty = new Bitmap(RowWidth, ColHeight);
                Graphics g = Graphics.FromImage(empty);
                g.Clear(Color.White);
                g.Dispose();
                Bitmap tmpWall = DrawImageAtSpecificRectangle(empty,new Rectangle( removePosition.row * RowWidth, removePosition.col * ColHeight, RowWidth, ColHeight),new Size( RowWidth, ColHeight), false);
                Bitmap temp1 = SignatureImg;
                Bitmap temp2 = ShowSignatureImg;
                SignatureImg = tmpWall.Clone() as Bitmap;
                ShowSignatureImg = SignatureImg.Clone() as Bitmap;
                empty.Dispose();
                tmpWall.Dispose();
                temp1.Dispose();
                temp2.Dispose();

                return removePosition;
            }
        }
        private void ResizeScene()
        {
            if (CurrentOccupancyRate() != 1 )
            {
                return;
            }
            int oldCols = Cols;
            int oldRows = Rows;
//             Cols = (int)Math.Round(Cols * 1.09,MidpointRounding.AwayFromZero);
//             Rows = (int)Math.Round(Rows * 1.09, MidpointRounding.AwayFromZero);

            Cols++;
            Rows++;

            int[,] tmpOccupancy = new int[Rows,Cols];
            for(int i = 0; i < oldRows; i++)
            {
                for (int j = 0; j < oldCols; j++)
                {
                    tmpOccupancy[i, j] = Occupancy[i, j];
                }
            }
            //Occupancy = null;
            Occupancy = tmpOccupancy;
            SignatureImg = ResizeSignatureImg(oldRows, oldCols);
            Graphics g = Graphics.FromImage(SignatureImg);
            //g.DrawRectangle(new Pen(Brushes.Tomato, 2), new Rectangle(0, 0, SignatureImg.Width, SignatureImg.Height));
            g.Dispose();
            //SignatureImg.Save(@"D:\RCN_demo\tmpResult\_" + tmpOccupancy.Length + ".jpg");
        }


        private double CurrentOccupancyRate()
        {
            if(Rows == 0 || Cols == 0)
            {
                return -1;
            }
            else
            {
                double d = (double)PositionDictionary.Count / (double)(Rows * Cols);
                return d;
            }
        }
        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            if(bitmap == null)
            {
                return null;
            }
            using (var memory = new MemoryStream())
            {
                object lockThis = new object();
                lock (lockThis)
                {
                    Bitmap tmpBitmap = bitmap.Clone() as Bitmap;
                    //bitmap.Dispose();
                    tmpBitmap.Save(memory, ImageFormat.Jpeg);
                    memory.Position = 0;
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    return bitmapImage;
                }
            }
        }
        private Bitmap ResizeSignatureImg(int oldRows,int oldCols)
        {
            //计算缩放后的像素宽高
            int destHeight =(int)(((double)oldCols/ (double)Cols) *  stageHeight);
            int destWidth = (int)(((double)oldRows / (double)Rows) * stageWidth);
            try
            {
                Image sourImage = SignatureImg;
                int width = 0, height = 0;
                //按比例缩放             
                int sourWidth = sourImage.Width;
                int sourHeight = sourImage.Height;
                if (sourHeight > destHeight || sourWidth > destWidth)
                {
                    if ((sourWidth * destHeight) > (sourHeight * destWidth))
                    {
                        width = destWidth;
                        height = (destWidth * sourHeight) / sourWidth;
                    }
                    else
                    {
                        height = destHeight;
                        width = (sourWidth * destHeight) / sourHeight;
                    }
                }
                else
                {
                    width = sourWidth;
                    height = sourHeight;
                }

                return DrawImageAtSpecificRectangle(SignatureImg, new Rectangle(0, 0, destWidth, destHeight), new Size(SignatureImg.Width, SignatureImg.Height), true);

//                 Bitmap destBitmap = new Bitmap(destWidth, destHeight);
//                 Graphics g = Graphics.FromImage(destBitmap);
//                 g.Clear(Color.Transparent);
//                 g.CompositingQuality = CompositingQuality.HighQuality;
//                 g.SmoothingMode = SmoothingMode.HighQuality;
//                 g.InterpolationMode = InterpolationMode.HighQualityBicubic;
//                 g.DrawImage(sourImage, new Rectangle((destWidth - width) / 2, (destHeight - height) / 2, width, height), 0, 0, sourImage.Width, sourImage.Height, GraphicsUnit.Pixel);
//                 g.Dispose();
//                 EncoderParameters encoderParams = new EncoderParameters();
//                 long[] quality = new long[1];
//                 quality[0] = 100;
//                 EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
//                 encoderParams.Param[0] = encoderParam;
//                 sourImage.Dispose();
//                 return destBitmap;
            }
            catch
            {
                return SignatureImg;
            }
        }

        private Bitmap DrawImageAtSpecificRectangle(Bitmap srcBmp, Rectangle targetRect,Size targetSize, bool needEmpty)
        {
            object lockThis = new object();
            lock (lockThis)
            {
                Bitmap targetBmp;

                if (needEmpty)
                {
                    targetBmp = new Bitmap(targetSize.Width, targetSize.Height);
                }
                else
                {
                    targetBmp = SignatureImg.Clone() as Bitmap;
                }

                Graphics g = Graphics.FromImage(targetBmp);
                if (needEmpty)
                {
                    g.Clear(Color.White);
                }
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(srcBmp, targetRect, 0, 0, srcBmp.Width, srcBmp.Height, GraphicsUnit.Pixel);
                g.Dispose();
//                 EncoderParameters encoderParams = new EncoderParameters();
//                 long[] quality = new long[1];
//                 quality[0] = 100;
//                 EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
//                 encoderParams.Param[0] = encoderParam;
                //srcBmp.Dispose();
                return targetBmp;
            }
        }

        private ImagePosition GetRandomPosition()
        {
            List<ImagePosition> emptyPositons = new List<ImagePosition>();
            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0; j < Cols; j++)
                {
                    if(Occupancy[i,j] == 0)
                    {
                        ImagePosition emptyPositon = new ImagePosition();
                        emptyPositon.row = i;
                        emptyPositon.col = j;
                        emptyPositons.Add(emptyPositon);
                    }
                }
            }

            Random random = new Random();
            int r = random.Next(0, emptyPositons.Count-1);
            return emptyPositons[r];
        }
    }
}
