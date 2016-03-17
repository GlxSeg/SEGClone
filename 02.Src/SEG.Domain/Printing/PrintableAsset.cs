using SEG.Domain.Helpers;
using SEG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Printing
{
    public class PrintableAsset
    {
        public Guid aId;
        public Guid areaId;
        public Guid projectId;
        public string id;

        public SEGRepository segR;

        public Asset a;
        public ProjectArea area;
        public Project project;
        public string projectCode;

        public bool hasMechanical;
        public List<AssetInfo> aInfo;
        public List<Item> genSI;
        public List<Item> mecSI;
        public List<Item> eleSI;

        public Diagnostics dm;
        public List<DiagnosticsDetail> dmD;
        public List<SEG.Domain.Model.Image> mimgs;

        public List<Diagnostics> de;
        public List<List<DiagnosticsDetail>> deD;
        public List<List<SEG.Domain.Model.Image>> eimgs;

        public RiskAnalysis rS, rE, rO, rA;

        public PrintableAsset(SEGRepository segR, Guid assetId, Guid aareaId, Guid aprojectId)
        {
            aId = assetId;
            areaId = aareaId;
            projectId = aprojectId;

            area = ProjectHelper.GetProjectArea(segR, areaId);
            project = ProjectHelper.GetProject(segR, projectId);
            projectCode = ProjectHelper.GetProjectCode(segR, projectId);

            a = ProjectHelper.GetAsset(segR, aId);
            aInfo = ProjectHelper.GetAssetInfo(segR, aId);
            hasMechanical = (aInfo.FirstOrDefault(x => x.Key == "MECHDIAG").Value == "YES");
            id = aInfo.FirstOrDefault(x => x.Key == "ID").Value;
            genSI = DiagnosticsHelper.GetStandardsForType(segR, "General");
            mecSI = DiagnosticsHelper.GetStandardsForType(segR, "Mechanical");
            eleSI = DiagnosticsHelper.GetStandardsForType(segR, "Electrical");

            // Mechanical
            dm = DiagnosticsHelper.GetMechanicalDiagnostics(segR, aId);
            mimgs = DiagnosticsHelper.GetDiagnosticsImages(segR, dm.Id);
            dmD = DiagnosticsHelper.GetDiagnosticsDetails(segR, dm.Id);

            // Electrical
            de = DiagnosticsHelper.GetElectricalDiagnostics(segR, aId);
            eimgs = new List<List<Domain.Model.Image>>();
            deD = new List<List<DiagnosticsDetail>>();
            foreach (var d in de)
            {
                var edi = DiagnosticsHelper.GetDiagnosticsImages(segR, d.Id);
                eimgs.Add(edi);
                var edd = DiagnosticsHelper.GetDiagnosticsDetails(segR, d.Id);
                deD.Add(edd);
            }

            // Risk
            List<RiskAnalysis> risks = ProjectHelper.GetAssetRisk(segR, aId);
            List<RiskAnalysisType> rTypes = ProjectHelper.GetRiskAnalysisTypes(segR);
            rS = risks.FirstOrDefault(x => x.RiskAnalysisTypeId == rTypes.FirstOrDefault(y => y.Name == "Seriousness").Id);
            rE = risks.FirstOrDefault(x => x.RiskAnalysisTypeId == rTypes.FirstOrDefault(y => y.Name == "Exposure").Id);
            rO = risks.FirstOrDefault(x => x.RiskAnalysisTypeId == rTypes.FirstOrDefault(y => y.Name == "Occurrence").Id);
            rA = risks.FirstOrDefault(x => x.RiskAnalysisTypeId == rTypes.FirstOrDefault(y => y.Name == "Avoidance").Id);
        }
    }
}
