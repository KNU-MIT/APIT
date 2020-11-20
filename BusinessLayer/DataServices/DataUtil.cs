using System;
using System.IO;
using System.Threading.Tasks;
using DatabaseLayer.ConfigModels;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using SautinSoft;


namespace BusinessLayer.DataServices
{
    public static class DataUtil
    {
        /// <summary>
        /// Generate some unique key to use it as object route address
        /// </summary>
        /// <param name="data">Repository reference object</param>
        /// <param name="length">key chars summary count</param>
        /// <typeparam name="TKey">IAddressedData Type</typeparam>
        /// <returns>Fixed size (from length param) string</returns>
        public static string GenerateUniqueAddress<TKey>(IAddressedData<TKey> data, int length)
        {
            int iter = 0;
            string address;
            do
            {
                var guid = Guid.NewGuid().ToString("N");
                address = guid.Substring(guid.Length - length);
            } while (data.GetByUniqueAddress(address) != null && iter++ < 500);

            return iter >= 500 ? null : address;
        }


        public static async Task<string> TrySaveDocFile(IFormFile docFile,
            string fileName, string extension, ProjectConfig.DataPathConfig config)
        {
            // try
            {
                string docxPath = Path.Combine(config.ArticleDocsDir, fileName + extension);
                string htmlPath = Path.Combine(config.ArticleHtmlDir, fileName + Extension.Htm);

                await using (var stream = new FileStream(docxPath, FileMode.Create))
                    await docFile.CopyToAsync(stream);

                // Save original .doc(x) and .htm(l) copy 
                await MSWordParser.Save(docxPath, htmlPath, RtfToHtml.eOutputFormat.HTML_5);

                string imagesPath = htmlPath + "_images";

                if (!Directory.Exists(imagesPath)) return null;
                string imgDestDir = Path.Combine(config.ArticleImagesDir, fileName + Extension.Htm);
                if (!Directory.Exists(imgDestDir)) Directory.CreateDirectory(imgDestDir);

                foreach (string imgPath in Directory.EnumerateFiles(imagesPath))
                    File.Move(imgPath, Path.Combine(imgDestDir, Path.GetFileName(imgPath)));

                Directory.Delete(imagesPath);

                return null;
            }
            // catch (Exception e)
            // {
                // return e.Message;
            // }
        }

        public static async void SaveFile(IFormFile sourceFile, string fileName)
        {
            await using var file = new FileStream(fileName, FileMode.Create);
            await sourceFile.CopyToAsync(file);
        }


        internal static async Task<string> LoadHtmlFile(string fileName, ProjectConfig.DataPathConfig config)
        {
            var htmlPath = Path.Combine(Directory.GetCurrentDirectory(), config.ArticleHtmlDir, fileName);
            if (!File.Exists(htmlPath)) return null;
            using var file = new StreamReader(htmlPath);
            return await file.ReadToEndAsync();
        }


        public static PhysicalFileData GetLoadDocFileOptions(string fileName, ProjectConfig.DataPathConfig config)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), config.ArticleDocsDir, fileName);
            if (!File.Exists(filePath)) return null;

            string mimeType;
            switch (Path.GetExtension(fileName))
            {
                case Extension.Doc:
                    mimeType = MIME.DOC;
                    break;
                case Extension.Docx:
                    mimeType = MIME.DOCX;
                    break;
                default:
                    return null;
            }

            return new PhysicalFileData
            {
                FilePath = filePath,
                MimeType = mimeType,
                FileName = fileName
            };
        }
    }
}