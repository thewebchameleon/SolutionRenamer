using System.Threading.Tasks;

namespace SolutionRenamer.Blazor.Data
{
    public interface IRenamerService
    {
        Task<UploadedFile> CreateProject(CreateProjectRequest request);

        UploadedFile GetUploadedFile(GetUploadedFileRequest request);
    }
}
