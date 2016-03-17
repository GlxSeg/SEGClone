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
using SEG.Desktop.Control;
using SEG.Desktop.UserControls;
using SEG.Domain.Model;
using SEG.Domain.Helpers;
using SEG.Desktop.Windows.Popups;
using System.IO;

namespace SEG.Desktop.Windows.Panels
{
    /// <summary>
    /// Interaction logic for Asset_Edit_Mechanical_Img.xaml
    /// </summary>
    public partial class Asset_Edit_Mechanical_Img : UserControl
    {
        public bool isPrepared = false;
        public bool hasElectrical = false;
        public List<ImageHolder> imgHolders;
        public ImageHolder imgHolderSel;

        public Diagnostics dm;

        public Asset_Edit_Mechanical_Img()
        {
            InitializeComponent();
        }

        public void Update(double estimatedWidth)
        {
            // First, clean all the Children of the panel
            sPanel.Children.Clear();

            // Find the mechanical diagnostics
            dm = DiagnosticsHelper.GetMechanicalDiagnostics(ControlCenter.Instance.segR,
                                                                       ControlCenter.Instance.cAssetId);

            List<Diagnostics> electrical = DiagnosticsHelper.GetElectricalDiagnostics(ControlCenter.Instance.segR,
                                                                                      ControlCenter.Instance.cAssetId);

            if (electrical.Count > 0)
            {
                hasElectrical = true;
                labName_S3_0.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                hasElectrical = false;
                labName_S3_0.Foreground = new SolidColorBrush(Colors.Gray);
            }

            // Obtain all the images for the Diagnostics
            List<SEG.Domain.Model.Image> imgs = DiagnosticsHelper.GetDiagnosticsImages(ControlCenter.Instance.segR, dm.Id);
            imgHolders = new List<ImageHolder>();
            foreach(SEG.Domain.Model.Image img in imgs)
            {
                ImageHolder ih = new ImageHolder();
                ih.Width = 180;
                ih.Height = 180;
                ih.Initialize(img);
                sPanel.Children.Add(ih);
                imgHolders.Add(ih);
            }

            imgHolderSel = null;
            cbDelete.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
            lbDelete.Foreground = new SolidColorBrush(Colors.Silver);
            cbSet.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
            lbSet.Foreground = new SolidColorBrush(Colors.Silver);

            if(imgHolders.Count>0)
                SelectImage(imgHolders[0]);
            else
            {
                iMain.Source = null;
            }
        }

        public void SelectImage(ImageHolder ih)
        { 
            if(imgHolderSel!=null)
            {
                imgHolderSel.isSelected = false;
                imgHolderSel.Update();
            }

            imgHolderSel = ih;
            ih.isSelected = true;
            ih.Update();

            if (imgHolderSel.isAssetImg)
            {
                cbDelete.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                lbDelete.Foreground = new SolidColorBrush(Colors.Silver);
                cbSet.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                lbSet.Foreground = new SolidColorBrush(Colors.Silver);
            }
            else
            {
                cbDelete.Background = new SolidColorBrush(Colors.Silver);
                lbDelete.Foreground = new SolidColorBrush(Colors.Black);
                cbSet.Background = new SolidColorBrush(Colors.Silver);
                lbSet.Foreground = new SolidColorBrush(Colors.Black);
            }


            // Now update the big image
            iMain.Source = ih.i.DataImage;
        }

        public void SetSize(double cW, double cH)
        {
            this.Width = cW;
            this.Height = cH;

            cSubMenu.Width = cW;

            cSubMenu2.Width = cW;
            Canvas.SetLeft(cbRemove, cW - cbRemove.Width - 20);
            double smw = cW - cbRemove.Width - 40;

            cSubMenu2_1.Width = smw / 2;
            labName_2_1.Width = smw / 2;
            cSubMenu2_2.Width = smw / 2;
            labName_2_2.Width = smw / 2;

            Canvas.SetLeft(cSubMenu2_2, smw / 2);
            Canvas.SetLeft(rcSubMenu, smw + 10);

            double svl = Canvas.GetLeft(scrollView);
            scrollView.Width = cW - svl - 30;
            double svt = Canvas.GetTop(scrollView);
            scrollView.Height = cH - svt - 20;

            double rX = Canvas.GetLeft(rcButtons);
            rcButtons.Width = cW - rX - 20;
            rcImages.Width = cW - rX - 20;
            double rY = Canvas.GetTop(rcImages);
            rcImages.Height = cH - rY - 10;
        }

