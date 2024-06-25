using iTextSharp.text;
using iTextSharp.text.pdf;
using Knjiznica.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Knjiznica.Reports
{
    public class KnjigeReport
    {
        public byte[] Podaci { get; private set; }

        //metoda za formatiranu celiju

        private PdfPCell GenerirajCeliju(string sadrzaj, Font font,BaseColor boja, bool wrap)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(sadrzaj, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c1.Padding = 6;
            c1.NoWrap = wrap;
            c1.Border = Rectangle.BOTTOM_BORDER;
            c1.BorderColor = BaseColor.GRAY;
            return c1;
        }

        public void ListaKnjiga(List<Knjiga> knjige)
        {
            //definiranje fontova
            BaseFont bfontZaglavlje = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, false);
            BaseFont bfontText = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            BaseFont bfontPodnozje = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);

            Font fontZaglavlje = new Font(bfontZaglavlje, 12, Font.NORMAL, BaseColor.DARK_GRAY);
            Font fontZaglavljeBold = new Font(bfontZaglavlje, 12, Font.BOLD, BaseColor.DARK_GRAY);
            Font fontNaslov = new Font(bfontText, 14, Font.BOLDITALIC, BaseColor.DARK_GRAY);
            Font fontTablicaZaglavlje = new Font(bfontText, 10, Font.BOLD, BaseColor.BLACK);
            Font fontTekst = new Font(bfontText, 10, Font.NORMAL, BaseColor.BLACK);

            //header
            BaseColor tPozadinaZaglavlje = new BaseColor(189, 136, 122);
            //celija
            BaseColor tPozadinaSadrzaj = new BaseColor(243, 241, 141);


            using (MemoryStream mstream = new MemoryStream())
            {
                using (Document pdfDokument = new Document(PageSize.A4, 50, 50, 20, 50))
                {
                    PdfWriter.GetInstance(pdfDokument, mstream).CloseStream = false;

                    pdfDokument.Open();

                    //ispis zaglavlja: prva kolona logo, druga tekst
                    PdfPTable tZaglavlje = new PdfPTable(2);
                    tZaglavlje.HorizontalAlignment = Element.ALIGN_LEFT;
                    tZaglavlje.DefaultCell.Border = Rectangle.NO_BORDER;
                    tZaglavlje.WidthPercentage = 100f;
                    float[] sirinaKolonaZaglavlja = new float[] { 1f, 3f };
                    tZaglavlje.SetWidths(sirinaKolonaZaglavlja);

                    
                    //header
                    Paragraph info = new Paragraph();
                    info.Alignment = Element.ALIGN_RIGHT;

                    //prored
                    info.SetLeading(0, 1.2f);
                    info.Add(new Chunk("MEV \n", fontZaglavljeBold));
                    info.Add(new Chunk("Bana Josipa Jelačića 22a, Čakovec \n", fontZaglavlje));

                    PdfPCell cInfo = new PdfPCell();
                    cInfo.AddElement(info);
                    cInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cInfo.Border = Rectangle.NO_BORDER;
                    tZaglavlje.AddCell(info);

                    pdfDokument.Add(tZaglavlje);

                    //naslov
                    Paragraph pNaslov = new Paragraph("POPIS KNJIGA", fontNaslov);
                    pNaslov.Alignment = Element.ALIGN_CENTER;
                    pNaslov.SpacingBefore = 20;
                    pNaslov.SpacingAfter = 20;
                    pdfDokument.Add(pNaslov);

                    //tablica knjige
                    PdfPTable t = new PdfPTable(5);
                    t.WidthPercentage = 100;
                    t.SetWidths(new float[] { 1, 3, 2, 1, 2 });//relativna sirina

                    //zaglavlje
                    t.AddCell(GenerirajCeliju("Redni broj:", fontTablicaZaglavlje, tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("Naslov i autor", fontTablicaZaglavlje, tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("Godina objave", fontTablicaZaglavlje, tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("Broj stranica", fontTablicaZaglavlje, tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("Izdavačka kuća", fontTablicaZaglavlje, tPozadinaZaglavlje, true));

                    //redovi
                    int i = 1;
                    foreach (Knjiga k in knjige)
                    {
                        t.AddCell(GenerirajCeliju(i.ToString() + ".", fontTekst, tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(k.NaslovIAutor, fontTekst, tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(k.GodinaObjave.ToString("dd.MM.yyyy"),fontTekst, tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(k.BrojStranica.ToString(), fontTekst, tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(k.IzdavackaKucaId.ToString(), fontTekst, tPozadinaSadrzaj, false));
                        i++;
                    }   

                    pdfDokument.Add(t);


                    //vrijeme
                    pNaslov = new Paragraph(System.DateTime.Now.ToString("dd.MM.yyyy"), fontTekst);
                    pNaslov.Alignment = Element.ALIGN_RIGHT;
                    pNaslov.SpacingBefore = 10;
                    pdfDokument.Add(pNaslov);
                }

                Podaci = mstream.ToArray();

                //redni broj stranice
                using (var reader = new PdfReader(Podaci))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var stamper = new PdfStamper(reader, ms))
                        {
                            int PageCount = reader.NumberOfPages;
                            for (int i = 1; i <= PageCount; i++)
                            {
                                Rectangle pageSize = reader.GetPageSize(i);
                                PdfContentByte canvas = stamper.GetOverContent(i);

                                canvas.BeginText();
                                canvas.SetFontAndSize(bfontPodnozje, 10);

                                canvas.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, $"{i}.", pageSize.Right - 50, 30, 0);
                                canvas.EndText();
                            }
                        }
                        Podaci = ms.ToArray();
                    }
                }
            }
        }


    }
}