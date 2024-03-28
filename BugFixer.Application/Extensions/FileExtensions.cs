using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using System.Xml;

namespace BugFixer.Application.Extensions
{
    public static class FileExtensions
    {
        public static bool UploadFile(this IFormFile file, string fileName, string path,
            List<string>? validFormats = null)
        {
            if (validFormats != null && validFormats.Any())
            {
                var fileFormat = Path.GetExtension(file.FileName);

                if (validFormats.All(s => s != fileFormat))
                {
                    return false;
                }
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var finalPath = path + fileName;

            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return true;
        }

        public static void DeleteFile(this string fileName, string path)
        {
            var finalPath = path + fileName;

            if (File.Exists(finalPath))
            {
                File.Delete(finalPath);
            }
        }

        public static List<string> GetSrcValues(this string text)
        {
            List<string> imgScrs = new List<string>();

            HtmlDocument doc = new HtmlDocument();

            doc.LoadHtml(text);

            var nodes = doc.DocumentNode.SelectNodes(@"//img[@src]");

            if (nodes != null && nodes.Any())
            {
                foreach (var img in nodes)
                {
                    HtmlAttribute att = img.Attributes["src"];
                    imgScrs.Add(att.Value.Split("/").Last());
                }
            }

            return imgScrs;
        }

        public static void ManageEditorImages(string currentText, string newText, string path)
        {
            var currentSrcs = currentText.GetSrcValues();

            var newSrcs = newText.GetSrcValues();

            if (currentSrcs.Count == 0) return;

            if (newSrcs.Count == 0)
            {
                foreach (var img in currentSrcs)
                {
                    img.DeleteFile(path);
                }
            }

            foreach (var img in currentSrcs)
            {
                if (newSrcs.All(s => s != img))
                {
                    img.DeleteFile(path);
                }
            }
        }

    }
}
