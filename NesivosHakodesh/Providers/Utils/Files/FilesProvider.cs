using Microsoft.AspNetCore.Http;
using NesivosHakodesh.Core.Config;
using NesivosHakodesh.Providers.Utils.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Utils.Files
{
    public class FilesProvider
    {
        public enum FileType
        {
            Torahs,
            Maamarim,
            Sefurim,
        }

        public static string GetFilePath(string fileName, FileType fileType)
        {
            if(string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            return $"{AppSettingsProvider.GetOtherSettings().FilesBaseDir}{fileType}//{fileName}";
        }

        public static async Task<ProviderResponse> UploadFileAsync(FileType fileType, int id, string subType, HttpRequest Request)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                if (Request.Form == null || !Request.Form.Files.Any())
                {
                    response.Messages.Add("קובץ לא תקין");
                }
                else
                {
                    IFormFile file = Request.Form.Files[0];

                    string newFileName = await SaveFile(id, file, fileType, subType);

                    response.Data = newFileName;
                }

            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        private static async Task<string> SaveFile(int id, IFormFile file, FileType fileType, string subType)
        {
            string newFileName = Regex.Replace(file.FileName, @"\s+", " ").Trim();
            newFileName = newFileName.Replace(" ", "-");

            if(string.IsNullOrEmpty(subType))
            {
                newFileName = $"{id}_{newFileName}";
            }
            else
            {
                newFileName = $"{id}_{subType}_{newFileName}";
            }

            using (Stream stream = new FileStream(GetFilePath(newFileName, fileType), FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(stream);
            }

            return newFileName;
        }
    }
}
