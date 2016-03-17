using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Configuration;

using SEG.Desktop.Windows;
using SEG.Domain;
using SEG.Domain.Model;
using System.Windows.Media;
using SEG.Desktop.Windows.Popups;
using SEG.Domain.Helpers;
using SEG.Desktop.Windows.Panels;
using System.Windows.Input;

namespace SEG.Desktop.Control
{
    public sealed class ControlCenter
    {
        private static readonly Lazy<ControlCenter> lazy =
            new Lazy<ControlCenter>(() => new ControlCenter());
    
        public static ControlCenter Instance { get { return lazy.Value; } }

        public Main wMain;
        public Edit_DiagDetail wPEditDiagDetail;

        // Repository
        public SEGRepository segR;

        // Static Diagnostic Standard Item
        public List<Item> items;
        public List<ItemClass> itemClasses;
        public List<ItemSection> itemSections;
        public List<ItemSubSection> itemSubSections;
        public List<DiagnosticsType> diagnosticsTypes;

        // Current
        public Guid cProjectId;
        public Guid cProjectAreaId;
        public Guid cAssetId;
        public Guid cDiagnosticsId;
        public int cAssetEidx;

        // Cached Data
        public Project cProject;
        public ProjectArea cProjectArea;
        public Asset cAsset;
        public List<AssetInfo> cAssetInfoList;
        public Image cAssetImage;

        // Navigation
        public string page;

        // Popup data handlers
        public double imgPop_snapW;
        public double imgPop_snapH;
        public BitmapImage imgPopResult;
        public ImageHolder imgHolderClick;
        public string cNewAssetId;
        public string cNewAssetTag;
        public string cNewAreaName;
        public Guid cCopyToAssetId;
        public Guid cCopyToAreaId;
















































        private ControlCenter()
        {
            // This is the main constructor for the ControlCenter management class
            // -------------------------------------------------------------------------------
            // For out app, the ControlCenter takes care of configuration, data management and storage

            wMain = null;
            wPEditDiagDetail = null;

            // Initial indexes
            cProjectId = Guid.Empty;
            cProjectAreaId = Guid.Empty;
            cAssetId = Guid.Empty;

            // ConnectionString
            string connStr = ConfigurationManager.ConnectionStrings["SEGDatabase"].ConnectionString;
            segR = new SEGRepository(connStr);

            // Initialize Control Center
            Initialize();

            // Navigation
            page = "project-list";

            // Fix Risk
            ProjectHelper.FixRisk(segR);
        }


        public Dictionary<string, Color> configColors;
        public Dictionary<string, string> configStrings;
        public Dictionary<string, int> configInts;
        public Dictionary<string, double> configDoubles;

        public void Initialize()
        {
            // Setup configuration of the system
            configColors = new Dictionary<string,Color>();
            configDoubles = new Dictionary<string,double>();
            configStrings = new Dictionary<string,string>();
            configInts = new Dictionary<string,int>();

            configColors.Add("MainBlue",Color.FromArgb( 255,  0, 85, 110));
            configColors.Add("LightBlue",Color.FromArgb( 255, 41,113, 143));
            configColors.Add("MainOrange", Color.FromArgb(255, 250, 165, 25));
            configColors.Add("LightOrange", Color.FromArgb(255, 255, 203, 4));
        }



        public Color GetConfigColor(string key)
        {
            return configColors[key];
        }


































































