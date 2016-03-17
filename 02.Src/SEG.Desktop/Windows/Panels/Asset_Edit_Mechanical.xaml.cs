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

namespace SEG.Desktop.Windows.Panels
{
    /// <summary>
    /// Interaction logic for Asset_Edit_Mechanical.xaml
    /// </summary>
    public partial class Asset_Edit_Mechanical : UserControl
    {
        List<SEGDiagnosticItem> listItems = null;
        List<SEGDiagnosticSubSection> listSubSections = null;
        public bool isPrepared = false;

        List<ItemSection> sections = null;
        List<ScrollViewer> scrollers = null;
        int icSection;

        public bool hasElectrical = false;

        public Diagnostics dm;

        public Asset_Edit_Mechanical()
        {
            InitializeComponent();
        }



        public void Prepare(double estimatedWidth)
        {
            // Get the sections
            sections = new List<ItemSection>();
            scrollers = new List<ScrollViewer>();

            // Now, obtain the list of diagnostic items
            List<Item> dtItems = DiagnosticsHelper.GetStandardsForType(ControlCenter.Instance.segR, "Mechanical");

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

            // Find the mechanical diagnostics
            Diagnostics d = DiagnosticsHelper.GetMechanicalDiagnostics(ControlCenter.Instance.segR,
                                                                       ControlCenter.Instance.cAssetId);

            List<DiagnosticsDetail> dL = DiagnosticsHelper.GetDiagnosticsDetails(ControlCenter.Instance.segR, d.Id);

            // Get the scrollViewer internal width
            foreach (SEGDiagnosticItem di in listItems)
            {
                DiagnosticsDetail cDL = dL.FirstOrDefault(x => x.ItemId == di.i.Id);
                if (cDL != null)
                {
                    di.Initialize(cDL, estimatedWidth);
                }
            }

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
            double smw = cW - cbRemove.Width - 40;

            cSubMenu2_1.Width = smw / 2;
            labName_2_1.Width = smw / 2;
            cSubMenu2_2.Width = smw / 2;
            labName_2_2.Width = smw / 2;

            Canvas.SetLeft(cSubMenu2_2, smw / 2);
            Canvas.SetLeft(rcSubMenu, smw + 10);

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



        private void cSubMenu_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(hasElectrical)
                ControlCenter.Instance.Asset_SelectElectrical();
        }



        private void cSubMenu2_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Asset_SelectMechanical_Img();
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
    }
}
