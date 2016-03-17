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

namespace SEG.Desktop.Windows.Panels
{
    /// <summary>
    /// Interaction logic for Asset_Edit_Electrical.xaml
    /// </summary>
    public partial class Asset_Edit_Electrical_Info : UserControl
    {

        public Diagnostics d;
        public int idiagx;
        public int ndiagx;

        public string mode;

        public bool hasMechanical;

        public Asset_Edit_Electrical_Info()
        {
            InitializeComponent();
        }

        public void Update(double estimatedWidth)
        {
            // Update the buttons and texts
            idiagx = ControlCenter.Instance.cAssetEidx;
            ndiagx = DiagnosticsHelper.GetElectricalDiagnostics(ControlCenter.Instance.segR,
                                                                    ControlCenter.Instance.cAssetId).Count;

            d = DiagnosticsHelper.GetElectricalDiagnostics(ControlCenter.Instance.segR,
                                                           ControlCenter.Instance.cAssetId).OrderBy(x => x.Index).ToList()
                                                           [ControlCenter.Instance.cAssetEidx];

            labECount.Content = (idiagx + 1).ToString() + "  /  " + ndiagx.ToString();
            if(idiagx>0)
                labECBack.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 85, 110));
            else            
                labECBack.Foreground = new SolidColorBrush(Colors.Silver);
            if (idiagx < ndiagx-1)
                labECForward.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 85, 110));
            else
                labECForward.Foreground = new SolidColorBrush(Colors.Silver);

            // Info

            ControlCenter.Instance.cAsset = ProjectHelper.GetAsset(ControlCenter.Instance.segR, 
                                                                   ControlCenter.Instance.cAssetId); 

            List<AssetInfo> assetInfo = ProjectHelper.GetAssetInfo(ControlCenter.Instance.segR, 
                                                                   ControlCenter.Instance.cAssetId);

            eInfo1.Text = assetInfo.FirstOrDefault(x => x.Key == "EDIAG_" + (idiagx + 1) + "_INFO1").Value;
            eInfo2.Text = assetInfo.FirstOrDefault(x => x.Key == "EDIAG_" + (idiagx + 1) + "_INFO2").Value;
            eInfo3.Text = assetInfo.FirstOrDefault(x => x.Key == "EDIAG_" + (idiagx + 1) + "_INFO3").Value;
            eInfo4.Text = assetInfo.FirstOrDefault(x => x.Key == "EDIAG_" + (idiagx + 1) + "_INFO4").Value;

            mode = "view";

            eInfo1.IsEnabled = false;
            eInfo2.IsEnabled = false;
            eInfo3.IsEnabled = false;
            eInfo4.IsEnabled = false;


            // Has Mechanical
            hasMechanical = (assetInfo.FirstOrDefault(x => x.Key == "MECHDIAG").Value == "YES");

            if (hasMechanical)
                labName_S2.Foreground = new SolidColorBrush(Colors.White);
            else
                labName_S2.Foreground = new SolidColorBrush(Colors.Gray);
        }


        public void SetSize(double cW, double cH)
        {
            this.Width = cW;
            this.Height = cH;

            cSubMenu.Width = cW;
            cSubMenu2.Width = cW;
            Canvas.SetLeft(cbRemove, cW - cbRemove.Width - 20);
            Canvas.SetLeft(rcSubMenuE, cW - cbRemove.Width - 20 - 10);
            Canvas.SetLeft(cSubMenu2_E, cW - cbRemove.Width - 20 - 10 - cSubMenu2_E.Width - 10);
            //Canvas.SetLeft(rcSubMenu, cW - cbRemove.Width - 20 - 10 - cSubMenu2_E.Width - 20);

            double smw = cW - cbRemove.Width - 20 - 10 - cSubMenu2_E.Width - 30;

            cSubMenu2_1.Width = smw / 3;
            labName_2_1.Width = smw / 3;
            cSubMenu2_2.Width = smw / 3;
            labName_2_2.Width = smw / 3;
            cSubMenu2_3.Width = smw / 3;
            labName_2_3.Width = smw / 3;

            Canvas.SetLeft(rcSubMenu, smw / 3);

            Canvas.SetLeft(cSubMenu2_2, smw / 3);
            Canvas.SetLeft(cSubMenu2_3, 2 * smw / 3);
        }

        public void UpdateData()
        {
            //foreach (UIElement e in sPanel.Children)
            //{
            //    if (e is SEGDiagnosticItem)
            //    {
            //        (e as SEGDiagnosticItem).Update();
            //    }
            //}
        }

        private void cSubMenu_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Asset_SelectGeneral();
        }

        private void cSubMenu_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(hasMechanical)
                ControlCenter.Instance.Asset_SelectMechanical();
        }

        private void labECBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(idiagx>0)
            {
                ControlCenter.Instance.cAssetEidx--;
                ControlCenter.Instance.Asset_SelectElectrical_Info();
            }
        }

        private void labECForward_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (idiagx < ndiagx-1)
            {
                ControlCenter.Instance.cAssetEidx++;
                ControlCenter.Instance.Asset_SelectElectrical_Info();
            }
        }

        private void cSubMenu2_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Asset_SelectElectrical_Img();
        }

        private void cSubMenu2_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Asset_SelectElectrical();
        }

        private void cSubMenu_4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Asset_SelectRisk();
        }




        private void cbEditAI_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mode == "view")
            {
                mode = "edit";

                eInfo1.IsEnabled = true;
                eInfo2.IsEnabled = true;
                eInfo3.IsEnabled = true;
                eInfo4.IsEnabled = true;

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
            if (mode == "edit")
            {
                mode = "view";

                ProjectHelper.ModifyAssetInfo(ControlCenter.Instance.segR,
                                              ControlCenter.Instance.cAssetId,
                                              "EDIAG_" + (idiagx + 1) + "_INFO1", eInfo1.Text);

                ProjectHelper.ModifyAssetInfo(ControlCenter.Instance.segR,
                                              ControlCenter.Instance.cAssetId,
                                              "EDIAG_" + (idiagx + 1) + "_INFO2", eInfo2.Text);

                ProjectHelper.ModifyAssetInfo(ControlCenter.Instance.segR,
                                              ControlCenter.Instance.cAssetId,
                                              "EDIAG_" + (idiagx + 1) + "_INFO3", eInfo3.Text);

                ProjectHelper.ModifyAssetInfo(ControlCenter.Instance.segR,
                                              ControlCenter.Instance.cAssetId,
                                              "EDIAG_" + (idiagx + 1) + "_INFO4", eInfo4.Text);

                eInfo1.IsEnabled = false;
                eInfo2.IsEnabled = false;
                eInfo3.IsEnabled = false;
                eInfo4.IsEnabled = false;

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

                eInfo1.Text = assetInfo.FirstOrDefault(x => x.Key == "EDIAG_" + (idiagx + 1) + "_INFO1").Value;
                eInfo2.Text = assetInfo.FirstOrDefault(x => x.Key == "EDIAG_" + (idiagx + 1) + "_INFO2").Value;
                eInfo3.Text = assetInfo.FirstOrDefault(x => x.Key == "EDIAG_" + (idiagx + 1) + "_INFO3").Value;
                eInfo4.Text = assetInfo.FirstOrDefault(x => x.Key == "EDIAG_" + (idiagx + 1) + "_INFO4").Value;

                eInfo1.IsEnabled = false;
                eInfo2.IsEnabled = false;
                eInfo3.IsEnabled = false;
                eInfo4.IsEnabled = false;

                // Set buttons for mode
                cbEditAI.Background = new SolidColorBrush(Colors.Silver);
                cbSaveAI.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                cbCancelAI.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                lbEditAI.Foreground = new SolidColorBrush(Colors.Black);
                lbSaveAI.Foreground = new SolidColorBrush(Colors.Silver);
                lbCancelAI.Foreground = new SolidColorBrush(Colors.Silver);

            }
        }









        private void cbRemove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessagePopup mp = new MessagePopup();
            mp.Setup("Delete Diagnostics", "Are you sure you want to delete these Electrical Diagnostics?", true);
            mp.SetSize();
            if (mp.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                // We copy the diagnostics
                ProjectHelper.DeleteElectricalDiagnostics(ControlCenter.Instance.segR,
                    ControlCenter.Instance.cAssetId, d.Id);
                Mouse.OverrideCursor = null;

                mp = new MessagePopup();
                mp.Setup("Delete Diagnostics", "The Diagnostics were deleted from the Asset", false);
                mp.SetSize();
                mp.ShowDialog();

                // Go back to asset in general
                ControlCenter.Instance.SelectAsset(ControlCenter.Instance.cAssetId);
            }
        }
    }
}
