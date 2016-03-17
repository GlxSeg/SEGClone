using SEG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Helpers
{
    public static class DiagnosticsHelper
    {
        static public Item GetItem(SEGRepository segR, Guid iId)
        {
            Item item = null;
            using (SEGContext seg = segR.GetContext())
            {
                item = seg.Items.FirstOrDefault(x => x.Id == iId);
            }
            return item;
        }

        static public ItemSection GetItemSection(SEGRepository segR, Guid id)
        {
            ItemSection item = null;
            using (SEGContext seg = segR.GetContext())
            {
                item = seg.ItemSections.FirstOrDefault(x => x.Id == id);
            }
            return item;
        }

        static public ItemSubSection GetItemSubSection(SEGRepository segR, Guid id)
        {
            ItemSubSection item = null;
            using (SEGContext seg = segR.GetContext())
            {
                item = seg.ItemSubSections.FirstOrDefault(x => x.Id == id);
            }
            return item;
        }

        static public List<DiagnosticsDetail> GetDiagnosticsDetails(SEGRepository segR, Guid dId)
        {
            List<DiagnosticsDetail> dd = new List<DiagnosticsDetail>();
            using (SEGContext seg = segR.GetContext())
            {
                dd = seg.DiagnosticsDetails.Where(a => a.DiagnosticsId == dId).ToList();
            }
            return dd;
        }

        static public DiagnosticsDetail GetDiagnosticsDetail(SEGRepository segR, Guid diId)
        {
            DiagnosticsDetail di = null;
            using (SEGContext seg = segR.GetContext())
            {
                di = seg.DiagnosticsDetails.FirstOrDefault(x => x.Id == diId);
            }
            return di;
        }

        static public void ModifyDetails(SEGRepository segR, Guid diId, string newStatus, string newComments, Guid newImgId)
        {
            using (SEGContext seg = segR.GetContext())
            {
                DiagnosticsDetail di = seg.DiagnosticsDetails.FirstOrDefault(x => x.Id == diId);
                if(di.ImageId!=newImgId && di.ImageId!=Guid.Empty)
                {
                    Image img = seg.Images.FirstOrDefault(x=>x.Id == di.ImageId);
                    seg.Images.Remove(img);
                }
                di.ImageId = newImgId;
                di.Comments = newComments;
                di.Status = newStatus;
                seg.SaveChanges();
            }
        }

        static public Diagnostics GetAreaDiagnostics(SEGRepository segR, Guid areaId)
        {
            Diagnostics d = null;
            using (SEGContext seg = segR.GetContext())
            {
                d = seg.Diagnostics.FirstOrDefault(x => x.AreaId == areaId);
            }
            return d;
        }

        static public Diagnostics GetMechanicalDiagnostics(SEGRepository segR, Guid assetId)
        {
            Diagnostics d = null;
            using (SEGContext seg = segR.GetContext())
            {
                DiagnosticsType dt = seg.DiagnosticsTypes.FirstOrDefault(t => t.Name == "Mechanical");
                d = seg.Diagnostics.FirstOrDefault(a => a.AssetId == assetId && a.DiagnosticsTypeId == dt.Id);
            }
            return d;
        }

        static public List<Diagnostics> GetElectricalDiagnostics(SEGRepository segR, Guid assetId)
        {
            List<Diagnostics> d = null;
            using (SEGContext seg = segR.GetContext())
            {
                DiagnosticsType dt = seg.DiagnosticsTypes.FirstOrDefault(t => t.Name == "Electrical");
                d = seg.Diagnostics.Where(a => a.AssetId == assetId && a.DiagnosticsTypeId == dt.Id).ToList();
            }
            return d;
        }

        static public List<Image> GetDiagnosticsImages(SEGRepository segR, Guid diagId)
        {
            List<Image> imgs = new List<Image>();
            using (SEGContext seg = segR.GetContext())
            {
                List<DiagnosticsImages> di = seg.DiagnosticsImages.Where(x => x.DiagnosticID == diagId).ToList();
                foreach(DiagnosticsImages d in di)
                {
                    Image i = seg.Images.FirstOrDefault(x => x.Id == d.ImageId);
                    ImageHelper.FillImage(i);
                    imgs.Add(i);
                }
            }
            return imgs;
        }

        static public void SaveDiagnosticsImage(SEGRepository segR, Guid diagId, Guid imgId)
        {
            using (SEGContext seg = segR.GetContext())
            {
                List<DiagnosticsImages> imgs = seg.DiagnosticsImages.Where(x=>x.DiagnosticID == diagId).ToList();
                int nextOrder = 1;
                if(imgs.Count>0)
                    nextOrder = seg.DiagnosticsImages.Where(x=>x.DiagnosticID == diagId).OrderBy(x=>x.Order).ToList().Last().Order+1;
                DiagnosticsImages ih = new DiagnosticsImages();
                ih.ImageId = imgId;
                ih.DiagnosticID = diagId;
                ih.Order = nextOrder;
                seg.DiagnosticsImages.Add(ih);
                seg.SaveChanges();
            }
        }

        static public void DeleteDiagnosticsImage(SEGRepository segR, Guid diagId, Guid imgId)
        {
            using (SEGContext seg = segR.GetContext())
            {
                DiagnosticsImages ih = seg.DiagnosticsImages.FirstOrDefault(x => x.DiagnosticID == diagId && x.ImageId == imgId);
                seg.DiagnosticsImages.Remove(ih);
                Image i = seg.Images.FirstOrDefault(x => x.Id == imgId);
                seg.Images.Remove(i);
                seg.SaveChanges();
            }
        }

        static public void AddElectricalDiagnostics(SEGRepository segR, Guid assetId)
        {
            List<Diagnostics> electrical = DiagnosticsHelper.GetElectricalDiagnostics(segR,assetId).ToList();
            int nextIndex = 1;
            if(electrical.Count>0)
            {
                nextIndex = electrical.OrderBy(x => x.Index).Last().Index + 1;
            }

            using (SEGContext seg = segR.GetContext())
            {
                DiagnosticsType dtEle = seg.DiagnosticsTypes.FirstOrDefault(a => a.Name == "Electrical");
                ItemClass icEle = seg.ItemClasses.FirstOrDefault(a => a.Name == "Electrical");
                List<Item> dtEItems = seg.Items.Where(x => x.ItemClassId == icEle.Id).OrderBy(x => x.Index).ToList();

                // Asset info
                AssetInfo ai = new AssetInfo();
                ai.Key = "EDIAG_" + nextIndex + "_INFO1";
                ai.Value = "";
                ai.AssetId = assetId;
                seg.AssetInfos.Add(ai);

                ai = new AssetInfo();
                ai.Key = "EDIAG_" + nextIndex + "_INFO2";
                ai.Value = "";
                ai.AssetId = assetId;
                seg.AssetInfos.Add(ai);

                ai = new AssetInfo();
                ai.Key = "EDIAG_" + nextIndex + "_INFO3";
                ai.Value = "";
                ai.AssetId = assetId;
                seg.AssetInfos.Add(ai);

                ai = new AssetInfo();
                ai.Key = "EDIAG_" + nextIndex + "_INFO4";
                ai.Value = "";
                ai.AssetId = assetId;
                seg.AssetInfos.Add(ai);

                // Mechanical Diagnostics
                Diagnostics d = new Diagnostics();
                d.AreaId = Guid.Empty;
                d.AssetId = assetId;
                d.Index = nextIndex;
                d.DiagnosticsTypeId = dtEle.Id;
                seg.Diagnostics.Add(d);
                seg.SaveChanges();

                foreach (Item i in dtEItems)
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

        static public List<Item> GetStandardsForType(SEGRepository segR, string standardsType)
        {
            List<Item> dtItems = new List<Item>();
            using (SEGContext seg = segR.GetContext())
            {
                ItemClass ic = seg.ItemClasses.FirstOrDefault(a => a.Name == standardsType);
                dtItems = seg.Items.Where(x => x.ItemClassId == ic.Id).OrderBy(x => x.Index).ToList();
            }
            return dtItems;
        }

    }
}
