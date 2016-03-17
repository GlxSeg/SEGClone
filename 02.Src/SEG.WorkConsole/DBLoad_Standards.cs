using SEG.Domain;
using SEG.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.WorkConsole
{
    static public class DBLoad_Standards
    {
        public static void Load_Types(SEGRepository segR)
        {
            using (SEGContext seg = segR.GetContext())
            {
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
            }

            using (SEGContext seg = segR.GetContext())
            {
                DiagnosticsType dt = new DiagnosticsType();
                dt.Name = "General";
                seg.DiagnosticsTypes.Add(dt);

                dt = new DiagnosticsType();
                dt.Name = "Mechanical";
                seg.DiagnosticsTypes.Add(dt);

                dt = new DiagnosticsType();
                dt.Name = "Electrical";
                seg.DiagnosticsTypes.Add(dt);

                ItemClass ic = new ItemClass();
                ic.Description = "General Items";
                ic.Name = "General";
                seg.ItemClasses.Add(ic);

                ic = new ItemClass();
                ic.Description = "Mechanical Items";
                ic.Name = "Mechanical";
                seg.ItemClasses.Add(ic);

                ic = new ItemClass();
                ic.Description = "Electrical Items";
                ic.Name = "Electrical";
                seg.ItemClasses.Add(ic);

                seg.SaveChanges();
            }
        }

        public static void Load_General(SEGRepository segR)
        {
            using (SEGContext seg = segR.GetContext())
            {
                // Get the types We'll be using
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

                StreamReader sr = new StreamReader(@"Data\standards-general.txt", System.Text.Encoding.GetEncoding(1252), true);
                while (!sr.EndOfStream)
                {
                    string sline = sr.ReadLine();
                    char[] delimiters = new char[] { '\t' };
                    string[] sitems = sline.Split(delimiters);

                    // Let's see what the first value has
                    if (sitems[0] == "section")
                    {
                        // Section
                        iS = new ItemSection();
                        string scode = sitems[1].Trim();

                        iS.Code = scode;
                        iS.Title = sitems[5].Replace("\"", "").Trim();
                        seg.ItemSections.Add(iS);
                        seg.SaveChanges();
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

                        iSS.Code = scode;
                        iSS.Title = sitems[5].Replace("\"", "").Trim();
                        seg.ItemSubSections.Add(iSS);
                        seg.SaveChanges();
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
                        idx++;
                        i.ItemSectionId = iS.Id;
                        if (hasSS)
                            i.ItemSubSectionId = iSS.Id;
                        else
                            i.ItemSubSectionId = Guid.Empty;
                        i.ItemClassId = icGen.Id;
                        i.Code = scode;
                        i.Title = sitems[5].Replace("\"", "").Trim();
                        seg.Items.Add(i);
                        seg.SaveChanges();
                    }
                }
                sr.Close();


                hasSS = false;
                idx = 0;
            }
        }

        public static void Load_Mechanical(SEGRepository segR)
        {
            using (SEGContext seg = segR.GetContext())
            {
                // Get the types We'll be using
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

                StreamReader sr = new StreamReader(@"Data\standards-mechanical.txt", System.Text.Encoding.GetEncoding(1252), true);
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
                        seg.ItemSections.Add(iS);
                        seg.SaveChanges();
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
                        seg.ItemSubSections.Add(iSS);
                        seg.SaveChanges();
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
                        idx++;
                        i.ItemSectionId = iS.Id;
                        if (hasSS)
                            i.ItemSubSectionId = iSS.Id;
                        else
                            i.ItemSubSectionId = Guid.Empty;
                        i.ItemClassId = icMec.Id;
                        i.Code = scode;
                        i.Title = sitems[5].Replace("\"", "").Trim();
                        seg.Items.Add(i);
                        seg.SaveChanges();
                    }
                }
                sr.Close();



                hasSS = false;
                idx = 0;

                sr = new StreamReader(@"Data\standards-electrical.txt", System.Text.Encoding.GetEncoding(1252), true);
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
                        seg.ItemSections.Add(iS);
                        seg.SaveChanges();
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
                        seg.ItemSubSections.Add(iSS);
                        seg.SaveChanges();
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
                        idx++;
                        i.ItemSectionId = iS.Id;
                        if (hasSS)
                            i.ItemSubSectionId = iSS.Id;
                        else
                            i.ItemSubSectionId = Guid.Empty;
                        i.ItemClassId = icEle.Id;
                        i.Code = scode;
                        i.Title = sitems[5].Replace("\"", "").Trim();
                        seg.Items.Add(i);
                        seg.SaveChanges();
                    }
                }
                sr.Close();

                seg.SaveChanges();
            }
        }
    }
}
