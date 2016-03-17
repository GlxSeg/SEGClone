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
    /// Interaction logic for Asset_Edit_Risk.xaml
    /// </summary>
    public partial class Asset_Edit_Risk : UserControl
    {
        public bool hasElectrical = false;
        public bool hasMechanical;

        public Asset_Edit_Risk()
        {
            InitializeComponent();
        }

        public void Update()
        {
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

            // Has Mechanical
            var assetInfo = ProjectHelper.GetAssetInfo(ControlCenter.Instance.segR, ControlCenter.Instance.cAssetId);
            hasMechanical = (assetInfo.FirstOrDefault(x => x.Key == "MECHDIAG").Value == "YES");

            if (hasMechanical)
                labName_S2.Foreground = new SolidColorBrush(Colors.White);
            else
                labName_S2.Foreground = new SolidColorBrush(Colors.Gray);

            // Setup Risk Buttons
            List<RiskAnalysis> risks = ProjectHelper.GetAssetRisk(ControlCenter.Instance.segR,
                                                                  ControlCenter.Instance.cAssetId);

            List<RiskAnalysisType> rTypes = ProjectHelper.GetRiskAnalysisTypes(ControlCenter.Instance.segR);

            rS = risks.FirstOrDefault(x => x.RiskAnalysisTypeId == rTypes.FirstOrDefault(y => y.Name == "Seriousness").Id);
            rE = risks.FirstOrDefault(x => x.RiskAnalysisTypeId == rTypes.FirstOrDefault(y => y.Name == "Exposure").Id);
            rO = risks.FirstOrDefault(x => x.RiskAnalysisTypeId == rTypes.FirstOrDefault(y => y.Name == "Occurrence").Id);
            rA = risks.FirstOrDefault(x => x.RiskAnalysisTypeId == rTypes.FirstOrDefault(y => y.Name == "Avoidance").Id);

            UpdateDisplay();
        }

        RiskAnalysis rS,rE,rO,rA;

        public void UpdateDisplay()
        {
            if(rS.Value == "S1")
            {
                bcR1_1.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                bcR1_2.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                labR1_1.Foreground = new SolidColorBrush(Colors.White);
                labR1_2.Foreground = new SolidColorBrush(Colors.Gray);
                labR1_1.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                labR1_2.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
            }
            else
            {
                bcR1_1.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                bcR1_2.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                labR1_1.Foreground = new SolidColorBrush(Colors.Gray);
                labR1_2.Foreground = new SolidColorBrush(Colors.White);
                labR1_1.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
                labR1_2.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            }

            if (rE.Value == "F1")
            {
                bcR2_1.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                bcR2_2.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                labR2_1.Foreground = new SolidColorBrush(Colors.White);
                labR2_2.Foreground = new SolidColorBrush(Colors.Gray);
                labR2_1.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                labR2_2.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
            }
            else
            {
                bcR2_1.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                bcR2_2.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                labR2_1.Foreground = new SolidColorBrush(Colors.Gray);
                labR2_2.Foreground = new SolidColorBrush(Colors.White);
                labR2_1.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
                labR2_2.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            }


            if (rO.Value == "VERY LOW")
            {
                bcR3_1.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                bcR3_2.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                bcR3_3.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                labR3_1.Foreground = new SolidColorBrush(Colors.White);
                labR3_2.Foreground = new SolidColorBrush(Colors.Gray);
                labR3_3.Foreground = new SolidColorBrush(Colors.Gray);
                labR3_1.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                labR3_2.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
                labR3_3.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
            }
            else if (rO.Value == "LOW")
            {
                bcR3_1.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                bcR3_2.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                bcR3_3.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                labR3_1.Foreground = new SolidColorBrush(Colors.Gray);
                labR3_2.Foreground = new SolidColorBrush(Colors.White);
                labR3_3.Foreground = new SolidColorBrush(Colors.Gray);
                labR3_1.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
                labR3_2.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                labR3_3.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
            }
            else
            {
                bcR3_1.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                bcR3_2.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                bcR3_3.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                labR3_1.Foreground = new SolidColorBrush(Colors.Gray);
                labR3_2.Foreground = new SolidColorBrush(Colors.Gray);
                labR3_3.Foreground = new SolidColorBrush(Colors.White);
                labR3_1.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
                labR3_2.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
                labR3_3.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            }

            if (rA.Value == "P1")
            {
                bcR4_1.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                bcR4_2.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                labR4_1.Foreground = new SolidColorBrush(Colors.White);
                labR4_2.Foreground = new SolidColorBrush(Colors.Gray);
                labR4_1.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                labR4_2.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
            }
            else
            {
                bcR4_1.Background = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                bcR4_2.Background = new SolidColorBrush(Color.FromArgb(255, 41, 113, 143));
                labR4_1.Foreground = new SolidColorBrush(Colors.Gray);
                labR4_2.Foreground = new SolidColorBrush(Colors.White);
                labR4_1.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
                labR4_2.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            }

            string srisk = "1";
            if(rS.Value == "S1")
            {
                if(rO.Value == "HIGH")
                {
                    srisk = "2";
                }
            }
            else if(rS.Value == "S2")
            {
                if(rE.Value == "F1")
                {
                    if(rO.Value == "LOW")
                    {
                        if (rA.Value == "P2")
                            srisk = "2";
                    }
                    else if(rO.Value == "HIGH")
                    {
                        if(rA.Value == "P1")
                            srisk = "2";
                        else
                            srisk = "3";
                    }
                }
                else
                {
                    if(rO.Value == "VERY LOW")
                    {
                        if(rA.Value == "P1")
                            srisk = "2";
                        else
                            srisk = "3";
                    }
                    else if(rO.Value == "LOW")
                    {
                        srisk = "3";
                    }
                    else if(rO.Value == "HIGH")
                    {
                        if (rA.Value == "P1")
                            srisk = "3";
                        else
                            srisk = "4";
                    }
                }
            }
            labR.Content = srisk;
        }

        public void SetSize(double cW, double cH)
        {
            this.Width = cW;
            this.Height = cH;

            cSubMenu.Width = cW;

            double xD = (cW - 40) / 5;
            
            // Set Columns
            labR1.Width = xD;
            labR2.Width = xD;
            labR3.Width = xD;
            labR4.Width = xD;
            labR5.Width = xD;
            labR.Width = xD;

            Canvas.SetLeft(rV1, 20 + xD);
            Canvas.SetLeft(rV2, 20 + xD * 2);
            Canvas.SetLeft(rV3, 20 + xD * 3);
            Canvas.SetLeft(rV4, 20 + xD * 4);
            rH.Width = cW - 40;

            bcR1_1.Width = xD - 20;
            bcR1_2.Width = xD - 20;
            bcR2_1.Width = xD - 20;
            bcR2_2.Width = xD - 20;
            bcR3_1.Width = xD - 20;
            bcR3_2.Width = xD - 20;
            bcR3_3.Width = xD - 20;
            bcR4_1.Width = xD - 20;
            bcR4_2.Width = xD - 20;

            labR1_1.Width = xD - 20;
            labR1_2.Width = xD - 20;
            labR2_1.Width = xD - 20;
            labR2_2.Width = xD - 20;
            labR3_1.Width = xD - 20;
            labR3_2.Width = xD - 20;
            labR3_3.Width = xD - 20;
            labR4_1.Width = xD - 20;
            labR4_2.Width = xD - 20;

            Canvas.SetLeft(labR1, 20);
            Canvas.SetLeft(labR2, 20 + xD * 1);
            Canvas.SetLeft(labR3, 20 + xD * 2);
            Canvas.SetLeft(labR4, 20 + xD * 3);
            Canvas.SetLeft(labR5, 20 + xD * 4);
            Canvas.SetLeft(labR, 20 + xD * 4);

            Canvas.SetLeft(bcR1_1, 20 + 10);
            Canvas.SetLeft(bcR1_2, 20 + 10);

            Canvas.SetLeft(bcR2_1, 20 + 10 + xD * 1);
            Canvas.SetLeft(bcR2_2, 20 + 10 + xD * 1);

            Canvas.SetLeft(bcR3_1, 20 + 10 + xD * 2);
            Canvas.SetLeft(bcR3_2, 20 + 10 + xD * 2);
            Canvas.SetLeft(bcR3_3, 20 + 10 + xD * 2);

            Canvas.SetLeft(bcR4_1, 20 + 10 + xD * 3);
            Canvas.SetLeft(bcR4_2, 20 + 10 + xD * 3);
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

        private void cSubMenu_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(hasMechanical)
                ControlCenter.Instance.Asset_SelectMechanical();
        }

        private void bcR1_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rS.Value = "S1";
            ProjectHelper.SetAssetRisk(ControlCenter.Instance.segR, rS.Id, rS.Value);
            UpdateDisplay();
        }

        private void bcR1_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rS.Value = "S2";
            ProjectHelper.SetAssetRisk(ControlCenter.Instance.segR, rS.Id, rS.Value);
            UpdateDisplay();
        }

        private void bcR2_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rE.Value = "F1";
            ProjectHelper.SetAssetRisk(ControlCenter.Instance.segR, rE.Id, rE.Value);
            UpdateDisplay();
        }

        private void bcR2_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rE.Value = "F2";
            ProjectHelper.SetAssetRisk(ControlCenter.Instance.segR, rE.Id, rE.Value);
            UpdateDisplay();
        }

        private void bcR3_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rO.Value = "VERY LOW";
            ProjectHelper.SetAssetRisk(ControlCenter.Instance.segR, rO.Id, rO.Value);
            UpdateDisplay();
        }

        private void bcR3_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rO.Value = "LOW";
            ProjectHelper.SetAssetRisk(ControlCenter.Instance.segR, rO.Id, rO.Value);
            UpdateDisplay();
        }

        private void bcR3_3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rO.Value = "HIGH";
            ProjectHelper.SetAssetRisk(ControlCenter.Instance.segR, rO.Id, rO.Value);
            UpdateDisplay();
        }

        private void bcR4_1_MouseDown(object sender, MouseEventArgs e)
        {
            rA.Value = "P1";
            ProjectHelper.SetAssetRisk(ControlCenter.Instance.segR, rA.Id, rA.Value);
            UpdateDisplay();
        }

        private void bcR4_2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rA.Value = "P2";
            ProjectHelper.SetAssetRisk(ControlCenter.Instance.segR, rA.Id, rA.Value);
            UpdateDisplay();
        }


    }
}
