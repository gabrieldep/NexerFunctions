using FunctionWeather.Models;
using System.Collections.Generic;
using System.IO;
namespace FunctionWeather.Services
{
    public class ArchiveService
    {
        internal static IEnumerable<SensorReading> GetArrayFromStream(Stream archive)
        {
            IList<SensorReading> values = new List<SensorReading>();
            StreamReader reader = new(archive);
            string? line;
            while ((line = reader.ReadLine()) != null)
                values.Add(SensorReading.FromStringData(line));
            return values;
        }
    }
}
