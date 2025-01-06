using Azure.Storage.Blobs;
using EllipticCurve.Utils;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Configuration;
using System.Net;
using System.Text;

namespace MiniCarRentalAPI.Services
{
    public class AzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobService(IOptions<AzureBlobServiceOptions> options)
        {
            _blobServiceClient = new (options.Value.ConnectionString);
        }

        public async Task<string> Upload(byte[] image)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("carimages");

            await containerClient.CreateIfNotExistsAsync();

            string blobName = Guid.NewGuid().ToString() + ".png";

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            using (var stream = new MemoryStream(image))
            {
                await blobClient.UploadAsync(stream);
            }

            return blobClient.Uri.ToString();
        }
    }

    public class AzureBlobServiceOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
}
