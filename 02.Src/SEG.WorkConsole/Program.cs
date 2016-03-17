using SEG.Domain;
using SEG.Domain.Helpers;
using SEG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.WorkConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //string path = @"C:\Develop\SEG\Multimedia\Taschibra\";

            //for (int i = 1; i <= 135; i++)
            //{
            //    Bitmap img = new Bitmap(path + i.ToString() + ".jpg");

            //    int iW = img.Width;
            //    int iH = img.Height;

            //    Bitmap img2;

            //    if (iW > iH)
            //    {
            //        int idx = (iW - iH) / 2;
            //        img2 = new Bitmap(iH, iH, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //        img2.SetResolution(img.HorizontalResolution, img.VerticalResolution);
            //        Graphics g = Graphics.FromImage(img2);
            //        g.DrawImage(img, -idx, 0);
            //    }
            //    else
            //    {
            //        int idx = (iH - iW) / 2;
            //        img2 = new Bitmap(iW, iW, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //        img2.SetResolution(img.HorizontalResolution, img.VerticalResolution);
            //        Graphics g = Graphics.FromImage(img2);
            //        g.DrawImage(img, 0, -idx);
            //    }

            //    img2.Save(path + @"Final\" + i.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //}

            //SEGRepository segR = new SEGRepository(@"Server=ONEFLOW-DEV\ONEFLOW;Database=seg;User Id=sa;Password=oneflow;");

            //// Select the project GUID
            //Guid pId = new Guid("879001B7-52CA-E411-A640-005056C00008");
            //StreamWriter sw = new StreamWriter("assetrisk.csv");

            //List<RiskAnalysisType> rTypes = ProjectHelper.GetRiskAnalysisTypes(segR);
            //List<ProjectArea> areas = ProjectHelper.GetProjectAreas(segR, pId);
            //RiskAnalysis rS, rE, rO, rA;

            //foreach(var area in areas)
            //{
            //    List<Asset> assets = ProjectHelper.GetAssets(segR, area.Id);
            //    foreach(var asset in assets)
            //    {
            //        List<AssetInfo> aInfo = ProjectHelper.GetAssetInfo(segR, asset.Id);
            //        List<RiskAnalysis> risks = ProjectHelper.GetAssetRisk(segR, asset.Id);

            //        rS = risks.FirstOrDefault(x => x.RiskAnalysisTypeId == rTypes.FirstOrDefault(y => y.Name == "Seriousness").Id);
            //        rE = risks.FirstOrDefault(x => x.RiskAnalysisTypeId == rTypes.FirstOrDefault(y => y.Name == "Exposure").Id);
            //        rO = risks.FirstOrDefault(x => x.RiskAnalysisTypeId == rTypes.FirstOrDefault(y => y.Name == "Occurrence").Id);
            //        rA = risks.FirstOrDefault(x => x.RiskAnalysisTypeId == rTypes.FirstOrDefault(y => y.Name == "Avoidance").Id);

            //        string srisk = "1";
            //        if (rS.Value == "S2")
            //        {
            //            if (rE.Value == "F1")
            //            {
            //                if (rO.Value == "LOW")
            //                {
            //                    if (rA.Value == "P2")
            //                        srisk = "2";
            //                }
            //                else if (rO.Value == "HIGH")
            //                {
            //                    if (rA.Value == "P1")
            //                        srisk = "2";
            //                    else
            //                        srisk = "3";
            //                }
            //            }
            //            else
            //            {
            //                if (rO.Value == "VERY LOW")
            //                {
            //                    if (rA.Value == "P1")
            //                        srisk = "2";
            //                    else
            //                        srisk = "3";
            //                }
            //                else if (rO.Value == "LOW")
            //                {
            //                    srisk = "3";
            //                }
            //                else if (rO.Value == "HIGH")
            //                {
            //                    if (rA.Value == "P1")
            //                        srisk = "3";
            //                    else
            //                        srisk = "4";
            //                }
            //            }
            //        }

            //        // Now we print out the line
            //        sw.WriteLine(
            //            area.Name + ";" +
            //            aInfo.FirstOrDefault(x => x.Key == "ID").Value + ";" +
            //            aInfo.FirstOrDefault(x => x.Key == "TAG").Value + ";" +
            //            aInfo.FirstOrDefault(x => x.Key == "DESCRIPTION").Value + ";" +
            //            aInfo.FirstOrDefault(x => x.Key == "LOCATION").Value + ";" +
            //            srisk
            //            );

            //    }
            //}

            //sw.Close();

            //// Let's fix the diagnostics
            //List<Asset> aa = null;
            //using(var seg = segR.GetContext())
            //{
            //    aa = seg.Assets.ToList();                
            //}

            //foreach(var a in aa)
            //{
            //    using (var seg = segR.GetContext())
            //    {
            //        DiagnosticsType dt = seg.DiagnosticsTypes.FirstOrDefault(t => t.Name == "Electrical");
            //        var de = seg.Diagnostics.Where(x => x.AssetId == a.Id && x.DiagnosticsTypeId == dt.Id).ToList();

            //        if (de.Count > 1)
            //        {
            //            for (int i = 1; i <= de.Count; i++)
            //            {
            //                Diagnostics d = de[i - 1];
            //                System.Console.WriteLine(d.Id + " " + d.Index);
            //                d.Index = i;
            //            }
            //        }

            //        if (de.Count > 0)
            //        {
            //            for (int i = 1; i <= de.Count; i++)
            //            {
            //                AssetInfo ai = new AssetInfo();
            //                ai.Key = "EDIAG_" + i + "_INFO1";
            //                ai.Value = "";
            //                ai.AssetId = a.Id;
            //                seg.AssetInfos.Add(ai);

            //                ai = new AssetInfo();
            //                ai.Key = "EDIAG_" + i + "_INFO2";
            //                ai.Value = "";
            //                ai.AssetId = a.Id;
            //                seg.AssetInfos.Add(ai);

            //                ai = new AssetInfo();
            //                ai.Key = "EDIAG_" + i + "_INFO3";
            //                ai.Value = "";
            //                ai.AssetId = a.Id;
            //                seg.AssetInfos.Add(ai);

            //                ai = new AssetInfo();
            //                ai.Key = "EDIAG_" + i + "_INFO4";
            //                ai.Value = "";
            //                ai.AssetId = a.Id;
            //                seg.AssetInfos.Add(ai);

            //            }
            //        }
            //        seg.SaveChanges();
            //    }
            //}
            //System.Console.WriteLine("End");

            //AssetPrinter ap = new AssetPrinter(segR, "asset.pdf", new Guid("94F6FABC-52CA-E411-A640-005056C00008"));
            //ap.Load();
            //ap.Print();

            //SqlConnection.ClearAllPools();
            //SEGRepository segR = new SEGRepository(@"Server=SOLANO\NR12;Database=seg;User Id=sa;Password=nr12;");

            SEGRepository segR = new SEGRepository(@"Server=OFDEV01\OFDB;Database=seg;User Id=sa;Password=1;");

            using (SEGContext seg = segR.GetContext())
            {

                RiskAnalysisType rTG = new RiskAnalysisType();
                rTG.Name = "Seriousness";
                rTG.Description = "Gravidade";
                seg.RiskAnalysisTypes.Add(rTG);

                RiskAnalysisType rTE = new RiskAnalysisType();
                rTE.Name = "Exposure";
                rTE.Description = "Exposição";
                seg.RiskAnalysisTypes.Add(rTE);

                RiskAnalysisType rTP = new RiskAnalysisType();
                rTP.Name = "Occurrence";
                rTP.Description = "Possibilidade de ocorrer o evento perigoso";
                seg.RiskAnalysisTypes.Add(rTP);

                RiskAnalysisType rTR = new RiskAnalysisType();
                rTR.Name = "Avoidance";
                rTR.Description = "Possibilidade de evitar o risco";
                seg.RiskAnalysisTypes.Add(rTR);

                seg.SaveChanges();
            }

            //DBLoad_Standards.Load_Types(segR);
            //DBLoad_Standards.Load_General(segR);
            //DBLoad_Standards.Load_Mechanical(segR);

            //DBLoad_Project.Load(segR);

            //DBFix_Mechanical.FixMechDiag(segR);
            //DBFix_Mechanical.ChangeAssets(segR);

        }
    }
}
