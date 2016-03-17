using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SEG.Desktop.Control;
using SEG.Domain.Model;
using SEG.Domain.Helpers;

namespace SEG.Desktop.UserControls
{
    /// <summary>
    /// Interaction logic for SEGMenuBar.xaml
    /// </summary>
    public partial class SEGMenuBar : UserControl
    {
        public SEGMenuBar()
        {
            InitializeComponent();

            sizeOk = false;
            // Possible pages
            // project-list
            // area-list
            // area
            // asset            
            droppedDown = false;

            // Configuration parameters
            config_dropMenuW = 350;
            config_menuH = 100;

            // Create the display dictionary
            Dictionary<string,double> dO = new Dictionary<string, double>();
            Dictionary<string, double> dN = new Dictionary<string, double>();
            p = new List<Dictionary<string, double>>();
            p.Add(dO);
            p.Add(dN);
        }

        public bool droppedDown;
        public bool sizeOk;
        public double pageW;
        public double pageH;

        public double config_dropMenuW;
        public double config_menuH;

        // Ok, placement/visual status of display items
        public List<Dictionary<string, double>> p; 


        public void SetSize(double totW, double totH)
        {
            // This routine defines the main page size, so that the menu control knows how to resize itself
            if (!sizeOk || pageW != totW || pageH != totH)
            {
                this.Width = pageW;

                pageW = totW;
                pageH = totH;

                // We've been properly sized
                sizeOk = true;

                // Now adjust based on the size
                CalcVisual(0); // First 
                Update();
            }
        }

        public void Update()
        {
            CalcVisual(1);
            SetContent();
            SetVisual(true);            
        }

        public void SetContent()
        {
            // User name
            lbUsername.Content = ControlCenter.Instance.cUser.FullName;

            // This routine just sets the content based on the selected menus
            if (ControlCenter.Instance.page == "project-list")
            {
                labProject.Content = "Projects"; 
                if (droppedDown)
                {
                    
                }
            }
            else if (ControlCenter.Instance.page == "project-new")
            {
                labProject.Content = "New Project"; 
                if (droppedDown)
                {

                }
            }
            else if (ControlCenter.Instance.page == "area-list")
            {
                labProject.Content = ControlCenter.Instance.cProject.Name; // Translate to Portuguese
                if (droppedDown)
                {

                }                
            }
            else if (ControlCenter.Instance.page == "asset-list" ||
                     ControlCenter.Instance.page == "asset-genDiag")
            {
                labProject.Content = ControlCenter.Instance.cProject.Name + "  /  " + ControlCenter.Instance.cProjectArea.Name; // Translate to Portuguese
                if (droppedDown)
                {

                }
            }
            else if (ControlCenter.Instance.page == "asset-edit-general" ||
                     ControlCenter.Instance.page == "asset-edit-mechanical" ||
                     ControlCenter.Instance.page == "asset-edit-mechanical_img" ||
                     ControlCenter.Instance.page == "asset-edit-electrical" ||
                     ControlCenter.Instance.page == "asset-edit-electrical_img" ||
                     ControlCenter.Instance.page == "asset-edit-electrical_info" )
            {
                labProject.Content = ControlCenter.Instance.cProject.Name + "  /  " + ControlCenter.Instance.cProjectArea.Name; // Translate to Portuguese
                labAsset.Content = ControlCenter.Instance.cAssetInfoList.FirstOrDefault(x => x.Key == "ID").Value + " [" +
                                   ControlCenter.Instance.cAssetInfoList.FirstOrDefault(x => x.Key == "TAG").Value + "]";

                if (ControlCenter.Instance.cAsset.ImageId != Guid.Empty)
                    iThumbMB.Source = ControlCenter.Instance.cAssetImage.ThumbDataImage;
                else
                    iThumbMB.Source = null;

                if (droppedDown)
                {

                }
            }

        }

