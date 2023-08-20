using Microsoft.AspNetCore.Mvc;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Providers.Utils.Api;
using NesivosHakodesh.Providers.Utils.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Controllers
{
    [ApiController]
    [Route("api/Files")]
    public class FilesController : BaseApiController
    {
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [HttpPost, Route("upload/{type}/{id}/{subType}")]
        public async Task<ActionResult<ProviderResponse>> UploadFileAsync(FilesProvider.FileType type, int id, string subType)
        {
            return await ReturnResponseAsync(FilesProvider.UploadFileAsync(type, id, subType, Request));
        }

        [HttpPost, Route("read/{type}")]
        public ActionResult GetSeferFile(FilesProvider.FileType type, [FromBody] SearchCriteria url)
        {
            return new PhysicalFileResult(FilesProvider.GetFilePath(url.SearchTerm, type), "application/pdf");
        }
    }
}
