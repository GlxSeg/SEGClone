using AForge.Video.DirectShow;
using SEG.Desktop.Control;
using SEG.Domain.Helpers;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TakeSnapsWithWebcamUsingWpfMvvm.Video;

namespace SEG.Desktop.Windows.Popups
{
    /// <summary>
    /// Interaction logic for Edit_CaptureImage.xaml
    /// </summary>
    public partial class Edit_CaptureImage : Window
    {
        public VideoCapabilities vres;
        public string mode;
        public bool fromFile;

        public bool isBusy;
        public bool hasCamera;


        public Edit_CaptureImage()
        {
            InitializeComponent();
            mode = "capture";
            fromFile = false;
            isBusy = false;

            hasCamera = false;
        }


        public void SetSize()
        {
            // This routine sets up the popup size based on the screen size
            double sW = ControlCenter.Instance.wMain.Width;
            double sH = ControlCenter.Instance.wMain.Height;
            double sX = ControlCenter.Instance.wMain.Left;
            double sY = ControlCenter.Instance.wMain.Top;

            double x = (sW - Width) / 2.0; // sX + (sW - Width) / 2;
            double y = sY + (sH - Height) / 2;

            this.Left = x;
            this.Top = y;
        }


        public void Start()
        {
            // Turn on flash
            isBusy = true;

            try
            {

                // Webcam stuff
                int idx = -1;
                List<MediaInformation> media = WebcamDevice.GetVideoDevices.ToList<MediaInformation>();
                VideoCapabilities[] cap = null;

                if (media.Count > 0)
                {
                    cam.VideoSourceId = media[0].UsbId;
                    cap = (cam.VideoSourcePlayer.VideoSource as VideoCaptureDevice).VideoCapabilities;
                    for (int i = 0; i < cap.Length; i++)
                        if (idx == -1 || (Math.Abs(cap[idx].FrameSize.Height - 600) > Math.Abs(cap[i].FrameSize.Height - 600)))
                            idx = i;
                }


                if (idx < 0)
                {
                    hasCamera = false;
                    vres = null;

                    cam.Visibility = Visibility.Visible;
                    cropArea.Visibility = Visibility.Hidden;
                    cropArea.Width = cam.Width;
                    imgSnap.Width = cam.Width;

                    ControlCenter.Instance.imgPop_snapH = cam.Height;
                    ControlCenter.Instance.imgPop_snapW = cam.Width;

                    cbRetake.Visibility = Visibility.Hidden;

                    rB1_0.Visibility = Visibility.Hidden;
                    rB1_1.Visibility = Visibility.Hidden;
                    rB1_2.Visibility = Visibility.Hidden;
                    rB2_0.Visibility = Visibility.Visible;
                    rB2_1.Visibility = Visibility.Visible;
                    rB2_2.Visibility = Visibility.Visible;

                    labB1.Foreground = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
                    labB2.Foreground = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
                    imgB1.Source = new BitmapImage(new Uri("pack://application:,,,/Media/camera-inactive-btn.png"));
                    imgB2.Source = new BitmapImage(new Uri("pack://application:,,,/Media/crop-inactive-btn.png"));
                }
                else
                {
                    hasCamera = true;
                    vres = cap[idx];

                    cam.Visibility = Visibility.Visible;
                    cropArea.Visibility = Visibility.Hidden;
                    cropArea.Width = cam.Width;
                    imgSnap.Width = cam.Width;

                    ControlCenter.Instance.imgPop_snapH = cam.Height;
                    ControlCenter.Instance.imgPop_snapW = cam.Width;

                    cbRetake.Visibility = Visibility.Hidden;

                    rB1_0.Visibility = Visibility.Hidden;
                    rB1_1.Visibility = Visibility.Hidden;
                    rB1_2.Visibility = Visibility.Hidden;
                    rB2_0.Visibility = Visibility.Visible;
                    rB2_1.Visibility = Visibility.Visible;
                    rB2_2.Visibility = Visibility.Visible;

                    labB1.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    labB2.Foreground = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
                    imgB1.Source = new BitmapImage(new Uri("pack://application:,,,/Media/camera-active-btn.png"));
                    imgB2.Source = new BitmapImage(new Uri("pack://application:,,,/Media/crop-inactive-btn.png"));
                }

                // Calculate width and resize accordingly
                //double vw = (cam.Height / vres.FrameSize.Height) * vres.FrameSize.Width;
                //cam.Width = vw;

            }
            catch (Exception ex)
            {
                LogError(ex);
            }


            isBusy = false;
        }

