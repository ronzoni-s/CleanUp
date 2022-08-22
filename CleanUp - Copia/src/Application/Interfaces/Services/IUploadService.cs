using ErbertPranzi.Application.Requests;

namespace ErbertPranzi.Application.Interfaces.Services
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
    }
}