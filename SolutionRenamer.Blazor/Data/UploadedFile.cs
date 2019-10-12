using System;
using System.IO;

namespace SolutionRenamer.Blazor.Data
{
    public class UploadedFile
    {
        public DateTime ExpiryDate { get; set; }

        public MemoryStream Stream { get; set; }

        public string Id { get; set; }
    }
}