        private void cbCancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isBusy)
            {
                DialogResult = false;
                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void imgB1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mode == "capture" && !isBusy && hasCamera)
            {
                isBusy = true;

                try
                {

                    cam.TakeSnapshotCallback();

                    //cam.SnapshotBitmap.Save("snapshot-webcam.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                    MemoryStream ms = new MemoryStream();
                    cam.SnapshotBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    System.Windows.Media.Imaging.BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();
                    bImg.BeginInit();
                    bImg.StreamSource = new MemoryStream(ms.ToArray());
                    bImg.EndInit();
                    cam.VideoSourcePlayer.Stop();

                    imgSnap.Source = bImg;
                    cropArea.Visibility = Visibility.Visible;
                    cam.Visibility = Visibility.Hidden;

                    Canvas.SetLeft(DesignerItem, (imgSnap.Width - imgSnap.Height) / 2);
                    Canvas.SetTop(DesignerItem, 0);
                    DesignerItem.Width = imgSnap.Height;
                    DesignerItem.Height = imgSnap.Height;

                    fromFile = false;

                    rB1_0.Visibility = Visibility.Visible;
                    rB1_1.Visibility = Visibility.Visible;
                    rB1_2.Visibility = Visibility.Visible;
                    rB2_0.Visibility = Visibility.Hidden;
                    rB2_1.Visibility = Visibility.Hidden;
                    rB2_2.Visibility = Visibility.Hidden;

                    cbRetake.Visibility = Visibility.Visible;

                    labB2.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    labB1.Foreground = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
                    imgB1.Source = new BitmapImage(new Uri("pack://application:,,,/Media/camera-inactive-btn.png"));
                    imgB2.Source = new BitmapImage(new Uri("pack://application:,,,/Media/crop-active-btn.png"));
                    mode = "crop";

                }
                catch (Exception ex)
                {
                    LogError(ex);
                }

                isBusy = false;
            }
        }

        private void cbRetake_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isBusy && hasCamera)
            {
                isBusy = true;

                try
                {

                cropArea.Visibility = Visibility.Hidden;
                cam.VideoSourcePlayer.Start();
                cam.Visibility = Visibility.Visible;

                cbRetake.Visibility = Visibility.Hidden;

                rB1_0.Visibility = Visibility.Hidden;
                rB1_1.Visibility = Visibility.Hidden;
                rB1_2.Visibility = Visibility.Hidden;
                rB2_0.Visibility = Visibility.Visible;
                rB2_1.Visibility = Visibility.Visible;
                rB2_2.Visibility = Visibility.Visible;

                labB1.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                labB2.Foreground = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
                imgB1.Source = new BitmapImage(new Uri("pack://application:,,,/Media/camera-active-btn.png"));
                imgB2.Source = new BitmapImage(new Uri("pack://application:,,,/Media/crop-inactive-btn.png"));

                mode = "capture";

                }
                catch (Exception ex)
                {
                    LogError(ex);
                }

                isBusy = false;
            }
        }

        public BitmapImage ReturnImage()
        {
            try
            {

                Matrix m =
                PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice;
                double dx = m.M11;
                double dy = m.M22;

                if (fromFile)
                {
                    dx = 1.0;
                    dy = 1.0;
                }

                StreamWriter sw = new StreamWriter("crop-info.txt");
                sw.WriteLine(dx);
                sw.WriteLine(dy);
                sw.WriteLine(imgSnap.Width);
                sw.WriteLine(imgSnap.Height);
                sw.WriteLine(imgSnap.Source.Width);
                sw.WriteLine(imgSnap.Source.Height);
                sw.WriteLine(Convert.ToInt32(Canvas.GetLeft(DesignerItem) * dx));
                sw.WriteLine(Convert.ToInt32(Canvas.GetTop(DesignerItem) * dy));
                sw.WriteLine(Convert.ToInt32(DesignerItem.Width * dx));
                sw.WriteLine(Convert.ToInt32(DesignerItem.Height * dy));

                // We need to crop and close
                CroppedBitmap cb = new CroppedBitmap((BitmapSource)imgSnap.Source,
                    new Int32Rect(Convert.ToInt32(Canvas.GetLeft(DesignerItem) * dx),
                                  Convert.ToInt32(Canvas.GetTop(DesignerItem) * dy),
                                  Convert.ToInt32(DesignerItem.Width * dx),
                                  Convert.ToInt32(DesignerItem.Height * dy)));

                MemoryStream mStream = new MemoryStream();
                PngBitmapEncoder pEncoder = new PngBitmapEncoder();
                pEncoder.Frames.Add(BitmapFrame.Create(cb));  //the croppedBitmap is a CroppedBitmap object 
                pEncoder.Save(mStream);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = mStream;
                image.EndInit();

                return image;            
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        private void imgB2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isBusy)
            {
                try
                {
                    ControlCenter.Instance.imgPopResult = ReturnImage();
                    DialogResult = true;
                    Close();

                }
                catch (Exception ex)
                {
                    LogError(ex);
                }

            }
        }

        private void cbUpload_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isBusy)
            {
                isBusy = true;

                try
                {

                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                    dlg.DefaultExt = ".jpg";
                    dlg.Filter = "Image Files (*.jpeg,*.jpg,*.png)|*.jpeg;*.png;*.jpg";

                    cam.VideoSourcePlayer.Stop();
                    imgSnap.Source = null;
                    cropArea.Visibility = Visibility.Visible;
                    cam.Visibility = Visibility.Hidden;

                    Nullable<bool> result = dlg.ShowDialog();

                    // Get the selected file name and display in a TextBox 
                    if (result == true)
                    {
                        // Open document 
                        string filename = dlg.FileName;

                        // Load the image and crop it if needed
                        System.Drawing.Bitmap bOrig = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(filename);

                        // Check aspect ratio
                        double cA = (bOrig.Height * 1.0) / (bOrig.Width * 1.0);
                        double fA = imgSnap.Height / imgSnap.Width;

                        System.Drawing.Bitmap img2;
                        if (cA > fA)
                        {
                            int idx = (bOrig.Height - Convert.ToInt32(bOrig.Width * fA)) / 2;
                            img2 = new System.Drawing.Bitmap(bOrig.Width, bOrig.Height - 2 * idx, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                            img2.SetResolution(bOrig.HorizontalResolution, bOrig.VerticalResolution);
                            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img2);
                            g.DrawImage(bOrig, 0, -idx);
                        }
                        else
                        {
                            int idx = (bOrig.Width - Convert.ToInt32(bOrig.Height / fA)) / 2;
                            img2 = new System.Drawing.Bitmap(bOrig.Width - 2 * idx, bOrig.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                            img2.SetResolution(bOrig.HorizontalResolution, bOrig.VerticalResolution);
                            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img2);
                            g.DrawImage(bOrig, -idx, 0);
                        }

                        BitmapSource bs = ImageHelper.Bitmap2BitmapSource(img2);
                        var bitmap = new TransformedBitmap(bs,
                             new ScaleTransform(imgSnap.Width / bs.PixelWidth,
                                                imgSnap.Height / bs.PixelHeight));

                        fromFile = true;

                        imgSnap.Source = bitmap;
                        cropArea.Visibility = Visibility.Visible;
                        cam.Visibility = Visibility.Hidden;

                        Canvas.SetLeft(DesignerItem, (imgSnap.Width - imgSnap.Height) / 2);
                        Canvas.SetTop(DesignerItem, 0);
                        DesignerItem.Width = imgSnap.Height;
                        DesignerItem.Height = imgSnap.Height;

                        rB1_0.Visibility = Visibility.Visible;
                        rB1_1.Visibility = Visibility.Visible;
                        rB1_2.Visibility = Visibility.Visible;
                        rB2_0.Visibility = Visibility.Hidden;
                        rB2_1.Visibility = Visibility.Hidden;
                        rB2_2.Visibility = Visibility.Hidden;

                        cbRetake.Visibility = Visibility.Visible;

                        labB2.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                        labB1.Foreground = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
                        imgB1.Source = new BitmapImage(new Uri("pack://application:,,,/Media/camera-inactive-btn.png"));
                        imgB2.Source = new BitmapImage(new Uri("pack://application:,,,/Media/crop-active-btn.png"));
                        mode = "crop";
                    }
                    else
                    {
                        cropArea.Visibility = Visibility.Hidden;
                        cam.VideoSourcePlayer.Start();
                        cam.Visibility = Visibility.Visible;
                    }

                }
                catch (Exception ex)
                {
                    LogError(ex);
                }

                isBusy = false;
            }
        }

        public void LogError(Exception ex)
        {
            StreamWriter sw = new StreamWriter("capture-error-" + DateTime.Now.ToString("yyyyMMdd_hhmm") + ".txt");
            sw.WriteLine(ex.ToString());
            sw.Close();

            MessagePopup mp = new MessagePopup();
            mp.Setup("Webcam Capture", "Uhmmm.... we had a software crash.  Please inform the developer.  You can retry the capture if you want.  Really. It's fine, go ahead.", true);
            mp.SetSize();
            mp.ShowDialog();
            Close();
        }
    }
}
