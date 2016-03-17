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

using SEG.Domain.Model;
using SEG.Desktop.Control;
using SEG.Domain.Helpers;
using SEG.Desktop.Windows.Popups;

namespace SEG.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for SEGDiagnosticItem.xaml
    /// </summary>
    public partial class SEGDiagnosticItem : UserControl
    {
        DiagnosticsDetail dI;
        public Item i;

        bool hasDiagnostics = false;
        bool isEditing = false;

        public SEGDiagnosticItem()
        {
            InitializeComponent();
            isEditing = false;
            hasDiagnostics = false;
        }

        public void Prepare(Item ai, double cW)
        {
            i = ai;

            // Visual settings
            Thickness margin = Margin;
            margin.Top = 10;
            margin.Bottom = 10;
            Margin = margin;

            // Place the buttons
            Canvas.SetLeft(cDN, cW - 20 - cDI.Width);
            Canvas.SetLeft(cDI, cW - 20 * 2 - cDI.Width * 2);
            Canvas.SetLeft(cDR, cW - 20 * 3 - cDI.Width * 3);

            double lbx = Canvas.GetLeft(labBlock);
            labBlock.Width = cW - lbx - 20 * 4 - cDI.Width * 3;

            string[] sblocks = i.Code.Split('|');
            if (sblocks.Length > 1)
            {
                labCode.Content = sblocks[0];
                labCode2.Visibility = Visibility.Visible;
                labCode2.Content = sblocks[1];
            }
            else
            {
                labCode.Content = i.Code;
                labCode2.Visibility = Visibility.Hidden;
            }

            Height = 20 + labCode.Height;

            labContent.Text = i.Title;

        }

        public void Initialize(DiagnosticsDetail aDD, double cW)
        {
            if (!isEditing)
            {
                dI = aDD;
                SEG.Domain.Model.Image img = ImageHelper.GetImage(ControlCenter.Instance.segR, dI.ImageId);
                hasDiagnostics = true;

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

                    double lbx = Canvas.GetLeft(labBlock);
                    cDetails.Visibility = Visibility.Visible;
                    cDetails.Width = cW - lbx - 20;
                    rDetails.Width = cDetails.Width;

                    if (dI.ImageId != Guid.Empty)
                    {
                        SEG.Domain.Model.Image diImg = ImageHelper.GetImage(ControlCenter.Instance.segR, dI.ImageId);

                        imgDetails.Visibility = Visibility.Visible;
                        imgDetails.Source = diImg.ThumbDataImage;
                        Canvas.SetLeft(imgDetails, cDetails.Width - 10 - imgDetails.Width);
                        labBlockD.Width = cDetails.Width - 20 - 10 - imgDetails.Width;
                    }
                    else
                    {
                        imgDetails.Visibility = Visibility.Hidden;
                        imgDetails.Source = null;
                        labBlockD.Width = cDetails.Width - 20;
                    }

                    double hD = labBlockD.ActualHeight;
                    if (hD < imgDetails.Width && dI.ImageId != Guid.Empty)
                        hD = imgDetails.Width;
                    cDetails.Height = hD + 40;
                    rDetails.Height = cDetails.Height;

                    labContentD.Text = dI.Comments;
                }

                this.Width = cW;
                rTop.Width = cW - 10;
                this.Height = 20 + labBlock.ActualHeight;
            }
        }

        public void SetSize(double cW)
        {
            if (!isEditing)
            {
                this.Width = cW;
                rTop.Width = cW - 10;

                // Place the buttons
                Canvas.SetLeft(cDN, cW - 20 - cDI.Width);
                Canvas.SetLeft(cDI, cW - 20 * 2 - cDI.Width * 2);
                Canvas.SetLeft(cDR, cW - 20 * 3 - cDI.Width * 3);

                double lbx = Canvas.GetLeft(labBlock);
                labBlock.Width = cW - lbx - 20 * 4 - cDI.Width * 3;

                double h = labBlock.ActualHeight;
                if (hasDiagnostics)
                {
                    if (dI.Status == "I")
                    {
                        Canvas.SetTop(cDetails, 30 + labBlock.ActualHeight);

                        if (dI.ImageId != Guid.Empty)
                        {
                            Canvas.SetLeft(imgDetails, cDetails.Width - 10 - imgDetails.Width);
                            labBlockD.Width = cDetails.Width - 20 - 10 - imgDetails.Width;
                        }
                        else
                        {
                            labBlockD.Width = cDetails.Width - 20;
                        }

                        double hD = labBlockD.ActualHeight;
                        if (hD < imgDetails.Width && dI.ImageId != Guid.Empty)
                            hD = imgDetails.Width;
                        cDetails.Height = hD + 40;
                        rDetails.Width = cDetails.Width;
                        rDetails.Height = cDetails.Height;
                        h = h + cDetails.Height + 15;
                    }
                }

                this.Height = 20 + h;
            }
        }

        private void labBlock_LayoutUpdated(object sender, EventArgs e)
        {
            if (!isEditing && hasDiagnostics)
            {
                double h = labBlock.ActualHeight;
                if (dI.Status == "I")
                {
                    Canvas.SetTop(cDetails, 30 + labBlock.ActualHeight);
                    double hD = labBlockD.ActualHeight;
                    if (hD < imgDetails.Width && dI.ImageId != Guid.Empty)
                        hD = imgDetails.Width;
                    cDetails.Height = hD + 40;
                    rDetails.Width = cDetails.Width;
                    rDetails.Height = cDetails.Height;
                    h = h + cDetails.Height + 15;
                }

                this.Height = 20 + h;
            }
        }

        private void labBlockD_LayoutUpdated(object sender, EventArgs e)
        {
            if (!isEditing && hasDiagnostics)
            {
                double h = labBlock.ActualHeight;
                if (dI.Status == "I")
                {
                    Canvas.SetTop(cDetails, 30 + labBlock.ActualHeight);
                    double hD = labBlockD.ActualHeight;
                    if (hD < imgDetails.Width && dI.ImageId != Guid.Empty)
                        hD = imgDetails.Width;
                    cDetails.Height = hD + 40;
                    rDetails.Width = cDetails.Width;
                    rDetails.Height = cDetails.Height;
                    h = h + cDetails.Height + 15;
                }

                this.Height = 20 + h;
            }
        }

        private void c_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isEditing = true;

            Edit_DiagDetail diag = new Edit_DiagDetail();
            diag.SetSize();
            diag.Initialize(dI.Id);
            //ControlCenter.Instance.wPEditDiagDetail = new Windows.Popups.Edit_DiagDetail();
            //ControlCenter.Instance.wPEditDiagDetail.SetSize();
            //ControlCenter.Instance.wPEditDiagDetail.Initialize(dI.Id);

            if (diag.ShowDialog() == true)
            {
                isEditing = false;
                // Reload this object
                dI = DiagnosticsHelper.GetDiagnosticsDetail(ControlCenter.Instance.segR, dI.Id);
                // And now update
                Update();
            }
            isEditing = false;
        }

        public void Update()
        {
            Initialize(dI, this.Width);
        }
    }
}