        public void CalcVisual(int idx)
        {
            // This routine calculates the placement and visual status of each element of the menu

            // The static placement of bcMenu
            p[idx]["bcMenu_H"] = config_menuH;
            p[idx]["bcMenu_X"] = 0;
            p[idx]["bcMenu_Y"] = 0;
            p[idx]["bcMenu_V"] = 1;

            if (!droppedDown)
            {
                // Do the main menu button
                p[idx]["bcMenu_W"] = config_menuH;

                // Hide the dropmenu & shadow
                p[idx]["cDropMenu_W"] = config_dropMenuW;
                p[idx]["cDropMenu_H"] = pageH - config_menuH;
                p[idx]["cDropMenu_X"] = 0;
                p[idx]["cDropMenu_Y"] = config_menuH;
                p[idx]["cDropMenu_V"] = -1;

                p[idx]["cShadow_W"] = pageW;
                p[idx]["cShadow_H"] = pageH - config_menuH;
                p[idx]["cShadow_X"] = 0;
                p[idx]["cShadow_Y"] = config_menuH;
                p[idx]["cShadow_V"] = -1;
            }
            else
            {
                // Do the main menu button
                p[idx]["bcMenu_W"] = config_dropMenuW;

                // Hide the dropmenu & shadow
                p[idx]["cDropMenu_W"] = config_dropMenuW;
                p[idx]["cDropMenu_H"] = pageH - config_menuH;
                p[idx]["cDropMenu_X"] = 0;
                p[idx]["cDropMenu_Y"] = config_menuH;
                p[idx]["cDropMenu_V"] = 1;

                p[idx]["cShadow_W"] = pageW;
                p[idx]["cShadow_H"] = pageH - config_menuH;
                p[idx]["cShadow_X"] = 0;
                p[idx]["cShadow_Y"] = config_menuH;
                p[idx]["cShadow_V"] = 1;
            }

            // Now, place the other buttons & texts
            if (ControlCenter.Instance.page == "project-list")
            {
                // Moving X
                double x = p[idx]["bcMenu_W"];
                double xInfoW = pageW - p[idx]["bcMenu_W"];

                // Nav Back
                p[idx]["bcNavBack_W"] = config_menuH*0.8;
                p[idx]["bcNavBack_H"] = config_menuH;
                p[idx]["bcNavBack_X"] = x;
                p[idx]["bcNavBack_Y"] = 0;
                p[idx]["bcNavBack_V"] = -1;

                // Nav Forward
                p[idx]["bcNavForward_W"] = config_menuH * 0.8;
                p[idx]["bcNavForward_H"] = config_menuH;
                p[idx]["bcNavForward_Y"] = 0;
                if (ControlCenter.Instance.cProject != null)
                {
                    p[idx]["bcNavForward_V"] = 1;
                    xInfoW -= p[idx]["bcNavForward_W"];
                }
                else
                {
                    p[idx]["bcNavForward_V"] = -1;
                }

                // Thumb
                p[idx]["cThumb_W"] = config_menuH * 1.8;
                p[idx]["cThumb_H"] = config_menuH * 1.8;
                p[idx]["cThumb_X"] = pageW - p[idx]["cThumb_W"];
                p[idx]["cThumb_Y"] = 0;
                p[idx]["cThumb_V"] = -1;

                // Info
                p[idx]["cInfo_W"] = xInfoW;
                p[idx]["cInfo_H"] = config_menuH;
                p[idx]["cInfo_X"] = x;
                p[idx]["cInfo_Y"] = 0;
                p[idx]["cInfo_V"] = 1;
                x += p[idx]["cInfo_W"];

                // Info Labels
                p[idx]["labProject_W"] = xInfoW - 10;
                p[idx]["labProject_H"] = labProject.ActualHeight; // TEXT SIZE ADJUST - :TO:DO:
                p[idx]["labProject_X"] = 5; // Inside cInfo
                p[idx]["labProject_Y"] = p[idx]["cInfo_H"]/2 - p[idx]["labProject_H"]/2;
                p[idx]["labProject_V"] = 1;

                p[idx]["labAsset_W"] = xInfoW - 10;
                p[idx]["labAsset_H"] = labAsset.ActualHeight; // TEXT SIZE ADJUST - :TO:DO:
                p[idx]["labAsset_X"] = 5; // Inside cInfo
                p[idx]["labAsset_Y"] = p[idx]["cInfo_H"] / 2 - p[idx]["labAsset_H"] / 2;
                p[idx]["labAsset_V"] = -1;

                // Now Place
                p[idx]["bcNavForward_X"] = x;
            }
            else if (ControlCenter.Instance.page == "area-list" || 
                     ControlCenter.Instance.page == "asset-list" ||
                     ControlCenter.Instance.page == "asset-genDiag" ||
                     ControlCenter.Instance.page == "project-new")
            {
                // Moving X
                double x = p[idx]["bcMenu_W"];
                double xInfoW = pageW - p[idx]["bcMenu_W"];

                // Nav Back
                p[idx]["bcNavBack_W"] = config_menuH*0.8;
                p[idx]["bcNavBack_H"] = config_menuH;
                p[idx]["bcNavBack_X"] = x;
                p[idx]["bcNavBack_Y"] = 0;
                p[idx]["bcNavBack_V"] = 1;
                x += p[idx]["bcNavBack_W"];
                xInfoW -= p[idx]["bcNavBack_W"];

                // Nav Forward
                p[idx]["bcNavForward_W"] = config_menuH * 0.8;
                p[idx]["bcNavForward_H"] = config_menuH;
                p[idx]["bcNavForward_Y"] = 0;
                if (ControlCenter.Instance.page == "area-list")
                {
                    if (ControlCenter.Instance.cProjectAreaId != Guid.Empty)
                    {
                        p[idx]["bcNavForward_V"] = 1;
                        xInfoW -= p[idx]["bcNavForward_W"];
                    }
                    else
                    {
                        p[idx]["bcNavForward_V"] = -1;
                    }
                }
                else if (ControlCenter.Instance.page == "asset-list")
                {
                    if (ControlCenter.Instance.cAssetId != Guid.Empty)
                    {
                        p[idx]["bcNavForward_V"] = 1;
                        xInfoW -= p[idx]["bcNavForward_W"];
                    }
                    else
                    {
                        p[idx]["bcNavForward_V"] = -1;
                    }
                }
                else if (ControlCenter.Instance.page == "project-new")
                {
                    p[idx]["bcNavForward_V"] = -1;
                }

                // Thumb
                p[idx]["cThumb_W"] = config_menuH * 1.8;
                p[idx]["cThumb_H"] = config_menuH * 1.8;
                p[idx]["cThumb_X"] = pageW - p[idx]["cThumb_W"];
                p[idx]["cThumb_Y"] = 0;
                p[idx]["cThumb_V"] = -1;

                // Info
                p[idx]["cInfo_W"] = xInfoW;
                p[idx]["cInfo_H"] = config_menuH;
                p[idx]["cInfo_X"] = x;
                p[idx]["cInfo_Y"] = 0;
                p[idx]["cInfo_V"] = 1;
                x += p[idx]["cInfo_W"];

                // Info Labels
                p[idx]["labProject_W"] = xInfoW - 10;
                p[idx]["labProject_H"] = labProject.ActualHeight; // TEXT SIZE ADJUST - :TO:DO:
                p[idx]["labProject_X"] = 5; // Inside cInfo
                p[idx]["labProject_Y"] = p[idx]["cInfo_H"]/2 - p[idx]["labProject_H"]/2;
                p[idx]["labProject_V"] = 1;

                p[idx]["labAsset_W"] = xInfoW - 10;
                p[idx]["labAsset_H"] = labAsset.ActualHeight; // TEXT SIZE ADJUST - :TO:DO:
                p[idx]["labAsset_X"] = 5; // Inside cInfo
                p[idx]["labAsset_Y"] = p[idx]["cInfo_H"] / 2 - p[idx]["labAsset_H"] / 2;
                p[idx]["labAsset_V"] = -1;

                // Now Place
                p[idx]["bcNavForward_X"] = x;
            }
            else if (ControlCenter.Instance.page == "asset-edit-general" ||
                     ControlCenter.Instance.page == "asset-edit-mechanical" ||
                     ControlCenter.Instance.page == "asset-edit-mechanical_img" ||
                     ControlCenter.Instance.page == "asset-edit-electrical" ||
                     ControlCenter.Instance.page == "asset-edit-electrical_info" ||
                     ControlCenter.Instance.page == "asset-edit-electrical_img")
            {
                // Moving X
                double x = p[idx]["bcMenu_W"];
                double xInfoW = pageW - p[idx]["bcMenu_W"];

                // Nav Back
                p[idx]["bcNavBack_W"] = config_menuH * 0.8;
                p[idx]["bcNavBack_H"] = config_menuH;
                p[idx]["bcNavBack_X"] = x;
                p[idx]["bcNavBack_Y"] = 0;
                p[idx]["bcNavBack_V"] = 1;
                x += p[idx]["bcNavBack_W"];
                xInfoW -= p[idx]["bcNavBack_W"];

                // Nav Forward
                p[idx]["bcNavForward_W"] = config_menuH * 0.8;
                p[idx]["bcNavForward_H"] = config_menuH;
                p[idx]["bcNavForward_Y"] = 0;
                p[idx]["bcNavForward_V"] = -1;

                // Thumb
                p[idx]["cThumb_W"] = config_menuH * 1.6;
                p[idx]["cThumb_H"] = config_menuH * 1.6;
                p[idx]["cThumb_X"] = pageW - p[idx]["cThumb_W"];
                p[idx]["cThumb_Y"] = 0;
                p[idx]["cThumb_V"] = 1;
                xInfoW -= p[idx]["cThumb_W"];

                // Info
                p[idx]["cInfo_W"] = xInfoW;
                p[idx]["cInfo_H"] = config_menuH;
                p[idx]["cInfo_X"] = x;
                p[idx]["cInfo_Y"] = 0;
                p[idx]["cInfo_V"] = 1;
                x += p[idx]["cInfo_W"];

                // Info Labels
                p[idx]["labProject_W"] = xInfoW - 10;
                p[idx]["labProject_H"] = labProject.ActualHeight; // TEXT SIZE ADJUST - :TO:DO:
                p[idx]["labProject_X"] = 5; // Inside cInfo
                p[idx]["labProject_Y"] = p[idx]["cInfo_H"] / 2 - p[idx]["labAsset_H"] + 5;
                p[idx]["labProject_V"] = 1;

                p[idx]["labAsset_W"] = xInfoW - 10;
                p[idx]["labAsset_H"] = labAsset.ActualHeight; // TEXT SIZE ADJUST - :TO:DO:
                p[idx]["labAsset_X"] = 5; // Inside cInfo
                p[idx]["labAsset_Y"] = p[idx]["cInfo_H"] / 2;
                p[idx]["labAsset_V"] = 1;

                // Now Place
                p[idx]["bcNavForward_X"] = x;
            }
        }

