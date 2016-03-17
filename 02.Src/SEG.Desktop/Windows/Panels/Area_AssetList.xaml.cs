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
using SEG.Domain.Printing;
using SEG.Desktop.Windows.Popups;

namespace SEG.Desktop.Windows.Panels
{
    /// <summary>
    /// Interaction logic for Area_AssetList.xaml
    /// </summary>
    public partial class Area_AssetList : UserControl
    {
        public List<Asset> assets;
        public Area_AssetList()
        {
            InitializeComponent();

            SEGAssetList_AddButton addButton = new SEGAssetList_AddButton();
            addButton.Width = 200;
            addButton.Height = 260;

            sPanel.Children.Add(addButton);
        }

        public void Update()
        {
            // Clear only some children
            List<SEGAssetItem> assetControls = new List<SEGAssetItem>();
            foreach (var child in sPanel.Children)
            {
                if (child is SEGAssetItem)
                    assetControls.Add(child as SEGAssetItem);
            }

            foreach (var a in assetControls)
            {
                sPanel.Children.Remove(a);
            }

            // Add new items one at a time
            assets = ProjectHelper.GetAssets(ControlCenter.Instance.segR, ControlCenter.Instance.cProjectAreaId);
            foreach (Asset a in assets)
            {
                SEGAssetItem s = new SEGAssetItem();
                s.Width = 200;
                s.Height = 260;
                bool selected = false;
                if (ControlCenter.Instance.cAssetId == a.Id)
                    selected = true;
                s.Initialize(a, selected);

                sPanel.Children.Add(s);
            }
        }

        public void SetSize(double cW, double cH)
        {
            this.Width = cW;
            this.Height = cH;

            cSubMenu.Width = cW;

            scrollView.Width = cW - 20;
            double svt = Canvas.GetTop(scrollView);
            scrollView.Height = cH - svt - 10;

            Canvas.SetLeft(cbPrint, cW - cbPrint.Width - 20);
            Canvas.SetLeft(cbCopyDiag, cW - cbPrint.Width - 20 - cbCopyDiag.Width - 20);
            Canvas.SetLeft(cbDelete, cW - cbPrint.Width - 20 - cbCopyDiag.Width - 20 - cbDelete.Width - 20);
        }

        private void SubMenu_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Switch to the General Diagnostics for this area
            ControlCenter.Instance.Area_Tab_GenDiag();
        }

        private void cbPrint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "area_report_" + ControlCenter.Instance.cProjectArea.Name; // Default file name
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


                SEGPrinter.PrintArea(ControlCenter.Instance.segR,
                                     ControlCenter.Instance.cProjectAreaId,
                                     ControlCenter.Instance.cProjectId,
                                     filename);

                Mouse.OverrideCursor = null;

                MessagePopup mp = new MessagePopup();
                mp.Setup("Asset", "The area report was generated successfully", false);
                mp.SetSize();
                mp.ShowDialog();

            }

        }

        private void cbCopyDiag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var s = new SelectAreaPopup();
            s.Setup();
            s.SetSize();
            if (s.ShowDialog() == true)
            {
                if (ControlCenter.Instance.cCopyToAreaId == ControlCenter.Instance.cProjectAreaId)
                {
                    MessagePopup mp = new MessagePopup();
                    mp.Setup("Area Diagnostics", "The source and destination areas cannot be the same", false);
                    mp.SetSize();
                    mp.ShowDialog();
                }
                else
                {
                    MessagePopup mp = new MessagePopup();
                    mp.Setup("Copy Diagnostics", "Are you sure you want to overwrite the diagnostics in the destination Area?", true);
                    mp.SetSize();
                    if (mp.ShowDialog() == true)
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                        // We copy the diagnostics
                        ProjectHelper.CopyDiagnosticsToArea(ControlCenter.Instance.segR,
                            ControlCenter.Instance.cProjectAreaId, ControlCenter.Instance.cCopyToAreaId);

                        Mouse.OverrideCursor = null;

                        mp = new MessagePopup();
                        mp.Setup("Area Diagnostics", "The diagnostics were copied to the destination Area", false);
                        mp.SetSize();
                        mp.ShowDialog();
                    }

                }
            }
        }

        private void cbDelete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (assets.Count > 0)
            {
                MessagePopup mp = new MessagePopup();
                mp.Setup("Delete Area", "Please delete all assets first.  Only empty areas can be deleted from the system.", false);
                mp.SetSize();
                mp.ShowDialog();
            }
            else
            {
                MessagePopup mp = new MessagePopup();
                mp.Setup("Delete Area", "Are you sure you want to delete the Area from the Project?", true);
                mp.SetSize();
                if (mp.ShowDialog() == true)
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                    // We copy the diagnostics
                    ProjectHelper.DeleteArea(ControlCenter.Instance.segR, ControlCenter.Instance.cProjectAreaId);

                    Mouse.OverrideCursor = null;

                    mp = new MessagePopup();
                    mp.Setup("Project Management", "The Area was deleted from the Project", false);
                    mp.SetSize();
                    mp.ShowDialog();

                    ControlCenter.Instance.SelectProject(ControlCenter.Instance.cProjectId);
                }

            }
        }

    }
}
