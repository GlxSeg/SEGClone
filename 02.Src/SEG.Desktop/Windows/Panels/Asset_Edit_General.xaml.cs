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
using SEG.Domain.Printing;

namespace SEG.Desktop.Windows.Panels
{
    /// <summary>
    /// Interaction logic for Asset_Edit_General.xaml
    /// </summary>
    public partial class Asset_Edit_General : UserControl
    {
        public string mode;
        public bool hasMechanical;
        public bool hasElectrical;
        List<AssetInfo> assetInfo;

        public Asset_Edit_General()
        {
            InitializeComponent();
            mode = "view";
        }

        public void Update()
        {
            // Freshen up data for the asset in the ControlCenter
            ControlCenter.Instance.cAsset = ProjectHelper.GetAsset(ControlCenter.Instance.segR,
                                                                   ControlCenter.Instance.cAssetId);

            assetInfo = ProjectHelper.GetAssetInfo(ControlCenter.Instance.segR,
                                                                   ControlCenter.Instance.cAssetId);

            SEG.Domain.Model.Image img = ImageHelper.GetImage(ControlCenter.Instance.segR,
                                                              ControlCenter.Instance.cAsset.ImageId);
            
            eID.Text = assetInfo.FirstOrDefault(x => x.Key == "ID").Value;
            eTAG.Text = assetInfo.FirstOrDefault(x => x.Key == "TAG").Value;
            eLocation.Text = assetInfo.FirstOrDefault(x => x.Key == "LOCATION").Value;
            eDescrip.Text = assetInfo.FirstOrDefault(x => x.Key == "DESCRIPTION").Value;
            hasMechanical = (assetInfo.FirstOrDefault(x => x.Key == "MECHDIAG").Value == "YES");

            mode = "view";

            eID.IsEnabled = false;
            eTAG.IsEnabled = false;
            eLocation.IsEnabled = false;
            eDescrip.IsEnabled = false;

            // Set buttons for mode
            cbEditAI.Background = new SolidColorBrush(Colors.Silver);
            cbSaveAI.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
            cbCancelAI.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
            lbEditAI.Foreground = new SolidColorBrush(Colors.Black);
            lbSaveAI.Foreground = new SolidColorBrush(Colors.Silver);
            lbCancelAI.Foreground = new SolidColorBrush(Colors.Silver);

            if(img!=null)
            {
                iMain.Source = img.DataImage;
            }
            else
            {
                iMain.Source = null;
            }

            Diagnostics mechanical = DiagnosticsHelper.GetMechanicalDiagnostics(ControlCenter.Instance.segR,
                                                                                ControlCenter.Instance.cAssetId);            

            List<Diagnostics> electrical = DiagnosticsHelper.GetElectricalDiagnostics(ControlCenter.Instance.segR,
                                                                                      ControlCenter.Instance.cAssetId);

            if(electrical.Count>0)
            {
                hasElectrical = true;
                labElecN.Content = electrical.Count.ToString();
                labName_S3_0.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                hasElectrical = false;
                labElecN.Content = "0";
                labName_S3_0.Foreground = new SolidColorBrush(Colors.Gray);
            }

            if(hasMechanical)
            {
                bcMechanicalYes.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                bcMechanicalNo.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                labMechYes.Foreground = new SolidColorBrush(Colors.White);
                labMechNo.Foreground = new SolidColorBrush(Colors.Gray);
                labName_S2.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                bcMechanicalNo.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                bcMechanicalYes.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                labMechYes.Foreground = new SolidColorBrush(Colors.Gray);
                labMechNo.Foreground = new SolidColorBrush(Colors.White);
                labName_S2.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        public void SetSize(double cW, double cH)
        {
            this.Width = cW;
            this.Height = cH;

            Canvas.SetLeft(cImgMain, cW - cImgMain.Width - 20);
            //Canvas.SetLeft(labAssetImg, cW - cImgMain.Width - 20);

            rcAssetDiags.Width = cW - cImgMain.Width - 20 - 40;
            rcAssetInfo.Width = cW - cImgMain.Width - 20 - 40;

            double cx0 = Canvas.GetLeft(eID);

            eID.Width = rcAssetInfo.Width - cx0;
            eTAG.Width = rcAssetInfo.Width - cx0;
            eLocation.Width = rcAssetInfo.Width - cx0;
            eDescrip.Width = rcAssetInfo.Width - cx0;

            cSubMenu.Width = cW;
        }

        private void cSubMenu_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (hasMechanical)
            {
                ControlCenter.Instance.Asset_SelectMechanical();
            }
        }

        public void UpdateData()
        {

        }

        private void cbEditAI_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mode == "view")
            {
                mode = "edit";

                eID.IsEnabled = true;
                eTAG.IsEnabled = true;
                eLocation.IsEnabled = true;
                eDescrip.IsEnabled = true;

                // Set buttons for mode
                cbEditAI.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                cbSaveAI.Background = new SolidColorBrush(Colors.Silver);
                cbCancelAI.Background = new SolidColorBrush(Colors.Silver);
                lbEditAI.Foreground = new SolidColorBrush(Colors.Silver);
                lbSaveAI.Foreground = new SolidColorBrush(Colors.Black);
                lbCancelAI.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void cbSaveAI_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(mode=="edit")
            {
                mode = "view";

                ProjectHelper.ModifyAssetInfo(ControlCenter.Instance.segR,
                                              ControlCenter.Instance.cAssetId,
                                              "ID", eID.Text);

                ProjectHelper.ModifyAssetInfo(ControlCenter.Instance.segR,
                                              ControlCenter.Instance.cAssetId,
                                              "TAG", eTAG.Text);

                ProjectHelper.ModifyAssetInfo(ControlCenter.Instance.segR,
                                              ControlCenter.Instance.cAssetId,
                                              "LOCATION", eLocation.Text);

                ProjectHelper.ModifyAssetInfo(ControlCenter.Instance.segR,
                                              ControlCenter.Instance.cAssetId,
                                              "DESCRIPTION", eDescrip.Text);

                eID.IsEnabled = false;
                eTAG.IsEnabled = false;
                eLocation.IsEnabled = false;
                eDescrip.IsEnabled = false;

                // Set buttons for mode
                cbEditAI.Background = new SolidColorBrush(Colors.Silver);
                cbSaveAI.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                cbCancelAI.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                lbEditAI.Foreground = new SolidColorBrush(Colors.Black);
                lbSaveAI.Foreground = new SolidColorBrush(Colors.Silver);
                lbCancelAI.Foreground = new SolidColorBrush(Colors.Silver);

            }
        }

