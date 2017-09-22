using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InvMe.BLL.AzureLibraries
{
    public class InvMeBlobStorage
    {
        const string connectionString = "DefaultEndpointsProtocol=https;AccountName=officialdoniald;AccountKey=xKdU5Ijg9oU6HBS2zi60get5qeLLrUPYvwVjdTxcoRhBCTVvssSZn1jKzXyhyEklCRL/Gx6CsttTM6uxnBqKtg==;EndpointSuffix=core.windows.net";

        CloudBlobClient cloudBlobClient;
        CloudBlobContainer invmeContainer;

        public InvMeBlobStorage()
        {
            CloudBlobClient cloudBlobClient = CloudStorageAccount
            .Parse(connectionString)
            .CreateCloudBlobClient();

            invmeContainer = cloudBlobClient.GetContainerReference("invme");
        }

        public async Task<List<Uri>> GetAllBlobUrisAsync()
        {
            var containerToken = new BlobContinuationToken();
            var allBlobs = await invmeContainer.ListBlobsSegmentedAsync(containerToken).ConfigureAwait(false);

            var uris = allBlobs.Results.Select( b => b.Uri ).ToList();

            return uris;
        }

        public async Task<string> UploadFileAsync(string filepath, Stream file)
        {
            var uniqueBlobName = Guid.NewGuid().ToString();
            uniqueBlobName += Path.GetExtension(filepath);

            var blobRef = invmeContainer.GetBlockBlobReference(uniqueBlobName);

            await blobRef.UploadFromStreamAsync(file).ConfigureAwait(false);

            return uniqueBlobName;
        }
    }
}
