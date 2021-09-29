using System.Threading.Tasks;

namespace CinemaSystem.Services.StorageServices
{
    public interface IFileStorageServices
    {
        Task<string> SaveFile(byte[] content, string extension,
            string container, string contentType);

        Task<string> EditFile(byte[] content, string path, 
            string extension, string container, string contentType);
        Task DeleteFile(string path, string container);
    }
}
