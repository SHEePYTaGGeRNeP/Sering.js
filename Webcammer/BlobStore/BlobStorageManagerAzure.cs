using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlobStorageAzure
{
    public class BlobStorageManagerAzure : IDisposable
    {
        const string connectionstring = "DefaultEndpointsProtocol=https;AccountName=avengersarepussy;AccountKey=jM3dz8LfzDgjBNP80iileEqnvG4pXOzEhKnjoacTTiKFc3ENIPp5hRp9dLQZpWC03f1wrba6jA3GySyV1w0T4w==;EndpointSuffix=core.windows.net";

        private readonly CloudStorageAccount cloudStorageAccount;
        private readonly CloudBlobClient cloudBlobClient;

        public BlobStorageManagerAzure()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(connectionstring);
            cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        }

        public void Dispose()
        {
            // todo
        }

        public async Task PublishFile(string directory, string fileName, Stream file)
        {
            var container = cloudBlobClient.GetContainerReference(directory);
            var blob = container.GetBlockBlobReference(fileName);
            await blob.UploadFromStreamAsync(file);
        }

        public async Task<Stream> GetFileAsStream(string directory, string fileName)
        {
            var ms = new MemoryStream();
            var container = cloudBlobClient.GetContainerReference(directory);
            var blob = container.GetBlockBlobReference(fileName);
            await blob.DownloadToStreamAsync(ms);

            return ms;
        }
    }
}
