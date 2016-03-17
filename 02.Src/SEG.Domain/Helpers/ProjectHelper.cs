using SEG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Helpers
{
    static public class ProjectHelper
    {
        static public List<Project> GetProjects(SEGRepository segRep)
        {
            List<Project> projects = new List<Project>();
            using (SEGContext seg = segRep.GetContext())
            {
                projects = seg.Projects.ToList();
            }
            return projects;
        }

        static public Project GetProject(SEGRepository segRep, Guid pId)
        {
            Project project = null;
            using (SEGContext seg = segRep.GetContext())
            {
                project = seg.Projects.FirstOrDefault(x => x.Id == pId);
            }
            return project;
        }

        static public string GetProjectCode(SEGRepository segRep, Guid pId)
        {
            string code = null;
            using (SEGContext seg = segRep.GetContext())
            {
                code = seg.ProjectInfos.FirstOrDefault(x => x.ProjectId == pId && x.Key == "CODE").Value;
            }
            return code;
        }

        static public List<ProjectArea> GetProjectAreas(SEGRepository segRep, Guid projectId)
        {
            List<ProjectArea> areas = new List<ProjectArea>();
            using (SEGContext seg = segRep.GetContext())
            {
                areas = seg.ProjectAreas.Where(a => a.ProjectId == projectId).ToList();
            }

            return areas;
        }

        static public ProjectArea GetProjectArea(SEGRepository segRep, Guid aId)
        {
            ProjectArea area = null;
            using (SEGContext seg = segRep.GetContext())
            {
                area = seg.ProjectAreas.FirstOrDefault(x => x.Id == aId);
            }
            return area;
        }

        static public Guid AddArea(SEGRepository segRep, Guid pId, string areaName)
        {
            Guid areaId = Guid.Empty;

            using (SEGContext seg = segRep.GetContext())
            {
                ProjectArea a = new ProjectArea();
                a.ProjectId = pId;
                a.Name = areaName;
                a.ImageId = Guid.Empty;
                a.Active = true;
                seg.ProjectAreas.Add(a);
                seg.SaveChanges();
                areaId = a.Id;

                var icGen = seg.ItemClasses.FirstOrDefault(x => x.Name == "General");
                var dtGen = seg.DiagnosticsTypes.FirstOrDefault(x => x.Name == "General");
                var dtGItems = seg.Items.Where(x => x.ItemClassId == icGen.Id).OrderBy(x => x.Index).ToList();

                Diagnostics d = new Diagnostics();
                d.AreaId = a.Id;
                d.AssetId = Guid.Empty;
                d.DiagnosticsTypeId = dtGen.Id;
                d.Index = 0;
                seg.Diagnostics.Add(d);
                seg.SaveChanges();

                foreach (Item i in dtGItems)
                {
                    DiagnosticsDetail dd = new DiagnosticsDetail();
                    dd.Comments = "";
                    dd.DiagnosticsId = d.Id;
                    dd.EntryDate = DateTime.MinValue;
                    dd.ExecutorId = Guid.Empty;
                    dd.ImageId = Guid.Empty;
                    dd.ItemId = i.Id;
                    dd.Status = "_new_";
                    dd.VerifierId = Guid.Empty;
                    dd.VerifyDate = DateTime.MinValue;
                    seg.DiagnosticsDetails.Add(dd);
                }
                seg.SaveChanges();
            }

            return areaId;
        }

        static public void ChangeAreaName(SEGRepository segRep, Guid projectAreaId, string newName)
        {
            using (SEGContext seg = segRep.GetContext())
            {
                var area = seg.ProjectAreas.FirstOrDefault(x => x.Id == projectAreaId);
                area.Name = newName;
                seg.SaveChanges();
            }
        }

        static public bool CheckUniqueAreaName(SEGRepository segRep, string newName)
        {
            bool isNew = true;

            using (SEGContext seg = segRep.GetContext())
            {
                var area = seg.ProjectAreas.FirstOrDefault(x => x.Name == newName);
                if (area != null)
                    isNew = false;
            }

            return isNew;
        }









        static public bool CheckUniqueAssetID(SEGRepository segRep, string newID)
        {
            bool isNew = true;

            using (SEGContext seg = segRep.GetContext())
            {
                var assetInfo = seg.AssetInfos.FirstOrDefault(x => x.Key == "ID" && x.Value == newID);
                if (assetInfo != null)
                    isNew = false;
            }

            return isNew;
        }







        static public List<Asset> GetAssets(SEGRepository segRep, Guid projectAreaId)
        {
            List<Asset> assets = new List<Asset>();
            using (SEGContext seg = segRep.GetContext())
            {
                assets = seg.Assets.Where(a => a.ProjectAreaId == projectAreaId).ToList();
            }

            return assets;
        }

        static public Asset GetAsset(SEGRepository segRep, Guid aId)
        {
            Asset asset = null;
            using (SEGContext seg = segRep.GetContext())
            {
                asset = seg.Assets.FirstOrDefault(x => x.Id == aId);
            }
            return asset;
        }

        static public List<AssetInfo> GetAssetInfo(SEGRepository segRep, Guid aId)
        {
            List<AssetInfo> aInfo = new List<AssetInfo>();
            using (SEGContext seg = segRep.GetContext())
            {
                aInfo = seg.AssetInfos.Where(x => x.AssetId == aId).ToList();
                // Make sure that the MECHDIAG flag exists for the asset, set to true if not
                if(!aInfo.Exists(x=>x.Key=="MECHDIAG"))
                {
                    AssetInfo ai = new AssetInfo();
                    ai.AssetId = aId;
                    ai.Key = "MECHDIAG";
                    ai.Value = "YES";
                    seg.AssetInfos.Add(ai);
                    seg.SaveChanges();
                    aInfo.Add(ai);
                }
            }
            return aInfo;
        }

        static public void ModifyAssetInfo(SEGRepository segR, Guid aId, string key, string value)
        {
            using (SEGContext seg = segR.GetContext())
            {
                AssetInfo aInfo = seg.AssetInfos.FirstOrDefault(x => x.AssetId == aId && x.Key == key);

                if(aInfo==null)
                {
                    AssetInfo ai = new AssetInfo();
                    ai.AssetId = aId;
                    ai.Key = key;
                    ai.Value = value;
                    seg.AssetInfos.Add(ai);
                }
                else
                    aInfo.Value = value;

                seg.SaveChanges();
            }
        }

        static public void SetAssetImage(SEGRepository segR, Guid assetId, Guid imgId)
        {
            using (SEGContext seg = segR.GetContext())
            {
                Asset a = seg.Assets.FirstOrDefault(x => x.Id == assetId);
                a.ImageId = imgId;
                seg.SaveChanges();
            }
        }

        static public void ClearAssetImage(SEGRepository segR, Guid assetId)
        {
            using (SEGContext seg = segR.GetContext())
            {
                Asset a = seg.Assets.FirstOrDefault(x => x.Id == assetId);
                a.ImageId = Guid.Empty;
                seg.SaveChanges();
            }
        }



        static public Guid AddAsset(SEGRepository segR, Guid areaId, Guid userId, 
            string assetId, string assetTag, string assetLoc, string assetDesc)
        {
            List<RiskAnalysisType> rTypes = ProjectHelper.GetRiskAnalysisTypes(segR);

            var rTS = rTypes.FirstOrDefault(y => y.Name == "Seriousness");
            var rTE = rTypes.FirstOrDefault(y => y.Name == "Exposure");
            var rTO = rTypes.FirstOrDefault(y => y.Name == "Occurrence");
            var rTA = rTypes.FirstOrDefault(y => y.Name == "Avoidance");

            Guid id = Guid.Empty;
            using (SEGContext seg = segR.GetContext())
            {
                DiagnosticsType dtMec = seg.DiagnosticsTypes.FirstOrDefault(a => a.Name == "Mechanical");
                ItemClass icMec = seg.ItemClasses.FirstOrDefault(a => a.Name == "Mechanical");
                List<Item> dtMItems = seg.Items.Where(x => x.ItemClassId == icMec.Id).OrderBy(x => x.Index).ToList();                

                Asset asset = new Asset();
                asset.ProjectAreaId = areaId;
                asset.ExecutorId = userId;
                asset.VerifierId = Guid.Empty;
                asset.ApproverId = Guid.Empty;
                asset.ImageId = Guid.Empty;
                asset.EntryDate = DateTime.Now;
                asset.VerifyDate = DateTime.MinValue;
                asset.ApprovalDate = DateTime.MinValue;
                seg.Assets.Add(asset);
                seg.SaveChanges();

                AssetInfo aInfo = new AssetInfo();
                aInfo.AssetId = asset.Id;
                aInfo.Key = "ID";
                aInfo.Value = assetId;
                seg.AssetInfos.Add(aInfo);

                aInfo = new AssetInfo();
                aInfo.AssetId = asset.Id;
                aInfo.Key = "TAG";
                aInfo.Value = assetTag;
                seg.AssetInfos.Add(aInfo);

                aInfo = new AssetInfo();
                aInfo.AssetId = asset.Id;
                aInfo.Key = "LOCATION";
                aInfo.Value = assetLoc;
                seg.AssetInfos.Add(aInfo);

                aInfo = new AssetInfo();
                aInfo.AssetId = asset.Id;
                aInfo.Key = "DESCRIPTION";
                aInfo.Value = assetDesc;
                seg.AssetInfos.Add(aInfo);


                RiskAnalysis r = new RiskAnalysis();
                r.AssetId = asset.Id;
                r.EntryDate = DateTime.MinValue;
                r.ExecutorId = Guid.Empty;
                r.RiskAnalysisTypeId = rTS.Id;
                r.VerifierId = Guid.Empty;
                r.VerifyDate = DateTime.MinValue;
                r.Value = "S1";
                seg.RiskAnalyses.Add(r);

                r = new RiskAnalysis();
                r.AssetId = asset.Id;
                r.EntryDate = DateTime.MinValue;
                r.ExecutorId = Guid.Empty;
                r.RiskAnalysisTypeId = rTE.Id;
                r.VerifierId = Guid.Empty;
                r.VerifyDate = DateTime.MinValue;
                r.Value = "F1";
                seg.RiskAnalyses.Add(r);

                r = new RiskAnalysis();
                r.AssetId = asset.Id;
                r.EntryDate = DateTime.MinValue;
                r.ExecutorId = Guid.Empty;
                r.RiskAnalysisTypeId = rTO.Id;
                r.VerifierId = Guid.Empty;
                r.VerifyDate = DateTime.MinValue;
                r.Value = "VERY LOW";
                seg.RiskAnalyses.Add(r);

                r = new RiskAnalysis();
                r.AssetId = asset.Id;
                r.EntryDate = DateTime.MinValue;
                r.ExecutorId = Guid.Empty;
                r.RiskAnalysisTypeId = rTA.Id;
                r.VerifierId = Guid.Empty;
                r.VerifyDate = DateTime.MinValue;
                r.Value = "P1";
                seg.RiskAnalyses.Add(r);

                // Mechanical Diagnostics
                Diagnostics d = new Diagnostics();
                d.AreaId = Guid.Empty;
                d.AssetId = asset.Id;
                d.Index = 0;
                d.DiagnosticsTypeId = dtMec.Id;
                seg.Diagnostics.Add(d);
                seg.SaveChanges();

                foreach (Item i in dtMItems)
                {
                    DiagnosticsDetail dd = new DiagnosticsDetail();
                    dd.Comments = "";
                    dd.DiagnosticsId = d.Id;
                    dd.EntryDate = DateTime.MinValue;
                    dd.ExecutorId = Guid.Empty;
                    dd.ImageId = Guid.Empty;
                    dd.ItemId = i.Id;
                    dd.Status = "_new_";
                    dd.VerifierId = Guid.Empty;
                    dd.VerifyDate = DateTime.MinValue;
                    seg.DiagnosticsDetails.Add(dd);
                }
                seg.SaveChanges();
                id = asset.Id;
            }
            return id;
        }

        static public void DeleteAsset(SEGRepository segR, Guid assetId)
        {
            var mechT = DiagnosticsHelper.GetMechanicalDiagnostics(segR, assetId);
            var elecT = DiagnosticsHelper.GetElectricalDiagnostics(segR, assetId);

            using (SEGContext seg = segR.GetContext())
            {
                List<DiagnosticsDetail> dd = seg.DiagnosticsDetails.Where(x => x.DiagnosticsId == mechT.Id).ToList();
                foreach (var d in dd)
                {
                    List<DiagnosticsDetailInfo> ddi = seg.DiagnosticsDetailInfos.Where(x => x.DiagnosticDetailId == d.Id).ToList();
                    foreach (var di in ddi)
                        seg.DiagnosticsDetailInfos.Remove(di);
                    seg.DiagnosticsDetails.Remove(d);
                }

                var mT = seg.Diagnostics.FirstOrDefault(x => x.Id == mechT.Id);
                seg.Diagnostics.Remove(mT);

                foreach (var e in elecT)
                {
                    List<DiagnosticsDetail> edd = seg.DiagnosticsDetails.Where(x => x.DiagnosticsId == e.Id).ToList();
                    foreach (var d in edd)
                    {
                        List<DiagnosticsDetailInfo> eddi = seg.DiagnosticsDetailInfos.Where(x => x.DiagnosticDetailId == d.Id).ToList();
                        foreach (var di in eddi)
                            seg.DiagnosticsDetailInfos.Remove(di);
                        seg.DiagnosticsDetails.Remove(d);
                    }
                    var mE = seg.Diagnostics.FirstOrDefault(x => x.Id == e.Id);
                    seg.Diagnostics.Remove(mE);
                }

                Asset a = seg.Assets.FirstOrDefault(x => x.Id == assetId);
                seg.Assets.Remove(a);

                seg.SaveChanges();
            }
        }

        static public void DeleteArea(SEGRepository segR, Guid areaId)
        {
            var mechT = DiagnosticsHelper.GetAreaDiagnostics(segR, areaId);

            using (SEGContext seg = segR.GetContext())
            {
                List<DiagnosticsDetail> dd = seg.DiagnosticsDetails.Where(x => x.DiagnosticsId == mechT.Id).ToList();
                foreach (var d in dd)
                {
                    List<DiagnosticsDetailInfo> ddi = seg.DiagnosticsDetailInfos.Where(x => x.DiagnosticDetailId == d.Id).ToList();
                    foreach (var di in ddi)
                        seg.DiagnosticsDetailInfos.Remove(di);
                    seg.DiagnosticsDetails.Remove(d);
                }

                var mT = seg.Diagnostics.FirstOrDefault(x => x.Id == mechT.Id);
                seg.Diagnostics.Remove(mT);


                var a = seg.ProjectAreas.FirstOrDefault(x=>x.Id == areaId);
                // Area should be empty
                seg.ProjectAreas.Remove(a);

                seg.SaveChanges();
            }
        }

        static public void DeleteProject(SEGRepository segR, Guid projectId)
        {

            using (SEGContext seg = segR.GetContext())
            {
                var a = seg.Projects.FirstOrDefault(x => x.Id == projectId);
                // Area should be empty
                seg.Projects.Remove(a);

                seg.SaveChanges();
            }
        }

        static public void DeleteElectricalDiagnostics(SEGRepository segR, Guid assetId,Guid diagId)
        {
            using (SEGContext seg = segR.GetContext())
            {
                List<DiagnosticsDetail> edd = seg.DiagnosticsDetails.Where(x => x.DiagnosticsId == diagId).ToList();
                foreach (var d in edd)
                {
                    List<DiagnosticsDetailInfo> eddi = seg.DiagnosticsDetailInfos.Where(x => x.DiagnosticDetailId == d.Id).ToList();
                    foreach (var di in eddi)
                        seg.DiagnosticsDetailInfos.Remove(di);
                    seg.DiagnosticsDetails.Remove(d);
                }
                var mE = seg.Diagnostics.FirstOrDefault(x => x.Id == diagId);
                seg.Diagnostics.Remove(mE);

                seg.SaveChanges();
            }
        }




















































        static public void CopyDiagnosticsToArea(SEGRepository segR, Guid areaFromId,Guid areaToId)
        {
            var genF = DiagnosticsHelper.GetAreaDiagnostics(segR, areaFromId);
            var genT = DiagnosticsHelper.GetAreaDiagnostics(segR, areaToId);

            using (SEGContext seg = segR.GetContext())
            {
                // Clear out the old diagnostics
                List<DiagnosticsDetail> dd = seg.DiagnosticsDetails.Where(x => x.DiagnosticsId == genT.Id).ToList();
                foreach (var d in dd)
                {
                    List<DiagnosticsDetailInfo> ddi = seg.DiagnosticsDetailInfos.Where(x => x.DiagnosticDetailId == d.Id).ToList();
                    foreach (var di in ddi)
                        seg.DiagnosticsDetailInfos.Remove(di);
                    seg.DiagnosticsDetails.Remove(d);
                }

                seg.SaveChanges();

                dd = seg.DiagnosticsDetails.Where(x => x.DiagnosticsId == genF.Id).ToList();
                foreach (var ad in dd)
                {
                    DiagnosticsDetail ndd = new DiagnosticsDetail();
                    ndd.Comments = ad.Comments;
                    ndd.DiagnosticsId = genT.Id;
                    ndd.EntryDate = ad.EntryDate;
                    ndd.ExecutorId = ad.ExecutorId;

                    if (ad.ImageId == Guid.Empty)
                        ndd.ImageId = Guid.Empty;
                    else
                        ndd.ImageId = ImageHelper.CopyImage(segR, ad.ImageId);

                    ndd.ItemId = ad.ItemId;
                    ndd.Status = ad.Status;
                    ndd.VerifierId = ad.VerifierId;
                    ndd.VerifyDate = ad.VerifyDate;
                    seg.DiagnosticsDetails.Add(ndd);
                    seg.SaveChanges();

                    List<DiagnosticsDetailInfo> ddi = seg.DiagnosticsDetailInfos.Where(x => x.DiagnosticDetailId == ad.Id).ToList();
                    foreach (var addi in ddi)
                    {
                        DiagnosticsDetailInfo nddi = new DiagnosticsDetailInfo();
                        nddi.DiagnosticDetailId = ndd.Id;
                        nddi.Key = addi.Key;
                        nddi.Value = addi.Value;
                        seg.DiagnosticsDetailInfos.Add(nddi);
                    }
                    seg.SaveChanges();
                }
            }
        }

        static public void CopyDiagnosticsToAsset(SEGRepository segR, Guid assetFromId,Guid assetToId)
        {
            var mechF = DiagnosticsHelper.GetMechanicalDiagnostics(segR, assetFromId);
            var elecF = DiagnosticsHelper.GetElectricalDiagnostics(segR, assetFromId);

            var mechT = DiagnosticsHelper.GetMechanicalDiagnostics(segR, assetToId);
            var elecT = DiagnosticsHelper.GetElectricalDiagnostics(segR, assetToId);

            using (SEGContext seg = segR.GetContext())
            {
                // Clear out the old diagnostics
                List<DiagnosticsDetail> dd = seg.DiagnosticsDetails.Where(x=>x.DiagnosticsId == mechT.Id).ToList();
                foreach(var d in dd)
                {
                    List<DiagnosticsDetailInfo> ddi = seg.DiagnosticsDetailInfos.Where(x=>x.DiagnosticDetailId == d.Id).ToList();
                    foreach(var di in ddi)
                        seg.DiagnosticsDetailInfos.Remove(di);
                    seg.DiagnosticsDetails.Remove(d);
                }

                // Don't remove the diagnostics
                //var mT = seg.Diagnostics.FirstOrDefault(x => x.Id == mechT.Id);
                //seg.Diagnostics.Remove(mT);

                foreach(var e in elecT)
                {
                    List<DiagnosticsDetail> edd = seg.DiagnosticsDetails.Where(x=>x.DiagnosticsId == e.Id).ToList();
                    foreach(var d in edd)
                    {
                        List<DiagnosticsDetailInfo> eddi = seg.DiagnosticsDetailInfos.Where(x=>x.DiagnosticDetailId == d.Id).ToList();
                        foreach(var di in eddi)
                            seg.DiagnosticsDetailInfos.Remove(di);
                        seg.DiagnosticsDetails.Remove(d);
                    }

                    // Don't remove the diagnostics
                    //var mE = seg.Diagnostics.FirstOrDefault(x => x.Id == e.Id);
                    //seg.Diagnostics.Remove(mE);
                }
                seg.SaveChanges();







                // Now copy from asset
                //Diagnostics nD = new Diagnostics();
                //nD.AreaId = Guid.Empty;
                //nD.AssetId = assetToId;
                //nD.DiagnosticsTypeId = mechF.DiagnosticsTypeId;
                //nD.Index = mechF.Index;
                //seg.Diagnostics.Add(nD);
                //seg.SaveChanges();
               
                // Copy all details
                dd = seg.DiagnosticsDetails.Where(x => x.DiagnosticsId == mechF.Id).ToList();
                foreach(var ad in dd)
                {
                    DiagnosticsDetail ndd = new DiagnosticsDetail();
                    ndd.Comments = ad.Comments;
                    ndd.DiagnosticsId = mechT.Id;
                    ndd.EntryDate = ad.EntryDate;
                    ndd.ExecutorId = ad.ExecutorId;

                    if(ad.ImageId == Guid.Empty)
                        ndd.ImageId = Guid.Empty;
                    else
                        ndd.ImageId = ImageHelper.CopyImage(segR, ad.ImageId);
                    
                    ndd.ItemId = ad.ItemId;
                    ndd.Status = ad.Status;
                    ndd.VerifierId = ad.VerifierId;
                    ndd.VerifyDate = ad.VerifyDate;
                    seg.DiagnosticsDetails.Add(ndd);
                    seg.SaveChanges();

                    List<DiagnosticsDetailInfo> ddi = seg.DiagnosticsDetailInfos.Where(x => x.DiagnosticDetailId == ad.Id).ToList();
                    foreach(var addi in ddi)
                    {
                        DiagnosticsDetailInfo nddi = new DiagnosticsDetailInfo();
                        nddi.DiagnosticDetailId = ndd.Id;
                        nddi.Key = addi.Key;
                        nddi.Value = addi.Value;
                        seg.DiagnosticsDetailInfos.Add(nddi);
                    }
                    seg.SaveChanges();
                }

                int ieT=-1;
                foreach(var e in elecF)
                {
                    ieT++;
                    var nD = elecT[ieT];

                    // Copy all details
                    dd = seg.DiagnosticsDetails.Where(x => x.DiagnosticsId == e.Id).ToList();
                    foreach (var ad in dd)
                    {
                        DiagnosticsDetail ndd = new DiagnosticsDetail();
                        ndd.Comments = ad.Comments;
                        ndd.DiagnosticsId = nD.Id;
                        ndd.EntryDate = ad.EntryDate;
                        ndd.ExecutorId = ad.ExecutorId;

                        if (ad.ImageId == Guid.Empty)
                            ndd.ImageId = Guid.Empty;
                        else
                            ndd.ImageId = ImageHelper.CopyImage(segR, ad.ImageId);

                        ndd.ItemId = ad.ItemId;
                        ndd.Status = ad.Status;
                        ndd.VerifierId = ad.VerifierId;
                        ndd.VerifyDate = ad.VerifyDate;
                        seg.DiagnosticsDetails.Add(ndd);
                        seg.SaveChanges();

                        List<DiagnosticsDetailInfo> ddi = seg.DiagnosticsDetailInfos.Where(x => x.DiagnosticDetailId == ad.Id).ToList();
                        foreach (var addi in ddi)
                        {
                            DiagnosticsDetailInfo nddi = new DiagnosticsDetailInfo();
                            nddi.DiagnosticDetailId = ndd.Id;
                            nddi.Key = addi.Key;
                            nddi.Value = addi.Value;
                            seg.DiagnosticsDetailInfos.Add(nddi);
                        }
                        seg.SaveChanges();
                    }
                }
            }
        }




















































        static public List<RiskAnalysis> GetAssetRisk(SEGRepository segR, Guid assetId)
        {
            List<RiskAnalysis> risks = new List<RiskAnalysis>();
            using (SEGContext seg = segR.GetContext())
            {
                risks = seg.RiskAnalyses.Where(x => x.AssetId == assetId).ToList();
            }
            return risks;
        }

        static public void SetAssetRisk(SEGRepository segR, Guid riskId, string value)
        {
            using (SEGContext seg = segR.GetContext())
            {
                var risk = seg.RiskAnalyses.FirstOrDefault(x => x.Id == riskId);
                risk.Value = value;
                seg.SaveChanges();
            }
        }

        static public List<RiskAnalysisType> GetRiskAnalysisTypes(SEGRepository segR)
        {
            List<RiskAnalysisType> risks = new List<RiskAnalysisType>();
            using (SEGContext seg = segR.GetContext())
            {
                risks = seg.RiskAnalysisTypes.ToList();
            }
            return risks;
        }

        static public void FixRisk(SEGRepository segR)
        {
            using(var seg = segR.GetContext())
            {
                // Get Risk Analysis Types
                List<RiskAnalysisType> types = seg.RiskAnalysisTypes.ToList();
                if(types.Count==0)
                {
                    // Create the types and then add the values for all assets
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

                    foreach(var asset in seg.Assets)
                    {
                        RiskAnalysis r = new RiskAnalysis();
                        r.AssetId = asset.Id;
                        r.EntryDate = DateTime.MinValue;
                        r.ExecutorId = Guid.Empty;
                        r.RiskAnalysisTypeId = rTG.Id;
                        r.VerifierId = Guid.Empty;
                        r.VerifyDate = DateTime.MinValue;
                        r.Value = "S1";
                        seg.RiskAnalyses.Add(r);

                        r = new RiskAnalysis();
                        r.AssetId = asset.Id;
                        r.EntryDate = DateTime.MinValue;
                        r.ExecutorId = Guid.Empty;
                        r.RiskAnalysisTypeId = rTE.Id;
                        r.VerifierId = Guid.Empty;
                        r.VerifyDate = DateTime.MinValue;
                        r.Value = "F1";
                        seg.RiskAnalyses.Add(r);

                        r = new RiskAnalysis();
                        r.AssetId = asset.Id;
                        r.EntryDate = DateTime.MinValue;
                        r.ExecutorId = Guid.Empty;
                        r.RiskAnalysisTypeId = rTP.Id;
                        r.VerifierId = Guid.Empty;
                        r.VerifyDate = DateTime.MinValue;
                        r.Value = "VERY LOW";
                        seg.RiskAnalyses.Add(r);

                        r = new RiskAnalysis();
                        r.AssetId = asset.Id;
                        r.EntryDate = DateTime.MinValue;
                        r.ExecutorId = Guid.Empty;
                        r.RiskAnalysisTypeId = rTR.Id;
                        r.VerifierId = Guid.Empty;
                        r.VerifyDate = DateTime.MinValue;
                        r.Value = "P1";
                        seg.RiskAnalyses.Add(r);
                    }
                    seg.SaveChanges();
                }
            }
        }
    }
}
