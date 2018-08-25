using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CrystalReportsNinja
{
    class PageBreaker
    {
        const string PDF_KIDS = "Kids";
        const string PDF_TITLE = "Title";
        const string PDF_PAGE = "Page";

        /// <summary>
        /// Split PDF file into multiple files. 
        /// Each file contains one group in Crystal Reports
        /// </summary>
        /// <param name="sourceFile">Source PDF File</param>
        /// <param name="outDirectory">PDF output directory</param>
        /// <returns></returns>
        public int SplitPDFByBookmark(string sourceFile, string outDirectory)
        {
            if (!File.Exists(sourceFile))
            {
                throw new Exception($"Unable to locate source file {sourceFile}");
            }

            PdfReader pdfReader = new PdfReader(sourceFile);
            try
            {
                IList<Dictionary<string, object>> bookmarks = SimpleBookmark.GetBookmark(pdfReader);
                if (bookmarks == null)
                    return 0;

                if (bookmarks != null && bookmarks.Count > 0)
                {
                    var root = bookmarks.First();
                    var hasChild = root.ContainsKey(PDF_KIDS);
                    if (hasChild)
                    {
                        var topLevelBookmarks = (IList<Dictionary<string, object>>)root[PDF_KIDS]; //will not scan further level
                        for (var i = 0; i < topLevelBookmarks.Count; i++)
                        {
                            IDictionary<string, object> nextBM = i == topLevelBookmarks.Count - 1 ? null : topLevelBookmarks[i + 1];

                            var title = topLevelBookmarks[i][PDF_TITLE];
                            var pageFrom = Convert.ToInt32(Regex.Match(topLevelBookmarks[i][PDF_PAGE].ToString(), "[0-9]+").Value);
                            var pageTo = nextBM != null ? Convert.ToInt32(Regex.Match(nextBM[PDF_PAGE].ToString(), "[0-9]+").Value) - 1 : (pdfReader.NumberOfPages);
                            SplitPDFByPageRange(sourceFile, outDirectory, $"{title}.pdf", pageFrom, pageTo);
                        }
                        return topLevelBookmarks.Count;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SplitPDFByPageRange(string sourcePdf, string outDirectory, string outFilename, int startpage, int endpage)
        {
            Directory.CreateDirectory(outDirectory);
            var outputFilePath = $"{outDirectory}\\{outFilename}";

            var reader = new PdfReader(sourcePdf);
            var sourceDocument = new Document(reader.GetPageSizeWithRotation(startpage));
            var pdfCopyProvider = new PdfCopy(sourceDocument, new FileStream(outputFilePath, FileMode.Create));

            sourceDocument.Open();
            for (int i = startpage; i <= endpage; i++)
            {
                var importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                pdfCopyProvider.AddPage(importedPage);
            }
            sourceDocument.Close();
            reader.Close();
        }
    }
}
