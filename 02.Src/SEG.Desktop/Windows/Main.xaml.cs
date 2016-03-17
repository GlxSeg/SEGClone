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
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Management;
using System.Runtime.InteropServices;

using SEG.Desktop.Control;
using SEG.Desktop.Windows.Panels;
using SEG.Domain.Helpers;
using System.Configuration;

namespace SEG.Desktop.Windows
{
    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate() { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }

    /// <summary>
    /// Interaction logic for Project_List.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Project_New panelProjectNew;
        public Project_List panelProjectList;
        public ProjectArea_List panelAreaList;
        public Area_AssetList panelAssetList;
        public Area_GenDiag panelAreaGenDiag;

        public Asset_Edit_General panelAssetEditGeneral;
        public Asset_Edit_Mechanical panelAssetEditMechanical;
        public Asset_Edit_Mechanical_Img panelAssetEditMechanicalImg;
        public Asset_Edit_Electrical panelAssetEditElectrical;
        public Asset_Edit_Electrical_Img panelAssetEditElectricalImg;
        public Asset_Edit_Electrical_Info panelAssetEditElectricalInfo;
        public Asset_Edit_Risk panelAssetEditRisk;

        public bool diagPanelsInit = false;
        public bool diagInitialized = false;

        public bool inLogin;

        public Main()
        {
            InitializeComponent();

            // Check if the SEG Database is available
            var sql = new SqlHelper(ConfigurationManager.ConnectionStrings["SEGDatabase"].ConnectionString);
            if (sql.TestSEG())
            {

                inLogin = true;

                // Initialize basic components - Background
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Media/Cover-background-landscape.png"));
                ib.Stretch = Stretch.UniformToFill;
                cL.Background = ib;

                eUser.Text = "";
                ePass.Password = "";

                cL.Visibility = Visibility.Visible;
                c.Visibility = Visibility.Hidden;

                // Create the content providers here
                panelProjectNew = new Project_New();
                panelProjectList = new Project_List();
                panelAreaList = new ProjectArea_List();
                panelAssetList = new Area_AssetList();
                panelAreaGenDiag = new Area_GenDiag();
                panelAssetEditGeneral = new Asset_Edit_General();
                panelAssetEditMechanical = new Asset_Edit_Mechanical();
                panelAssetEditMechanicalImg = new Asset_Edit_Mechanical_Img();
                panelAssetEditElectrical = new Asset_Edit_Electrical();
                panelAssetEditElectricalImg = new Asset_Edit_Electrical_Img();
                panelAssetEditElectricalInfo = new Asset_Edit_Electrical_Info();
                panelAssetEditRisk = new Asset_Edit_Risk();

                ControlCenter.Instance.wMain = this;
            }
            else
            {
                MessageBox.Show("The database was missing or the SEG.Desktop was not correctly configured.  Please use the SEG.Tools to correct.", "Init Error");
            }

            //// Disables inking in the WPF application and enables us to track touch events to properly trigger the touch keyboard
            //InkInputHelper.DisableWPFTabletSupport();

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

        private void bcLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Attempt Login
            DoLogin();
        }

        private void ePass_KeyDown(object sender, KeyEventArgs e)
        {
            // Attempt Login
            DoLogin();
        }

        private void DoLogin()
        {
            if (ControlCenter.Instance.CheckLogin(eUser.Text, ePass.Password))
            {
                inLogin = false;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                //waitControl.Visibility = System.Windows.Visibility.Visible;
                //waitControl.Refresh();

                // Initialize the content Presenter
                panelProjectList.Update();
                panelProjectList.SetSize(cpContent.Width, cpContent.Height);
                cpContent.Content = panelProjectList;

                panelAreaGenDiag.Prepare(cpContent.Width);
                panelAssetEditMechanical.Prepare(cpContent.Width);
                panelAssetEditElectrical.Prepare(cpContent.Width);
                diagInitialized = true;

                c.Visibility = Visibility.Visible;
                cL.Visibility = Visibility.Hidden;

                waitControl.Visibility = System.Windows.Visibility.Collapsed;
                cL.Children.Remove(waitControl);

                Mouse.OverrideCursor = null;

                SetSize();
            }
        }


































        // These methods control the visual aspects of the contents of the main window
        // Basically, this means size changes and panel changes

