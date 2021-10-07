using System.Threading.Tasks;

namespace CinemaSystem.Services.StorageServices
{
    public interface IFileStorageServices
    {
        Task<string> SaveFileAsync(byte[] content, string extension,
            string container, string contentType);

        Task<string> EditFileAsync(byte[] content, string path, 
            string extension, string container, string contentType);
        Task DeleteFileAsync(string path, string container);
    }
}