        public void SetVisual(bool animate)
        {
            if (animate)
            {
                Storyboard story = new Storyboard();
                TimeSpan span = new TimeSpan(0,0,0,0,300);

                SetAnimateUI(bcMenu, "bcMenu", story,span);
                SetAnimateUI(bcNavBack, "bcNavBack", story, span);
                SetAnimateUI(bcNavForward, "bcNavForward", story, span);
                SetAnimateUI(cInfo, "cInfo", story, span);
                SetAnimateUI(labProject, "labProject", story, span);
                SetAnimateUI(labAsset, "labAsset", story, span);
                SetAnimateUI(cThumb, "cThumb", story, span);
                SetAnimateUI(cDropMenu, "cDropMenu", story, span);
                SetAnimateUI(cShadow, "cShadow", story, span);

                story.Completed += ((s, e) => SetVisual(false));
                story.Begin(this);
            }
            else
            {
                SetVisualUI(bcMenu, "bcMenu", 1);
                SetVisualUI(bcNavBack, "bcNavBack", 1);
                SetVisualUI(bcNavForward, "bcNavForward", 1);
                SetVisualUI(cInfo, "cInfo", 1);
                SetVisualUI(labProject, "labProject", 1);
                SetVisualUI(labAsset, "labAsset", 1);
                SetVisualUI(cThumb, "cThumb", 1);
                SetVisualUI(cDropMenu, "cDropMenu", 1);
                SetVisualUI(cShadow, "cShadow", 1);
            }
        }

