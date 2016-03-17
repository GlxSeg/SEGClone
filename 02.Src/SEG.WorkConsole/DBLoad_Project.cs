using SEG.Domain;
using SEG.Domain.Helpers;
using SEG.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.WorkConsole
{
    static public class DBLoad_Project
    {
        static public void Load(SEGRepository segR)
        {
            Guid iId = ImageHelper.SaveImage(segR, @"Media\project_01.png");

            List<Guid> assetImg = new List<Guid>();
            for (int i = 1; i <= 135;i++ )
            {
                Guid gi = ImageHelper.SaveImage(segR, @"C:\Develop\SEG\Multimedia\Taschibra\Final\"+i.ToString()+".jpg");
                assetImg.Add(gi);
            }

            Guid pId;
            Guid uId;

            using (SEGContext seg = segR.GetContext())
            {
                Project p = new Project();
                p.Name = "Taschibra";
                p.ImageId = iId;
                p.Active = true;
                seg.Projects.Add(p);

                User u = new User();
                u.Active = true;
                u.FullName = "Isabela Paredes";
                u.IsAdmin = true;
                u.IsApprover = true;
                u.IsExecutor = true;
                u.IsVerifier = true;
                u.Name = "isaparedes";
                u.Password = "nr12";
                seg.Users.Add(u);
                seg.SaveChanges();

                pId = p.Id;
                uId = u.Id;
            }

            List<string> aNames = new List<string>();
            List<Guid> aIds = new List<Guid>();

            DiagnosticsType dtGen;
            DiagnosticsType dtMec;
            DiagnosticsType dtEle;

            ItemClass icGen;
            ItemClass icMec;
            ItemClass icEle;

            List<Item> dtGItems;
            List<Item> dtMItems;

            using (SEGContext seg = segR.GetContext())
            {
                aNames.Add("1. INJEÇÃO");
                aNames.Add("2. MOAGEM");
                aNames.Add("3. FERRAMENTARIA");
                aNames.Add("4. PERFILADEIRA");
                aNames.Add("5. PEÇAS ESPECIAIS");
                aNames.Add("6. PINTURA");
                aNames.Add("7. PINTURA COLORIDA");
                aNames.Add("8. OFICINA");
                aNames.Add("9. REFLETOR");
                aNames.Add("10. ALMOXARIFADO");
                aNames.Add("11. MONTAGEM");
                aNames.Add("12. RETRABALHO");
                aNames.Add("13. PEÇAS SOLTAS");
                aNames.Add("14. MARCENARIA");
                aNames.Add("15. RECICLAGEM");
                aNames.Add("16. FIGURAS");

                dtGen = seg.DiagnosticsTypes.FirstOrDefault(a => a.Name == "General");
                dtMec = seg.DiagnosticsTypes.FirstOrDefault(a => a.Name == "Mechanical");
                dtEle = seg.DiagnosticsTypes.FirstOrDefault(a => a.Name == "Electrical");

                icGen = seg.ItemClasses.FirstOrDefault(a => a.Name == "General");
                icMec = seg.ItemClasses.FirstOrDefault(a => a.Name == "Mechanical");
                icEle = seg.ItemClasses.FirstOrDefault(a => a.Name == "Electrical");

                dtGItems = seg.Items.Where(x => x.ItemClassId == icGen.Id).OrderBy(x => x.Index).ToList();
                dtMItems = seg.Items.Where(x => x.ItemClassId == icMec.Id).OrderBy(x => x.Index).ToList();
            }

            using (SEGContext seg = segR.GetContext())
            {
                foreach (string s in aNames)
                {
                    ProjectArea a = new ProjectArea();
                    a.ProjectId = pId;
                    a.Name = s;
                    a.ImageId = Guid.Empty;
                    a.Active = true;
                    seg.ProjectAreas.Add(a);
                    seg.SaveChanges();
                    aIds.Add(a.Id);

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
            }

            using (SEGContext seg = segR.GetContext())
            {
                StreamReader sr = new StreamReader(@"Data\assets.txt", System.Text.Encoding.GetEncoding(1252), true);
                int assetidx=0;
                while (!sr.EndOfStream)
                {
                    string sline = sr.ReadLine();
                    char[] delimiters = new char[] { '\t' };
                    string[] sitems = sline.Split(delimiters);

                    int aidx = Convert.ToInt32(sitems[1]);
                    string sid = sitems[2];
                    string stag = sitems[3];
                    string sdesc = sitems[4].Replace("\"", "").Trim();

                    Asset asset = new Asset();
                    asset.ProjectAreaId = aIds[aidx - 1];
                    asset.ExecutorId = uId;
                    asset.VerifierId = Guid.Empty;
                    asset.ApproverId = Guid.Empty;
                    asset.ImageId = assetImg[assetidx];
                    asset.EntryDate = DateTime.Now;
                    asset.VerifyDate = DateTime.MinValue;
                    asset.ApprovalDate = DateTime.MinValue;
                    seg.Assets.Add(asset);
                    seg.SaveChanges();

                    AssetInfo aInfo = new AssetInfo();
                    aInfo.AssetId = asset.Id;
                    aInfo.Key = "ID";
                    aInfo.Value = sid;
                    seg.AssetInfos.Add(aInfo);

                    aInfo = new AssetInfo();
                    aInfo.AssetId = asset.Id;
                    aInfo.Key = "TAG";
                    aInfo.Value = stag;
                    seg.AssetInfos.Add(aInfo);

                    aInfo = new AssetInfo();
                    aInfo.AssetId = asset.Id;
                    aInfo.Key = "LOCATION";
                    aInfo.Value = "";
                    seg.AssetInfos.Add(aInfo);

                    aInfo = new AssetInfo();
                    aInfo.AssetId = asset.Id;
                    aInfo.Key = "DESCRIPTION";
                    aInfo.Value = sdesc;
                    seg.AssetInfos.Add(aInfo);

                    // Mechanical Diagnostics
                    Diagnostics d = new Diagnostics();
                    d.AreaId = Guid.Empty;
                    d.AssetId = asset.Id;
                    d.Index = 0;
                    d.DiagnosticsTypeId = dtMec.Id;
                    seg.Diagnostics.Add(d);
                    seg.SaveChanges();

                    DiagnosticsImages di = new DiagnosticsImages();
                    di.DiagnosticID = d.Id;
                    di.ImageId = asset.ImageId;
                    di.Order = 0;
                    seg.DiagnosticsImages.Add(di);
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

                    assetidx++;
                }

            }
        }
    }
}
