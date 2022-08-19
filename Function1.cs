using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static FunctionWeather.Models.Enums;
using System.Net;
using System.Collections.Generic;
using FunctionWeather.Models;
using FunctionWeather.Services;

namespace FunctionWeather
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, DateTime date, string sensorType, string deviceId)
        {
            if (!Enum.TryParse(typeof(SensorType), sensorType.ToLower(), out _))
                return new BadRequestObjectResult(new { message = "This sensorType doesn't exist" });

            IEnumerable<SensorReading> sensorData = new AzureStorageService()
                .GetSensorData(deviceId, (SensorType)Enum.Parse(typeof(SensorType), sensorType), date);
            return new OkObjectResult(sensorData);
        }
    }
}
