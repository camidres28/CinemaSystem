using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CinemaSystem.Services.StorageServices
{
    public class AzureFileStorageServices : IFileStorageServices
    {
        private readonly string connectionString;

        public AzureFileStorageServices(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("AzureStorage");
        }
        public async Task DeleteFileAsync(string path, string container)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            BlobContainerClient client = new(this.connectionString, container);
            await client.CreateIfNotExistsAsync();
            string file = Path.GetFileName(path);
            BlobClient blob = client.GetBlobClient(file);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> EditFileAsync(byte[] content, string path, string extension, string container, string contentType)
        {
            await this.DeleteFileAsync(path, container);
            string uri = await this.SaveFileAsync(content, extension, container, contentType);

            return uri;
        }

        public async Task<string> SaveFileAsync(byte[] content, string extension, string container, string contentType)
        {
            BlobContainerClient client = new(this.connectionString, container);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);
            string fileName = $"{Guid.NewGuid()}{extension}";
            BlobClient blob = client.GetBlobClient(fileName);
            BlobUploadOptions blobUploadOptions = new();
            BlobHttpHeaders blobHttpHeaders = new();
            blobHttpHeaders.ContentType = contentType;

            blobUploadOptions.HttpHeaders = blobHttpHeaders;
            BinaryData binaryData = new(content);
            await blob.UploadAsync(binaryData, blobUploadOptions);

            string uri = blob.Uri.ToString();
            return uri;
        }
    }
}
