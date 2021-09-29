using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Services.StorageServices
{
    public class LocalFileStorageServices : IFileStorageServices
    {
        public LocalFileStorageServices()
        {

        }
        public Task DeleteFile(string path, string container)
        {
            throw new NotImplementedException();
        }

        public Task<string> EditFile(byte[] content, string path, string extension, string container, string contentType)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
        {
            throw new NotImplementedException();
        }
    }
}