        private void SetSize()
        {
            // Obtain the window's size
            double w = this.ActualWidth;
            double h = this.ActualHeight;

            if (inLogin)
            {
                // Let's find the window size and adjust to that
                // Obtain the relative positions of the controls
                double x0 = Canvas.GetLeft(labTitleA);
                double x1 = ePass.Width + x0;

                double y0 = Canvas.GetTop(labTitleA);
                double y1a = Canvas.GetTop(ePass);
                double y1 = ePass.Height + y1a;

                double xD = (w - (x1 - x0)) / 2.0 - x0;
                double yD = (h - (y1 - y0)) / 2.0 - y0;

                DeltaMove(labTitleA, xD, yD);
                DeltaMove(labTitleB, xD, yD);
                DeltaMove(eUser, xD, yD);
                DeltaMove(ePass, xD, yD);
                DeltaMove(bcLogin, xD, yD);

                double f1w = labFoot1.Width;
                double f2w = labFoot2.Width;
                double f1h = labFoot1.Height;

                double xF = (w - f1w) / 2.0;
                Canvas.SetLeft(labFoot1, xF);
                Canvas.SetTop(labFoot1,h - 50 - f1h * 2.0);
                xF = (w - f2w) / 2.0;
                Canvas.SetLeft(labFoot2, xF);
                Canvas.SetTop(labFoot2, h - 50 - f1h * 1.0);

                double yWait = Canvas.GetTop(ePass)+20;

                Canvas.SetLeft(waitControl, (w - waitControl.Width) / 2.0);
                Canvas.SetTop(waitControl, yWait);

                Canvas.SetTop(labExit, h - labExit.Height*2.0);
                Canvas.SetLeft(labExit, w - labExit.Width*2.0);
            }
            else
            { 
                // Size the menu control
                segM.SetSize(w, h);

                // Size the content area
                cpContent.Width = w;
                cpContent.Height = h - segM.Height;
                Canvas.SetTop(cpContent, segM.Height);

                // Based on the active content size that
                if (ControlCenter.Instance.page == "project-list")
                {
                    panelProjectList.SetSize(cpContent.Width, cpContent.Height);
                }
                else if (ControlCenter.Instance.page == "project-new")
                {
                    panelProjectNew.SetSize(cpContent.Width, cpContent.Height);
                }
                else if (ControlCenter.Instance.page == "area-list")
                {
                    panelAreaList.SetSize(cpContent.Width, cpContent.Height);
                }
                else if (ControlCenter.Instance.page == "asset-list")
                {
                    panelAssetList.SetSize(cpContent.Width, cpContent.Height);
                }
                else if (ControlCenter.Instance.page == "asset-genDiag")
                {
                    panelAreaGenDiag.SetSize(cpContent.Width, cpContent.Height);
                }
                else if (ControlCenter.Instance.page == "asset-edit-general")
                {
                    panelAssetEditGeneral.SetSize(cpContent.Width, cpContent.Height);
                }
                else if (ControlCenter.Instance.page == "asset-edit-risk")
                {
                    panelAssetEditRisk.SetSize(cpContent.Width, cpContent.Height);
                }
                else if (ControlCenter.Instance.page == "asset-edit-mechanical")
                {
                    panelAssetEditMechanical.SetSize(cpContent.Width, cpContent.Height);
                }
                else if (ControlCenter.Instance.page == "asset-edit-mechanical_img")
                {
                    panelAssetEditMechanicalImg.SetSize(cpContent.Width, cpContent.Height);
                }
                else if (ControlCenter.Instance.page == "asset-edit-electrical")
                {
                    panelAssetEditElectrical.SetSize(cpContent.Width, cpContent.Height);
                }
                else if (ControlCenter.Instance.page == "asset-edit-electrical_img")
                {
                    panelAssetEditElectricalImg.SetSize(cpContent.Width, cpContent.Height);
                }
                else if (ControlCenter.Instance.page == "asset-edit-electrical_info")
                {
                    panelAssetEditElectricalInfo.SetSize(cpContent.Width, cpContent.Height);
                }
            }
        }



        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetSize();
        }



        private void DeltaMove(UIElement e, double xD, double yD)
        {
            double x = Canvas.GetLeft(e);
            double y = Canvas.GetTop(e);

            Canvas.SetLeft(e, x + xD);
            Canvas.SetTop(e, y + yD);
        }



        public void UpdatePanels()
        {
            segM.Update();

            // So, when a panel is updated, the following takes place
            // First, the panel is prepared for the update.  This means visually cleaning the lists, etc.
            // Second, then panel is populated with data.
            // Third, the panel is applied as the content.
            // Fourth, the panel is resized to fit if needed.

            if (ControlCenter.Instance.page == "project-list")
            {
                panelProjectList.Update();
                panelProjectList.SetSize(cpContent.Width, cpContent.Height);
                cpContent.Content = panelProjectList;
            }
            else if (ControlCenter.Instance.page == "project-new")
            {
                panelProjectNew.Update();
                panelProjectNew.SetSize(cpContent.Width, cpContent.Height);
                cpContent.Content = panelProjectNew;
            }
            else if (ControlCenter.Instance.page == "area-list")
            {
                panelAreaList.Update();
                panelAreaList.SetSize(cpContent.Width, cpContent.Height);
                cpContent.Content = panelAreaList;
            }
            else if (ControlCenter.Instance.page == "asset-list")
            {
                panelAssetList.Update();
                panelAssetList.SetSize(cpContent.Width, cpContent.Height);
                cpContent.Content = panelAssetList;
            }
            else if (ControlCenter.Instance.page == "asset-genDiag")
            {
                panelAreaGenDiag.Update(cpContent.Width - 20 - 40); // scrollview margin and scrollbar width
                panelAreaGenDiag.SetSize(cpContent.Width, cpContent.Height);
                cpContent.Content = panelAreaGenDiag;
            }
            else if (ControlCenter.Instance.page == "asset-edit-general")
            {
                panelAssetEditGeneral.Update();
                panelAssetEditGeneral.SetSize(cpContent.Width, cpContent.Height);
                cpContent.Content = panelAssetEditGeneral;
            }
            else if (ControlCenter.Instance.page == "asset-edit-risk")
            {
                panelAssetEditRisk.Update(); // scrollview margin and scrollbar width
                panelAssetEditRisk.SetSize(cpContent.Width, cpContent.Height);
                cpContent.Content = panelAssetEditRisk;
            }
            else if (ControlCenter.Instance.page == "asset-edit-mechanical")
            {
                panelAssetEditMechanical.Update(cpContent.Width - 20 - 40); // scrollview margin and scrollbar width
                panelAssetEditMechanical.SetSize(cpContent.Width, cpContent.Height);
                cpContent.Content = panelAssetEditMechanical;
            }
            else if (ControlCenter.Instance.page == "asset-edit-mechanical_img")
            {
                panelAssetEditMechanicalImg.Update(cpContent.Width - 20 - 40); // scrollview margin and scrollbar width
                panelAssetEditMechanicalImg.SetSize(cpContent.Width, cpContent.Height);
                cpContent.Content = panelAssetEditMechanicalImg;
            }
            else if (ControlCenter.Instance.page == "asset-edit-electrical")
            {
                panelAssetEditElectrical.Update(cpContent.Width - 20 - 40); // scrollview margin and scrollbar width & first diagnostic
                panelAssetEditElectrical.SetSize(cpContent.Width, cpContent.Height);
                cpContent.Content = panelAssetEditElectrical;
            }
            else if (ControlCenter.Instance.page == "asset-edit-electrical_img")
            {
                panelAssetEditElectricalImg.Update(cpContent.Width - 20 - 40); // scrollview margin and scrollbar width & first diagnostic
                panelAssetEditElectricalImg.SetSize(cpContent.Width, cpContent.Height);
                cpContent.Content = panelAssetEditElectricalImg;
            }
            else if (ControlCenter.Instance.page == "asset-edit-electrical_info")
            {
                panelAssetEditElectricalInfo.Update(cpContent.Width - 20 - 40); // scrollview margin and scrollbar width & first diagnostic
                panelAssetEditElectricalInfo.SetSize(cpContent.Width, cpContent.Height);
                cpContent.Content = panelAssetEditElectricalInfo;
            }
        }

        public void UpdateData()
        {
            // Called when a popup needs a panel refresh

            if (ControlCenter.Instance.page == "project-list")
            {

            }
            else if (ControlCenter.Instance.page == "project-new")
            {

            }
            else if (ControlCenter.Instance.page == "area-list")
            {

            }
            else if (ControlCenter.Instance.page == "asset-list")
            {

            }
            else if (ControlCenter.Instance.page == "asset-genDiag")
            {
                panelAreaGenDiag.UpdateData();
            }
            else if (ControlCenter.Instance.page == "asset-edit-general")
            {
                panelAssetEditGeneral.UpdateData();
            }
            else if (ControlCenter.Instance.page == "asset-edit-risk")
            {
                panelAssetEditRisk.UpdateData();
            }
            else if (ControlCenter.Instance.page == "asset-edit-mechanical")
            {
                panelAssetEditMechanical.UpdateData();
            }
            else if (ControlCenter.Instance.page == "asset-edit-mechanical_img")
            {
                panelAssetEditMechanicalImg.UpdateData();
            }
            else if (ControlCenter.Instance.page == "asset-edit-electrical")
            {
                panelAssetEditElectrical.UpdateData();
            }
            else if (ControlCenter.Instance.page == "asset-edit-electrical_img")
            {
                panelAssetEditElectricalImg.UpdateData();
            }
            else if (ControlCenter.Instance.page == "asset-edit-electrical_info")
            {
                panelAssetEditElectricalInfo.UpdateData();
            }
        }



        private void Window_Activated(object sender, EventArgs e)
        {
            if(!diagInitialized)
            {
                panelAreaGenDiag.Prepare(cpContent.Width);
                panelAssetEditMechanical.Prepare(cpContent.Width);
                panelAssetEditElectrical.Prepare(cpContent.Width);
                diagInitialized = true;
            }
        }

        private void labExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Menu_Exit();
        }

    }
}
