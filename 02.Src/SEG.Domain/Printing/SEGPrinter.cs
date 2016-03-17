using iTextSharp.text;
using iTextSharp.text.pdf;
using SEG.Domain.Helpers;
using SEG.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Printing
{
    static public class SEGPrinter
    {

        static public void PrintArea(SEGRepository segR, Guid areaId, Guid projectId, string filename)
        {
            // Setup the document
            var doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filename, FileMode.Create));
            writer.PageEvent = new SEGFooter();
            doc.Open();

            var pa = new PrintableArea(segR, areaId, projectId);

            // Print the document header
            Print_Title(doc);

            List<Asset> aList = ProjectHelper.GetAssets(segR, areaId);
            List<PrintableAsset> paList = new List<PrintableAsset>();
            for(int i=0;i<aList.Count;i++)
                paList.Add(new PrintableAsset(segR,aList[i].Id,areaId,projectId));

            paList = paList.OrderBy(x => x.id).ToList();

            PrintArea_Header(doc, pa);

            Print_GeneralPrinciples(doc);

            doc.NewPage();

            // Mechanical
            PrintArea_GeneralDiagnostics(segR, doc, writer, pa);

            for (int i = 0; i < aList.Count; i++)
            {
                // New Page
                doc.NewPage();

                PrintAsset_Header(doc, paList[i], false);

                // Risk
                PrintAsset_Risk(doc, paList[i]);

                // Mechanical
                if (paList[i].hasMechanical)
                    PrintAsset_MechanicalDiagnostics(segR, doc, writer, paList[i]);

                // Electrical (multiple)
                PrintAsset_ElectricalDiagnostics(segR, doc, writer, paList[i]);
            }

            // Done with document
            doc.Close();
        }



        static public void PrintAsset(SEGRepository segR, Guid assetId, Guid areaId, Guid projectId, string filename)
        {
            // Setup the document
            var doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filename, FileMode.Create));
            writer.PageEvent = new SEGFooter();
            doc.Open();

            // Now obtain the asset
            PrintableAsset pa = new PrintableAsset(segR, assetId, areaId, projectId);

            // Print the document header
            Print_Title(doc);

            // And the asset header
            PrintAsset_Header(doc, pa);

            // Print General Principles
            Print_GeneralPrinciples(doc);

            // New Page
            doc.NewPage();

            // Risk
            PrintAsset_Risk(doc, pa);

            // Mechanical
            if(pa.hasMechanical)
                PrintAsset_MechanicalDiagnostics(segR, doc, writer, pa);

            // Electrical (multiple)
            PrintAsset_ElectricalDiagnostics(segR, doc, writer, pa);

            // Done with document
            doc.Close();
        }





































        static public void Print_GeneralPrinciples(Document doc)
        {
            // Print the header of the print section
            float[] cw = new float[] { 30f, 70f };
            var t = new PdfPTable(cw);
            t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            t.WidthPercentage = 100f;
            t.LockedWidth = true;
            t.SpacingBefore = 20f;
            t.SpacingAfter = 5f;

            var ch = new Chunk("PRINCIPIOS GERAIS");
            ch.Font = GetFont_Segoe_Normal(18, Font.BOLD);
            var c = new PdfPCell(new Phrase(ch));
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.NO_BORDER;
            t.AddCell(c);

            c = new PdfPCell();
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.BOTTOM_BORDER;
            t.AddCell(c);

            doc.Add(t);

            List<string> sgptext = new List<string>();
            sgptext.Add("Esta Norma Regulamentadora e seus anexos definem referências técnicas, princípios fundamentais e medidas de proteção para garantir a saúde e a integridade física dos trabalhadores e estabelece requisitos mínimos para a prevenção de acidentes e doenças do trabalho nas fases de projeto e de utilização de máquinas e equipamentos de todos os tipos, e ainda à sua fabricação, importação, comercialização, exposição e cessão a qualquer título, em todas as atividades econômicas, sem prejuízo da observância do disposto nas demais Normas Regulamentadoras - NR aprovadas pela Portaria n.º 3.214, de 8 de junho de 1978, nas normas técnicas oficiais e, na ausência ou omissão destas, nas normas internacionais aplicáveis.");
            sgptext.Add("Entende-se como fase de utilização o transporte, montagem, instalação, ajuste, operação, limpeza, manutenção, inspeção, desativação e desmonte da máquina ou equipamento.");
            sgptext.Add("As disposições desta Norma referem-se a máquinas e equipamentos novos e usados, exceto nos itens em que houver menção específica quanto à sua aplicabilidade.");
            sgptext.Add("As máquinas e equipamentos comprovadamente destinados à exportação estão isentos do atendimento dos requisitos técnicos de segurança previstos nesta norma.");
            sgptext.Add("Esta norma não se aplica às máquinas e equipamentos");
            sgptext.Add("movidos ou impulsionados por força humana ou animal;");
            sgptext.Add("expostos em museus, feiras e eventos, para fins históricos ou que sejam considerados como antiguidades e não sejam mais empregados com fins produtivos, desde que sejam adotadas medidas que garantam a preservação da integridade física dos visitantes e expositores;");
            sgptext.Add("classificados como eletrodomésticos.");
            sgptext.Add("É permitida a movimentação segura de máquinas e equipamentos fora das instalações físicas da empresa para reparos, adequações, modernização tecnológica, desativação, desmonte e descarte.");
            sgptext.Add("O empregador deve adotar medidas de proteção para o trabalho em máquinas e equipamentos, capazes de garantir a saúde e a integridade física dos trabalhadores, e medidas apropriadas sempre que houver pessoas com deficiência envolvidas direta ou indiretamente no trabalho.");
            sgptext.Add("São consideradas medidas de proteção, a ser adotadas nessa ordem de prioridade:");
            sgptext.Add("medidas de proteção coletiva;");
            sgptext.Add("medidas administrativas ou de organização do trabalho; e");
            sgptext.Add("medidas de proteção individual.");
            sgptext.Add("Na aplicação desta Norma devem-se considerar as características das máquinas e equipamentos, do processo, a apreciação de riscos e o estado da técnica.");
            sgptext.Add("Cabe aos trabalhadores:");
            sgptext.Add("cumprir todas as orientações relativas aos procedimentos seguros de operação, alimentação, abastecimento, limpeza, manutenção, inspeção, transporte, desativação, desmonte e descarte das máquinas e equipamentos;");
            sgptext.Add("não realizar qualquer tipo de alteração nas proteções mecânicas ou dispositivos de segurança de máquinas e equipamentos, de maneira que possa colocar em risco a sua saúde e integridade física ou de terceiros;");
            sgptext.Add("comunicar seu superior imediato se uma proteção ou dispositivo de segurança foi removido, danificado ou se perdeu sua função;");
            sgptext.Add("participar dos treinamentos fornecidos pelo empregador para atender às exigências/requisitos descritos nesta Norma;");
            sgptext.Add("colaborar com o empregador na implementação das disposições contidas nesta Norma.");

            List<string> sgpcode = new List<string>();
            sgpcode.Add("12.1");
            sgpcode.Add("12.1.1");
            sgpcode.Add("12.2");
            sgpcode.Add("12.2.A");
            sgpcode.Add("12.2.B");
            sgpcode.Add("12.2.B  a)");
            sgpcode.Add("12.2.B  b)");
            sgpcode.Add("12.2.B  c)");
            sgpcode.Add("12.2.C");
            sgpcode.Add("12.3");
            sgpcode.Add("12.4");
            sgpcode.Add("12.4.0  a)");
            sgpcode.Add("12.4.0  b)");
            sgpcode.Add("12.4.0  c)");
            sgpcode.Add("12.5");
            sgpcode.Add("12.5.A");
            sgpcode.Add("12.5.A  a)");
            sgpcode.Add("12.5.A  b)");
            sgpcode.Add("12.5.A  c)");
            sgpcode.Add("12.5.A  d)");
            sgpcode.Add("12.5.A  e)");


            //// Add the General Standards
            //List<string> sgptext = new List<string>();
            //sgptext.Add("Esta Norma Regulamentadora e seus anexos definem referências técnicas, princípios fundamentais e medidas de proteção para garantir a saúde e a integridade física dos trabalhadores e estabelece requisitos mínimos para a prevenção de acidentes e doenças do trabalho nas fases de projeto e de utilização de máquinas e equipamentos de todos os tipos, e ainda à sua fabricação, importação, comercialização, exposição e cessão a qualquer título, em todas as atividades econômicas, sem prejuízo da observância do disposto nas demais Normas Regulamentadoras - NR aprovadas pela Portaria n.º 3.214, de 8 de junho de 1978, nas normas técnicas oficiais e, na ausência ou omissão destas, nas normas internacionais aplicáveis.");
            //sgptext.Add("Entende-se como fase de utilização a construção, transporte, montagem, instalação, ajuste, operação, limpeza, manutenção, inspeção, desativação e desmonte da máquina ou equipamento.");
            //sgptext.Add("As disposições desta Norma referem-se a máquinas e equipamentos novos e usados, exceto nos itens em que houver menção específica quanto à sua aplicabilidade.");
            //sgptext.Add("O empregador deve adotar medidas de proteção para o trabalho em máquinas e equipamentos, capazes de garantir a saúde e a integridade física dos trabalhadores, e medidas apropriadas sempre que houver pessoas com deficiência envolvidas direta ou indiretamente no trabalho.");
            //sgptext.Add("São consideradas medidas de proteção, a ser adotadas nessa ordem de prioridade: " +
            //         "a) medidas de proteção coletiva;" + Chunk.NEWLINE +
            //         "b) medidas administrativas ou de organização do trabalho; e" + Chunk.NEWLINE +
            //         "c) medidas de proteção individual." + Chunk.NEWLINE);
            //sgptext.Add("A concepção de máquinas deve atender ao princípio da falha segura.");

            //List<string> sgpcode = new List<string>();
            //sgpcode.Add("12.1");
            //sgpcode.Add("12.1.1");
            //sgpcode.Add("12.2");
            //sgpcode.Add("12.3");
            //sgpcode.Add("12.4");
            //sgpcode.Add("12.5");

            cw = new float[] { 15f, 85f };
            t = new PdfPTable(cw);
            t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            t.WidthPercentage = 100f;
            t.LockedWidth = true;
            t.SpacingBefore = 0f;
            t.SpacingAfter = 0f;

            for (int i = 0; i < sgpcode.Count; i++)
            {
                ch = new Chunk(sgpcode[i]);
                ch.Font = GetFont_Segoe_Normal(13, Font.BOLD);
                c = new PdfPCell(new Phrase(ch));
                c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                c.VerticalAlignment = PdfPCell.ALIGN_TOP;
                c.Border = Rectangle.NO_BORDER;
                c.PaddingBottom = 5f;
                t.AddCell(c);

                ch = new Chunk(sgptext[i]);
                ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                c = new PdfPCell(new Phrase(ch));
                c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                c.VerticalAlignment = PdfPCell.ALIGN_TOP;
                c.Border = Rectangle.NO_BORDER;
                c.PaddingBottom = 5f;
                t.AddCell(c);
            }

            doc.Add(t);
        }





































        static public void PrintArea_Header(Document doc, PrintableArea pa)
        {
            // Add top table
            float[] cw = new float[] { 10f, 90f };
            PdfPTable t = new PdfPTable(cw);
            t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            t.WidthPercentage = 100f;
            t.LockedWidth = true;
            t.SpacingBefore = 20f;
            t.SpacingAfter = 20f;

            var c = new PdfPCell();
            c.Border = Rectangle.NO_BORDER;
            t.AddCell(c);

            var ch = new Chunk(pa.area.Name);
            ch.Font = GetFont_Segoe_Light(45, Font.NORMAL);
            c = new PdfPCell(new Phrase(ch));
            c.PaddingTop = 0;
            c.PaddingBottom = 20f;
            c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            c.Border = Rectangle.NO_BORDER;
            t.AddCell(c);


            ch = new Chunk((Chunk.NEWLINE + pa.projectCode + " - " + pa.project.Name + Chunk.NEWLINE + " ").ToUpper());
            ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
            c = new PdfPCell(new Phrase(ch));
            c.Colspan = 2;
            c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            t.AddCell(c);

            doc.Add(t);
        }

        static public void PrintArea_GeneralDiagnostics(SEGRepository segR, Document doc, PdfWriter writer, PrintableArea pa)
        {
            // Mechanical Diagnostics
            var cw = new float[] { 30f, 70f };
            var t = new PdfPTable(cw);
            t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            t.WidthPercentage = 100f;
            t.LockedWidth = true;
            t.SpacingBefore = 20f;
            t.SpacingAfter = 10f;

            var ch = new Chunk("LAUDO GERAL");
            ch.Font = GetFont_Segoe_Normal(18, Font.BOLD);
            var c = new PdfPCell(new Phrase(ch));
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.NO_BORDER;
            t.AddCell(c);

            c = new PdfPCell();
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.BOTTOM_BORDER;
            t.AddCell(c);

            doc.Add(t);


            int imgdx = 0;
            // Images first
            cw = new float[] { 50f, 50f };
            t = new PdfPTable(cw);
            t.TotalWidth = doc.PageSize.Width * 0.9f - doc.LeftMargin - doc.RightMargin;
            t.WidthPercentage = 100f;
            t.LockedWidth = true;
            t.SpacingBefore = 0f;
            t.SpacingAfter = 20f;




            // Description line
            ch = new Chunk("Detalhamento das Não conformidades");
            ch.Font = GetFont_Segoe_Normal(18, Font.BOLD);
            var pr = new Paragraph(ch);
            doc.Add(pr);

            cw = new float[] { 75f, 25f };
            t = new PdfPTable(cw);
            t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            t.WidthPercentage = 100f;
            t.LockedWidth = true;
            t.SpacingBefore = 20f;
            t.SpacingAfter = 5f;

            ch = new Chunk("Tópico");
            ch.Font = GetFont_Segoe_Normal(13, Font.BOLD);
            c = new PdfPCell(new Phrase(ch));
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.NO_BORDER;
            c.PaddingLeft = 10f;
            t.AddCell(c);

            ch = new Chunk("Situação");
            ch.Font = GetFont_Segoe_Normal(13, Font.BOLD);
            c = new PdfPCell(new Phrase(ch));
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.NO_BORDER;
            t.AddCell(c);

            doc.Add(t);

            // Now we run through the mechanical diagnostics themselves
            Guid cSid = Guid.Empty;
            Guid cSSid = Guid.Empty;

            float cDIW = 0f;

            // We need to check if the diagnostic item has information
            foreach (Item i in pa.genSI)
            {
                DiagnosticsDetail cDL = pa.dD.FirstOrDefault(x => x.ItemId == i.Id);
                if (cDL.Status == "I")
                {
                    // Check to see if the section has changed
                    if (i.ItemSectionId != cSid)
                    {
                        cSid = i.ItemSectionId;

                        ItemSection iS = DiagnosticsHelper.GetItemSection(segR, cSid);
                        cSSid = Guid.Empty;

                        // Create a new header
                        cw = new float[] { 75f, 25f };
                        t = new PdfPTable(cw);
                        t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                        t.WidthPercentage = 100f;
                        t.LockedWidth = true;
                        t.SpacingBefore = 0f;
                        t.SpacingAfter = 0f;

                        ch = new Chunk(iS.Code + "  " + iS.Title);
                        ch.Font = GetFont_Segoe_Normal(13, Font.BOLD, System.Drawing.Color.White);
                        c = new PdfPCell(new Phrase(ch));
                        c.BackgroundColor = new BaseColor(System.Drawing.Color.Black);
                        c.Colspan = 1;
                        c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.NO_BORDER;
                        c.PaddingLeft = 10f;
                        c.PaddingTop = 8f;
                        c.PaddingBottom = 10f;
                        t.AddCell(c);

                        ch = new Chunk("I");
                        ch.Font = GetFont_Segoe_Normal(13, Font.BOLD, System.Drawing.Color.White);
                        c = new PdfPCell(new Phrase(ch));
                        c.BackgroundColor = new BaseColor(System.Drawing.Color.Black);
                        c.Colspan = 1;
                        c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.NO_BORDER;
                        c.PaddingTop = 8f;
                        c.PaddingBottom = 10f;
                        t.AddCell(c);

                        doc.Add(t);
                    }

                    if (i.ItemSubSectionId != Guid.Empty && (cSSid == Guid.Empty || cSSid != i.ItemSubSectionId))
                    {
                        cSSid = i.ItemSubSectionId;
                        ItemSubSection iSS = DiagnosticsHelper.GetItemSubSection(segR, cSSid);
                        string sscode = iSS.Code.Replace("|", "  ");

                        // Add subsection
                        // Create a new header
                        cw = new float[] { 25f, 75f };
                        t = new PdfPTable(cw);
                        t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                        t.WidthPercentage = 100f;
                        t.LockedWidth = true;
                        t.SpacingBefore = 0f;
                        t.SpacingAfter = 0f;

                        ch = new Chunk(sscode);
                        ch.Font = GetFont_Segoe_Normal(13, Font.NORMAL);
                        c = new PdfPCell(new Phrase(ch));
                        c.Colspan = 1;
                        c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                        c.BorderWidthTop = 2f;
                        //c.BorderWidthBottom = 2f;
                        c.PaddingLeft = 10f;
                        c.PaddingTop = 8f;
                        c.PaddingBottom = 10f;
                        t.AddCell(c);

                        ch = new Chunk(iSS.Title);
                        ch.Font = GetFont_Segoe_Normal(13, Font.NORMAL);
                        c = new PdfPCell(new Phrase(ch));
                        c.Colspan = 1;
                        c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                        c.BorderWidthTop = 2f;
                        //c.BorderWidthBottom = 2f;
                        c.PaddingTop = 8f;
                        c.PaddingBottom = 10f;
                        t.AddCell(c);

                        doc.Add(t);
                    }

                    // Add item
                    cw = new float[] { 25f, 75f };
                    t = new PdfPTable(cw);
                    t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                    t.WidthPercentage = 100f;
                    t.LockedWidth = true;
                    t.SpacingBefore = 0f;
                    t.SpacingAfter = 0f;

                    string icode = i.Code.Replace("|", "  ");

                    ch = new Chunk(icode);
                    ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                    c = new PdfPCell(new Phrase(ch));
                    c.Colspan = 1;
                    c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    c.Border = Rectangle.TOP_BORDER;//| Rectangle.BOTTOM_BORDER;
                    c.PaddingLeft = 10f;
                    c.PaddingTop = 8f;
                    c.PaddingBottom = 10f;
                    t.AddCell(c);

                    ch = new Chunk(i.Title);
                    ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                    c = new PdfPCell(new Phrase(ch));
                    c.Colspan = 1;
                    c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    c.Border = Rectangle.TOP_BORDER;//| Rectangle.BOTTOM_BORDER;
                    c.PaddingTop = 8f;
                    c.PaddingBottom = 10f;
                    cDIW = t.TotalWidth * 75f / 100f - c.PaddingLeft - c.PaddingRight;
                    t.AddCell(c);


                    doc.Add(t);


                    float cY = writer.GetVerticalPosition(true) - doc.BottomMargin;
                    if (cY < cDIW * 0.6f && cDL.ImageId != Guid.Empty)
                    {
                        doc.NewPage();
                    }




                    // Details
                    cw = new float[] { 25f, 75f };
                    t = new PdfPTable(cw);
                    t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                    t.WidthPercentage = 100f;
                    t.LockedWidth = true;
                    t.SpacingBefore = 0f;
                    t.SpacingAfter = 0f;

                    ch = new Chunk("Descrição");
                    ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                    c = new PdfPCell(new Phrase(ch));
                    c.Colspan = 1;
                    c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    c.Border = Rectangle.BOTTOM_BORDER;//| Rectangle.BOTTOM_BORDER;
                    c.PaddingLeft = 10f;
                    c.PaddingTop = 8f;
                    c.PaddingBottom = 10f;
                    t.AddCell(c);


                    if (cDL.ImageId != Guid.Empty)
                    {
                        var cw2 = new float[] { 50f, 50f };
                        var t2 = new PdfPTable(cw2);
                        t2.TotalWidth = cDIW;

                        ch = new Chunk(cDL.Comments);
                        ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                        c = new PdfPCell(new Phrase(ch));
                        c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.NO_BORDER;
                        t2.AddCell(c);

                        c = new PdfPCell();
                        //c.MinimumHeight = cDIW * 0.5f;
                        c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.NO_BORDER;
                        c.PaddingBottom = 10f;
                        c.PaddingTop = 10f;
                        c.PaddingRight = 10f;
                        c.PaddingLeft = 10f;

                        imgdx++;
                        SEG.Domain.Model.Image segImage = ImageHelper.GetImage(segR, cDL.ImageId);
                        string simg = @"Printing\temp_" + imgdx + ".jpg";
                        System.Drawing.Bitmap b = (System.Drawing.Bitmap)ImageHelper.BytesToImage(segImage.Data);
                        b.SetResolution(96, 96);
                        b.Save(simg, System.Drawing.Imaging.ImageFormat.Jpeg);

                        iTextSharp.text.Image bimg = iTextSharp.text.Image.GetInstance(simg);
                        //wx = (cDIW*0.4f - c.PaddingLeft - c.PaddingRight) / bimg.Width * 100.0;
                        bimg.ScaleToFit(cDIW * 0.5f - c.PaddingLeft - c.PaddingRight, cDIW * 0.5f - c.PaddingLeft - c.PaddingRight);
                        c.AddElement(bimg);

                        t2.AddCell(c);

                        c = new PdfPCell(t2);
                        //c.MinimumHeight = cDIW * 0.5f;
                    }
                    else
                    {
                        ch = new Chunk(cDL.Comments);
                        ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                        c = new PdfPCell(new Phrase(ch));
                    }

                    c.Colspan = 1;
                    c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    c.Border = Rectangle.BOTTOM_BORDER;//| Rectangle.BOTTOM_BORDER;
                    c.PaddingLeft = 0f;
                    c.PaddingRight = 0f;
                    c.PaddingTop = 8f;
                    c.PaddingBottom = 10f;
                    t.AddCell(c);

                    doc.Add(t);
                }
            }
        }







































        static public void PrintAsset_Header(Document doc, PrintableAsset pa, bool printAreaTitle = true)
        {
            float[] cw = new float[] { 30f, 70f };
            PdfPTable t = new PdfPTable(cw);
            t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            t.WidthPercentage = 100f;
            t.LockedWidth = true;
            t.SpacingBefore = 20f;
            t.SpacingAfter = 10f;

            var c = new PdfPCell();
            c.Border = Rectangle.NO_BORDER;
            t.AddCell(c);

            var ch = new Chunk(pa.aInfo.FirstOrDefault(x => x.Key == "ID").Value);
            ch.Font = GetFont_Segoe_Light(45, Font.NORMAL);
            c = new PdfPCell(new Phrase(ch));
            c.PaddingTop = 0;
            c.PaddingBottom = 10f;
            c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            c.Border = Rectangle.NO_BORDER;
            t.AddCell(c);

            c = new PdfPCell();
            c.Border = Rectangle.NO_BORDER;
            t.AddCell(c);

            ch = new Chunk("TAG: "+pa.aInfo.FirstOrDefault(x => x.Key == "TAG").Value);
            ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
            c = new PdfPCell(new Phrase(ch));
            c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            c.PaddingTop = 0;
            c.PaddingBottom = 20f;
            c.Border = Rectangle.NO_BORDER;
            t.AddCell(c);





            ch = new Chunk((Chunk.NEWLINE + pa.projectCode + " - " + pa.project.Name + " - " + pa.area.Name + Chunk.NEWLINE + " ").ToUpper());
            ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
            c = new PdfPCell(new Phrase(ch));
            c.Colspan = 2;
            c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            t.AddCell(c);

            doc.Add(t);

            // Add top table
            //if(printAreaTitle)
            //{
            //    ch = new Chunk(Chunk.NEWLINE + "P 15.018 - " + pa.project.Name + Chunk.NEWLINE + pa.area.Name + Chunk.NEWLINE + "  ");
            //    ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
            //    c = new PdfPCell(new Phrase(ch));
            //    c.Colspan = 1;
            //    c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //    c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            //    t.AddCell(c);
            //}
            //else
            //{
            //    c = new PdfPCell();
            //    c.Border = Rectangle.NO_BORDER;
            //    t.AddCell(c);
            //}

            //float[] cw2 = new float[] { 40f, 60f };
            //PdfPTable t2 = new PdfPTable(cw2);

            //ch = new Chunk("EQUIPAMENTO");
            //ch.Font = GetFont_Segoe_Light(9, Font.NORMAL);
            //c = new PdfPCell(new Phrase(ch));
            //c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            //c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            //c.Border = Rectangle.NO_BORDER;
            //t2.AddCell(c);

            //ch = new Chunk(pa.aInfo.FirstOrDefault(x => x.Key == "ID").Value);
            //ch.Font = GetFont_Segoe_Light(45, Font.NORMAL);
            //c = new PdfPCell(new Phrase(ch));
            //c.PaddingTop = -9;
            //c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            //c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            //c.Border = Rectangle.NO_BORDER;
            //t2.AddCell(c);

            // Description line
            ch = new Chunk(pa.aInfo.FirstOrDefault(x => x.Key == "DESCRIPTION").Value);
            ch.Font = GetFont_Segoe_Light(22, Font.NORMAL);
            Paragraph pr = new Paragraph(ch);
            doc.Add(pr);

            ch = new Chunk(" ");
            ch.Font = GetFont_Segoe_Light(22, Font.NORMAL);
            pr = new Paragraph(ch);
            doc.Add(pr);

        }

        static public void PrintAsset_Risk(Document doc, PrintableAsset pa)
        {
            // Risk Analysis
            var cw = new float[] { 30f, 70f };
            var t = new PdfPTable(cw);
            t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            t.WidthPercentage = 100f;
            t.LockedWidth = true;
            t.SpacingBefore = 20f;
            t.SpacingAfter = 5f;

            var ch = new Chunk("ANÁLISE DE RISCO");
            ch.Font = GetFont_Segoe_Normal(18, Font.BOLD);
            var c = new PdfPCell(new Phrase(ch));
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.NO_BORDER;
            t.AddCell(c);

            c = new PdfPCell();
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.BOTTOM_BORDER;
            t.AddCell(c);

            doc.Add(t);

            string srisk = "1";
            if (pa.rS.Value == "S1")
            {
                if (pa.rO.Value == "HIGH")
                {
                    srisk = "2";
                }
            }
            else if (pa.rS.Value == "S2")
            {
                if (pa.rE.Value == "F1")
                {
                    if (pa.rO.Value == "LOW")
                    {
                        if (pa.rA.Value == "P2")
                            srisk = "2";
                    }
                    else if (pa.rO.Value == "HIGH")
                    {
                        if (pa.rA.Value == "P1")
                            srisk = "2";
                        else
                            srisk = "3";
                    }
                }
                else
                {
                    if (pa.rO.Value == "VERY LOW")
                    {
                        if (pa.rA.Value == "P1")
                            srisk = "2";
                        else
                            srisk = "3";
                    }
                    else if (pa.rO.Value == "LOW")
                    {
                        srisk = "3";
                    }
                    else if (pa.rO.Value == "HIGH")
                    {
                        if (pa.rA.Value == "P1")
                            srisk = "3";
                        else
                            srisk = "4";
                    }
                }
            }

            string sS = "S1 LEVE";
            if (pa.rS.Value == "S2") sS = "S2 GRAVE";
            string sE = "F1 RARAMENTE";
            if (pa.rE.Value == "F2") sE = "F2 FREQUENTEMENTE";
            string sO = "MUITO BAIXO";
            if (pa.rO.Value == "LOW") sO = "BAIXO";
            if (pa.rO.Value == "HIGH") sO = "ALTO";
            string sA = "POSSÍVEL";
            if (pa.rA.Value == "P2") sA = "IMPOSSÍVEL";


            cw = new float[] { 40f, 60f };
            t = new PdfPTable(cw);
            t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            t.WidthPercentage = 100f;
            t.LockedWidth = true;
            t.SpacingBefore = 0f;
            t.SpacingAfter = 20f;

            ch = new Chunk("Gravidade");
            ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
            c = new PdfPCell(new Phrase(ch));
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            c.Border = Rectangle.NO_BORDER;
            c.PaddingBottom = 5f;
            t.AddCell(c);

            ch = new Chunk(sS);
            ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
            c = new PdfPCell(new Phrase(ch));
            c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            c.Border = Rectangle.NO_BORDER;
            c.PaddingBottom = 5f;
            t.AddCell(c);

            ch = new Chunk("Exposição");
            ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
            c = new PdfPCell(new Phrase(ch));
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            c.Border = Rectangle.NO_BORDER;
            c.PaddingBottom = 5f;
            t.AddCell(c);

            ch = new Chunk(sE);
            ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
            c = new PdfPCell(new Phrase(ch));
            c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            c.Border = Rectangle.NO_BORDER;
            c.PaddingBottom = 5f;
            t.AddCell(c);

            ch = new Chunk("Possibilidade de ocorrer risco");
            ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
            c = new PdfPCell(new Phrase(ch));
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            c.Border = Rectangle.NO_BORDER;
            c.PaddingBottom = 5f;
            t.AddCell(c);

            ch = new Chunk(sO);
            ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
            c = new PdfPCell(new Phrase(ch));
            c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            c.Border = Rectangle.NO_BORDER;
            c.PaddingBottom = 5f;
            t.AddCell(c);

            ch = new Chunk("Possibilidade de evitar o risco");
            ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
            c = new PdfPCell(new Phrase(ch));
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            c.Border = Rectangle.NO_BORDER;
            c.PaddingBottom = 5f;
            t.AddCell(c);

            ch = new Chunk(sA);
            ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
            c = new PdfPCell(new Phrase(ch));
            c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            c.Border = Rectangle.NO_BORDER;
            c.PaddingBottom = 5f;
            t.AddCell(c);

            ch = new Chunk("CATEGORIA DE RISCO");
            ch.Font = GetFont_Segoe_Normal(15, Font.BOLD);
            c = new PdfPCell(new Phrase(ch));
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
            c.PaddingBottom = 5f;
            c.PaddingTop = 3f;
            t.AddCell(c);

            ch = new Chunk(srisk);
            ch.Font = GetFont_Segoe_Normal(15, Font.BOLD);
            c = new PdfPCell(new Phrase(ch));
            c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            c.VerticalAlignment = PdfPCell.ALIGN_TOP;
            c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
            c.PaddingBottom = 5f;
            c.PaddingTop = 3f;
            t.AddCell(c);

            doc.Add(t);
        }






































        static public void PrintAsset_MechanicalDiagnostics(SEGRepository segR, Document doc, PdfWriter writer, PrintableAsset pa)
        {
            // Mechanical Diagnostics
            var cw = new float[] { 30f, 70f };
            var t = new PdfPTable(cw);
            t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            t.WidthPercentage = 100f;
            t.LockedWidth = true;
            t.SpacingBefore = 20f;
            t.SpacingAfter = 10f;

            var ch = new Chunk("LAUDO MECÂNICO");
            ch.Font = GetFont_Segoe_Normal(18, Font.BOLD);
            var c = new PdfPCell(new Phrase(ch));
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.NO_BORDER;
            t.AddCell(c);

            c = new PdfPCell();
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.BOTTOM_BORDER;
            t.AddCell(c);

            doc.Add(t);


            int imgdx = 0;
            // Images first
            cw = new float[] { 50f, 50f };
            t = new PdfPTable(cw);
            t.TotalWidth = doc.PageSize.Width * 0.9f - doc.LeftMargin - doc.RightMargin;
            t.WidthPercentage = 100f;
            t.LockedWidth = true;
            t.SpacingBefore = 0f;
            t.SpacingAfter = 20f;

            for (int i = 0; i < pa.mimgs.Count; i++)
            {
                c = new PdfPCell();
                c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                c.VerticalAlignment = PdfPCell.ALIGN_TOP;
                c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                c.PaddingBottom = 20f;
                c.PaddingTop = 20f;
                c.PaddingRight = 20f;
                c.PaddingLeft = 20f;

                imgdx++;
                string simg = @"Printing\temp_" + imgdx + ".jpg";
                System.Drawing.Image b = ImageHelper.BytesToImage(pa.mimgs[i].Data);
                b.Save(simg, System.Drawing.Imaging.ImageFormat.Jpeg);
                iTextSharp.text.Image aimg = iTextSharp.text.Image.GetInstance(simg);
                double wx = ((doc.PageSize.Width * 0.9f - doc.LeftMargin - doc.RightMargin) / 2f - c.PaddingLeft - c.PaddingRight) / aimg.Width * 100.0;
                aimg.ScalePercent((float)(wx));
                c.AddElement(aimg);

                t.AddCell(c);
            }

            doc.Add(t);

            // Description line
            ch = new Chunk("Detalhamento das Não conformidades");
            ch.Font = GetFont_Segoe_Normal(18, Font.BOLD);
            var pr = new Paragraph(ch);
            doc.Add(pr);

            cw = new float[] { 75f, 25f };
            t = new PdfPTable(cw);
            t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            t.WidthPercentage = 100f;
            t.LockedWidth = true;
            t.SpacingBefore = 20f;
            t.SpacingAfter = 5f;

            ch = new Chunk("Tópico");
            ch.Font = GetFont_Segoe_Normal(13, Font.BOLD);
            c = new PdfPCell(new Phrase(ch));
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.NO_BORDER;
            c.PaddingLeft = 10f;
            t.AddCell(c);

            ch = new Chunk("Situação");
            ch.Font = GetFont_Segoe_Normal(13, Font.BOLD);
            c = new PdfPCell(new Phrase(ch));
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.NO_BORDER;
            t.AddCell(c);

            doc.Add(t);

            // Now we run through the mechanical diagnostics themselves
            Guid cSid = Guid.Empty;
            Guid cSSid = Guid.Empty;

            float cDIW = 0f;

            // We need to check if the diagnostic item has information
            foreach (Item i in pa.mecSI)
            {
                DiagnosticsDetail cDL = pa.dmD.FirstOrDefault(x => x.ItemId == i.Id);
                if (cDL.Status == "I")
                {
                    // Check to see if the section has changed
                    if (i.ItemSectionId != cSid)
                    {
                        cSid = i.ItemSectionId;

                        ItemSection iS = DiagnosticsHelper.GetItemSection(segR, cSid);
                        cSSid = Guid.Empty;

                        // Create a new header
                        cw = new float[] { 75f, 25f };
                        t = new PdfPTable(cw);
                        t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                        t.WidthPercentage = 100f;
                        t.LockedWidth = true;
                        t.SpacingBefore = 0f;
                        t.SpacingAfter = 0f;

                        ch = new Chunk(iS.Code + "  " + iS.Title);
                        ch.Font = GetFont_Segoe_Normal(13, Font.BOLD, System.Drawing.Color.White);
                        c = new PdfPCell(new Phrase(ch));
                        c.BackgroundColor = new BaseColor(System.Drawing.Color.Black);
                        c.Colspan = 1;
                        c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.NO_BORDER;
                        c.PaddingLeft = 10f;
                        c.PaddingTop = 8f;
                        c.PaddingBottom = 10f;
                        t.AddCell(c);

                        ch = new Chunk("I");
                        ch.Font = GetFont_Segoe_Normal(13, Font.BOLD, System.Drawing.Color.White);
                        c = new PdfPCell(new Phrase(ch));
                        c.BackgroundColor = new BaseColor(System.Drawing.Color.Black);
                        c.Colspan = 1;
                        c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.NO_BORDER;
                        c.PaddingTop = 8f;
                        c.PaddingBottom = 10f;
                        t.AddCell(c);

                        doc.Add(t);
                    }

                    if (i.ItemSubSectionId != Guid.Empty && (cSSid == Guid.Empty || cSSid != i.ItemSubSectionId))
                    {
                        cSSid = i.ItemSubSectionId;
                        ItemSubSection iSS = DiagnosticsHelper.GetItemSubSection(segR, cSSid);
                        string sscode = iSS.Code.Replace("|", "  ");

                        // Add subsection
                        // Create a new header
                        cw = new float[] { 25f, 75f };
                        t = new PdfPTable(cw);
                        t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                        t.WidthPercentage = 100f;
                        t.LockedWidth = true;
                        t.SpacingBefore = 0f;
                        t.SpacingAfter = 0f;

                        ch = new Chunk(sscode);
                        ch.Font = GetFont_Segoe_Normal(13, Font.NORMAL);
                        c = new PdfPCell(new Phrase(ch));
                        c.Colspan = 1;
                        c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                        c.BorderWidthTop = 2f;
                        //c.BorderWidthBottom = 2f;
                        c.PaddingLeft = 10f;
                        c.PaddingTop = 8f;
                        c.PaddingBottom = 10f;
                        t.AddCell(c);

                        ch = new Chunk(iSS.Title);
                        ch.Font = GetFont_Segoe_Normal(13, Font.NORMAL);
                        c = new PdfPCell(new Phrase(ch));
                        c.Colspan = 1;
                        c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                        c.BorderWidthTop = 2f;
                        //c.BorderWidthBottom = 2f;
                        c.PaddingTop = 8f;
                        c.PaddingBottom = 10f;
                        t.AddCell(c);

                        doc.Add(t);
                    }

                    // Add item
                    cw = new float[] { 25f, 75f };
                    t = new PdfPTable(cw);
                    t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                    t.WidthPercentage = 100f;
                    t.LockedWidth = true;
                    t.SpacingBefore = 0f;
                    t.SpacingAfter = 0f;

                    string icode = i.Code.Replace("|", "  ");

                    ch = new Chunk(icode);
                    ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                    c = new PdfPCell(new Phrase(ch));
                    c.Colspan = 1;
                    c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    c.Border = Rectangle.TOP_BORDER;//| Rectangle.BOTTOM_BORDER;
                    c.PaddingLeft = 10f;
                    c.PaddingTop = 8f;
                    c.PaddingBottom = 10f;
                    t.AddCell(c);

                    ch = new Chunk(i.Title);
                    ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                    c = new PdfPCell(new Phrase(ch));
                    c.Colspan = 1;
                    c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    c.Border = Rectangle.TOP_BORDER;//| Rectangle.BOTTOM_BORDER;
                    c.PaddingTop = 8f;
                    c.PaddingBottom = 10f;
                    cDIW = t.TotalWidth * 75f / 100f - c.PaddingLeft - c.PaddingRight;
                    t.AddCell(c);


                    doc.Add(t);


                    float cY = writer.GetVerticalPosition(true) - doc.BottomMargin;
                    if (cY < cDIW * 0.6f && cDL.ImageId != Guid.Empty)
                    {
                        doc.NewPage();
                    }




                    // Details
                    cw = new float[] { 25f, 75f };
                    t = new PdfPTable(cw);
                    t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                    t.WidthPercentage = 100f;
                    t.LockedWidth = true;
                    t.SpacingBefore = 0f;
                    t.SpacingAfter = 0f;

                    ch = new Chunk("Descrição");
                    ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                    c = new PdfPCell(new Phrase(ch));
                    c.Colspan = 1;
                    c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    c.Border = Rectangle.BOTTOM_BORDER;//| Rectangle.BOTTOM_BORDER;
                    c.PaddingLeft = 10f;
                    c.PaddingTop = 8f;
                    c.PaddingBottom = 10f;
                    t.AddCell(c);


                    if (cDL.ImageId != Guid.Empty)
                    {
                        var cw2 = new float[] { 50f, 50f };
                        var t2 = new PdfPTable(cw2);
                        t2.TotalWidth = cDIW;

                        ch = new Chunk(cDL.Comments);
                        ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                        c = new PdfPCell(new Phrase(ch));
                        c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.NO_BORDER;
                        t2.AddCell(c);

                        c = new PdfPCell();
                        //c.MinimumHeight = cDIW * 0.5f;
                        c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.NO_BORDER;
                        c.PaddingBottom = 10f;
                        c.PaddingTop = 10f;
                        c.PaddingRight = 10f;
                        c.PaddingLeft = 10f;

                        imgdx++;
                        SEG.Domain.Model.Image segImage = ImageHelper.GetImage(segR, cDL.ImageId);
                        string simg = @"Printing\temp_" + imgdx + ".jpg";
                        System.Drawing.Bitmap b = (System.Drawing.Bitmap)ImageHelper.BytesToImage(segImage.Data);
                        b.SetResolution(96, 96);
                        b.Save(simg, System.Drawing.Imaging.ImageFormat.Jpeg);

                        iTextSharp.text.Image bimg = iTextSharp.text.Image.GetInstance(simg);
                        //wx = (cDIW*0.4f - c.PaddingLeft - c.PaddingRight) / bimg.Width * 100.0;
                        bimg.ScaleToFit(cDIW * 0.5f - c.PaddingLeft - c.PaddingRight, cDIW * 0.5f - c.PaddingLeft - c.PaddingRight);
                        c.AddElement(bimg);

                        t2.AddCell(c);

                        c = new PdfPCell(t2);
                        //c.MinimumHeight = cDIW * 0.5f;
                    }
                    else
                    {
                        ch = new Chunk(cDL.Comments);
                        ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                        c = new PdfPCell(new Phrase(ch));
                    }

                    c.Colspan = 1;
                    c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    c.Border = Rectangle.BOTTOM_BORDER;//| Rectangle.BOTTOM_BORDER;
                    c.PaddingLeft = 0f;
                    c.PaddingRight = 0f;
                    c.PaddingTop = 8f;
                    c.PaddingBottom = 10f;
                    t.AddCell(c);

                    doc.Add(t);
                }
            }
        }






































        static public void PrintAsset_ElectricalDiagnostics(SEGRepository segR, Document doc, PdfWriter writer, PrintableAsset pa)
        {
            int imgdx = 1000;
            bool firstOne = true;
            int didx = -1;
            foreach (Diagnostics d in pa.de)
            {
                if (firstOne)
                {
                    doc.NewPage();
                    firstOne = false;
                }
                didx++;

                // Electrical Diagnostics
                var cw = new float[] { 30f, 70f };
                var t = new PdfPTable(cw);
                t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                t.WidthPercentage = 100f;
                t.LockedWidth = true;
                t.SpacingBefore = 20f;
                t.SpacingAfter = 10f;

                var ch = new Chunk("LAUDO ELÉTRICO");
                ch.Font = GetFont_Segoe_Normal(18, Font.BOLD);
                var c = new PdfPCell(new Phrase(ch));
                c.Colspan = 1;
                c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                c.Border = Rectangle.NO_BORDER;
                t.AddCell(c);

                c = new PdfPCell();
                c.Colspan = 1;
                c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                c.Border = Rectangle.BOTTOM_BORDER;
                t.AddCell(c);

                doc.Add(t);

                if (pa.de.Count > 1)
                {
                    // Add top table
                    cw = new float[] { 58f, 4f, 38f };
                    t = new PdfPTable(cw);
                    t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                    t.WidthPercentage = 100f;
                    t.LockedWidth = true;
                    t.SpacingBefore = 30f;
                    t.SpacingAfter = 5f;

                    ch = new Chunk(Chunk.NEWLINE + pa.projectCode + " - TASCHIBRA" + Chunk.NEWLINE + pa.area.Name + Chunk.NEWLINE + "  ");
                    ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
                    c = new PdfPCell(new Phrase(ch));
                    c.Colspan = 1;
                    c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    t.AddCell(c);

                    float[] cw2 = new float[] { 40f, 60f };
                    PdfPTable t2 = new PdfPTable(cw2);

                    ch = new Chunk("EQUIPAMENTO");
                    ch.Font = GetFont_Segoe_Light(9, Font.NORMAL);
                    c = new PdfPCell(new Phrase(ch));
                    c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    c.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    c.Border = Rectangle.NO_BORDER;
                    t2.AddCell(c);

                    ch = new Chunk(pa.aInfo.FirstOrDefault(x => x.Key == "EDIAG_" + (didx + 1) + "_INFO1").Value);
                    ch.Font = GetFont_Segoe_Light(45, Font.NORMAL);
                    c = new PdfPCell(new Phrase(ch));
                    c.PaddingTop = -9;
                    c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    c.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    c.Border = Rectangle.NO_BORDER;
                    t2.AddCell(c);

                    ch = new Chunk(pa.aInfo.FirstOrDefault(x => x.Key == "EDIAG_" + (didx + 1) + "_INFO2").Value);
                    ch.Font = GetFont_Segoe_Light(15, Font.NORMAL);
                    c = new PdfPCell(new Phrase(ch));
                    c.Colspan = 2;
                    c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    c.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    c.PaddingTop = 10;
                    c.Border = Rectangle.NO_BORDER;
                    t2.AddCell(c);

                    c = new PdfPCell();
                    c.Border = Rectangle.NO_BORDER;
                    t.AddCell(c);

                    c = new PdfPCell(t2);
                    c.Border = Rectangle.NO_BORDER;
                    t.AddCell(c);

                    doc.Add(t);

                    // Description line
                    ch = new Chunk(pa.aInfo.FirstOrDefault(x => x.Key == "DESCRIPTION").Value);
                    ch.Font = GetFont_Segoe_Light(22, Font.NORMAL);
                    Paragraph pr = new Paragraph(ch);
                    doc.Add(pr);

                    ch = new Chunk("Localização: " + pa.aInfo.FirstOrDefault(x => x.Key == "EDIAG_" + (didx + 1) + "_INFO3").Value);
                    ch.Font = GetFont_Segoe_Light(22, Font.NORMAL);
                    pr = new Paragraph(ch);
                    doc.Add(pr);

                    ch = new Chunk("Portas: " + pa.aInfo.FirstOrDefault(x => x.Key == "EDIAG_" + (didx + 1) + "_INFO4").Value);
                    ch.Font = GetFont_Segoe_Light(22, Font.NORMAL);
                    pr = new Paragraph(ch);
                    doc.Add(pr);
                }






                // Images first
                cw = new float[] { 50f, 50f };
                t = new PdfPTable(cw);
                t.TotalWidth = doc.PageSize.Width * 0.9f - doc.LeftMargin - doc.RightMargin;
                t.WidthPercentage = 100f;
                t.LockedWidth = true;
                t.SpacingBefore = 30f;
                t.SpacingAfter = 20f;

                for (int i = 0; i < pa.eimgs[didx].Count; i++)
                {
                    c = new PdfPCell();
                    c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    c.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER;
                    c.PaddingBottom = 20f;
                    c.PaddingTop = 20f;
                    c.PaddingRight = 20f;
                    c.PaddingLeft = 20f;

                    imgdx++;
                    string simg = @"Printing\temp_" + imgdx + ".jpg";
                    System.Drawing.Image b = ImageHelper.BytesToImage(pa.eimgs[didx][i].Data);
                    b.Save(simg, System.Drawing.Imaging.ImageFormat.Jpeg);
                    iTextSharp.text.Image aimg = iTextSharp.text.Image.GetInstance(simg);
                    double wx = ((doc.PageSize.Width * 0.9f - doc.LeftMargin - doc.RightMargin) / 2f - c.PaddingLeft - c.PaddingRight) / aimg.Width * 100.0;
                    aimg.ScalePercent((float)(wx));
                    c.AddElement(aimg);

                    t.AddCell(c);
                }

                doc.Add(t);

                // Add electrical diagnostics themselves

                // Description line
                ch = new Chunk("Detalhamento das Não conformidades");
                ch.Font = GetFont_Segoe_Normal(18, Font.BOLD);
                var pr2 = new Paragraph(ch);
                doc.Add(pr2);

                cw = new float[] { 75f, 25f };
                t = new PdfPTable(cw);
                t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                t.WidthPercentage = 100f;
                t.LockedWidth = true;
                t.SpacingBefore = 20f;
                t.SpacingAfter = 5f;

                ch = new Chunk("Tópico");
                ch.Font = GetFont_Segoe_Normal(13, Font.BOLD);
                c = new PdfPCell(new Phrase(ch));
                c.Colspan = 1;
                c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                c.Border = Rectangle.NO_BORDER;
                c.PaddingLeft = 10f;
                t.AddCell(c);

                ch = new Chunk("Situação");
                ch.Font = GetFont_Segoe_Normal(13, Font.BOLD);
                c = new PdfPCell(new Phrase(ch));
                c.Colspan = 1;
                c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                c.Border = Rectangle.NO_BORDER;
                t.AddCell(c);

                doc.Add(t);





                // Now we run through the mechanical diagnostics themselves
                Guid cSid = Guid.Empty;
                Guid cSSid = Guid.Empty;

                float cDIW = 0f;

                // We need to check if the diagnostic item has information
                foreach (Item i in pa.eleSI)
                {
                    DiagnosticsDetail cDL = pa.deD[didx].FirstOrDefault(x => x.ItemId == i.Id);
                    if (cDL.Status == "I")
                    {
                        // Check to see if the section has changed
                        if (i.ItemSectionId != cSid)
                        {
                            cSid = i.ItemSectionId;

                            ItemSection iS = DiagnosticsHelper.GetItemSection(segR, cSid);
                            cSSid = Guid.Empty;

                            // Create a new header
                            cw = new float[] { 75f, 25f };
                            t = new PdfPTable(cw);
                            t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                            t.WidthPercentage = 100f;
                            t.LockedWidth = true;
                            t.SpacingBefore = 0f;
                            t.SpacingAfter = 0f;

                            ch = new Chunk(iS.Code + "  " + iS.Title);
                            ch.Font = GetFont_Segoe_Normal(13, Font.BOLD, System.Drawing.Color.White);
                            c = new PdfPCell(new Phrase(ch));
                            c.BackgroundColor = new BaseColor(System.Drawing.Color.Black);
                            c.Colspan = 1;
                            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                            c.Border = Rectangle.NO_BORDER;
                            c.PaddingLeft = 10f;
                            c.PaddingTop = 8f;
                            c.PaddingBottom = 10f;
                            t.AddCell(c);

                            ch = new Chunk("I");
                            ch.Font = GetFont_Segoe_Normal(13, Font.BOLD, System.Drawing.Color.White);
                            c = new PdfPCell(new Phrase(ch));
                            c.BackgroundColor = new BaseColor(System.Drawing.Color.Black);
                            c.Colspan = 1;
                            c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                            c.Border = Rectangle.NO_BORDER;
                            c.PaddingTop = 8f;
                            c.PaddingBottom = 10f;
                            t.AddCell(c);

                            doc.Add(t);
                        }

                        if (i.ItemSubSectionId != Guid.Empty && (cSSid == Guid.Empty || cSSid != i.ItemSubSectionId))
                        {
                            cSSid = i.ItemSubSectionId;
                            ItemSubSection iSS = DiagnosticsHelper.GetItemSubSection(segR, cSSid);
                            string sscode = iSS.Code.Replace("|", "  ");

                            // Add subsection
                            // Create a new header
                            cw = new float[] { 25f, 75f };
                            t = new PdfPTable(cw);
                            t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                            t.WidthPercentage = 100f;
                            t.LockedWidth = true;
                            t.SpacingBefore = 0f;
                            t.SpacingAfter = 0f;

                            ch = new Chunk(sscode);
                            ch.Font = GetFont_Segoe_Normal(13, Font.NORMAL);
                            c = new PdfPCell(new Phrase(ch));
                            c.Colspan = 1;
                            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                            c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                            c.BorderWidthTop = 2f;
                            //c.BorderWidthBottom = 2f;
                            c.PaddingLeft = 10f;
                            c.PaddingTop = 8f;
                            c.PaddingBottom = 10f;
                            t.AddCell(c);

                            ch = new Chunk(iSS.Title);
                            ch.Font = GetFont_Segoe_Normal(13, Font.NORMAL);
                            c = new PdfPCell(new Phrase(ch));
                            c.Colspan = 1;
                            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                            c.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                            c.BorderWidthTop = 2f;
                            //c.BorderWidthBottom = 2f;
                            c.PaddingTop = 8f;
                            c.PaddingBottom = 10f;
                            t.AddCell(c);

                            doc.Add(t);
                        }

                        // Add item
                        cw = new float[] { 25f, 75f };
                        t = new PdfPTable(cw);
                        t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                        t.WidthPercentage = 100f;
                        t.LockedWidth = true;
                        t.SpacingBefore = 0f;
                        t.SpacingAfter = 0f;

                        string icode = i.Code.Replace("|", "  ");

                        ch = new Chunk(icode);
                        ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                        c = new PdfPCell(new Phrase(ch));
                        c.Colspan = 1;
                        c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.TOP_BORDER;//| Rectangle.BOTTOM_BORDER;
                        c.PaddingLeft = 10f;
                        c.PaddingTop = 8f;
                        c.PaddingBottom = 10f;
                        t.AddCell(c);

                        ch = new Chunk(i.Title);
                        ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                        c = new PdfPCell(new Phrase(ch));
                        c.Colspan = 1;
                        c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.TOP_BORDER;//| Rectangle.BOTTOM_BORDER;
                        c.PaddingTop = 8f;
                        c.PaddingBottom = 10f;
                        cDIW = t.TotalWidth * 75f / 100f - c.PaddingLeft - c.PaddingRight;
                        t.AddCell(c);


                        doc.Add(t);


                        // Here we check the position
                        float cY = writer.GetVerticalPosition(true) - doc.BottomMargin;
                        if (cY < cDIW * 0.6f && cDL.ImageId != Guid.Empty)
                        {
                            doc.NewPage();
                        }

                        // Details
                        cw = new float[] { 25f, 75f };
                        t = new PdfPTable(cw);
                        t.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                        t.WidthPercentage = 100f;
                        t.LockedWidth = true;
                        t.SpacingBefore = 0f;
                        t.SpacingAfter = 0f;

                        ch = new Chunk("Descrição");
                        ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                        c = new PdfPCell(new Phrase(ch));
                        c.Colspan = 1;
                        c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.BOTTOM_BORDER;//| Rectangle.BOTTOM_BORDER;
                        c.PaddingLeft = 10f;
                        c.PaddingTop = 8f;
                        c.PaddingBottom = 10f;
                        t.AddCell(c);


                        if (cDL.ImageId != Guid.Empty)
                        {
                            var cw2 = new float[] { 50f, 50f };
                            var t2 = new PdfPTable(cw2);
                            t2.TotalWidth = cDIW;

                            ch = new Chunk(cDL.Comments);
                            ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                            c = new PdfPCell(new Phrase(ch));
                            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                            c.Border = Rectangle.NO_BORDER;
                            t2.AddCell(c);

                            c = new PdfPCell();
                            c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                            c.Border = Rectangle.NO_BORDER;
                            c.PaddingBottom = 10f;
                            c.PaddingTop = 10f;
                            c.PaddingRight = 10f;
                            c.PaddingLeft = 10f;

                            imgdx++;
                            SEG.Domain.Model.Image segImage = ImageHelper.GetImage(segR, cDL.ImageId);
                            string simg = @"Printing\temp_" + imgdx + ".jpg";
                            System.Drawing.Bitmap b = (System.Drawing.Bitmap)ImageHelper.BytesToImage(segImage.Data);
                            b.SetResolution(96, 96);
                            b.Save(simg, System.Drawing.Imaging.ImageFormat.Jpeg);

                            iTextSharp.text.Image bimg = iTextSharp.text.Image.GetInstance(simg);
                            //wx = (cDIW*0.4f - c.PaddingLeft - c.PaddingRight) / bimg.Width * 100.0;
                            bimg.ScaleToFit(new Rectangle(cDIW * 0.5f - c.PaddingLeft - c.PaddingRight, cDIW * 0.5f - c.PaddingLeft - c.PaddingRight));
                            c.AddElement(bimg);

                            t2.AddCell(c);

                            c = new PdfPCell(t2);
                        }
                        else
                        {
                            ch = new Chunk(cDL.Comments);
                            ch.Font = GetFont_Segoe_Light(13, Font.NORMAL);
                            c = new PdfPCell(new Phrase(ch));
                        }

                        c.Colspan = 1;
                        c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        c.Border = Rectangle.BOTTOM_BORDER;//| Rectangle.BOTTOM_BORDER;
                        c.PaddingLeft = 0f;
                        c.PaddingRight = 0f;
                        c.PaddingTop = 8f;
                        c.PaddingBottom = 10f;
                        t.AddCell(c);

                        doc.Add(t);

                    }
                }

                doc.NewPage();
            }
        }































        static public void Print_Title(Document doc)
        {
            // Add title
            iTextSharp.text.Image topImg = iTextSharp.text.Image.GetInstance(@"Printing\titlebar.png");
            double wx = (doc.PageSize.Width - doc.LeftMargin - doc.RightMargin) / topImg.Width * 100.0;
            topImg.ScalePercent((float)(wx));
            doc.Add(topImg);
        }
































        static public iTextSharp.text.Font GetFont_Segoe_Light(float size, int style)
        {
            var fontName = "Segoe UI Light";
            if (!FontFactory.IsRegistered(fontName))
            {
                var fontPath = "Printing\\segoeuil.ttf";
                FontFactory.Register(fontPath);
            }
            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, size, style);
        }

        static public iTextSharp.text.Font GetFont_Segoe_Normal(float size, int style)
        {
            var fontName = "Segoe UI Light";
            if (!FontFactory.IsRegistered(fontName))
            {
                var fontPath = "Printing\\segoeuil.ttf";
                FontFactory.Register(fontPath);
            }
            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, size, style);
        }

        static public iTextSharp.text.Font GetFont_Segoe_Normal(float size, int style, System.Drawing.Color c)
        {
            var fontName = "Segoe UI Light";
            if (!FontFactory.IsRegistered(fontName))
            {
                var fontPath = "Printing\\segoeuil.ttf";
                FontFactory.Register(fontPath);
            }
            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, size, style, new BaseColor(c));
        }

    }









    public class SEGFooter : PdfPageEventHelper
    {


        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            var cw = new float[] { 35f, 30f, 35f };
            var t = new PdfPTable(cw);
            t.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            t.WidthPercentage = 100f;
            t.LockedWidth = true;
            t.SpacingBefore = 5f;
            t.SpacingAfter = 5f;


            var ch = new Chunk("GreyLogix Brasil");
            ch.Font = GetFont_Segoe_Normal(11, Font.NORMAL);

            var c = new PdfPCell(new Phrase(ch));
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.NO_BORDER;//| Rectangle.BOTTOM_BORDER;
            c.PaddingTop = 5f;
            c.PaddingBottom = 5f;            
            t.AddCell(c);

            ch = new Chunk("www.greylogix.com.br");
            ch.Font = GetFont_Segoe_Normal(11, Font.UNDERLINE, System.Drawing.Color.Blue);
            ch.SetAnchor("http://www.greylogix.com.br");

            c = new PdfPCell(new Phrase(ch));
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.NO_BORDER;//| Rectangle.BOTTOM_BORDER;
            c.PaddingTop = 5f;
            c.PaddingBottom = 5f;
            t.AddCell(c);

            ch = new Chunk(writer.PageNumber.ToString());
            ch.Font = GetFont_Segoe_Normal(11, Font.NORMAL);

            c = new PdfPCell(new Phrase(ch));
            c.Colspan = 1;
            c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            c.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            c.Border = Rectangle.NO_BORDER;//| Rectangle.BOTTOM_BORDER;
            c.PaddingTop = 5f;
            c.PaddingBottom = 5f;
            t.AddCell(c);

            t.WriteSelectedRows(0, -1, document.LeftMargin, document.Bottom, writer.DirectContent);
        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }

        static public iTextSharp.text.Font GetFont_Segoe_Normal(float size, int style)
        {
            var fontName = "Segoe UI Light";
            if (!FontFactory.IsRegistered(fontName))
            {
                var fontPath = "Printing\\segoeuil.ttf";
                FontFactory.Register(fontPath);
            }
            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, size, style);
        }

        static public iTextSharp.text.Font GetFont_Segoe_Normal(float size, int style, System.Drawing.Color c)
        {
            var fontName = "Segoe UI Light";
            if (!FontFactory.IsRegistered(fontName))
            {
                var fontPath = "Printing\\segoeuil.ttf";
                FontFactory.Register(fontPath);
            }
            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, size, style, new BaseColor(c));
        }

    }
}