        private void cbCancelAI_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mode == "edit")
            {
                mode = "view";

                List<AssetInfo> assetInfo = ProjectHelper.GetAssetInfo(ControlCenter.Instance.segR,
                                                                       ControlCenter.Instance.cAssetId);

                eID.Text = assetInfo.FirstOrDefault(x => x.Key == "ID").Value;
                eTAG.Text = assetInfo.FirstOrDefault(x => x.Key == "TAG").Value;
                eLocation.Text = assetInfo.FirstOrDefault(x => x.Key == "LOCATION").Value;
                eDescrip.Text = assetInfo.FirstOrDefault(x => x.Key == "DESCRIPTION").Value;

                eID.IsEnabled = false;
                eTAG.IsEnabled = false;
                eLocation.IsEnabled = false;
                eDescrip.IsEnabled = false;

                // Set buttons for mode
                cbEditAI.Background = new SolidColorBrush(Colors.Silver);
                cbSaveAI.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                cbCancelAI.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                lbEditAI.Foreground = new SolidColorBrush(Colors.Black);
                lbSaveAI.Foreground = new SolidColorBrush(Colors.Silver);
                lbCancelAI.Foreground = new SolidColorBrush(Colors.Silver);

            }
        }

        private void labName_S3_0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (hasElectrical)
            {
                ControlCenter.Instance.Asset_SelectElectrical();
            }
        }

        private void bcElec_Add_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DiagnosticsHelper.AddElectricalDiagnostics(ControlCenter.Instance.segR, 
                                                       ControlCenter.Instance.cAssetId);

            List<Diagnostics> electrical = DiagnosticsHelper.GetElectricalDiagnostics(ControlCenter.Instance.segR,
                                                                                      ControlCenter.Instance.cAssetId);

            hasElectrical = true;
            labName_S3_0.Foreground = new SolidColorBrush(Colors.White);
            labElecN.Content = electrical.Count.ToString();
        }

        private void cSubMenu_4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Asset_SelectRisk();
        }

        private void cbCopyToAsset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var s = new SelectAssetPopup();
            s.Setup();
            s.SetSize();
            if(s.ShowDialog() == true)
            {
                if(ControlCenter.Instance.cCopyToAssetId == ControlCenter.Instance.cAssetId)
                {
                    MessagePopup mp = new MessagePopup();
                    mp.Setup("Asset Diagnostics", "The source and destination assets cannot be the same", false);
                    mp.SetSize();
                    mp.ShowDialog();
                }
                else
                {
                    var elecF = DiagnosticsHelper.GetElectricalDiagnostics(ControlCenter.Instance.segR, 
                                                                           ControlCenter.Instance.cAssetId);

                    var elecT = DiagnosticsHelper.GetElectricalDiagnostics(ControlCenter.Instance.segR,
                                                                           ControlCenter.Instance.cCopyToAssetId);
                    if(elecF.Count != elecT.Count)
                    {
                        MessagePopup mp = new MessagePopup();
                        mp.Setup("Asset Diagnostics", "The source and destination Assets must have the same number of Electrical Diagnostics", false);
                        mp.SetSize();
                        mp.ShowDialog();
                    }
                    else
                    {
                        MessagePopup mp = new MessagePopup();
                        mp.Setup("Copy Diagnostics", "Are you sure you want to overwrite the diagnostics in the destination Asset?", true);
                        mp.SetSize();
                        if (mp.ShowDialog() == true)
                        {
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            // We copy the diagnostics
                            ProjectHelper.CopyDiagnosticsToAsset(ControlCenter.Instance.segR,
                                ControlCenter.Instance.cAssetId, ControlCenter.Instance.cCopyToAssetId);
                            Mouse.OverrideCursor = null;

                            mp = new MessagePopup();
                            mp.Setup("Asset Diagnostics", "The diagnostics were copied to the destination Asset", false);
                            mp.SetSize();
                            mp.ShowDialog();
                        }
                    }
                }
            }
        }

        private void cbDeleteAsset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessagePopup mp = new MessagePopup();
            mp.Setup("Delete Asset", "Are you sure you want to delete the Asset?", true);
            mp.SetSize();
            if (mp.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                // We copy the diagnostics
                ProjectHelper.DeleteAsset(ControlCenter.Instance.segR, ControlCenter.Instance.cAssetId);
                Mouse.OverrideCursor = null;

                mp = new MessagePopup();
                mp.Setup("Asset", "The Asset was removed from the system", false);
                mp.SetSize();
                mp.ShowDialog();

                ControlCenter.Instance.SelectArea(ControlCenter.Instance.cProjectAreaId);
            }
        }

        private void cbPrintAsset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "asset_report_" + assetInfo.FirstOrDefault(x => x.Key == "ID").Value; // Default file name
            dlg.DefaultExt = ".pdf"; // Default file extension
            dlg.Filter = "PDF File (.pdf)|*.pdf"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;


                SEGPrinter.PrintAsset(ControlCenter.Instance.segR,
                                      ControlCenter.Instance.cAssetId,
                                      ControlCenter.Instance.cProjectAreaId,
                                      ControlCenter.Instance.cProjectId,
                                      filename);

                Mouse.OverrideCursor = null;

                MessagePopup mp = new MessagePopup();
                mp.Setup("Asset", "The asset report was generated successfully", false);
                mp.SetSize();
                mp.ShowDialog();

            }
        }

        private void cbMechDiagYes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(!hasMechanical)
            {

                hasMechanical = true;
                ProjectHelper.ModifyAssetInfo(ControlCenter.Instance.segR,
                                              ControlCenter.Instance.cAssetId,
                                              "MECHDIAG", "YES");

                if (hasMechanical)
                {
                    bcMechanicalYes.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                    bcMechanicalNo.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                    labMechYes.Foreground = new SolidColorBrush(Colors.White);
                    labMechNo.Foreground = new SolidColorBrush(Colors.Gray);
                    labName_S2.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    bcMechanicalNo.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                    bcMechanicalYes.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                    labMechYes.Foreground = new SolidColorBrush(Colors.Gray);
                    labMechNo.Foreground = new SolidColorBrush(Colors.White);
                    labName_S2.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        private void cbMechDiagNo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (hasMechanical)
            {

                hasMechanical = false;
                ProjectHelper.ModifyAssetInfo(ControlCenter.Instance.segR,
                                              ControlCenter.Instance.cAssetId,
                                              "MECHDIAG", "NO");

                if (hasMechanical)
                {
                    bcMechanicalYes.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                    bcMechanicalNo.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                    labMechYes.Foreground = new SolidColorBrush(Colors.White);
                    labMechNo.Foreground = new SolidColorBrush(Colors.Gray);
                    labName_S2.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    bcMechanicalNo.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                    bcMechanicalYes.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                    labMechYes.Foreground = new SolidColorBrush(Colors.Gray);
                    labMechNo.Foreground = new SolidColorBrush(Colors.White);
                    labName_S2.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }
        }

    }
}
