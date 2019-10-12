using BlazorInputFile;
using System.Collections.Generic;

namespace SolutionRenamer.Blazor.Data
{
    public class CreateProjectRequest
    {
        public SelectedProjectSourceEnum SelectedProjectSource { get; set; }

        public string ZipUrl { get; set; }

        public string GitUrl { get; set; }

        public IFileListEntry File { get; set; }

        public List<KeywordReplacement> KeywordReplacements { get; set; }

        public CreateProjectRequest()
        {
            KeywordReplacements = new List<KeywordReplacement>();
        }
    }

    public enum SelectedProjectSourceEnum
    {
        Undefined,
        UrlZip,
        Git,
        File
    }
}
