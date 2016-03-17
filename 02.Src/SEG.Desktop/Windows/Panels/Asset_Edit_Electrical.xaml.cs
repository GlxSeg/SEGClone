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
    public partial class Asset_Edit_Electrical : UserControl
    {
        List<SEGDiagnosticItem> listItems = null;
        List<SEGDiagnosticSubSection> listSubSections = null;
        public bool isPrepared = false;

        List<ItemSection> sections = null;
        List<ScrollViewer> scrollers = null;
        int icSection;

        public Diagnostics d;
        public int idiagx;
        public int ndiagx;

        public bool hasMechanical;

        public Asset_Edit_Electrical()
        {
            InitializeComponent();
        }

        public void Prepare(double estimatedWidth)
        {
            // Get the sections
            sections = new List<ItemSection>();
            scrollers = new List<ScrollViewer>();

            // Now, obtain the list of diagnostic items
            List<Item> dtItems = DiagnosticsHelper.GetStandardsForType(ControlCenter.Instance.segR, "Electrical");

            double w = estimatedWidth;

            Guid cSid = Guid.Empty;
            Guid cSSid = Guid.Empty;

            listItems = new List<SEGDiagnosticItem>();
            listSubSections = new List<SEGDiagnosticSubSection>();

            foreach (Item i in dtItems)
            {
                // Check to see if the section has changed
                if (i.ItemSectionId != cSid)
                {
                    cSid = i.ItemSectionId;

                    ItemSection iS = DiagnosticsHelper.GetItemSection(ControlCenter.Instance.segR, cSid);
                    cSSid = Guid.Empty;

                    // Create a new Scrollviewer
                    ScrollViewer sw = new ScrollViewer();
                    sw.CanContentScroll = true;
                    sw.PanningMode = PanningMode.VerticalOnly;
                    sw.Width = w - 40;
                    StackPanel sPanel = new StackPanel();
                    sw.Content = sPanel;
                    scrollers.Add(sw);
                    sections.Add(iS);
                }

                if (i.ItemSubSectionId != Guid.Empty && (cSSid == Guid.Empty || cSSid != i.ItemSubSectionId))
                {
                    cSSid = i.ItemSubSectionId;
                    SEGDiagnosticSubSection di = new SEGDiagnosticSubSection();
                    di.Initialize(DiagnosticsHelper.GetItemSubSection(ControlCenter.Instance.segR, cSSid), w);
                    listSubSections.Add(di);
                    (scrollers.Last().Content as StackPanel).Children.Add(di);
                }

                SEGDiagnosticItem ditem = new SEGDiagnosticItem();
                ditem.Prepare(i, w);
                listItems.Add(ditem);
                (scrollers.Last().Content as StackPanel).Children.Add(ditem);
            }

            icSection = 0;
            isPrepared = true;
        }

        public void SelectSection()
        {
            cpC.Content = scrollers[icSection];

            labCode.Content = sections[icSection].Code;
            labContent.Text = sections[icSection].Title;

            if (icSection == 0)
                bBack.Foreground = new SolidColorBrush(Color.FromArgb(255, 92, 92, 92));
            else
                bBack.Foreground = new SolidColorBrush(Colors.White);

            if (icSection == sections.Count - 1)
                bForward.Foreground = new SolidColorBrush(Color.FromArgb(255, 92, 92, 92));
            else
                bForward.Foreground = new SolidColorBrush(Colors.White);
        }

        public void Update(double estimatedWidth)
        {
            if (!isPrepared)
            {
                Prepare(estimatedWidth);
            }

            d = DiagnosticsHelper.GetElectricalDiagnostics(ControlCenter.Instance.segR,
                                                           ControlCenter.Instance.cAssetId).OrderBy(x=>x.Index).ToList()
                                                           [ControlCenter.Instance.cAssetEidx];

            List<DiagnosticsDetail> dL = DiagnosticsHelper.GetDiagnosticsDetails(ControlCenter.Instance.segR, d.Id);

            foreach (SEGDiagnosticItem di in listItems)
            {
                DiagnosticsDetail cDL = dL.FirstOrDefault(x => x.ItemId == di.i.Id);
                if (cDL != null)
                {
                    di.Initialize(cDL, estimatedWidth);
                }
            }


            // Has Mechanical
            var assetInfo = ProjectHelper.GetAssetInfo(ControlCenter.Instance.segR,ControlCenter.Instance.cAssetId);
            hasMechanical = (assetInfo.FirstOrDefault(x => x.Key == "MECHDIAG").Value == "YES");

            if (hasMechanical)
                labName_S2.Foreground = new SolidColorBrush(Colors.White);
            else
                labName_S2.Foreground = new SolidColorBrush(Colors.Gray);






            // Update the buttons and texts
            idiagx = ControlCenter.Instance.cAssetEidx;
            ndiagx = DiagnosticsHelper.GetElectricalDiagnostics(ControlCenter.Instance.segR,
                                                                    ControlCenter.Instance.cAssetId).Count;

            labECount.Content = (idiagx + 1).ToString() + "  /  " + ndiagx.ToString();
            if(idiagx>0)
                labECBack.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 85, 110));
            else            
                labECBack.Foreground = new SolidColorBrush(Colors.Silver);
            if (idiagx < ndiagx-1)
                labECForward.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 85, 110));
            else
                labECForward.Foreground = new SolidColorBrush(Colors.Silver);

            icSection = 0;
            SelectSection();
        }


        public void SetSize(double cW, double cH)
        {
            this.Width = cW;
            this.Height = cH;

            cSubMenu.Width = cW;

            cSubMenu2.Width = cW;
            Canvas.SetLeft(cbRemove, cW - cbRemove.Width - 20);
            Canvas.SetLeft(rcSubMenu2, cW - cbRemove.Width - 20 - 10);
            Canvas.SetLeft(cSubMenu2_E, cW - cbRemove.Width - 20 - 10 - cSubMenu2_E.Width - 10);
            Canvas.SetLeft(rcSubMenu, cW - cbRemove.Width - 20 - 10 - cSubMenu2_E.Width - 20);

            double smw = cW - cbRemove.Width - 20 - 10 - cSubMenu2_E.Width - 30;

            cSubMenu2_1.Width = smw / 3;
            labName_2_1.Width = smw / 3;
            cSubMenu2_2.Width = smw / 3;
            labName_2_2.Width = smw / 3;
            cSubMenu2_3.Width = smw / 3;
            labName_2_3.Width = smw / 3;

            Canvas.SetLeft(cSubMenu2_2, smw / 3);
            Canvas.SetLeft(cSubMenu2_3, 2 * smw / 3);
            Canvas.SetLeft(rcSubMenu3, 2 * smw / 3);

            cSectionBox.Width = cW - 20;

            Canvas.SetLeft(bForward, cW - 20 - 20 - bForward.Width);
            Canvas.SetLeft(rSep, cW - 20 - 20 - bForward.Width - 10);
            Canvas.SetLeft(bBack, cW - 20 - 20 - 20 - bForward.Width * 2);

            double wx = cW - 20 - 20 - 20 - bForward.Width * 2;
            labBlock.Width = wx - 120;

            cpC.Width = cW - 20;
            double svt = Canvas.GetTop(cpC);
            cpC.Height = cH - svt - 10;

            double w = scrollers[0].Width - 40;

            // Resize all the stackPanel elements
            foreach (SEGDiagnosticItem sdi in listItems)
            {
                sdi.SetSize(w);
            }

            foreach (SEGDiagnosticSubSection sds in listSubSections)
            {
                sds.SetSize(w);
            }
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
            if (hasMechanical)
            {
                ControlCenter.Instance.Asset_SelectMechanical();
            }
        }

        private void labECBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(idiagx>0)
            {
                ControlCenter.Instance.cAssetEidx--;
                ControlCenter.Instance.Asset_SelectElectrical();
            }
        }

        private void labECForward_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (idiagx < ndiagx-1)
            {
                ControlCenter.Instance.cAssetEidx++;
                ControlCenter.Instance.Asset_SelectElectrical();
            }
        }

        private void cSubMenu2_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Asset_SelectElectrical_Img();
        }

        private void cSubMenu2_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Asset_SelectElectrical_Info();
        }

        private void bBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (icSection > 0)
            {
                icSection--;
                SelectSection();
            }
        }

        private void bForward_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (icSection < sections.Count - 1)
            {
                icSection++;
                SelectSection();
            }
        }

        private void cSubMenu_4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Asset_SelectRisk();
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
