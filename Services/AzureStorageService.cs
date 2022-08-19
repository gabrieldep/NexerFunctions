using Azure.Storage.Blobs;
using FunctionWeather.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace FunctionWeather.Services
{
    public class AzureStorageService
    {
        private readonly string _connectionString;

        public AzureStorageService()
        {
            _connectionString = "BlobEndpoint=https://sigmaiotexercisetest.blob.core.windows.net/;QueueEndpoint=https://sigmaiotexercisetest.queue.core.windows.net/;FileEndpoint=https://sigmaiotexercisetest.file.core.windows.net/;TableEndpoint=https://sigmaiotexercisetest.table.core.windows.net/;SharedAccessSignature=sv=2017-11-09&ss=bfqt&srt=sco&sp=rl&se=2028-09-27T16:27:24Z&st=2018-09-27T08:27:24Z&spr=https&sig=eYVbQneRuiGn103jUuZvNa6RleEeoCFx1IftVin6wuA%3D";
        }

        internal IEnumerable<SensorReading> GetSensorData(string deviceId, Enums.SensorType sensorType, DateTime date)
        {
            BlobContainerClient containerClient = GetBlobContainerClient("iotbackend");
            IEnumerable<SensorReading> sensorData = new List<SensorReading>();
            BlobClient tempData = containerClient.GetBlobClient($"{deviceId}/{sensorType}/{date:yyyy-MM-dd}.csv");
            if (tempData.Exists())
                sensorData = ArchiveService.GetArrayFromStream(tempData.OpenRead());
            else
            {
                Stream str = GetHistoricalStream(deviceId, sensorType.ToString(), containerClient);
                using ZipArchive package = new(str, ZipArchiveMode.Read);
                ZipArchiveEntry? a = package.Entries.FirstOrDefault(e => e.Name == $"{date:yyyy-MM-dd}.csv");
                if (a != null)
                    sensorData = ArchiveService.GetArrayFromStream(a.Open());
            }
            return sensorData;
        }

        internal BlobContainerClient GetBlobContainerClient(string blobName) => new BlobServiceClient(_connectionString).GetBlobContainerClient(blobName);

        internal static Stream GetHistoricalStream(string deviceId, string sensorType, BlobContainerClient containerClient)
        {
            var blobClient = containerClient.GetBlobClient($"{deviceId}/{sensorType}/historical.zip");
            return blobClient.OpenRead();
        }
    }
}
