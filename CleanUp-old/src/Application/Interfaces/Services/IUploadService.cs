using CleanUp.Application.Requests;

namespace CleanUp.Application.Interfaces.Services
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
    }
}