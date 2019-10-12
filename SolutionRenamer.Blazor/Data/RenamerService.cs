using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolutionRenamer.Blazor.Data
{
    public class RenamerService : IRenamerService
    {
        private readonly CacheSettings _settings;
        private readonly ICacheProvider _cacheProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public RenamerService(IHttpClientFactory httpClientFactory, ICacheProvider cacheProvider, IOptions<CacheSettings> settings)
        {
            _settings = settings.Value;
            _httpClientFactory = httpClientFactory;
            _cacheProvider = cacheProvider;
        }

        public async Task<UploadedFile> CreateProject(CreateProjectRequest request)
        {
            if (request.SelectedProjectSource == SelectedProjectSourceEnum.File)
            {
                var destinationStream = new MemoryStream();
                await request.File.Data.CopyToAsync(destinationStream);
                var processedFile = ProcessFile(destinationStream, request.KeywordReplacements);
                return processedFile;
            }

            if (request.SelectedProjectSource == SelectedProjectSourceEnum.Git)
            {
                throw new NotImplementedException();
            }

            if (request.SelectedProjectSource == SelectedProjectSourceEnum.UrlZip)
            {
                var file = await GetZipUrlSource(request.ZipUrl);
                var processedFile = ProcessFile(file, request.KeywordReplacements);

                return processedFile;
            }
            throw new Exception("Could not determine the project source");
        }

        public UploadedFile GetUploadedFile(GetUploadedFileRequest request)
        {
            if (_cacheProvider.TryGet(request.Id, out UploadedFile file))
            {
                return file;
            }
            throw new Exception($"Could not find file with Id {request.Id}");
        }

        #region Private Methods

        private string ProcessText(string text, List<KeywordReplacement> keywordReplacements)
        {
            if (text == null)
            {
                return string.Empty;
            }

            foreach (var keywordReplacement in keywordReplacements)
            {
                text = text.Replace(keywordReplacement.Keyword, keywordReplacement.ReplacementValue, StringComparison.InvariantCulture);
            }

            return text;
        }

        private async Task<Stream> GetZipUrlSource(string uri)
        {
            var httpResponse = await HttpHelper.Get(_httpClientFactory, uri);
            return await httpResponse.Content.ReadAsStreamAsync();
        }

        private UploadedFile ProcessFile(Stream stream, List<KeywordReplacement> keywordReplacements)
        {
            using (var destinationStream = new MemoryStream())
            {
                using (var sourceArchive = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    using (var destinationArchive = new ZipArchive(destinationStream, ZipArchiveMode.Create, true))
                    {
                        foreach (ZipArchiveEntry entry in sourceArchive.Entries)
                        {
                            var newEntry = destinationArchive.CreateEntry(ProcessText(entry.FullName, keywordReplacements));
                            using (var streamWriter = new StreamWriter(newEntry.Open()))
                            {
                                using (var streamReader = new StreamReader(entry.Open()))
                                {
                                    var sourceText = streamReader.ReadToEnd();
                                    var processedText = ProcessText(sourceText, keywordReplacements);
                                    streamWriter.Write(processedText);
                                }
                            }
                        }
                    }
                }

                var id = Guid.NewGuid().ToString();
                var file = new UploadedFile()
                {
                    Stream = destinationStream,
                    ExpiryDate = DateTime.Now.AddMinutes(_settings.ExpiryTimeMinutes),
                    Id = id
                };
                _cacheProvider.Set(id, file);
                return file;
            }
        }

        #endregion
    }
}