        public void SetAnimateUI(Canvas c, string name, Storyboard sb, TimeSpan span)
        {
            CubicEase easing = new CubicEase();
            easing.EasingMode = EasingMode.EaseOut;

            if (p[0][name + "_W"] != p[1][name + "_W"])
            {
                DoubleAnimation animation = new DoubleAnimation();

                animation.EasingFunction = easing;
                animation.From = p[0][name + "_W"];
                animation.To = p[1][name + "_W"];
                animation.Duration = new Duration(span);
                // Configure the animation to target de property Opacity
                Storyboard.SetTargetName(animation, name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.WidthProperty));
                // Add the animation to the storyboard
                sb.Children.Add(animation);
            }

            if (p[0][name + "_H"] != p[1][name + "_H"])
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.EasingFunction = easing;

                animation.From = p[0][name + "_H"];
                animation.To = p[1][name + "_H"];
                animation.Duration = new Duration(span);
                // Configure the animation to target de property Opacity
                Storyboard.SetTargetName(animation, name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.HeightProperty));
                // Add the animation to the storyboard
                sb.Children.Add(animation);
            }

            if (p[0][name + "_X"] != p[1][name + "_X"])
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.EasingFunction = easing;

                animation.From = p[0][name + "_X"];
                animation.To = p[1][name + "_X"];
                animation.Duration = new Duration(span);
                // Configure the animation to target de property Opacity
                Storyboard.SetTargetName(animation, name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));
                // Add the animation to the storyboard
                sb.Children.Add(animation);
            }

            if (p[0][name + "_Y"] != p[1][name + "_Y"])
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.EasingFunction = easing;

                animation.From = p[0][name + "_Y"];
                animation.To = p[1][name + "_Y"];
                animation.Duration = new Duration(span);
                // Configure the animation to target de property Opacity
                Storyboard.SetTargetName(animation, name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.TopProperty));
                // Add the animation to the storyboard
                sb.Children.Add(animation);
            }

            if (p[0][name + "_V"] > 0 && p[1][name + "_V"] > 0)
            {
                c.Opacity = 1.0;
                c.Width = p[1][name + "_W"];
                c.Height = p[1][name + "_H"];
                Canvas.SetLeft(c, p[1][name + "_X"]);
                Canvas.SetTop(c, p[1][name + "_Y"]);
                c.Visibility = Visibility.Visible;                
            }
            else if (p[0][name + "_V"] < 0 && p[1][name + "_V"] < 0)
            {
                c.Opacity = 0.0;
                c.Width = p[1][name + "_W"];
                c.Height = p[1][name + "_H"];
                Canvas.SetLeft(c, p[1][name + "_X"]);
                Canvas.SetTop(c, p[1][name + "_Y"]);
                c.Visibility = Visibility.Hidden;
            }
            else if (p[0][name + "_V"] > 0 && p[1][name + "_V"] < 0)
            {
                // Place it at 0
                c.Opacity = 1.0;
                c.Width = p[0][name + "_W"];
                c.Height = p[0][name + "_H"];
                Canvas.SetLeft(c, p[0][name + "_X"]);
                Canvas.SetTop(c, p[0][name + "_Y"]);
                c.Visibility = Visibility.Visible;

                // We're hiding it
                DoubleAnimation animation = new DoubleAnimation();

                animation.From = 1.0;
                animation.To = 0.0;
                animation.Duration = new Duration(span);
                // Configure the animation to target de property Opacity
                Storyboard.SetTargetName(animation, name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.OpacityProperty));
                // Add the animation to the storyboard
                sb.Children.Add(animation);                
            }
            else if (p[0][name + "_V"] < 0 && p[1][name + "_V"] > 0)
            {
                // Place it at 0
                c.Opacity = 0.0;
                c.Width = p[1][name + "_W"];
                c.Height = p[1][name + "_H"];
                Canvas.SetLeft(c, p[1][name + "_X"]);
                Canvas.SetTop(c, p[1][name + "_Y"]);
                c.Visibility = Visibility.Visible;

                // We're showing it
                DoubleAnimation animation = new DoubleAnimation();

                animation.From = 0.0;
                animation.To = 1.0;
                animation.Duration = new Duration(span);
                // Configure the animation to target de property Opacity
                Storyboard.SetTargetName(animation, name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.OpacityProperty));
                // Add the animation to the storyboard
                sb.Children.Add(animation);
            }
            CopyVisualUI(name);
        }

        public void SetAnimateUI(Label c, string name, Storyboard sb, TimeSpan span)
        {
            CubicEase easing = new CubicEase();
            easing.EasingMode = EasingMode.EaseOut;

            if (p[0][name + "_W"] != p[1][name + "_W"])
            {
                DoubleAnimation animation = new DoubleAnimation();

                animation.EasingFunction = easing;
                animation.From = p[0][name + "_W"];
                animation.To = p[1][name + "_W"];
                animation.Duration = new Duration(span);
                // Configure the animation to target de property Opacity
                Storyboard.SetTargetName(animation, name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Label.WidthProperty));
                // Add the animation to the storyboard
                sb.Children.Add(animation);
            }

            if (p[0][name + "_H"] != p[1][name + "_H"])
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.EasingFunction = easing;

                animation.From = p[0][name + "_H"];
                animation.To = p[1][name + "_H"];
                animation.Duration = new Duration(span);
                // Configure the animation to target de property Opacity
                Storyboard.SetTargetName(animation, name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Label.HeightProperty));
                // Add the animation to the storyboard
                sb.Children.Add(animation);
            }

            if (p[0][name + "_X"] != p[1][name + "_X"])
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.EasingFunction = easing;

                animation.From = p[0][name + "_X"];
                animation.To = p[1][name + "_X"];
                animation.Duration = new Duration(span);
                // Configure the animation to target de property Opacity
                Storyboard.SetTargetName(animation, name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));
                // Add the animation to the storyboard
                sb.Children.Add(animation);
            }

            if (p[0][name + "_Y"] != p[1][name + "_Y"])
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.EasingFunction = easing;

                animation.From = p[0][name + "_Y"];
                animation.To = p[1][name + "_Y"];
                animation.Duration = new Duration(span);
                // Configure the animation to target de property Opacity
                Storyboard.SetTargetName(animation, name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.TopProperty));
                // Add the animation to the storyboard
                sb.Children.Add(animation);
            }

            if (p[0][name + "_V"] > 0 && p[1][name + "_V"] > 0)
            {
                c.Opacity = 1.0;
                c.Width = p[1][name + "_W"];
                c.Height = p[1][name + "_H"];
                Canvas.SetLeft(c, p[1][name + "_X"]);
                Canvas.SetTop(c, p[1][name + "_Y"]);
                c.Visibility = Visibility.Visible;
            }
            else if (p[0][name + "_V"] < 0 && p[1][name + "_V"] < 0)
            {
                c.Opacity = 0.0;
                c.Width = p[1][name + "_W"];
                c.Height = p[1][name + "_H"];
                Canvas.SetLeft(c, p[1][name + "_X"]);
                Canvas.SetTop(c, p[1][name + "_Y"]);
                c.Visibility = Visibility.Hidden;
            }
            else if (p[0][name + "_V"] > 0 && p[1][name + "_V"] < 0)
            {
                // Place it at 0
                c.Opacity = 1.0;
                c.Width = p[0][name + "_W"];
                c.Height = p[0][name + "_H"];
                Canvas.SetLeft(c, p[0][name + "_X"]);
                Canvas.SetTop(c, p[0][name + "_Y"]);
                c.Visibility = Visibility.Visible;

                // We're hiding it
                DoubleAnimation animation = new DoubleAnimation();

                animation.From = 1.0;
                animation.To = 0.0;
                animation.Duration = new Duration(span);
                // Configure the animation to target de property Opacity
                Storyboard.SetTargetName(animation, name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Label.OpacityProperty));
                // Add the animation to the storyboard
                sb.Children.Add(animation);
            }
            else if (p[0][name + "_V"] < 0 && p[1][name + "_V"] > 0)
            {
                // Place it at 0
                c.Opacity = 0.0;
                c.Width = p[1][name + "_W"];
                c.Height = p[1][name + "_H"];
                Canvas.SetLeft(c, p[1][name + "_X"]);
                Canvas.SetTop(c, p[1][name + "_Y"]);
                c.Visibility = Visibility.Visible;

                // We're showing it
                DoubleAnimation animation = new DoubleAnimation();

                animation.From = 0.0;
                animation.To = 1.0;
                animation.Duration = new Duration(span);
                // Configure the animation to target de property Opacity
                Storyboard.SetTargetName(animation, name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Label.OpacityProperty));
                // Add the animation to the storyboard
                sb.Children.Add(animation);
            }
            CopyVisualUI(name);
        }

        public void SetVisualUI(Canvas c, string name, int idx)
        {
            if (p[idx][name + "_V"] > 0)
            {
                c.Width = p[idx][name + "_W"];
                c.Height = p[idx][name + "_H"];
                Canvas.SetLeft(c, p[idx][name + "_X"]);
                Canvas.SetTop(c, p[idx][name + "_Y"]);
                c.Visibility = Visibility.Visible;
            }
            else
            {
                c.Visibility = Visibility.Hidden;
            }
            CopyVisualUI(name);
        }

        public void SetVisualUI(Label c, string name, int idx)
        {
            if (p[idx][name + "_V"] > 0)
            {
                c.Width = p[idx][name + "_W"];
                c.Height = p[idx][name + "_H"];
                Canvas.SetLeft(c, p[idx][name + "_X"]);
                Canvas.SetTop(c, p[idx][name + "_Y"]);
                c.Visibility = Visibility.Visible;
            }
            else
            {
                c.Visibility = Visibility.Hidden;
            }
            CopyVisualUI(name);
        }

        public void CopyVisualUI(string name)
        {
            p[0][name + "_W"] = p[1][name + "_W"];
            p[0][name + "_H"] = p[1][name + "_H"];
            p[0][name + "_X"] = p[1][name + "_X"];
            p[0][name + "_Y"] = p[1][name + "_Y"];
            p[0][name + "_V"] = p[1][name + "_V"];
        }

        private void bcMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            droppedDown = !droppedDown;

            Update();
        }

        private void cInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void cThumb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void cShadow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            droppedDown = false;

            Update();
        }

        private void bNavBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            droppedDown = false;
            ControlCenter.Instance.Menu_Back();
        }

        private void bNavForward_MouseDown(object sender, MouseButtonEventArgs e)
        {
            droppedDown = false;
            ControlCenter.Instance.Menu_Forward();
        }




        // Drop down Menu Events

        private void cDExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlCenter.Instance.Menu_Exit();
        }

        private void labProject_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Only send the area name change event if the page is correct
            if(ControlCenter.Instance.page.StartsWith("asset-"))
            {
                ControlCenter.Instance.ChangeAreaName();
                SetContent();
            }
        }
    }
}
