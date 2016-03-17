using SEG.Domain.Helpers;
using SEG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Printing
{
    public class PrintableArea
    {
        public Guid areaId;
        public Guid projectId;

        public SEGRepository segR;

        public ProjectArea area;
        public Project project;
        public string projectCode;

        public List<Item> genSI;

        public Diagnostics d;
        public List<DiagnosticsDetail> dD;
        public List<SEG.Domain.Model.Image> imgs;

        public PrintableArea(SEGRepository segR, Guid aareaId, Guid aprojectId)
        {
            areaId = aareaId;
            projectId = aprojectId;

            area = ProjectHelper.GetProjectArea(segR, areaId);
            project = ProjectHelper.GetProject(segR, projectId);
            projectCode = ProjectHelper.GetProjectCode(segR, projectId);

            genSI = DiagnosticsHelper.GetStandardsForType(segR, "General");

            // Mechanical
            d = DiagnosticsHelper.GetAreaDiagnostics(segR, areaId);
            imgs = DiagnosticsHelper.GetDiagnosticsImages(segR, d.Id);
            dD = DiagnosticsHelper.GetDiagnosticsDetails(segR, d.Id);
        }
    }
}
