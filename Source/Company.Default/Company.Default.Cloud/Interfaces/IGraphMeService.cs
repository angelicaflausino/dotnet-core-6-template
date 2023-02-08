using Microsoft.Graph;

namespace Company.Default.Cloud.Interfaces
{
    public interface IGraphMeService
    {
        Task<dynamic> GetAsync(string[] properties = null);
        Task<Stream> GetProfilePhotoAsStream();
        Task<string> GetProfilePhotoAsBase64();
    }
}