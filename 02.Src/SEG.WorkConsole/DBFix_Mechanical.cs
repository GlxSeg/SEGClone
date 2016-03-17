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
    public static class DBFix_Mechanical
    {
        public static void FixMechDiag(SEGRepository segR)
        {
            using (SEGContext seg = segR.GetContext())
            {
                DiagnosticsType dtGen = seg.DiagnosticsTypes.FirstOrDefault(a => a.Name == "General");
                DiagnosticsType dtMec = seg.DiagnosticsTypes.FirstOrDefault(a => a.Name == "Mechanical");
                DiagnosticsType dtEle = seg.DiagnosticsTypes.FirstOrDefault(a => a.Name == "Electrical");

                ItemClass icGen = seg.ItemClasses.FirstOrDefault(a => a.Name == "General");
                ItemClass icMec = seg.ItemClasses.FirstOrDefault(a => a.Name == "Mechanical");
                ItemClass icEle = seg.ItemClasses.FirstOrDefault(a => a.Name == "Electrical");

                bool hasSS = false;
                int idx = 0;
                ItemSection iS = null;
                ItemSubSection iSS = null;

                List<Item> fItems = seg.Items.Where(x => x.ItemClassId == icMec.Id).ToList();
                List<ItemSection> fItemSections = new List<ItemSection>();
                List<ItemSubSection> fItemSubSections = new List<ItemSubSection>();
                foreach (Item i in fItems)
                {
                    ItemSection aiS = null;
                    if(fItemSections.Count>0)
                        aiS = fItemSections.FirstOrDefault(x => x.Id == i.ItemSectionId);
                    if(aiS==null)
                    {
                        aiS = seg.ItemSections.FirstOrDefault(x => x.Id == i.ItemSectionId);
                        fItemSections.Add(aiS);
                    }

                    if (i.ItemSubSectionId != Guid.Empty)
                    {
                        ItemSubSection aiSS = null;
                        if (fItemSubSections.Count > 0)
                            aiSS = fItemSubSections.FirstOrDefault(x => x.Id == i.ItemSubSectionId);
                        if (aiSS == null)
                        {
                            aiSS = seg.ItemSubSections.FirstOrDefault(x => x.Id == i.ItemSubSectionId);
                            fItemSubSections.Add(aiSS);
                        }
                    }
                }

                StreamReader sr = new StreamReader(@"Data\mechanical2.txt", System.Text.Encoding.GetEncoding(1252), true);
                while (!sr.EndOfStream)
                {
                    string sline = sr.ReadLine();
                    char[] delimiters = new char[] { '\t' };
                    string[] sitems = sline.Split(delimiters);

                    // Let's see what the first value has
                    if (sitems[0] == "section")
                    {
                        iS = new ItemSection();
                        string scode = sitems[1].Trim();
                        iS.Code = scode;
                        iS.Title = sitems[5].Replace("\"", "").Trim();

                        // Now we try to find the code within fItemSections
                        if (fItemSections.FirstOrDefault(x => x.Code == iS.Code) == null)
                        {
                            // We now add it
                            seg.ItemSections.Add(iS);
                            seg.SaveChanges();
                            Console.WriteLine("Added Section :" + iS.Code);
                        }
                        else
                            iS = fItemSections.FirstOrDefault(x => x.Code == iS.Code);


                        hasSS = false;
                    }
                    else if (sitems[0] == "print")
                    {
                        iSS = new ItemSubSection();
                        iSS.ItemSectionId = iS.Id;

                        string scode = sitems[1].Trim();
                        scode = scode + "." + sitems[2].Trim();
                        // Check if item 3 is a 0 or empty
                        if (sitems[3].Trim() == "0" || sitems[3].Trim() == "")
                        {
                            // Nothing
                        }
                        else
                        {
                            scode = scode + "." + sitems[3].Trim();
                        }
                        if (sitems[4].Trim().Length > 0)
                        {
                            scode = scode + "|" + sitems[4].Trim();
                        }
                        else
                        {
                            // Nothing
                        }

                        iSS.Code = scode;
                        iSS.Title = sitems[5].Replace("\"", "").Trim();

                        // Now we try to find the code within fItemSections
                        if (fItemSubSections.FirstOrDefault(x => x.Code == iSS.Code) == null)
                        {
                            // We now add it
                            seg.ItemSubSections.Add(iSS);
                            seg.SaveChanges();
                            Console.WriteLine("Added SubSection :" + iSS.Code);
                        }
                        else
                            iSS = fItemSubSections.FirstOrDefault(x => x.Code == iSS.Code);

                        hasSS = false;
                    }
                    else
                    {
                        // Check code, and based on the last item, see if it is in SS or not
                        string scode = sitems[1].Trim();
                        scode = scode + "." + sitems[2].Trim();
                        // Check if item 3 is a 0
                        if (sitems[3].Trim() == "0" || sitems[3].Trim() == "")
                        {
                            // Nothing
                        }
                        else
                        {
                            scode = scode + "." + sitems[3].Trim();
                        }

                        if (sitems[4].Trim().Length > 0)
                        {
                            scode = scode + "|" + sitems[4].Trim();
                            hasSS = true;
                        }
                        else
                        {
                            hasSS = false;
                        }

                        Item i = new Item();
                        i.Index = idx;
                        i.ItemSectionId = iS.Id;
                        if (hasSS)
                            i.ItemSubSectionId = iSS.Id;
                        else
                            i.ItemSubSectionId = Guid.Empty;
                        i.ItemClassId = icMec.Id;
                        i.Code = scode;
                        i.Title = sitems[5].Replace("\"", "").Trim();

                        if (fItems.FirstOrDefault(x => x.Code == i.Code) == null)
                        {
                            // We now add it
                            seg.Items.Add(i);
                            seg.SaveChanges();
                            Console.WriteLine("Added Item :" + i.Code);
                        }
                        else
                        {
                            i = seg.Items.FirstOrDefault(x => x.Code == i.Code && x.ItemClassId == icMec.Id);
                            i.Index = idx;
                            seg.SaveChanges();
                        }
                        idx++;
                    }
                }
                sr.Close();
            }
        }

        public static void ChangeAssets(SEGRepository segR)
        {
            List<Item> dtItems = DiagnosticsHelper.GetStandardsForType(segR, "Mechanical");

            using (SEGContext seg = segR.GetContext())
            {
                List<Asset> assets = seg.Assets.ToList();
                foreach(Asset a in assets)
                {
                    Diagnostics d = DiagnosticsHelper.GetMechanicalDiagnostics(segR,a.Id);
                    List<DiagnosticsDetail> dL = DiagnosticsHelper.GetDiagnosticsDetails(segR, d.Id);
                    // Go through each of the items
                    foreach(Item i in dtItems)
                    {
                        DiagnosticsDetail ddl = seg.DiagnosticsDetails.FirstOrDefault(x => x.DiagnosticsId == d.Id && x.ItemId == i.Id);
                        if(ddl==null)
                        {
                            // We need to add this diagnostics detail
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
                    }
                    seg.SaveChanges();
                    Console.WriteLine("Asset:" + a.Id);
                }
            }
        }
    }
}
