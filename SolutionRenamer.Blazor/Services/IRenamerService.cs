using SolutionRenamer.Blazor.Models;
using System.Threading.Tasks;

namespace SolutionRenamer.Blazor.Services
{
    public interface IRenamerService
    {
        Task<UploadedFile> CreateProject(CreateProjectRequest request);

        UploadedFile GetUploadedFile(GetUploadedFileRequest request);
    }
}
