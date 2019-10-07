using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolutionRenamer.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string URL { get; set; } = "https://github.com/thewebchameleon/TemplateV2.Razor/archive/master.zip";

        [BindProperty]
        public string Keyword { get; set; } = "TemplateV2";

        [BindProperty]
        public string ProjectName { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {


        }

        public async Task<IActionResult> OnPost()
        {
            using (var destinationStream = new MemoryStream())
            {
                using (var sourceArchive = new ZipArchive(await GET(URL)))
                {
                    using (var destinationArchive = new ZipArchive(destinationStream, ZipArchiveMode.Create, true))
                    {
                        foreach (ZipArchiveEntry entry in sourceArchive.Entries)
                        {
                            var newEntry = destinationArchive.CreateEntry(ProcessText(entry.FullName));
                            using (var streamWriter = new StreamWriter(newEntry.Open()))
                            {
                                using (var streamReader = new StreamReader(entry.Open()))
                                {
                                    var sourceText = streamReader.ReadToEnd();
                                    var processedText = ProcessText(sourceText);
                                    streamWriter.Write(processedText);
                                }
                            }
                        }
                    }
                }
                return File(destinationStream.ToArray(), "application/zip");
            }
        }

        string ProcessText(string text)
        {
            if (text == null)
            {
                return string.Empty;
            }
            return text.Replace(Keyword, ProjectName, System.StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<Stream> GET(string uri)
        {
            var httpResponse = await HttpHelper.Get(_httpClientFactory, uri);
            return await httpResponse.Content.ReadAsStreamAsync();
        }
    }
}
