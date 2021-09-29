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
        public async Task DeleteFile(string path, string container)
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

        public async Task<string> EditFile(byte[] content, string path, string extension, string container, string contentType)
        {
            await this.DeleteFile(path, container);
            string uri = await this.SaveFile(content, extension, container, contentType);

            return uri;
        }

        public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
        {
            string uri = string.Empty;

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

            uri = blob.Uri.ToString();

            return uri;
        }
    }
}
