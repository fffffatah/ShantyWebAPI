using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace ShantyWebAPI.Providers
{
    public class AzureBlobServiceProvider
    {
        public string UploadFile(IFormFile file, string blobName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("BLOB_CONN_STRING"));

            var containerClient = blobServiceClient.GetBlobContainerClient("shanty");
            var blobClient = containerClient.GetBlobClient(blobName);
            blobClient.Upload(file.OpenReadStream());
            return blobClient.Uri.ToString();
        }
        public string UploadFileToBlob(string strFileName, IFormFile file)
        {
            try
            {

                var _task = Task.Run(() => this.UploadFileToBlobAsync(strFileName, file));
                _task.Wait();
                string fileUrl = _task.Result;
                return fileUrl;
            }
            catch (Exception)
            {
                return "";
            }
        }
        private async Task<string> UploadFileToBlobAsync(string strFileName, IFormFile file)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("BLOB_CONN_STRING"));
                string containerName = "shanty";
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                BlobClient blobClient = containerClient.GetBlobClient(strFileName);
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream);
                }
                return blobClient.Uri.AbsoluteUri;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
