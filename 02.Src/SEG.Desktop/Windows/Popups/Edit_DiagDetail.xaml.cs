using SEG.Desktop.Control;
using SEG.Domain.Helpers;
using SEG.Domain.Model;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SEG.Desktop.Windows.Popups
{
    /// <summary>
    /// Interaction logic for Edit_DiagDetail.xaml
    /// </summary>
    public partial class Edit_DiagDetail : Window
    {
        DiagnosticsDetail dI;
        SEG.Domain.Model.Image img;

        public Edit_DiagDetail()
        {
            InitializeComponent();
            //this.Loaded += EnableKeyboard;
        }

        //private void EnableKeyboard(object sender, RoutedEventArgs e)
        //{
        //    // Enables WPF to mark edit field as supporting text pattern (Automation Concept)
        //    System.Windows.Automation.AutomationElement asForm =
        //        System.Windows.Automation.AutomationElement.FromHandle(new WindowInteropHelper(this).Handle);

        //    // Windows 8 API to enable touch keyboard to monitor for focus tracking in this WPF application
        //    InputPanelConfigurationLib.InputPanelConfiguration inputPanelConfig = new InputPanelConfigurationLib.InputPanelConfiguration();
        //    inputPanelConfig.EnableFocusTracking();
        //}

        public void Initialize(Guid dIid)
        {
            dI = DiagnosticsHelper.GetDiagnosticsDetail(ControlCenter.Instance.segR, dIid);
            img = ImageHelper.GetImage(ControlCenter.Instance.segR, dI.ImageId);

            InnerInitialize();
        }

        public void InnerInitialize()
        {            
            Item item = DiagnosticsHelper.GetItem(ControlCenter.Instance.segR, dI.ItemId);            

            labCode.Content = item.Code.Replace("|","      ");
            labContent.Text = item.Title;

            if (dI.Status == "_new_")
            {
                cDN.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                labDN.Foreground = new SolidColorBrush(Colors.White);

                cDR.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                labDR.Foreground = new SolidColorBrush(Colors.White);

                cDI.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                labDI.Foreground = new SolidColorBrush(Colors.White);

                cDetails.Visibility = Visibility.Hidden;
            }
            else if (dI.Status == "N")
            {
                cDN.Background = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
                labDN.Foreground = new SolidColorBrush(Colors.White);

                cDR.Background = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
                labDR.Foreground = new SolidColorBrush(Colors.Black);

                cDI.Background = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
                labDI.Foreground = new SolidColorBrush(Colors.Black);

                cDetails.Visibility = Visibility.Hidden;
            }
            else if (dI.Status == "R")
            {
                cDR.Background = new SolidColorBrush(Color.FromArgb(255, 0, 85, 110));
                labDR.Foreground = new SolidColorBrush(Colors.White);

                cDN.Background = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
                labDN.Foreground = new SolidColorBrush(Colors.Black);

                cDI.Background = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
                labDI.Foreground = new SolidColorBrush(Colors.Black);

                cDetails.Visibility = Visibility.Hidden;
            }
            else if (dI.Status == "I")
            {
                cDI.Background = new SolidColorBrush(Color.FromArgb(255, 255, 203, 4));
                labDI.Foreground = new SolidColorBrush(Colors.White);

                cDN.Background = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
                labDN.Foreground = new SolidColorBrush(Colors.Black);

                cDR.Background = new SolidColorBrush(Color.FromArgb(255, 204, 204, 204));
                labDR.Foreground = new SolidColorBrush(Colors.Black);

                cDetails.Visibility = Visibility.Visible;

                txtComments.Text = dI.Comments;
                if (img == null)
                    imgDetails.Source = new BitmapImage(new Uri("pack://application:,,,/Media/camera_image.png"));
                else
                    imgDetails.Source = img.ThumbDataImage;
            }
        }

        public void SetSize()
        {
            // This routine sets up the popup size based on the screen size
            double sW = ControlCenter.Instance.wMain.Width;
            double sH = ControlCenter.Instance.wMain.Height;
            double sX = ControlCenter.Instance.wMain.Left;
            double sY = ControlCenter.Instance.wMain.Top;

            double w = sW * 0.75; // 70% of main window
            double h = sH * 0.85;

            double x = sX + (sW - w) / 2;
            double y = sY + (sH - h) / 2;

            this.Width = w;
            this.Height = h;

            rBorder.Width = w - 6;
            rBorder.Height = h - 6;

            this.Left = x;
            this.Top = y;

            // Now setup the different widths
            double lbx = Canvas.GetLeft(labBlock);
            labBlock.Width = w - lbx - 26;
            rTop.Width = w - 32;

            rSep1.Width = w - lbx - 26;
            rSep2.Width = w - lbx - 26;

            cDetails.Width = w - lbx - 26;
            rDetails.Width = cDetails.Width;
            txtComments.Width = cDetails.Width - 30 - 190;
            Canvas.SetLeft(imgDetails, cDetails.Width - 190 - 10);

            // Buttons
            double w2 = (w - cDI.Width * 3 - 20 * 2)/2;
            Canvas.SetLeft(cDR, w2);
            Canvas.SetLeft(cDI, w2 + cDI.Width + 20);
            Canvas.SetLeft(cDN, w2 + cDI.Width * 2 + 20 * 2);

            // Bottom
            double yB = h - cbCancel.Height - 26;
            Canvas.SetTop(rBottom, yB);
            rBottom.Width = w - 32;
            Canvas.SetTop(cbCancel, yB + 10);
        }

        private void cbCancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void cDR_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Change the status of a DiagnosticItem
            DiagnosticsHelper.ModifyDetails(ControlCenter.Instance.segR, dI.Id, "R", "", Guid.Empty);

            DialogResult = true;
            this.Close();
        }

        private void cDI_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dI.Status = "I";
            InnerInitialize();
        }

        private void cDN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DiagnosticsHelper.ModifyDetails(ControlCenter.Instance.segR, dI.Id, "N", "", Guid.Empty);
            DialogResult = true;
            this.Close();
        }

        private void cbOkDetails_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(img!=null)
                DiagnosticsHelper.ModifyDetails(ControlCenter.Instance.segR, dI.Id, "I", txtComments.Text, img.Id);
            else
                DiagnosticsHelper.ModifyDetails(ControlCenter.Instance.segR, dI.Id, "I", txtComments.Text, Guid.Empty);
            DialogResult = true;
            this.Close();
        }

        private void imgDetails_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Edit_CaptureImage capImg = new Edit_CaptureImage();
            capImg.SetSize();
            if(capImg.ShowDialog() == true)
            {
                // Store the image back
                // First, we should delete the existing image
                // Then we should add the new one
                // And finally assign it
                dI.Status = "I";
                img = ImageHelper.SaveImage(ControlCenter.Instance.segR, ControlCenter.Instance.imgPopResult);
                ImageHelper.FillImage(img);

                InnerInitialize();
            }
        }
    }
}