        public void UpdateData()
        {

        }

        private void cSubMenu_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Asset_SelectGeneral();
        }

        private void cSubMenu_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(hasElectrical)
                ControlCenter.Instance.Asset_SelectElectrical();
        }

        private void cSubMenu2_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Asset_SelectMechanical();
        }

        private void cbNew_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Edit_CaptureImage capImg = new Edit_CaptureImage();
                capImg.SetSize();
                if (capImg.ShowDialog() == true)
                {
                    // Store the image back
                    // First, we should delete the existing image
                    // Then we should add the new one
                    // And finally assign it
                    try
                    {
                        SEG.Domain.Model.Image img = ImageHelper.SaveImage(ControlCenter.Instance.segR, ControlCenter.Instance.imgPopResult);
                        ImageHelper.FillImage(img);
                        DiagnosticsHelper.SaveDiagnosticsImage(ControlCenter.Instance.segR, dm.Id, img.Id);

                        ImageHolder ih = new ImageHolder();
                        ih.Width = 180;
                        ih.Height = 180;
                        ih.Initialize(img);
                        sPanel.Children.Add(ih);
                        imgHolders.Add(ih);

                        SelectImage(ih);
                    }
                    catch (Exception ex)
                    {
                        StreamWriter sw = new StreamWriter("capture-error.txt");
                        sw.WriteLine(ex.ToString());
                        sw.Close();
                    }
                }
            }
            catch (Exception ex2)
            {
                StreamWriter sw = new StreamWriter("capture-error.txt");
                sw.WriteLine(ex2.ToString());
                sw.Close();
            }
        }

        private void cbSet_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(imgHolderSel!=null)
            {                
                // We set the asset's image
                ProjectHelper.SetAssetImage(ControlCenter.Instance.segR, ControlCenter.Instance.cAssetId, imgHolderSel.i.Id);

                // Reload the asset
                ControlCenter.Instance.cAsset = ProjectHelper.GetAsset(ControlCenter.Instance.segR,
                                                                       ControlCenter.Instance.cAssetId);
                ControlCenter.Instance.cAssetImage = ImageHelper.GetImage(ControlCenter.Instance.segR,
                                                                          ControlCenter.Instance.cAsset.ImageId);
                ControlCenter.Instance.wMain.segM.Update();

                foreach (ImageHolder ih in imgHolders)
                    ih.Update();

                SelectImage(imgHolderSel);
            }
        }

        private void cbDelete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (imgHolderSel != null && ControlCenter.Instance.cAsset.ImageId!=imgHolderSel.i.Id)
            {
                MessagePopup mp = new MessagePopup();
                mp.Setup("Diagnostics Image", "Are you sure you want to delete the selected Image?", true);
                mp.SetSize();
                if(mp.ShowDialog()==true)
                {
                    // First, obtain the ID
                    Guid id = imgHolderSel.i.Id;
                    DiagnosticsHelper.DeleteDiagnosticsImage(ControlCenter.Instance.segR, dm.Id, id);
                    
                    // If the asset is affected, do so
                    Update(this.Width);
                }
            }
        }


        private void cbDownload_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (imgHolderSel != null)
            {
                // Configure save file dialog box
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "image"; // Default file name
                dlg.DefaultExt = ".jpg"; // Default file extension
                dlg.Filter = "JPEG Image (.jpg)|*.jpg"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string filename = dlg.FileName;

                    // Save to file
                    System.Drawing.Image img = ImageHelper.BytesToImage(imgHolderSel.i.Data);
                    try
                    {
                        img.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }



        private void cSubMenu_4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Asset_SelectRisk();
        }
    }
}