        public User cUser = null;
        public bool CheckLogin(string user, string pass)
        {
            // Check the user and password
            cUser = AccountHelper.CheckLogin(segR, user);

            if(cUser !=null)
            {
                if (cUser.Password == pass)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        // Command Management
        public void DoLogin(string user)
        {
            // Here we go to menu #2
            //wMain.Show();            
        }

        public void Menu_Exit()
        {
            Application.Current.Shutdown();
        }





























        public void SelectProjectList()
        {
            cProject = null;
            cProjectId = Guid.Empty;

            page = "project-list";
            wMain.UpdatePanels();
        }

        public void SelectProject(Guid pId)
        {
            cProjectId = pId;
            cProject = ProjectHelper.GetProject(segR, pId);

            // Check to see if the project has project info
            using(var seg = segR.GetContext())
            {
                var pI = seg.ProjectInfos.Where(x => x.ProjectId == pId).ToList();
                if(pI.Count == 0)
                {
                    var apI = new ProjectInfo();
                    apI.Key = "CODE";
                    apI.ProjectId = pId;
                    apI.Value = "P 15.018";
                    seg.ProjectInfos.Add(apI);
                    seg.SaveChanges();
                }
            }

            page = "area-list";
            wMain.UpdatePanels();
        }

        public void AddProject()
        {
            cProjectId = Guid.Empty;
            cProject = null;

            page = "project-new";
            wMain.UpdatePanels();
        }




























        public void SelectArea(Guid aId)
        {
            cProjectAreaId = aId;
            cProjectArea = ProjectHelper.GetProjectArea(segR, aId);           

            page = "asset-list";
            wMain.UpdatePanels();
        }

        public void Area_Tab_AssetList()
        {
            page = "asset-list";
            wMain.UpdatePanels();
        }

        public void Area_Tab_GenDiag()
        {
            page = "asset-genDiag";
            wMain.UpdatePanels();
        }

        public void ChangeAreaName()
        {
            // Add an asset
            AddAreaPopup mp = new AddAreaPopup();
            mp.SetupEdit(cProjectArea.Name);
            mp.SetSize();
            if (mp.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                ProjectHelper.ChangeAreaName(segR, cProjectAreaId, cNewAreaName);
                cProjectArea.Name = cNewAreaName;

                Mouse.OverrideCursor = null;
            }

        }

        public void AddArea()
        {
            // Add an asset
            AddAreaPopup mp = new AddAreaPopup();
            mp.SetupNew();
            mp.SetSize();
            if (mp.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                // We need to include the user later on
                Guid id = ProjectHelper.AddArea(segR, cProjectId, cNewAreaName);

                Mouse.OverrideCursor = null;

                var mp2 = new MessagePopup();
                mp2.Setup("Project Management", "The new Area was added successfully.", false);
                mp2.SetSize();
                mp2.ShowDialog();

                SelectArea(id);
            }
        }


























        public void AddAsset()
        {
            // Add an asset
            AddAssetPopup mp = new AddAssetPopup();
            mp.Setup();
            mp.SetSize();
            if (mp.ShowDialog() == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                // We need to include the user later on
                Guid id = ProjectHelper.AddAsset(segR, cProjectAreaId, Guid.Empty, cNewAssetId, cNewAssetTag, "", "");

                Mouse.OverrideCursor = null;

                SelectAsset(id);
            }
        }

        public void SelectAsset(Guid aId)
        {
            cAssetId = aId;
            cAsset = ProjectHelper.GetAsset(segR, aId);
            cAssetInfoList = ProjectHelper.GetAssetInfo(segR, aId);
            cAssetImage = ImageHelper.GetImage(segR, cAsset.ImageId);

            cAssetEidx = 0;
            page = "asset-edit-general";
            wMain.UpdatePanels();
        }

        public void Asset_SelectGeneral()
        {
            page = "asset-edit-general";
            wMain.UpdatePanels();
        }

        public void Asset_SelectMechanical()
        {
            page = "asset-edit-mechanical";
            wMain.UpdatePanels();
        }

        public void Asset_SelectRisk()
        {
            page = "asset-edit-risk";
            wMain.UpdatePanels();
        }

        public void Asset_SelectMechanical_Img()
        {
            page = "asset-edit-mechanical_img";
            wMain.UpdatePanels();
        }

        public void Asset_SelectElectrical()
        {
            page = "asset-edit-electrical";
            wMain.UpdatePanels();
        }

        public void Asset_SelectElectrical_Img()
        {
            page = "asset-edit-electrical_img";
            wMain.UpdatePanels();
        }

        public void Asset_SelectElectrical_Info()
        {
            page = "asset-edit-electrical_info";
            wMain.UpdatePanels();
        }

        public void Asset_SelectImage()
        {
            if(page == "asset-edit-mechanical_img")
            {
                wMain.panelAssetEditMechanicalImg.SelectImage(imgHolderClick);
            }
            else if(page == "asset-edit-electrical_img")
            {
                wMain.panelAssetEditElectricalImg.SelectImage(imgHolderClick);
            }
        }

























        public void Menu_Back()
        {
            if (page == "area-list" || page == "project-new")
            {
                page = "project-list";
                wMain.UpdatePanels();                
            }
            else if (page == "asset-list" || 
                     page == "asset-genDiag")
            {
                page = "area-list";

                // Unselect the assets
                cAssetId = Guid.Empty;
                cAsset = null;

                wMain.UpdatePanels();
            }
            else if (page == "asset-edit-general" || 
                     page == "asset-edit-risk" ||
                     page == "asset-edit-mechanical" ||
                     page == "asset-edit-mechanical_img" ||
                     page == "asset-edit-electrical_img" ||
                     page == "asset-edit-electrical_info" ||
                     page == "asset-edit-electrical")
            {
                page = "asset-list";
                wMain.UpdatePanels();
            }
        }

        public void Menu_Forward()
        {
            if (page == "project-list" && cProjectId!=Guid.Empty)
            {
                page = "area-list";
                wMain.UpdatePanels();
            }
            else if (page == "area-list" && cProjectAreaId != Guid.Empty)
            {
                page = "asset-list";
                wMain.UpdatePanels();
            }
            else if ((page == "asset-list" || page == "asset-genDiag") && cAssetId != Guid.Empty)
            {
                page = "asset-edit-general";
                wMain.UpdatePanels();
            } 
        }






























        public void DiagnosticDetailsUpdated()
        {
            //if (page == "asset-genDiag")
            //{
            //    wMain.panelAreaGenDiag.
            //}
            //else if(page == "asset-edit-mechanical" || page == "asset-edit-electrical")
            //{
            //    wMain.UpdateData();
            //} 
        }























        //public void PrepareDemo()
        //{
        //    Project p = new Project();
        //    p.Name = "GreyLogix";
        //    p.Id = Guid.NewGuid();
        //    p.Image = new Image();
        //    p.Image.DataImage = new BitmapImage(new Uri("pack://application:,,,/Media/demo/p1.png"));

        //    ProjectArea a = new ProjectArea();
        //    Demo_AreaGenDiagInit(a);
        //    a.Active = true;
        //    a.Id = Guid.NewGuid();
        //    a.Image = null;
        //    a.Info = new List<ProjectAreaInfo>();
        //    a.Name = "Area 1";
        //    a.ProjectId = p.Id;
        //    DemoAsset(a, 0, 1700);
        //    DemoAsset(a, 1, 1700);
        //    DemoAsset(a, 2, 1700);
        //    projectAreas.Add(a);

        //    a = new ProjectArea();
        //    Demo_AreaGenDiagInit(a);
        //    a.Active = true;
        //    a.Id = Guid.NewGuid();
        //    a.Image = new Image();
        //    a.Image.DataImage = new BitmapImage(new Uri("pack://application:,,,/Media/demo/p2_a1.png"));
        //    a.Info = new List<ProjectAreaInfo>();
        //    a.Name = "Area 2 (Img)";
        //    a.ProjectId = p.Id;
        //    DemoAsset(a, 0, 2100);
        //    DemoAsset(a, 1, 2100);
        //    DemoAsset(a, 2, 2100);
        //    DemoAsset(a, 3, 2100);
        //    DemoAsset(a, 4, 2100);
        //    DemoAsset(a, 5, 2100);
        //    DemoAsset(a, 6, 2100);
        //    DemoAsset(a, 2, 2200);
        //    DemoAsset(a, 3, 2200);
        //    DemoAsset(a, 4, 2200);
        //    DemoAsset(a, 5, 2200);
        //    DemoAsset(a, 6, 2200);
        //    DemoAsset(a, 2, 2300);
        //    DemoAsset(a, 3, 2300);
        //    DemoAsset(a, 4, 2300);
        //    DemoAsset(a, 5, 2300);
        //    DemoAsset(a, 6, 2300);
        //    DemoAsset(a, 2, 2400);
        //    DemoAsset(a, 3, 2400);
        //    DemoAsset(a, 4, 2400);
        //    DemoAsset(a, 5, 2400);
        //    DemoAsset(a, 2, 2500);
        //    DemoAsset(a, 3, 2500);
        //    DemoAsset(a, 4, 2500);
        //    DemoAsset(a, 5, 2500);
        //    projectAreas.Add(a);

        //    a = new ProjectArea();
        //    Demo_AreaGenDiagInit(a);
        //    a.Active = true;
        //    a.Id = Guid.NewGuid();
        //    a.Image = null;
        //    a.Info = new List<ProjectAreaInfo>();
        //    a.Name = "Area 3";
        //    a.ProjectId = p.Id;
        //    DemoAsset(a, 0, 3500);
        //    DemoAsset(a, 1, 3500);
        //    DemoAsset(a, 2, 3500);
        //    DemoAsset(a, 3, 3500);
        //    DemoAsset(a, 4, 3500);
        //    projectAreas.Add(a);

        //    a = new ProjectArea();
        //    Demo_AreaGenDiagInit(a);
        //    a.Active = true;
        //    a.Id = Guid.NewGuid();
        //    a.Image = null;
        //    a.Info = new List<ProjectAreaInfo>();
        //    a.Name = "Area 4";
        //    a.ProjectId = p.Id;
        //    DemoAsset(a, 4, 2200);
        //    DemoAsset(a, 5, 2200);
        //    DemoAsset(a, 6, 2200);
        //    DemoAsset(a, 7, 2200);
        //    DemoAsset(a, 8, 2200);
        //    projectAreas.Add(a);

        //    a = new ProjectArea();
        //    Demo_AreaGenDiagInit(a);
        //    a.Active = true;
        //    a.Id = Guid.NewGuid();
        //    a.Image = null;
        //    a.Info = new List<ProjectAreaInfo>();
        //    a.Name = "Area 5";
        //    a.ProjectId = p.Id;
        //    DemoAsset(a, 3, 1500);
        //    DemoAsset(a, 4, 1500);
        //    DemoAsset(a, 5, 1500);
        //    DemoAsset(a, 6, 1500);
        //    DemoAsset(a, 7, 1500);
        //    projectAreas.Add(a);


        //    projects.Add(p);





        //    p = new Project();
        //    p.Name = "Mili";
        //    p.Id = Guid.NewGuid();
        //    p.Image = new Image();
        //    p.Image.DataImage = new BitmapImage(new Uri("pack://application:,,,/Media/demo/p2.png"));

        //    a = new ProjectArea();
        //    Demo_AreaGenDiagInit(a);
        //    a.Active = true;
        //    a.Id = Guid.NewGuid();
        //    a.Image = null;
        //    a.Info = new List<ProjectAreaInfo>();
        //    a.Name = "Area 1";
        //    a.ProjectId = p.Id;
        //    DemoAsset(a, 0, 1100);
        //    DemoAsset(a, 1, 1100);
        //    DemoAsset(a, 2, 1100);
        //    projectAreas.Add(a);

        //    a = new ProjectArea();
        //    Demo_AreaGenDiagInit(a);
        //    a.Active = true;
        //    a.Id = Guid.NewGuid();
        //    a.Image = new Image();
        //    a.Image.DataImage = new BitmapImage(new Uri("pack://application:,,,/Media/demo/p2_a1.png"));
        //    a.Info = new List<ProjectAreaInfo>();
        //    a.Name = "Area 3";
        //    a.ProjectId = p.Id;
        //    DemoAsset(a, 4, 2400);
        //    DemoAsset(a, 5, 2400);
        //    DemoAsset(a, 3, 2400);
        //    DemoAsset(a, 2, 2400);
        //    DemoAsset(a, 1, 2400);
        //    DemoAsset(a, 8, 2400);
        //    DemoAsset(a, 6, 2400);
        //    projectAreas.Add(a);

        //    a = new ProjectArea();
        //    Demo_AreaGenDiagInit(a);
        //    a.Active = true;
        //    a.Id = Guid.NewGuid();
        //    a.Image = null;
        //    a.Info = new List<ProjectAreaInfo>();
        //    a.Name = "Area 5";
        //    a.ProjectId = p.Id;
        //    DemoAsset(a, 0, 2900);
        //    DemoAsset(a, 3, 2900);
        //    DemoAsset(a, 5, 2900);
        //    DemoAsset(a, 6, 2900);
        //    DemoAsset(a, 7, 2900);
        //    projectAreas.Add(a);

        //    projects.Add(p);
        //}

        //public void DemoAsset(ProjectArea a, int idx, int ibase)
        //{
        //    List<string> simgs = new List<string>();
        //    simgs.Add("pack://application:,,,/Media/demo/asset01.png");
        //    simgs.Add("pack://application:,,,/Media/demo/asset02.png");
        //    simgs.Add("pack://application:,,,/Media/demo/asset03.png");
        //    simgs.Add("pack://application:,,,/Media/demo/asset04.png");
        //    simgs.Add("pack://application:,,,/Media/demo/asset05.png");
        //    simgs.Add("pack://application:,,,/Media/demo/asset06.png");
        //    simgs.Add("pack://application:,,,/Media/demo/asset07.png");
        //    simgs.Add("pack://application:,,,/Media/demo/asset08.png");
        //    simgs.Add("pack://application:,,,/Media/demo/asset09.png");
        //    simgs.Add("pack://application:,,,/Media/demo/asset10.png");

        //    Asset aa = new Asset();
        //    aa.Id = Guid.NewGuid();
        //    aa.Name = (ibase + idx).ToString();
        //    aa.Image = new Image { DataImage = new BitmapImage(new Uri(simgs[idx])) };
        //    aa.Info = new List<AssetInfo>();
        //    aa.Info.Add(new AssetInfo { Id = Guid.NewGuid(), Key = "ID", Value = aa.Name });
        //    aa.Info.Add(new AssetInfo { Id = Guid.NewGuid(), Key = "TAG", Value = "FABR.1.RES.MO.001.00." + (300 + idx) });
        //    aa.Info.Add(new AssetInfo { Id = Guid.NewGuid(), Key = "LOCATION", Value = "" });
        //    aa.Info.Add(new AssetInfo { Id = Guid.NewGuid(), Key = "DESCRIPTION", Value = "" });
        //    aa.ProjectAreaId = a.Id;
        //    aa.Diagnostics = new List<Diagnostics>();

        //    Diagnostics m = new Diagnostics();
        //    m.Id = Guid.NewGuid();
        //    m.Type = diagnosticsTypes[1];
        //    m.Details = new List<DiagnosticsDetail>();

        //    foreach (Item i in items)
        //        if (i.Class.Name == "Mechanical")
        //        {
        //            DiagnosticsDetail dd = new DiagnosticsDetail();
        //            dd.Id = Guid.NewGuid();
        //            dd.Image = null;
        //            dd.Info = new List<DiagnosticsDetailInfo>();
        //            dd.Item = i;
        //            dd.Status = "_new_";
        //            dd.Comments = "";

        //            m.Details.Add(dd);
        //        }

        //    aa.Diagnostics.Add(m);
        //    assets.Add(aa);
        //}

        //public void Demo_AreaGenDiagInit(ProjectArea a)
        //{
        //    a.GeneralDiagnostics = new Diagnostics();
        //    a.GeneralDiagnostics.Id = Guid.NewGuid();
        //    a.GeneralDiagnostics.Type = diagnosticsTypes[0];
        //    a.GeneralDiagnostics.Details = new List<DiagnosticsDetail>();

        //    int idx = 0;

        //    foreach (Item i in items)
        //        if (i.Class.Name == "General")
        //        {
        //            DiagnosticsDetail dd = new DiagnosticsDetail();
        //            dd.Id = Guid.NewGuid();
        //            dd.Image = null;
        //            dd.Info = new List<DiagnosticsDetailInfo>();
        //            dd.Item = i;
        //            dd.Status = "_new_";
        //            dd.Comments = "";

        //            //idx++;

        //            if (idx == 6)
        //                idx = 1;

        //            if (idx == 2)
        //            {
        //                dd.Status = "N";
        //            }
        //            else if (idx == 3)
        //            {
        //                dd.Status = "R";
        //            }
        //            else if (idx >= 4)
        //            {
        //                dd.Status = "I";
        //                dd.Comments = "This is a test. " + dd.Item.Title;
        //                if (idx == 5)
        //                {
        //                    dd.Image = new Image();
        //                    dd.Image.DataImage = new BitmapImage(new Uri("pack://application:,,,/Media/demo/p2_a1.png"));
        //                }
        //            }

        //            a.GeneralDiagnostics.Details.Add(dd);
        //        }
        //}

        //public void Demo_Standards()
        //{
        //    items = new List<Item>();
        //    itemClasses = new List<ItemClass>();
        //    itemSections = new List<ItemSection>();
        //    itemSubSections = new List<ItemSubSection>();
        //    diagnosticsTypes = new List<DiagnosticsType>();

        //    DiagnosticsType dt = new DiagnosticsType();
        //    dt.Id = Guid.NewGuid();
        //    dt.Name = "General";
        //    diagnosticsTypes.Add(dt);

        //    ItemClass ic = new ItemClass();
        //    ic.Description = "General Items";
        //    ic.Name = "General";
        //    ic.Id = Guid.NewGuid();
        //    itemClasses.Add(ic);

        //    bool hasSS = false;
        //    int idx = 0;

        //    StreamReader sr = new StreamReader(@"Data\general.txt", System.Text.Encoding.GetEncoding(1252), true);
        //    while (!sr.EndOfStream)
        //    {
        //        string sline = sr.ReadLine();
        //        char[] delimiters = new char[] { '\t' };
        //        string[] sitems = sline.Split(delimiters);

        //        // Let's see what the first value has
        //        if (sitems[0] == "section")
        //        {
        //            // Section
        //            ItemSection iS = new ItemSection();
        //            iS.Id = Guid.NewGuid();
        //            string scode = sitems[1].Trim();

        //            iS.Code = scode;
        //            iS.Title = sitems[5].Replace("\"", "").Trim();
        //            itemSections.Add(iS);
        //            hasSS = false;
        //        }
        //        else if (sitems[0] == "print")
        //        {
        //            ItemSubSection iSS = new ItemSubSection();
        //            iSS.Id = Guid.NewGuid();

        //            string scode = sitems[1].Trim();
        //            scode = scode + "." + sitems[2].Trim();
        //            // Check if item 3 is a 0 or empty
        //            if (sitems[3].Trim() == "0" || sitems[3].Trim() == "")
        //            {
        //                // Nothing
        //            }
        //            else
        //            {
        //                scode = scode + "." + sitems[3].Trim();
        //            }

        //            iSS.Code = scode;
        //            iSS.Title = sitems[5].Replace("\"", "").Trim();
        //            itemSubSections.Add(iSS);
        //            hasSS = false;
        //        }
        //        else
        //        {
        //            // Check code, and based on the last item, see if it is in SS or not
        //            string scode = sitems[1].Trim();
        //            scode = scode + "." + sitems[2].Trim();
        //            // Check if item 3 is a 0
        //            if (sitems[3].Trim() == "0" || sitems[3].Trim() == "")
        //            {
        //                // Nothing
        //            }
        //            else
        //            {
        //                scode = scode + "." + sitems[3].Trim();
        //            }

        //            if (sitems[4].Trim().Length > 0)
        //            {
        //                scode = scode + "|" + sitems[4].Trim();
        //                hasSS = true;
        //            }
        //            else
        //            {
        //                hasSS = false;
        //            }

        //            Item i = new Item();
        //            i.Id = Guid.NewGuid();
        //            i.Index = idx;
        //            idx++;
        //            i.Info = null;
        //            i.Section = itemSections.Last();
        //            if (hasSS)
        //                i.SubSection = itemSubSections.Last();
        //            else
        //                i.SubSection = null;
        //            i.Class = itemClasses.Last();
        //            i.Code = scode;
        //            i.Title = sitems[5].Replace("\"", "").Trim();
        //            items.Add(i);
        //        }
        //    }
        //    sr.Close();


        //    // Now let's read mechanical
        //    dt = new DiagnosticsType();
        //    dt.Id = Guid.NewGuid();
        //    dt.Name = "Mechanical";
        //    diagnosticsTypes.Add(dt);

        //    ic = new ItemClass();
        //    ic.Description = "Mechanical Items";
        //    ic.Name = "Mechanical";
        //    ic.Id = Guid.NewGuid();
        //    itemClasses.Add(ic);

        //    hasSS = false;
        //    idx = 0;

        //    sr = new StreamReader(@"Data\mechanical.txt", System.Text.Encoding.GetEncoding(1252), true);
        //    while (!sr.EndOfStream)
        //    {
        //        string sline = sr.ReadLine();
        //        char[] delimiters = new char[] { '\t' };
        //        string[] sitems = sline.Split(delimiters);

        //        // Let's see what the first value has
        //        if (sitems[0] == "section")
        //        {
        //            // Section
        //            ItemSection iS = new ItemSection();
        //            iS.Id = Guid.NewGuid();
        //            string scode = sitems[1].Trim();

        //            iS.Code = scode;
        //            iS.Title = sitems[5].Replace("\"", "").Trim();
        //            itemSections.Add(iS);
        //            hasSS = false;
        //        }
        //        else if (sitems[0] == "print")
        //        {
        //            ItemSubSection iSS = new ItemSubSection();
        //            iSS.Id = Guid.NewGuid();

        //            string scode = sitems[1].Trim();
        //            scode = scode + "." + sitems[2].Trim();
        //            // Check if item 3 is a 0 or empty
        //            if (sitems[3].Trim() == "0" || sitems[3].Trim() == "")
        //            {
        //                // Nothing
        //            }
        //            else
        //            {
        //                scode = scode + "." + sitems[3].Trim();
        //            }

        //            if (sitems[4].Trim().Length > 0)
        //            {
        //                scode = scode + "|" + sitems[4].Trim();
        //            }
        //            else
        //            {
        //                // Nothing
        //            }

        //            iSS.Code = scode;
        //            iSS.Title = sitems[5].Replace("\"", "").Trim();
        //            itemSubSections.Add(iSS);
        //            hasSS = false;
        //        }
        //        else
        //        {
        //            // Check code, and based on the last item, see if it is in SS or not
        //            string scode = sitems[1].Trim();
        //            scode = scode + "." + sitems[2].Trim();
        //            // Check if item 3 is a 0
        //            if (sitems[3].Trim() == "0" || sitems[3].Trim() == "")
        //            {
        //                // Nothing
        //            }
        //            else
        //            {
        //                scode = scode + "." + sitems[3].Trim();
        //            }

        //            if (sitems[4].Trim().Length > 0)
        //            {
        //                scode = scode + "|" + sitems[4].Trim();
        //                hasSS = true;
        //            }
        //            else
        //            {
        //                hasSS = false;
        //            }

        //            Item i = new Item();
        //            i.Id = Guid.NewGuid();
        //            i.Index = idx;
        //            idx++;
        //            i.Info = null;
        //            i.Section = itemSections.Last();
        //            if (hasSS)
        //                i.SubSection = itemSubSections.Last();
        //            else
        //                i.SubSection = null;
        //            i.Class = itemClasses.Last();
        //            i.Code = scode;
        //            i.Title = sitems[5].Replace("\"", "").Trim();
        //            items.Add(i);
        //        }
        //    }
        //    sr.Close();

        //    // Now let's read mechanical
        //    dt = new DiagnosticsType();
        //    dt.Id = Guid.NewGuid();
        //    dt.Name = "Electrical";
        //    diagnosticsTypes.Add(dt);

        //    ic = new ItemClass();
        //    ic.Description = "Electrical Items";
        //    ic.Name = "Electrical";
        //    ic.Id = Guid.NewGuid();
        //    itemClasses.Add(ic);

        //    hasSS = false;
        //    idx = 0;

        //    sr = new StreamReader(@"Data\electrical.txt", System.Text.Encoding.GetEncoding(1252), true);
        //    while (!sr.EndOfStream)
        //    {
        //        string sline = sr.ReadLine();
        //        char[] delimiters = new char[] { '\t' };
        //        string[] sitems = sline.Split(delimiters);

        //        // Let's see what the first value has
        //        if (sitems[0] == "section")
        //        {
        //            // Section
        //            ItemSection iS = new ItemSection();
        //            iS.Id = Guid.NewGuid();
        //            string scode = sitems[1].Trim();

        //            iS.Code = scode;
        //            iS.Title = sitems[5].Replace("\"", "").Trim();
        //            itemSections.Add(iS);
        //            hasSS = false;
        //        }
        //        else if (sitems[0] == "print")
        //        {
        //            ItemSubSection iSS = new ItemSubSection();
        //            iSS.Id = Guid.NewGuid();

        //            string scode = sitems[1].Trim();
        //            scode = scode + "." + sitems[2].Trim();
        //            // Check if item 3 is a 0 or empty
        //            if (sitems[3].Trim() == "0" || sitems[3].Trim() == "")
        //            {
        //                // Nothing
        //            }
        //            else
        //            {
        //                scode = scode + "." + sitems[3].Trim();
        //            }

        //            if (sitems[4].Trim().Length > 0)
        //            {
        //                scode = scode + "|" + sitems[4].Trim();
        //            }
        //            else
        //            {
        //                // Nothing
        //            }

        //            iSS.Code = scode;
        //            iSS.Title = sitems[5].Replace("\"", "").Trim();
        //            itemSubSections.Add(iSS);
        //            hasSS = false;
        //        }
        //        else
        //        {
        //            // Check code, and based on the last item, see if it is in SS or not
        //            string scode = sitems[1].Trim();
        //            scode = scode + "." + sitems[2].Trim();
        //            // Check if item 3 is a 0
        //            if (sitems[3].Trim() == "0" || sitems[3].Trim() == "")
        //            {
        //                // Nothing
        //            }
        //            else
        //            {
        //                scode = scode + "." + sitems[3].Trim();
        //            }

        //            if (sitems[4].Trim().Length > 0)
        //            {
        //                scode = scode + "|" + sitems[4].Trim();
        //                hasSS = true;
        //            }
        //            else
        //            {
        //                hasSS = false;
        //            }

        //            Item i = new Item();
        //            i.Id = Guid.NewGuid();
        //            i.Index = idx;
        //            idx++;
        //            i.Info = null;
        //            i.Section = itemSections.Last();
        //            if (hasSS)
        //                i.SubSection = itemSubSections.Last();
        //            else
        //                i.SubSection = null;
        //            i.Class = itemClasses.Last();
        //            i.Code = scode;
        //            i.Title = sitems[5].Replace("\"", "").Trim();
        //            items.Add(i);
        //        }
        //    }
        //    sr.Close();
        //}

    }
}
