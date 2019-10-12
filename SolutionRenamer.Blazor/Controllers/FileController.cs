using Microsoft.AspNetCore.Mvc;
using SolutionRenamer.Blazor.Data;
using System.Threading.Tasks;

namespace SolutionRenamer.Blazor.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly IRenamerService _service;

        public FileController(IRenamerService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(string id)
        {
            var file = _service.GetUploadedFile(new GetUploadedFileRequest()
            {
                Id = id
            });
            return File(file.Stream.ToArray(), "application/zip");
        }

    }
}
